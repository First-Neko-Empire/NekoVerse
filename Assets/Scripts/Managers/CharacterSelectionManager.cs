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

    int currentSelectedCharacterId = -1;

    private List<CharacterSlot> selectionSlots = new List<CharacterSlot>();

    private void Start()
    {
        btn_play.interactable = false;
    }

    private void Update()
    {
        //BACKDOOR LULs
        btn_play.interactable = true;
    }

    public void SelectFirstCharacter()
    {
        if (selectionSlots.Count > 0)
        {
            currentSelectedCharacterId = selectionSlots[0].ID;
            selectionSlots[0].ShowAsSelected();
            TryEnableButtonPlay();
            networkManager.SetPlayerPrefab(NekoCharacterPrefabs.Instance.GetPrefabWithId(currentSelectedCharacterId));
        }
    }
    public void OnCharacterSelected(int id)
    {
        currentSelectedCharacterId = id;
        foreach (CharacterSlot slot in selectionSlots)
        {
            if (slot.ID == currentSelectedCharacterId)
            {
                slot.ShowAsSelected();
                TryEnableButtonPlay();
            }
            else
            {
                slot.ShowAsNotSelected();
            }
        }
        networkManager.SetPlayerPrefab(NekoCharacterPrefabs.Instance.GetPrefabWithId(currentSelectedCharacterId));
    }

    public void TryEnableButtonPlay()
    {
        if (if_nickname.text.Length > 0 && !string.IsNullOrEmpty(if_nickname.text) && !string.IsNullOrWhiteSpace(if_nickname.text) && currentSelectedCharacterId != -1)
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
        CanvasManager.Instance.ShowPanelMainMenu();
        CanvasManager.Instance.HideCharSelection();
    }
}
