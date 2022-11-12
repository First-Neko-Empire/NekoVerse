using Org.BouncyCastle.Asn1;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField]
    private GameObject passwordCreation;
    [SerializeField]
    private GameObject createImportAccountButtons;
    [SerializeField]
    private GameObject accountImporting;
    [SerializeField]
    private GameObject mnemonic;
    [SerializeField]
    private GameObject showMnemonicAndPrivateKey;
    [SerializeField]
    private TextMeshProUGUI txt_privateKey;
    [SerializeField]
    private GameObject panelMainMenu;
    [SerializeField]
    private GameObject panelCreateImportWallet;
    [SerializeField]
    private GameObject panelLogin;
    [SerializeField]
    private TMP_InputField if_password;
    [SerializeField]
    private GameObject txt_wrongPassword;
    [SerializeField]
    private GameObject panel_charSelection;
    [SerializeField]
    private GameObject txt_NoCharsInventory;
    [SerializeField]
    private GameObject txt_YouAreHost;
    [SerializeField]
    private GameObject txt_hostInfo;
    [SerializeField]
    private NekoVerseNetworkManager verseNetworkManager;

    public void PopulateHostInfo()
    {
        txt_hostInfo.GetComponent<TextMeshProUGUI>().text = "Address: \n" + verseNetworkManager.GetAddress() + "\n\n Port: \n"+verseNetworkManager.GetPort();
    }
    public void ShowHostInfo()
    {
        txt_hostInfo.SetActive(true);
        PopulateHostInfo();
    }
    public void HideHostInfo()
    {
        txt_hostInfo.SetActive(false);
    }
    public void ShowYouAreHost()
    {
        txt_YouAreHost.SetActive(true);
    }
    public void HideYouAreHost()
    {
        txt_YouAreHost.SetActive(false);
    }

    private void Start()
    {
    }
    public void ShowCharSelection()
    {
        panel_charSelection.SetActive(true);
    }
    public void HideCharSelection()
    {
        panel_charSelection.SetActive(false);
    }
    public void ShowNoChars()
    {
        txt_NoCharsInventory.SetActive(true);
    }
    public void HideNoChars()
    {
        txt_NoCharsInventory.SetActive(false);
    }
    public void OnConnectButtonPressed()
    {
        CharacterSlotsManager.Instance.PopulateCharacterSlotsSelection();
        ShowCharSelection();
        HidePanelMainMenu();
        CharacterSelectionManager.Instance.SelectFirstCharacter();

    }
    public async void ShowWrongPassword()
    {
        if (txt_wrongPassword.activeSelf) return;
        txt_wrongPassword.SetActive(true);
        await Task.Delay(3000);
        HideWrongPassword();
    }
    public void HideWrongPassword()
    {
        txt_wrongPassword.SetActive(false);
    }
    public void OnButtonCreateAccountInsteadPressed()
    {
        HidePanelLogin();
        ShowPanelCreateImportWallet();
    }
    public void OnGoButtonPressed()
    {
        AccountCreationManager.Instance.LoadUserFromPrefs(if_password.text);
    }
    public void ShowPanelLogin()
    {
        panelLogin.SetActive(true);
    }
    public void HidePanelLogin()
    {
        panelLogin.SetActive(false);
    }

    public void ShowPanelCreateImportWallet()
    {
        panelCreateImportWallet.SetActive(true);
    }
    public void HidePanelCreateImportWallet()
    {
        panelCreateImportWallet.SetActive(false);
    }

    public void OnCreateAccountButtonPressed()
    {
        ShowPasswordCreation();
        HideCreateImportAccountButtons();
    }
    public void OnDoneButtonPressed()
    {
        string userPassword = PasswordCreationManager.Instance.UserPassword;
        AccountCreationManager.Instance.OnDoneButtonPressed(userPassword);
        HidePasswordCreation();
        ShowShowMnemonicAndPrivateKey();
        SetMnemonicWords(AccountCreationManager.Instance.UserMnemonic.ToString());
        SetPrivateKey(AccountCreationManager.Instance.UserAccountPrivateKey);
    }
    public void OnImportAccountButtonPressed()
    {
        HideCreateImportAccountButtons();
        ShowAccountImporting();
    }

    public void ShowPanelMainMenu()
    {
        panelMainMenu.SetActive(true);
    }
    public void HidePanelMainMenu()
    {
        panelMainMenu.SetActive(false);
    }

    public void OnContinueButtonPressed()
    {
        HideShowMnemonicAndPrivateKey();
        ShowPanelMainMenu();
        MainMenuPanelManager.Instance.ShowPlay();
    }

    public void ShowShowMnemonicAndPrivateKey()
    {
        showMnemonicAndPrivateKey.SetActive(true);
    }
    public void HideShowMnemonicAndPrivateKey()
    {
        showMnemonicAndPrivateKey.SetActive(false);
    }
    public void ShowAccountImporting()
    {
        accountImporting.SetActive(true);
    }
    public void HideAccountImporting()
    {
        accountImporting.SetActive(false);
    }
    public void ShowPasswordCreation()
    {
        passwordCreation.SetActive(true);
    }
    public void HidePasswordCreation()
    {
        passwordCreation.SetActive(false);
    }
    public void ShowCreateImportAccountButtons()
    {
        createImportAccountButtons.SetActive(true);
    }
    public void HideCreateImportAccountButtons()
    {
        createImportAccountButtons.SetActive(false);
    }
    public void SetPrivateKey(string privateKey)
    {
        txt_privateKey.text = privateKey;
    }
    public void SetMnemonicWords(string mnemonic)
    {
        string[] mnemonicWords = mnemonic.Split(" ");
        int wordIndex = 0;
        foreach (Transform t in this.mnemonic.transform)
        {
            t.GetComponent<MnemonicWord>().SetWord(mnemonicWords[wordIndex++]);
        }
    }
}
