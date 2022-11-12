using NBitcoin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountImportingManager : Singleton<AccountImportingManager>
{
    [SerializeField]
    private TMP_InputField if_mnemonic;
    [SerializeField]
    private GameObject txt_isValidMnemonic;

    [SerializeField]
    private TMP_InputField if_accIndex;
   
    public async void OnContinueButtonPressed()
    {
        try
        {
            Mnemonic m = new Mnemonic(if_mnemonic.text);
            CanvasManager.Instance.HideAccountImporting();
            CanvasManager.Instance.ShowPanelMainMenu();
            AccountCreationManager.Instance.OnMnemonicImported(m.ToString(),int.Parse(if_accIndex.text));
            MainMenuPanelManager.Instance.ShowNextTimeYouLogin();
        }
        catch (Exception e)
        {
            print(e);
            if (!txt_isValidMnemonic.activeSelf)
            {
                txt_isValidMnemonic.SetActive(true);
                await Task.Delay(3000);
            }
            txt_isValidMnemonic.SetActive(false);
        }
    }
}
