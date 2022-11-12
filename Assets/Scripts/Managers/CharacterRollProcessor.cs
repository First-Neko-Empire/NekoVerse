using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterRollProcessor : Singleton<CharacterRollProcessor>
{
    [SerializeField]
    private GameObject txt_processing;
    [SerializeField]
    private GameObject img_processing;

    bool stopBalanceCheck = false;
    bool informAboutNewChar = false;

    Coroutine balanceChecker;

    private void Start()
    {
    }
    public async void CheckNewCharacters(Task<TransactionReceipt> txRcpTask)
    {
        if (stopBalanceCheck)
        {

        }
        stopBalanceCheck = false;
        CheckNewBalance();
        var data = await txRcpTask;
        print(data.TransactionHash);
        if (data.Failed())
        {
            HideProccessingTransaction();
            stopBalanceCheck = true;
        }

    }

    public void HideProccessingTransaction()
    {
        txt_processing.SetActive(false);
        img_processing.SetActive(false);
    }
    public void ShowProcessingTransaction()
    {
        txt_processing.SetActive(true);
        img_processing.SetActive(true);
    }


    private void CheckNewBalance()
    {
        if (balanceChecker != null)
        {
            StopCoroutine(balanceChecker);
            balanceChecker = null;
        }
        balanceChecker = StartCoroutine(I_CheckNewBalance());
    }

    IEnumerator I_CheckNewBalance()
    {
        string address = AccountCreationManager.Instance.UserAccountAddress;
        var prevFullBalance = CharacterSlotsManager.Instance.FullBalance;
        if (!stopBalanceCheck)
        {
            var task = NethereumManager.Instance.GetBalanceByAddress(address);
            yield return new WaitUntil(() => task.IsCompleted);
            var newFullBalance =  task.Result;
            if (prevFullBalance.CompareTo(newFullBalance))
            {
                yield return new WaitForSeconds(2.5f);
                CheckNewBalance();
            }
            else
            {
                print("GOT NEW CHARACTER");
                HideProccessingTransaction();
                CharacterSlotsManager.Instance.UpdateWithNewBalance();
                StopCoroutine(balanceChecker);
                balanceChecker = null;
            }
        }

    }

    private void Update()
    {

        if (img_processing.activeSelf)
        {
            img_processing.transform.Rotate(0, 0, 3);
        }
    }

}
