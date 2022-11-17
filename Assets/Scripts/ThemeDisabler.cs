using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeDisabler : MonoBehaviour
{
    [SerializeField]
    AudioSource theme;
    private void OnEnable()
    {
        SceneManager.sceneLoaded+= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if(scene.name=="GameWorld")
        {
            StartCoroutine(I_Fade());
        }
    }

    IEnumerator I_Fade()
    {
        while (theme.volume > 0)
        {
            yield return new WaitForSeconds(0.1f);
            theme.volume -= 0.05f;
        }
    }
}
