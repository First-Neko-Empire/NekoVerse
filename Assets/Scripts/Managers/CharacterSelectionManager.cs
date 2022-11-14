using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : Singleton<CharacterSelectionManager>
{
    [SerializeField]
    NekoVerseNetworkManager networkManager;
    [SerializeField]
    Button btn_play;
    [SerializeField]
    TMP_InputField if_nickname;
    [SerializeField]
    GameObject serverInfoChage;
    [SerializeField]
    TMP_InputField if_address;
    [SerializeField]
    TMP_InputField if_port;

    private Characters currentlySelectedCharacter = Characters.NullCharacter;

    private List<CharacterSlot> selectionSlots = new List<CharacterSlot>();

    private void Start()
    {
        btn_play.interactable = false;
    }

    public void OnChangeServerInfoButtonPressed()
    {
        serverInfoChage.SetActive(true);
    }

    public void OnGoButtonPressed()
    {
        serverInfoChage.SetActive(false);
        networkManager.OnServerInfoChanged(if_address.text, ushort.Parse(if_port.text));
        CanvasManager.Instance.ShowHostInfo();
    }

    public void SelectFirstCharacter()
    {
        if (selectionSlots.Count > 0)
        {
            currentlySelectedCharacter = (Characters)selectionSlots[0].ID;
            selectionSlots[0].ShowAsSelected();
            TryEnableButtonPlay();
            networkManager.CurrentlySelectedCharacter = currentlySelectedCharacter;
        }
    }
    public void OnCharacterSelected(int id)
    {
        int currentlySelectedCharacterID = id;
        foreach (CharacterSlot slot in selectionSlots)
        {
            if (slot.ID == currentlySelectedCharacterID)
            {
                slot.ShowAsSelected();
                TryEnableButtonPlay();
            }
            else
            {
                slot.ShowAsNotSelected();
            }
        }
        currentlySelectedCharacter = (Characters)currentlySelectedCharacterID;
        networkManager.CurrentlySelectedCharacter = currentlySelectedCharacter;
    }

    public void TryEnableButtonPlay()
    {
        if (if_nickname.text.Length > 0 && !string.IsNullOrEmpty(if_nickname.text) && !string.IsNullOrWhiteSpace(if_nickname.text) && currentlySelectedCharacter != Characters.NullCharacter)
        {
            btn_play.interactable = true;
        }
    }

    public void OnSelectionSlotCreated(CharacterSlot slot)
    {
        selectionSlots.Add(slot);
    }
    public void OnBackButtonPressed()
    {
        networkManager.shouldStartHost = false;
        CanvasManager.Instance.ShowPanelMainMenu();
        CanvasManager.Instance.HideCharSelection();
    }
}
