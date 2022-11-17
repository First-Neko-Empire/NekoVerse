using Nethereum.Contracts.Standards.ERC1155.ContractDefinition;
using Nethereum.RPC.Eth.Filters;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotsManager : Singleton<CharacterSlotsManager>
{
    NethereumManager.Uint256Array fullBalance;
    public NethereumManager.Uint256Array FullBalance { get { return fullBalance; } }


    [SerializeField]
    Button btn_inventory;
    [SerializeField]
    Button btn_connect;
    [SerializeField]
    Button btn_hostAndPlay;


    [SerializeField]
    private GameObject characterSlotRow;
    [SerializeField]
    private GameObject chraracterSlot;
    [SerializeField]
    private Transform contentInventory;
    List<GameObject> characterSlotsInventory = new List<GameObject>();

    List<GameObject> characterSlotRowsInventory = new List<GameObject>();



    public void ClearAllInventory()
    {
        foreach (var key in characterSlotRowsInventory)
        {
            Destroy(key);
        }
        characterSlotRowsInventory.Clear();
        characterSlotsInventory.Clear();
        ClearAllSelection();
        CharacterSelectionManager.Instance.Clear();
    }
    public  void UpdateWithNewBalance()
    {
        CanvasManager.Instance.HideNoChars();
        ClearAllInventory();
        CreateCharacterSlotsForUser(AccountCreationManager.Instance.UserAccountAddress,null,PopulateCharacterSlotsInventory);
    }
    private void Start()
    {
        btn_inventory.interactable = false;
        btn_connect.interactable = false;
        btn_hostAndPlay.interactable = false;
    }

    public GameObject CreateNewCharacterSlotRowInventory()
    {
        GameObject row = Instantiate(characterSlotRow, contentInventory);
        characterSlotRowsInventory.Add(row);
        return row;
    }
    public GameObject CreateNewCharacterSlotAtCharacterSlotRowInventory(GameObject characterSlotRow)
    {
        GameObject slot = Instantiate(chraracterSlot, characterSlotRow.transform);
        characterSlotsInventory.Add(slot);
        return slot;
    }

    public async void CreateCharacterSlotsForUser(string address, NethereumManager.Uint256Array newFullBalance = null,Action onComplete=null)
    {
        if (newFullBalance == null)
            fullBalance = await NethereumManager.Instance.GetBalanceByAddress(address);
        else
            fullBalance = newFullBalance;

        EnableButtons();

        var numberOfOwnedCharacters = NumberOfOwnedCharacters();
        if (numberOfOwnedCharacters < 4)
        {
            var row = CreateNewCharacterSlotRowInventory();
            for (int i = 0; i < numberOfOwnedCharacters; i++)
            {
                CreateNewCharacterSlotAtCharacterSlotRowInventory(row);
            }
            CreateCharacterSlotsForUserForSelection(address, fullBalance, onComplete);
            return;

        }
        else
        {
            int numberOfRows = 0;
            if (numberOfOwnedCharacters % 4 == 0)
            {
                numberOfRows = numberOfOwnedCharacters / 4;
                for (int i = 0; i < numberOfRows; i++)
                {
                    var row = CreateNewCharacterSlotRowInventory();
                    for (int j = 0; j < 4; j++)
                    {
                        CreateNewCharacterSlotAtCharacterSlotRowInventory(row);
                    }
                }
            }
            else
            {
                numberOfRows = numberOfOwnedCharacters / 4 + 1;
                for (int i = 0; i < numberOfRows - 1; i++)
                {
                    var row = CreateNewCharacterSlotRowInventory();
                    for (int j = 0; j < 4; j++)
                    {
                        CreateNewCharacterSlotAtCharacterSlotRowInventory(row);
                    }
                }
                int numberOfLastSlots = numberOfOwnedCharacters % 4;
                var lastRow = CreateNewCharacterSlotRowInventory();
                for (int i = 0; i < numberOfLastSlots; i++)
                {
                    CreateNewCharacterSlotAtCharacterSlotRowInventory(lastRow);
                }
            }
        }
        CreateCharacterSlotsForUserForSelection(address, fullBalance,onComplete);

    }
    [SerializeField]
    Sprite defaultIconForUnfinishedCharacters;
    [SerializeField]
    Button btn_server;
    public void EnableButtons()
    {
        btn_inventory.interactable = true;
        btn_connect.interactable = true;
        btn_hostAndPlay.interactable = true;
        btn_server.interactable = true;
    }
    public void PopulateCharacterSlotsInventory()
    {
        while (fullBalance == null) CreateCharacterSlotsForUser(AccountCreationManager.Instance.UserAccount.Address);


        int nextSlotIndex = 0;
        for (int i = 0; i < fullBalance.Values.Count; i++)
        {
            BigInteger balance = fullBalance.Values[i];
            if (balance > 0)
            {
                int id = i;
                string name = NekoCharacterNames.charactersNames[id];
                Sprite icon = NekoCharacterNames.LoadIconSpriteForId(id);
                if (icon == null) { icon = defaultIconForUnfinishedCharacters; print("THIS IS BED MEN"); };
                characterSlotsInventory[nextSlotIndex++].GetComponent<CharacterSlot>().Initialize(id, icon, name, balance.ToString(), true);

            }
        }
    }
    private int NumberOfOwnedCharacters()
    {
        int count = 0;
        for (int i = 0; i < fullBalance.Values.Count; i++)
        {
            BigInteger balance = fullBalance.Values[i];
            if (balance > 0)
            {
                count++;
            }
        }
        return count;
    }

    ////////////////////////////////////////////////////////////////////////////////////
    ///
    [SerializeField]
    private Transform contentSelection;
    List<GameObject> characterSlotsSelection = new List<GameObject>();

    List<GameObject> characterSlotRowsSelection = new List<GameObject>();

    public void ClearAllSelection()
    {
        foreach (var key in characterSlotRowsSelection)
        {
            Destroy(key);
        }
        characterSlotRowsSelection.Clear();
        characterSlotsSelection.Clear();
    }

    public GameObject CreateNewCharacterSlotRowSelection()
    {
        GameObject row = Instantiate(characterSlotRow, contentSelection);
        characterSlotRowsSelection.Add(row);
        return row;
    }
    public GameObject CreateNewCharacterSlotAtCharacterSlotRowSelection(GameObject characterSlotRow)
    {
        GameObject slot = Instantiate(chraracterSlot, characterSlotRow.transform);
        characterSlotsSelection.Add(slot);
        CharacterSelectionManager.Instance.OnSelectionSlotCreated(slot.GetComponent<CharacterSlot>());
        return slot;
    }



    public void CreateCharacterSlotsForUserForSelection(string address, NethereumManager.Uint256Array fullBalanceArgument,Action onComplete=null)
    {
        fullBalance = fullBalanceArgument;
        var numberOfOwnedCharacters = NumberOfOwnedCharacters();
        if (numberOfOwnedCharacters < 2)
        {
            var row = CreateNewCharacterSlotRowSelection();
            for (int i = 0; i < numberOfOwnedCharacters; i++)
            {
                CreateNewCharacterSlotAtCharacterSlotRowSelection(row);
            }
        }
        else
        {
            int numberOfRows = 0;
            if (numberOfOwnedCharacters % 2 == 0)
            {
                numberOfRows = numberOfOwnedCharacters / 2;
                for (int i = 0; i < numberOfRows; i++)
                {
                    var row = CreateNewCharacterSlotRowSelection();
                    for (int j = 0; j < 2; j++)
                    {
                        CreateNewCharacterSlotAtCharacterSlotRowSelection(row);
                    }
                }
            }
            else
            {
                numberOfRows = numberOfOwnedCharacters / 2 + 1;
                for (int i = 0; i < numberOfRows - 1; i++)
                {
                    var row = CreateNewCharacterSlotRowSelection();
                    for (int j = 0; j < 2; j++)
                    {
                        CreateNewCharacterSlotAtCharacterSlotRowSelection(row);
                    }
                }
                int numberOfLastSlots = numberOfOwnedCharacters % 2;
                var lastRow = CreateNewCharacterSlotRowSelection();
                for (int i = 0; i < numberOfLastSlots; i++)
                {
                    CreateNewCharacterSlotAtCharacterSlotRowSelection(lastRow);
                }
            }
        }
        if (onComplete != null) onComplete.Invoke();
    }

    public void PopulateCharacterSlotsSelection()
    {
        int nextSlotIndex = 0;
        if (fullBalance == null) return;
        for (int i = 0; i < fullBalance.Values.Count; i++)
        {
            BigInteger balance = fullBalance.Values[i];
            if (balance > 0)
            {
                int id = i;
                string name = NekoCharacterNames.charactersNames[id];
                Sprite icon = NekoCharacterNames.LoadFullSpriteForId(id);
                if (icon == null) { icon = defaultIconForUnfinishedCharacters; print("THIS IS BED MEN"); };
                characterSlotsSelection[nextSlotIndex++].GetComponent<CharacterSlot>().Initialize(id, icon, name, balance.ToString(), false);
            }
        }

    }
}
