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
    [SerializeField]
    GameObject gameManager;
    


    private bool hasUserLogedIn;

    public bool HasUserLogedIn
    {
        get { return hasUserLogedIn; }
        set { hasUserLogedIn = value; }
    }

    private new void Awake()
    {
        print("pls");
        gameManager.SetActive(true);
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
        if(!HasUserLogedIn)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                CanvasManager.Instance.OnGoButtonPressed();
            }
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
