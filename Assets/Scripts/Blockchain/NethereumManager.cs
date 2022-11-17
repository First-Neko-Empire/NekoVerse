using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using System;
using System.Diagnostics;
using BigInteger = System.Numerics.BigInteger;
using TMPro;
using System.Runtime.CompilerServices;

public class NethereumManager : Singleton<NethereumManager>
{
    [SerializeField]
    TextMeshProUGUI txt_maticPrice;


    [FunctionOutput]
    public class LatestRoundData : FunctionOutputDTO
    {
        [Parameter("uint80","roundId",1)]
        public BigInteger RoundId { get; set; }
        [Parameter("int256","answer",2)]
        public BigInteger Answer { get; set; }
        [Parameter("uint256","startedAt",3)]
        public BigInteger StartedAt { get; set; }
        [Parameter("uint256","updatedAt",4)]
        public BigInteger UpdatedAt{ get; set; }
        [Parameter("uint80","answeredInRound",5)]
        public BigInteger AnsweredInRound { get; set; }


        public override string ToString()
        {
            return Answer.ToString();
        }
    }


    [FunctionOutput]
    public class Uint256Array : FunctionOutputDTO
    {
        [Parameter("uint256[]", "", 1)]
        public List<BigInteger> Values { get; set; }

        public override string ToString()
        {
            int index = 0;
            StringBuilder sb = new StringBuilder("");
            foreach (BigInteger value in Values)
            {
                sb.Append(index + ":");
                sb.Append(value);
                sb.AppendLine();
                index++;
            }
            return sb.ToString();
        }
        public bool CompareTo(Uint256Array comparee)
        {
            for (int i = 0; i < Values.Count; i++)
            {
                if (!Values[i].Equals(comparee.Values[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }


    Stopwatch balanceUpdater = new Stopwatch();



    Web3 web3;
    Contract charactersContract;
    Contract starterKitContract;
    Contract roll;
    Contract priceFeed;

    private BigInteger userBalance;
    public BigInteger UserBalance {get{ return userBalance; }}

    private BigInteger decimals;
    private string formatedPrice;


    [SerializeField]
    TextMeshProUGUI txt_valuevalue;
    [SerializeField]
    TextMeshProUGUI txt_valueBalance;
    [SerializeField]
    GameObject txt_unreachable;
    [SerializeField]
    GameObject txt_connected;

    private void Start()
    {
        balanceUpdater.Start();
    }

    public async Task<Uint256Array> GetBalanceByAddress(string address)
    {
        return await charactersContract.GetFunction("getBalanceByAddress").CallAsync<Uint256Array>(address);
    }
    public async Task<BigInteger> GetMaticDecimals()
    {
        return await priceFeed.GetFunction("decimals").CallAsync<BigInteger>();
    }
    public async Task<LatestRoundData> GetLatestRoundData()
    {
        return await priceFeed.GetFunction("latestRoundData").CallAsync<LatestRoundData>();
    }
    public async void TryChangeEndPoint(string newEndpoint)
    {
        BigInteger trustedBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        BigInteger untrustedBlockNumber;
        try
        {
            Web3 web3tester = new Web3(AccountCreationManager.Instance.UserAccount, newEndpoint);
            untrustedBlockNumber = await web3tester.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            if(untrustedBlockNumber >= trustedBlockNumber && untrustedBlockNumber < trustedBlockNumber + 1)
            {
                ShowConnectedText();
                web3 = new Web3(AccountCreationManager.Instance.UserAccount, newEndpoint);
                web3.TransactionManager.UseLegacyAsDefault = true;
            }
            else
            {
                ShowUnreachableText();
            }
        }
        catch(Exception e)
        {
            ShowUnreachableText();
        }

    }

    async void ShowConnectedText()
    {
        txt_unreachable.SetActive(false);
        txt_connected.SetActive(true);
    }

    async void ShowUnreachableText()
    {
        if (!txt_unreachable.activeSelf)
        {
            txt_unreachable.SetActive(true);
            await Task.Delay(3000);
            txt_unreachable.SetActive(false);
        }
    }
    public async void InitializeNormal()
    {
        web3 = new Web3(Dotenv.endpoint);
        web3.TransactionManager.UseLegacyAsDefault = true;
        charactersContract = web3.Eth.GetContract(ABIs.characters, Addresses.characters);
        starterKitContract = web3.Eth.GetContract(ABIs.starterKit, Addresses.starterKit);
        roll = web3.Eth.GetContract(ABIs.roll, Addresses.roll);
        priceFeed = web3.Eth.GetContract(ABIs.priceFeed, Addresses.priceFeed);
        decimals = await GetMaticDecimals();
        UpdateMaticPrice();

    }
    public async void InitializeOnBehalf(Account userAccount)
    {
        web3 = new Web3(userAccount, Dotenv.endpoint);
        BalanceOf(userAccount.Address);

        web3.TransactionManager.UseLegacyAsDefault = true;
        charactersContract = web3.Eth.GetContract(ABIs.characters, Addresses.characters);
        starterKitContract = web3.Eth.GetContract(ABIs.starterKit, Addresses.starterKit);
        roll = web3.Eth.GetContract(ABIs.roll, Addresses.roll);
        priceFeed = web3.Eth.GetContract(ABIs.priceFeed, Addresses.priceFeed);
        decimals = await GetMaticDecimals();
        UpdateMaticPrice();


    }
    public async void SelfMint(string address, BigInteger id, BigInteger amount)
    {
        TransactionInput tx = new TransactionInput()
        {
            From = web3.Eth.TransactionManager.Account.Address,
            To = Addresses.characters,
            Gas = new HexBigInteger(199999),
            Type = new HexBigInteger(2),
            Data = Characters_EncodeABI("mint", address, id, amount),
        };
        var txRcpt = await web3.TransactionManager.SendTransactionAndWaitForReceiptAsync(tx,null);
        print("Transaction Rcpt: " + txRcpt.TransactionHash);
    }
    public async void requestRandomCharacter()
    {
        TransactionInput tx = new TransactionInput()
        {
            From = web3.Eth.TransactionManager.Account.Address,
            To = Addresses.roll,
            Gas = new HexBigInteger(199999),
            Type = new HexBigInteger(2),
            Data = roll.GetFunction("requestRandomCharacter").GetData(),
            Value = new HexBigInteger(new BigInteger(100000000000000000))
        };
        CharacterRollProcessor.Instance.CheckNewCharacters(web3.TransactionManager.SendTransactionAndWaitForReceiptAsync(tx,null));
    }
    public async Task<HexBigInteger> requestRandomCharacterEstimateGas()
    {
        TransactionInput tx = new TransactionInput()
        {
            From = web3.Eth.TransactionManager.Account.Address,
            To = Addresses.roll,
            Gas = new HexBigInteger(199999),
            Type = new HexBigInteger(2),
            Data = roll.GetFunction("requestRandomCharacter").GetData(),
            Value = new HexBigInteger(new BigInteger(100000000000000000))
        };
        return await web3.TransactionManager.EstimateGasAsync(tx);
    }
    public async void OpenStarterKit()
    {
        TransactionInput tx = new TransactionInput()
        {
            From = web3.Eth.TransactionManager.Account.Address,
            To = Addresses.starterKit,
            Gas = new HexBigInteger(199999),
            Type = new HexBigInteger(2),
            Data = starterKitContract.GetFunction("openStarterKit").GetData()
        };
        CharacterRollProcessor.Instance.CheckNewCharacters(web3.TransactionManager.SendTransactionAndWaitForReceiptAsync(tx, null));
    }

    public async Task<HexBigInteger> OpenStartedKitEstimateGas()
    {
        TransactionInput tx = new TransactionInput()
        {
            From = web3.Eth.TransactionManager.Account.Address,
            To = Addresses.starterKit,
            Gas = new HexBigInteger(199999),
            Type = new HexBigInteger(2),
            Data = starterKitContract.GetFunction("openStarterKit").GetData()
        };
        return await web3.TransactionManager.EstimateGasAsync(tx);
    }


    private string Characters_EncodeABI(string functionName, params object[] inputData)
    {
        var functionCallEncoder = new FunctionCallEncoder();
        var data = charactersContract.GetFunction(functionName).GetData(inputData);
        var inputParams = Characters_GetFunctionParameters(functionName); 
        return functionCallEncoder.EncodeRequest(data,inputParams,inputData);
    }
    public async Task<bool> Newbie_IsMinted(string address)
    {
        return await starterKitContract.GetFunction("isMinted").CallAsync<bool>(address);
    }
    private Parameter[] Characters_GetFunctionParameters(string functionName)
    {
        switch (functionName)
        {
            case "mint":return new[] { new Parameter("address","account"), new Parameter("uint256", "id"), new Parameter("uint256", "amount") };
            default:
                print("Wrong function name.");
                return null;
        }
    }
    public async Task<BigInteger> BalanceOf(string address)
    {
        userBalance = await web3.Eth.GetBalance.SendRequestAsync(address);
        MainMenuPanelManager.Instance.OnBalanceLoaded((String.Format("{0:F20}",(double)userBalance/1000000000000000000)).ToString());
        return userBalance;
    }
    public async Task<string> SendCrypto(string toAddress,decimal amount)
    {
        string transactionHash = "NULL";
        try
        {
            transactionHash = await web3.Eth.GetEtherTransferService()
                    .TransferEtherAsync(toAddress, amount, 2);
        }
        catch (Exception e)
        {
            print(e);
            MainMenuPanelManager.Instance.ShowCantSendCrypto();
        }
        return transactionHash;
    }


    public bool IsValidAddress(string address)
    {
        return new Nethereum.Util.AddressUtil().IsValidEthereumAddressHexFormat(address);
    }



    public async void UpdateMaticPrice()
    {
        if (this)
        {
            if (priceFeed == null) return;
            var price = await GetLatestRoundData();
            txt_maticPrice.text = "1 MATIC = " + FormatPrice(((float)price.Answer / (float)Math.Pow(10, (int)decimals)).ToString()) + "$";
            if (this)
            {
                var formatedBalance = FormatBalance();
                try {
                    txt_valuevalue.text = (float.Parse(formatedPrice) * float.Parse(formatedBalance)).ToString();
                }
                catch(Exception e)
                {

                }
                }
        }
    }

    string FormatBalance()
    {
        if (this)
        {
            string userBalanceString = txt_valueBalance.text;

            int numCount = 0;
            StringBuilder sb = new StringBuilder("");
            if (userBalanceString == null) return "";
            for (int i = 0; i < userBalanceString.Length; i++)
            {
                if (userBalanceString[i] != '.')
                {
                    numCount++;
                    sb.Append(userBalanceString[i]);
                }
                else
                {
                    sb.Append(".");
                    for (int j = 2; j < 4; j++)
                    {
                        sb.Append(userBalanceString[j]);
                    }
                    return sb.ToString();
                }
            }
            return "ERROR";
        }
        return "";
    }
    string FormatPrice(string price)
    {
        int numCount = 0;
        StringBuilder sb = new StringBuilder("");
        for(int i=0;i<price.Length;i++)
        {
            if (price[i] == '.')
            {
                break;
            }
            else
            {
                numCount++;
            }
        }
        for (int i = 0; i < numCount - 3; i++)
        {
            sb.Append(price[i]);
        }
        sb.Append('.');
        for (int i = numCount - 3; i < numCount-1; i++)
        {
            sb.Append(price[i]);
        }
        formatedPrice = sb.ToString();
        return sb.ToString();
    }

    async void Update()
    {
        if (balanceUpdater.ElapsedMilliseconds > 7000)
        {
            UpdateMaticPrice();
            if (AccountCreationManager.Instance.UserAccount != null)
                BalanceOf(AccountCreationManager.Instance.UserAccount.Address);
            balanceUpdater.Stop();
            balanceUpdater.Restart();
            balanceUpdater.Start();

        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print(await GetMaticDecimals());
            //print(await GetLatestRoundData());
            //var gas = await requestRandomCharacterEstimateGas();
            //print(gas);
            //print(gas.ToUlong());
            //print(gas.Value);
            //print((await OpenStartedKitEstimateGas()).ToUlong());
            //requestRandomCharacter();
            //OpenStarterKit();
            //SelfMint("0x6a04fac827DF78F652306cD547c6D90118F150CF", UnityEngine.Random.Range(0, 30), UnityEngine.Random.Range(1, 4));
            //print("Balance: "+ await BalanceOf("0x6a04fac827DF78F652306cD547c6D90118F150CF"));
        }
    }

    //1.637.84616274
}
