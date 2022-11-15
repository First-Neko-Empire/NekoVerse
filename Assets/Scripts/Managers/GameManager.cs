using Mirror;
using NBitcoin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : NetworkBehaviourSingleton<GameManager>
{
    [SerializeField]
    TMP_InputField if_nickname;
    [SerializeField]
    NekoVerseNetworkManager networkManager;

    private string playerNickname;

    public Characters CurrentlySelectedCharacter = Characters.NullCharacter;


    public string PlayerNickname
    {
        get { return playerNickname; }
        private set { playerNickname = value; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //print(AssetDatabase.GUIDToAssetPath("0"));
    }

    private struct PlayerData
    {
        public string Nickname { get; set; }
    }

    private readonly SyncDictionary<int, PlayerData> clientData = new SyncDictionary<int, PlayerData>();

    [Command (requiresAuthority = false)]
    public void CmdClientConnectedToServer(int connId,string nickname)
    {
        clientData[connId] = new PlayerData() { Nickname = nickname };

    }
    [Command (requiresAuthority = false)]
    public void CmdClientDisconnectedFromServer(int connId)
    {
        clientData.Remove(connId);
    }

    public void SetNickname(string nickname)
    {
        if_nickname.text = nickname;
    }
    public string GetNickname()
    {
        return if_nickname.text;
    }
    private void printttt()
    {
        foreach (KeyValuePair<int,PlayerData> el in clientData)
        {
            print(el.Key + " " + el.Value.Nickname);
}
    }

    private void Update()
    {
        if(if_nickname)
        playerNickname = if_nickname.text;

        print(PlayerNickname);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isServer)
            {
                printttt();
            }
        }
    }

    public void OnNicknameInputFieldEndEdit()
    {
    }

    //[Command(requiresAuthority = false)]
    //public void CmdSpawnSelectedCharacter(NetworkConnectionToClient sender = null)
    //{
    //    print("called");
    //    GameObject characterInstance = networkManager.ObjInst(networkManager.GetSpawnPrefab((int)GameManager.Instance.CurrentlySelectedCharacter));
    //    NetworkServer.Spawn(characterInstance, sender);
    //}
   

}
