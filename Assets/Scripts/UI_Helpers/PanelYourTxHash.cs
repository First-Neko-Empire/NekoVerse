using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class PanelYourTxHash : Singleton<PanelYourTxHash>
{
    [SerializeField]
    private TextMeshProUGUI txt_txHash;
    [SerializeField]
    private Button btn_OpenLink;

    [SerializeField]
    private string currentTxHash = "TEST";
    public void OnExitButtonPressed()
    {
        gameObject.SetActive(false);
    }

    public void OnCopyButtonPressed()
    {
        GUIUtility.systemCopyBuffer = "https://mumbai.polygonscan.com/tx/"+ currentTxHash;
    }

    public void PopulatePanel(string txHash)
    {
        txt_txHash.text = "https://mumbai.polygonscan.com/tx/" + txHash;
        btn_OpenLink.onClick.RemoveAllListeners();
        btn_OpenLink.onClick.AddListener(() =>
        {
            ApplicationManager.Instance.OpenLink("https://mumbai.polygonscan.com/tx/" + txHash);
        });
    }
}
