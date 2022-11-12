using NBitcoin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationManager : Singleton<ApplicationManager>
{
    [SerializeField]
    Image img;
    [SerializeField]
    TextMeshProUGUI txt;
    

    private bool hasUserLogedIn;

    public bool HasUserLogedIn
    {
        get { return hasUserLogedIn; }
        set { hasUserLogedIn = value; }
    }

    private new void Awake()
    {
        base.Awake();
        NekoCharacterNames.Initialize();
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("ENCRYPTED_PRIVATE_KEY"))
        {
            CanvasManager.Instance.HidePanelCreateImportWallet();
            CanvasManager.Instance.ShowPanelLogin();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            img.sprite = NekoCharacterNames.LoadFullSpriteForId(2);
        }
        if(!HasUserLogedIn)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                CanvasManager.Instance.OnGoButtonPressed();
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            txt.gameObject.SetActive(!txt.gameObject.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    public void print(string text)
    {
        txt.text += text + "\n\n";
    }
    public void resetPrint()
    {
        txt.text = "";
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
