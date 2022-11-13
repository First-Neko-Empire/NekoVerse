using Mirror;
using NBitcoin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviourSingleton<GameManager>
{
    [SerializeField]
    TMP_InputField if_nickname;

    private string playerNickname;

    public string PlayerNickname
    {
        get { return playerNickname; }
        private set { playerNickname = value; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private struct PlayerData
    {
        public string Nickname { get; set; }
    }

    private readonly SyncDictionary<int, PlayerData> clientData = new SyncDictionary<int, PlayerData>();

    [Command]
    public void CmdClientConnectedToServer(int connId,string nickname)
    {
        clientData[connId] = new PlayerData() { Nickname = nickname };

    }
    [Command]
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
        playerNickname = if_nickname.text;
    }

}
