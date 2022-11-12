using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekoCharacterPrefabs : Singleton<NekoCharacterPrefabs>
{
    [SerializeField]
    private List<GameObject> prefabs = new List<GameObject>();


    public GameObject GetPrefabWithId(int id)
    {
        if (prefabs[id] == null)
        {
            print("NULL CHARACTER.");
                return null;
        }
        return prefabs[id];
    }
}
