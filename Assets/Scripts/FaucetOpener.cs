using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaucetOpener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        Application.OpenURL("https://faucet.polygon.technology/");
    }
}
