using Nethereum.Web3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NethereumManager : Singleton<NethereumManager>
{
    Web3 web3;

    private async new void Awake()
    {
        base.Awake();
        web3 = new Web3("https://goerli.infura.io/v3/ca671c43f773424da52a9d42c1bd3fc3");
        print("Latest block: " + await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
