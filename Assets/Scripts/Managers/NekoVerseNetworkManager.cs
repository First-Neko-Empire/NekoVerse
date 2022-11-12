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


    bool shouldStartHost = false;


    private enum SERVER_SELECTION
    {
        NONE, LOCAL_HOST, SERVER1, SERVER2
    }

    SERVER_SELECTION currentSelection;

    private new void Start()
    {
        base.Start();
        currentSelection = SERVER_SELECTION.NONE;

    }
    public void HostAndPlay()
    {
        shouldStartHost = true;
        CanvasManager.Instance.OnConnectButtonPressed();
        CanvasManager.Instance.ShowYouAreHost();
        CanvasManager.Instance.ShowHostInfo();
        GameManager.Instance.SetNickname(LoadUserNicknameFromPrefs());
    }
    public void OnLocalHostButtonPressed()
    {
        currentSelection = SERVER_SELECTION.LOCAL_HOST;
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


    public void SaveUserNicknameToPrefs(string nickname)
    {
        PlayerPrefs.SetString("NICKNAME", nickname);
    }
    public string LoadUserNicknameFromPrefs()
    {
        if (PlayerPrefs.HasKey("NICKNAME"))
            return PlayerPrefs.GetString("NICKNAME");
        return "";
    }
    public async void OnPlayButtonPressed()
    {
        try
        {
            SaveUserNicknameToPrefs(GameManager.Instance.GetNickname());
            print(" dont do this.");
            //this is so fucking bad ,but it works for now
            if (currentSelection == SERVER_SELECTION.NONE)
            {
                currentSelection = SERVER_SELECTION.LOCAL_HOST;
            }
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


            switch (currentSelection)
            {
                case SERVER_SELECTION.NONE:
                    break;
                case SERVER_SELECTION.LOCAL_HOST:
                    StartClient();
                    break;
                case SERVER_SELECTION.SERVER1:
                    break;
                case SERVER_SELECTION.SERVER2:
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            try
            {
                File.WriteAllLines("C:\\Users\\Nikola\\Desktop\\lines.txt", new string[] { e.Message, e.ToString()});

                ApplicationManager.Instance.print(e.Message);
                ApplicationManager.Instance.print("\n\n");
                ApplicationManager.Instance.print(e.ToString());
                ApplicationManager.Instance.print("\n\n");
                //ApplicationManager.Instance.print(e.InnerException.ToString());
                //ApplicationManager.Instance.print("\n\n");
                //ApplicationManager.Instance.print(e.StackTrace.ToString());
                //ApplicationManager.Instance.print("\n\n");
                //ApplicationManager.Instance.print(e.Data.ToString());
                //ApplicationManager.Instance.print("\n\n");
                //ApplicationManager.Instance.print(e.Source.ToString());
                File.WriteAllLines("lines.txt", new string[] { "hello","how are you" });


            }
            catch (Exception ee)
            {
                ApplicationManager.Instance.print(ee.ToString());
            }
        }


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
