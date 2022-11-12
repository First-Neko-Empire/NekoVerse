using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBehaviourSingleton<T> : NetworkBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    protected void Awake()
    {
        if (instance != null && instance != this as T)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this as T;
        //DontDestroyOnLoad(gameObject);
    }
}