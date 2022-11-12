using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanelManager : Singleton<MainMenuPanelManager>
{
    public enum CURRENT_SELECTED_ROLL
    {
        NONE,HEAVENS,CHARACTER,NEWBEE,ITEMS,WEAPONS
    }

    [SerializeField]
    private GameObject txt_connected;
    [SerializeField]
    private Button btn_Play;
    [SerializeField]
    private Button btn_Wallet;
    [SerializeField]
    private Button btn_Inventory;
    [SerializeField]
    private Button btn_Fortune;
    [SerializeField]
    private Button btn_Options;
    [SerializeField]
    private Button btn_exit;
    [SerializeField]
    private GameObject play;
    [SerializeField]
    private GameObject wallet;
    [SerializeField]
    private GameObject inventory;
    [SerializeField]
    private GameObject fortune;
    [SerializeField]
    private GameObject options;
    [SerializeField]
    private GameObject panelSendCrypto;
    [SerializeField]
    private TextMeshProUGUI txt_Balance;
    [SerializeField]
    private TextMeshProUGUI txt_currentTxHash;
    [SerializeField]
    private TextMeshProUGUI txt_myCharacters;
    [SerializeField]
    private TextMeshProUGUI txt_myItems;
    [SerializeField]
    private TextMeshProUGUI txt_myWeapons;
    [SerializeField]
    private GameObject nextTimeYouLogin;
    [SerializeField]
    private TextMeshProUGUI txt_userAddress;

    [SerializeField]
    private Image img_heavensRoll;
    [SerializeField]
    private Image img_charRoll;
    [SerializeField]
    private Image img_NewbeeRoll;
    [SerializeField]
    private Image img_itemsRoll;
    [SerializeField]
    private Image img_weaponRoll;

    [SerializeField]
    private GameObject txt_processingTx;
    [SerializeField]
    private GameObject img_processingTx;

    [SerializeField]
    private GameObject panel_YourTxHash;
    [SerializeField]
    private GameObject txt_cantSendCrypto;
    [SerializeField]
    private GameObject btn_confirm;
    [SerializeField]
    private GameObject btn_reject;


    [SerializeField]
    private TextMeshProUGUI txt_info;
    [SerializeField]
    private TextMeshProUGUI txt_totalCost;
    [SerializeField]
    private GameObject txt_plsConfirm;
    [SerializeField]
    private Button btn_newbieRoll;
    CURRENT_SELECTED_ROLL currentRoll = CURRENT_SELECTED_ROLL.NONE;
    Color unclickedColor = new Color(0.9921569f, 0.4588235f, 0.1960784f, 0.3f);
    Color clickedColor = new Color(0.9921569f, 0.4588235f, 0.1960784f, 1f);


 
    private void populateTx(string roll,string cost,string gas,string totalCost)
    {
        txt_plsConfirm.SetActive(true);
        txt_info.gameObject.SetActive(true);
        txt_totalCost.gameObject.SetActive(true);
        txt_info.text = "I want: \r\n\t  " + roll + " Roll\r\n\r\nIm paying:\r\n\tcost: "+  cost + "\r\n\tgas: " + gas + "\r\n\r\n";
        txt_totalCost.text = "Total transaction cost:\r\n"+totalCost;
        btn_confirm.SetActive(true);
        btn_reject.SetActive(true);
    }
    public void MakeNewbieRollNotInteractable()
    {
        btn_newbieRoll.interactable = false;
    }

    public void OnConfirmButtonPressed()
    {
        txt_plsConfirm.SetActive(false);
        btn_confirm.SetActive(false);
        btn_reject.SetActive(false);
        txt_totalCost.text = "";
        txt_info.text = "";
        switch (currentRoll)
        {
            case CURRENT_SELECTED_ROLL.NONE:
                break;
            case CURRENT_SELECTED_ROLL.HEAVENS:
                break;
            case CURRENT_SELECTED_ROLL.CHARACTER:
                NethereumManager.Instance.requestRandomCharacter();
                CharacterRollProcessor.Instance.ShowProcessingTransaction();
                break;
            case CURRENT_SELECTED_ROLL.NEWBEE:
                NethereumManager.Instance.OpenStarterKit();
                CharacterRollProcessor.Instance.ShowProcessingTransaction();
                break;
            case CURRENT_SELECTED_ROLL.ITEMS:
                break;
            case CURRENT_SELECTED_ROLL.WEAPONS:
                break;
            default:
                break;
        }
    }

    private void Start()
    {
    }
    public void OnRejectButtonPressed()
    {
        txt_plsConfirm.SetActive(false);
        btn_confirm.SetActive(false);
        btn_reject.SetActive(false);
        txt_totalCost.text = "";
        txt_info.text = "";
    }
    public void OnCopyAddressButtonPressed()
    {
        GUIUtility.systemCopyBuffer = AccountCreationManager.Instance.UserAccount.Address;
    }

    public async void ShowCantSendCrypto()
    {
        if (txt_cantSendCrypto.activeSelf) return;
        txt_cantSendCrypto.SetActive(true);
        await Task.Delay(3000);
        HideCantSendCrypto();
    }
    public void HideCantSendCrypto()
    {
        txt_cantSendCrypto.SetActive(false);
    }
    public void ShowPanelYourTxHash()
    {
        panel_YourTxHash.SetActive(true);
    }
    public void HidePanelYourTxHash()
    {
        panel_YourTxHash.SetActive(false);
    }
    public void UpdateUserAddress(string address)
    {
        txt_userAddress.text = address;
    }
    public void ShowNextTimeYouLogin()
    {
        nextTimeYouLogin.SetActive(true);
    }
    public void HideNextTimeYouLogin()
    {
        nextTimeYouLogin.SetActive(false);
    }


    public async void OnRollButtonPressed()
    {
        switch (currentRoll)
        {
            case CURRENT_SELECTED_ROLL.NONE:
                print("No roll selected.");
                break;
            case CURRENT_SELECTED_ROLL.HEAVENS:
                break;
            case CURRENT_SELECTED_ROLL.CHARACTER:
                var gas1 = (await NethereumManager.Instance.requestRandomCharacterEstimateGas());
                populateTx("Character", 10000000000000000.ToString(), gas1.ToSafeString(), (1000000000000000 + gas1.ToLong()).ToSafeString()); ;
                break;
            case CURRENT_SELECTED_ROLL.NEWBEE:
                var gas2 = (await NethereumManager.Instance.OpenStartedKitEstimateGas());
                populateTx("Newbee", "0", gas2.ToString(), gas2.ToString());
                break;
            case CURRENT_SELECTED_ROLL.ITEMS:
                break; 
            case CURRENT_SELECTED_ROLL.WEAPONS:
                break;
            default:
                break;
        }
    }
    public void OnCharacterRollButtonPressed()
    {
        img_charRoll.color = clickedColor;
        img_heavensRoll.color = unclickedColor;
        img_NewbeeRoll.color = unclickedColor;
        img_itemsRoll.color = unclickedColor;
        img_weaponRoll.color = unclickedColor;
        currentRoll = CURRENT_SELECTED_ROLL.CHARACTER;
    }
    public void OnNewbeeRollButtonPressed()
    {
        img_NewbeeRoll.color = clickedColor;
        img_heavensRoll.color = unclickedColor;
        img_charRoll.color = unclickedColor;
        img_itemsRoll.color = unclickedColor;
        img_weaponRoll.color = unclickedColor;
        currentRoll = CURRENT_SELECTED_ROLL.NEWBEE;
    }
    public void OnHeavensRollButtonPressed()
    {
        img_heavensRoll.color = clickedColor;
        img_charRoll.color = unclickedColor;
        img_NewbeeRoll.color = unclickedColor;
        img_itemsRoll.color = unclickedColor;
        img_weaponRoll.color = unclickedColor;
        currentRoll = CURRENT_SELECTED_ROLL.HEAVENS;
    }
    public void OnItemsRollButtonPressed()
    {
        img_itemsRoll.color = clickedColor;
        img_heavensRoll.color = unclickedColor;
        img_NewbeeRoll.color = unclickedColor;
        img_charRoll.color = unclickedColor;
        img_weaponRoll.color = unclickedColor;
        currentRoll = CURRENT_SELECTED_ROLL.ITEMS;
    }
    public void OnWeaponsRollButtonPressed()
    {
        img_weaponRoll.color = clickedColor;
        img_heavensRoll.color = unclickedColor;
        img_NewbeeRoll.color = unclickedColor;
        img_itemsRoll.color = unclickedColor;
        img_charRoll.color = unclickedColor;
        currentRoll = CURRENT_SELECTED_ROLL.WEAPONS;
    }
    public void ShowTxCurrentHahs()
    {
        txt_currentTxHash.gameObject.SetActive(true);
    }
    public void HideTxCurrentHash()
    {
        txt_currentTxHash.gameObject.SetActive(false);
    }
 
    public void ShowSendCryptoPanel()
    {
        panelSendCrypto.SetActive(true);
    }
    public void HideSendCryptoPanel()
    {
        panelSendCrypto.SetActive(false);   
    }
    public void OnSendButtonPressed()
    {
        panelSendCrypto.SetActive(true);
    }
    public void OnOkayButtonPressed()
    {
        HideNextTimeYouLogin();
    }

    public async void OnRefreshBalanceButtonPressed()
    {
        var userBalance = (await NethereumManager.Instance.BalanceOf(AccountCreationManager.Instance.UserAccount.Address));
        string balanceString = (String.Format("{0:F20}", (double)userBalance / 1000000000000000000)).ToString();
        txt_Balance.text = balanceString.Remove(balanceString.Length-17,17);
    }
    public void OnBalanceLoaded(string balance)
    {
        txt_Balance.text = balance.Remove(balance.Length - 17, 17);
    }
    public void ShowPlay()
    {
        btn_Play.interactable = false;
        btn_Wallet.interactable = true;
        btn_Inventory.interactable = true;
        btn_Fortune.interactable = true;
        btn_Options.interactable = true;
        play.SetActive(true);
        wallet.SetActive(false);
        inventory.SetActive(false);
        fortune.SetActive(false);
        options.SetActive(false);
    }
    public void ShowWallet()
    {
        btn_Play.interactable = true;
        btn_Wallet.interactable = false;
        btn_Inventory.interactable = true;
        btn_Fortune.interactable = true;
        btn_Options.interactable = true;
        play.SetActive(false);
        wallet.SetActive(true);
        inventory.SetActive(false);
        options.SetActive(false);
        fortune.SetActive(false);
    }
    public void ShowInventory()
    {
        btn_Play.interactable = true;
        btn_Wallet.interactable = true;
        btn_Inventory.interactable = false;
        btn_Fortune.interactable = true;
        btn_Options.interactable = true;
        play.SetActive(false);
        wallet.SetActive(false);
        inventory.SetActive(true);
        fortune.SetActive(false);
        options.SetActive(false);
        CharacterSlotsManager.Instance.PopulateCharacterSlotsInventory();
    }
    public void ShowFortune()
    {
        btn_Play.interactable = true;
        btn_Wallet.interactable = true;
        btn_Inventory.interactable = true;
        btn_Fortune.interactable = false;
        btn_Options.interactable = true;
        play.SetActive(false);
        wallet.SetActive(false);
        inventory.SetActive(false);
        fortune.SetActive(true);
        options.SetActive(false);
    }
    public void ShowOptions()
    {
        btn_Play.interactable = true;
        btn_Wallet.interactable = true;
        btn_Inventory.interactable = true;
        btn_Fortune.interactable = true;
        btn_Options.interactable = false;
        play.SetActive(false);
        wallet.SetActive(false);
        inventory.SetActive(false);
        fortune.SetActive(false);
        options.SetActive(true);
        txt_connected.SetActive(false);
    }
}
