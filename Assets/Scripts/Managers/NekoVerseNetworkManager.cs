using Mirror;
using NBitcoin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct CreateCharacter : NetworkMessage
{
    public Characters id;
}


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

    public bool shouldStartHost = false;


    private new void Start()
    {
        base.Start();

    }

    public void ShowButtonChange()
    {
        btn_change.SetActive(true);
    }
    public void HideButtonChanage()
    {
        btn_change.SetActive(false);

    }

    public void HostAndPlay()
    {
        shouldStartHost = true;
        RedirectToHost();
        CanvasManager.Instance.ShowYouAreHost();
        CanvasManager.Instance.OnConnectButtonPressed(false);
        CanvasManager.Instance.ShowHostInfo();

        string nickname = LoadUserNicknameFromPrefs();
        GameManager.Instance.SetNickname(nickname);
        HideButtonChanage();

    }
    public void RedirectToHost()
    {
        networkAddress = "127.0.0.1";
        GetComponent<kcp2k.KcpTransport>().Port = 7777;
    }
    public void RedirectToServer()
    {
        networkAddress = "213.189.221.11";
        GetComponent<kcp2k.KcpTransport>().Port = 7777;
    }
    public void OnServerOneButtonPressed()
    {
        Color oldLH = img_localHost.color;
        Color oldServ = img_server.color;
        img_localHost.color = new Color(oldLH.r, oldLH.g, oldLH.b, 0.25f);
        img_server.color = new Color(oldServ.r, oldServ.g, oldServ.b, 1);
        networkAddress = "213.189.221.11";
        GetComponent<kcp2k.KcpTransport>().Port = 7777;
        CanvasManager.Instance.OnConnectButtonPressed(true);
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
        NetworkServer.RegisterHandler<CreateCharacter>(OnCreateCharacter);
    }
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        CreateCharacter charCreationMsg = new CreateCharacter { id=GameManager.Instance.CurrentlySelectedCharacter };
        NetworkClient.Send(charCreationMsg);
    }
    void OnCreateCharacter(NetworkConnectionToClient conn, CreateCharacter msg)
    {
        GameObject player = Instantiate(spawnPrefabs[(int)msg.id]);
        player.name = $"{player.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);
        //GameObject.FindGameObjectWithTag("PlayerCanvasManager").GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
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
            if (shouldStartHost)
            {
                shouldStartHost = false;
                StartHost();
                return;
            }
        StartClient();
    }


    public GameObject GetSpawnPrefab(int index)
    {
        return spawnPrefabs[index];
    }
    public Transform GetNextStartPos()
    {
        return GetStartPosition();
    }
    public GameObject ObjInst(GameObject obj)
    {
        return Instantiate(obj);
    }


    //public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    //{
    //    if (GameManager.Instance.CurrentlySelectedCharacter == Characters.NullCharacter)
    //    {
    //        Debug.LogError("No character is chosen.");
    //        return;
    //    }

    //    GameObject pickedCharacterPrefab = spawnPrefabs[(int)GameManager.Instance.CurrentlySelectedCharacter];

    //    Transform startPos = GetStartPosition();
    //    GameObject player;
    //    if (startPos != null)
    //    {
    //        player = Instantiate(pickedCharacterPrefab, startPos.position, startPos.rotation);
    //    }
    //    else
    //    {
    //        Debug.LogError("Network starting position in the World is not found.");
    //        return;
    //    }

    //    // instantiating a "Player" prefab gives it the name "Player(clone)"
    //    // => appending the connectionId is WAY more useful for debugging!
    //    player.name = $"{player.name} [connId={conn.connectionId}]";
    //    NetworkServer.AddPlayerForConnection(conn, player);
    //}

    //public override void OnClientConnect()
    //{

    //    if (!NetworkClient.ready)
    //    {
    //        NetworkClient.Ready();
    //    }

    //    if (autoCreatePlayer || networkSceneName == "Assets/Scenes/GameWorld.unity")
    //    {
    //        NetworkClient.AddPlayer();
    //    }
    //    //GameManager.Instance.CmdClientConnectedToServer(NetworkClient.connection.connectionId, GameManager.Instance.PlayerNickname);
    //}
}
