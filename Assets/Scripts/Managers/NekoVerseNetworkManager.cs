using Mirror;
using NBitcoin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NekoVerseNetworkManager : NetworkManager
{
    [SerializeField]
    GameObject defaultPlayerPrefab;
    [SerializeField]
    Button btn_connect;
    [SerializeField]
    GameObject btn_change;
    [SerializeField]
    Image img_localHost;
    [SerializeField]
    Image img_server;


    bool shouldStartHost = false;




    private new void Start()
    {
        base.Start();

    }
    public void HostAndPlay()
    {
        btn_change.SetActive(true);


        shouldStartHost = true;
        CanvasManager.Instance.OnConnectButtonPressed();

        CanvasManager.Instance.ShowYouAreHost();

        CanvasManager.Instance.ShowHostInfo();

        string nickname = LoadUserNicknameFromPrefs();
        GameManager.Instance.SetNickname(nickname);

    }
    public void OnServerOneButtonPressed()
    {
        Color oldLH = img_localHost.color;
        Color oldServ = img_server.color;
        img_localHost.color = new Color(oldLH.r, oldLH.g, oldLH.b, 0.25f);
        img_server.color = new Color(oldServ.r, oldServ.g, oldServ.b, 1);
        networkAddress = "";
        GetComponent<kcp2k.KcpTransport>().Port = 0;
    }

    public void OnLocalHostButtonPressed()
    {
        Color oldLH = img_localHost.color;
        Color oldServ = img_server.color;
        img_localHost.color = new Color(oldLH.r, oldLH.g, oldLH.b, 1);
        img_server.color = new Color(oldServ.r, oldServ.g, oldServ.b, 0.25f);
        networkAddress = "127.0.0.1";
        GetComponent<kcp2k.KcpTransport>().Port = 7777;
        btn_connect.interactable = true;

    }
    public void SetPlayerPrefab(GameObject prefab)
    {
        playerPrefab = prefab;
    }
    private string ValidateNickname(string nickname)
    {
        if (string.IsNullOrEmpty(nickname) || string.IsNullOrWhiteSpace(nickname))
        {
            return "Player" + NekoVerseNetworkManager.singleton.numPlayers;
        }
        return nickname;
    }
    public string GetAddress()
    {
        return networkAddress;
    }
    public string GetPort()
    {
        return GetComponent<kcp2k.KcpTransport>().Port.ToString();
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        print("Server started. Adress: " + networkAddress + "  Port: " + GetComponent<kcp2k.KcpTransport>().Port);
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        StopClient();
    }

    public void OnServerInfoChanged(string address, ushort port)
    {
        networkAddress = address;   
        GetComponent<kcp2k.KcpTransport>().Port = port;
    }


    public void SaveUserNicknameToPrefs(string nickname)
    {
        PlayerPrefs.SetString("NICKNAME", nickname);
    }
    public string LoadUserNicknameFromPrefs()
    {
        if (PlayerPrefs.HasKey("NICKNAME"))
            return PlayerPrefs.GetString("NICKNAME");
        return "DefaultNickname";
    }
    public void OnPlayButtonPressed()
    {
            SaveUserNicknameToPrefs(GameManager.Instance.GetNickname());
            //This shouldnt be here, all characters should already be ready
            if (playerPrefab == null)
            {
                playerPrefab = defaultPlayerPrefab;
            }

            if (shouldStartHost)
            {
                shouldStartHost = false;
                StartHost();
                return;
            }
        StartClient();
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        GameObject pickedCharacterPrefab = spawnPrefabs[((int)Characters.Kuro)];
        GameObject player = startPos != null
            ? Instantiate(pickedCharacterPrefab, startPos.position, startPos.rotation)
            : Instantiate(pickedCharacterPrefab);

        // instantiating a "Player" prefab gives it the name "Player(clone)"
        // => appending the connectionId is WAY more useful for debugging!
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnClientConnect()
    {
        // OnClientConnect by default calls AddPlayer but it should not do
        // that when we have online/offline scenes. so we need the
        // clientLoadedScene flag to prevent it.
        if (!clientLoadedScene)
        {
            // Ready/AddPlayer is usually triggered by a scene load completing.
            // if no scene was loaded, then Ready/AddPlayer it here instead.
            if (!NetworkClient.ready)
            {
                NetworkClient.Ready();
            }

            Debug.Log($"Client connected on scene: {networkSceneName}");

            if (autoCreatePlayer || networkSceneName == "Assets/Scenes/GameWorld.unity")
            {
                NetworkClient.AddPlayer();
            }
        }
        GameManager.Instance.CmdClientConnectedToServer(NetworkClient.connection.connectionId, GameManager.Instance.PlayerNickname);
    }
}
