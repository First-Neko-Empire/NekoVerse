using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : Singleton<Managers>
{
    [SerializeField]
    List<GameObject> destroyableManagers;
    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameWorld")
        {
            DestroyUsslessManagers();
        }
    }

    void DestroyUsslessManagers()
    {
        foreach(var obj in destroyableManagers)
        {
            Destroy(obj.gameObject);
        }
    }
}
