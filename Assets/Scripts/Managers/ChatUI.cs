using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ChatUI : NetworkBehaviour
{
    [Header("UI Bindings")]
    [SerializeField]
    private TextMeshProUGUI chatHistory;
    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private TMP_InputField chatMessage;

    public static string localPlayerName;

    public override void OnStartClient()
    {
        //localPlayerName = connectionToClient.connectionId.ToString();
        localPlayerName = "123";
        chatHistory.text = string.Empty;
    }

    [Command(requiresAuthority = false)]
    private void CmdSend(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
            RpcReceive("12", message.Trim());
    }

    private void RpcReceive(string senderName, string message)
    {
        string prettyMessage = senderName == localPlayerName ?
                $"<color=red>{senderName}:</color> {message}" :
                $"<color=blue>{senderName}:</color> {message}";
        StartCoroutine(AppendAndScroll(prettyMessage));
    }

    IEnumerator AppendAndScroll(string message)
    {
        chatHistory.text += message + "\n";

        // it takes 2 frames for the UI to update ?!?!
        yield return null;
        yield return null;

        // slam the scrollbar down
        scrollbar.value = 0;
    }

    public void OnEndEdit(string input)
    {
        // opportunity for validation

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            SendMessageToChat();
    }

    private void SendMessageToChat()
    {
        if (!string.IsNullOrWhiteSpace(chatMessage.text))
        {
            CmdSend(chatMessage.text.Trim());
            chatMessage.text = string.Empty;
            chatMessage.ActivateInputField();
        }
    }
}