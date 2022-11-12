using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MnemonicWord : MonoBehaviour
{
    private TextMeshProUGUI txt_word;
    private void Awake()
    {
        txt_word = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void SetWord(string word)
    {
        txt_word.text = word;
    }
}
