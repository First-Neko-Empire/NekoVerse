using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.InputSystem;
using System;

public class ChatUI : NetworkBehaviour
{
    [Header("UI Bindings")]
    [SerializeField]
    private GameObject chatPanel;
    [SerializeField]
    private TextMeshProUGUI chatHistory;
    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private TMP_InputField chatMessage;

    private PlayerInput localPlayerInput;
    private static string localPlayerName;

    public override void OnStartClient()
    {
        StartCoroutine(I_Wait(() =>
        {
            localPlayerInput = NetworkClient.localPlayer.gameObject.GetComponent<PlayerInput>();
            localPlayerName = GameManager.Instance.PlayerNickname;
            chatHistory.text = string.Empty;
        }));
    }

    IEnumerator I_Wait(Action todo)
    {
        yield return new WaitForSeconds(2);
        todo.Invoke();
    }

    [Command (requiresAuthority = false)]
    private void CmdSend(string senderName, string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
            RpcReceive(senderName, message);
    }

    [ClientRpc]
    private void RpcReceive(string senderName, string message)
    {
        string prettyMessage = senderName == localPlayerName ?
                $"<color=red>{senderName}:</color> {message}" :
                $"<color=blue>{senderName}:</color> {message}";
        StartCoroutine(AppendAndScroll(prettyMessage));
    }

    private IEnumerator AppendAndScroll(string message)
    {
        chatHistory.text += message + "\n";

        // it takes 2 frames for the UI to update ?!?!
        yield return null;
        yield return null;

        // slam the scrollbar down
        scrollbar.value = 0;
    }

    public void OnSelect(string input)
    {
        localPlayerInput.actions.Disable();
    }

    public void OnDeselect(string input)
    {
        localPlayerInput.actions.Enable();
    }

    public void OnEndEdit(string input)
    {
        // opportunity for validation

        //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        //    SendMessageToChat();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && chatMessage.text.Length == 0)
        {
            chatPanel.SetActive(!chatPanel.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (chatMessage.text.Length > 0)
            {
                SendMessageToChat();
            }
            else
            {
                chatMessage.ActivateInputField();
                localPlayerInput.actions.Disable();
            }
        }
    }

    private void SendMessageToChat()
    {
        if (!string.IsNullOrWhiteSpace(chatMessage.text))
        {
            CmdSend(localPlayerName, chatMessage.text.Trim());
            chatMessage.text = string.Empty;
            chatMessage.DeactivateInputField();
            StartCoroutine(ClearNewLineArtifact());
        }
        else
        {
            chatMessage.text = string.Empty;
            chatMessage.DeactivateInputField();
            StartCoroutine(ClearNewLineArtifact());
        }
    }

    private IEnumerator ClearNewLineArtifact()
    {
        yield return new WaitForEndOfFrame();

        localPlayerInput.actions.Enable();
        chatMessage.text = string.Empty;
    }    
}