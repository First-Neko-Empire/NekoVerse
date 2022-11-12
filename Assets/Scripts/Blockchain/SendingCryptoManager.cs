using Nethereum.Web3;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendingCryptoManager : Singleton<SendingCryptoManager>
{
    [SerializeField]
    private TMP_InputField if_amount;
    [SerializeField]
    private TMP_InputField if_to;



    public async void OnSendButtonPressed()
    {
        if (NethereumManager.Instance.IsValidAddress(if_to.text))
        {
            string hash = await NethereumManager.Instance.SendCrypto(if_to.text, decimal.Parse(if_amount.text));
            if (hash != "NULL")
            {
                MainMenuPanelManager.Instance.ShowPanelYourTxHash();
                PanelYourTxHash.Instance.PopulatePanel(hash);
                if_amount.text = "";
                if_to.text = "";
                print(hash);
            }
        }
        else
        {
            print("Invalid TO address.");
        }
        MainMenuPanelManager.Instance.HideSendCryptoPanel();
    }
}
