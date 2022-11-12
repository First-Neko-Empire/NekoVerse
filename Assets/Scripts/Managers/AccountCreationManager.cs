using NBitcoin;
using Nethereum.HdWallet;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountCreationManager : Singleton<AccountCreationManager>
{

    private Mnemonic userMnemonic;
    private Wallet userWallet;
    private Account userAccount;

    public Account UserAccount
    {
        get { return userAccount; }
        private set { userAccount = value; }
    }

    public Wallet UserWallet
    {
        get { return userWallet; }
        private set { userWallet = value; }
    }

    public Mnemonic UserMnemonic
    {
        get { return userMnemonic; }
        private set { userMnemonic = value; }
    }

    public string UserAccountAddress
    {
        get { if (userAccount == null) return "NULL"; else return userAccount.Address; }
    }
    public string UserAccountPrivateKey
    {
        get { if (userAccount == null) return "NULL"; else return userAccount.PrivateKey; }
    }

    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    public async void LoadUserFromPrefs(string userPassword)
    {
        try
        {
            string privateKey = StringCipher.Decrypt(PlayerPrefs.GetString("ENCRYPTED_PRIVATE_KEY"), userPassword);
            string mnemonic = StringCipher.Decrypt(PlayerPrefs.GetString("ENCRYPTED_MNEMNIC"), userPassword);
            ApplicationManager.Instance.print(PlayerPrefs.GetString("ENCRYPTED_PRIVATE_KEY"));
            ApplicationManager.Instance.print(PlayerPrefs.GetString("ENCRYPTED_MNEMNIC"));

            print(privateKey);
            Mnemonic m = new Mnemonic(mnemonic);
            CanvasManager.Instance.HidePanelLogin();
            CanvasManager.Instance.ShowPanelMainMenu();
            userMnemonic = m;
            userWallet = new Wallet(userMnemonic.ToString(), userPassword);
            userAccount = new Account(privateKey.HexToByteArray());
            NethereumManager.Instance.InitializeOnBehalf(UserAccount);
            CharacterSlotsManager.Instance.CreateCharacterSlotsForUser(UserAccount.Address);
            ApplicationManager.Instance.HasUserLogedIn = true;
            MainMenuPanelManager.Instance.UpdateUserAddress(userAccount.Address);
            if (await NethereumManager.Instance.Newbie_IsMinted(UserAccountAddress))
            {
                MainMenuPanelManager.Instance.MakeNewbieRollNotInteractable();
            }
        }
        catch (Exception e)
        {
            print(e);
            CanvasManager.Instance.ShowWrongPassword();
        }
    }
    public void OnDoneButtonPressed(string userPassword)
    {
        if (userAccount != null) { print("Account already created."); return; };
        userMnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
        userWallet = new Wallet(userMnemonic.ToString(), userPassword);
        userAccount = userWallet.GetAccount(0);
        NethereumManager.Instance.InitializeOnBehalf(UserAccount);
        CharacterSlotsManager.Instance.CreateCharacterSlotsForUser(UserAccount.Address);
        MainMenuPanelManager.Instance.UpdateUserAddress(userAccount.Address);
        SaveUserToPrefs(userPassword);
    }

    private void SaveUserToPrefs(string userPassword)
    {
        string encryptedPrivateKey = StringCipher.Encrypt(UserAccount.PrivateKey, userPassword);
        PlayerPrefs.SetString("ENCRYPTED_PRIVATE_KEY", encryptedPrivateKey);
        string encryptedMnemonic = StringCipher.Encrypt(userMnemonic.ToString(), userPassword);
        PlayerPrefs.SetString("ENCRYPTED_MNEMNIC", encryptedMnemonic);
    }

    public void OnMnemonicImported(string mnemonic,int accountIndex)
    {
        string firstWordOfMnemonic = mnemonic.Split(" ")[0];
        userMnemonic = new Mnemonic(mnemonic);
        userWallet = new Wallet(userMnemonic.ToString(), "");
        userAccount = userWallet.GetAccount(accountIndex);
        NethereumManager.Instance.InitializeOnBehalf(UserAccount);
        CharacterSlotsManager.Instance.CreateCharacterSlotsForUser(UserAccount.Address);
        MainMenuPanelManager.Instance.UpdateUserAddress(userAccount.Address);
        print(UserAccount.Address);
        SaveUserToPrefs(firstWordOfMnemonic);
    }
}
