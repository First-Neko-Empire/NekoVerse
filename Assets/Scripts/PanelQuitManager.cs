using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelQuitManager : Singleton<PanelQuitManager>
{
    [SerializeField]
    GameObject panel_Quit;



    public void ShowPanelQuit()
    {
        panel_Quit.SetActive(!panel_Quit.activeSelf);
    }
    public void HidePanelQuit()
    {
        panel_Quit.SetActive(false);
    }

    public void OnYesButtonPressed()
    {
        ApplicationManager.Instance.QuitGame();
        //NekoVerseNetworkManager.singleton.StopClient();
        //SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void OnNoButtonPressed()
    {
        HidePanelQuit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPanelQuit();
        }
    }
}
