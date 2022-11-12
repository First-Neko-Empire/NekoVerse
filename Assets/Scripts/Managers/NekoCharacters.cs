using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum Characters
{
    Kuro, Urania, Asami, Darkiel, Neva, Slava, Caio, Eirina, Gweyir, Anna,
    Elissa, Eileen, Aryna, Rokorid, Molly, Haiku, Sara, Rurdeth,
    Vulen, Nina, Edea, Thessalia, Viessa, Yuna, Eorwyn, Elaiza,
    Sphhie, Ruth, Darka, Ramiel
}

public static class NekoCharacterNames 
{
    private static bool isInitialized = false;
    public static Dictionary<int,string> charactersNames = new Dictionary<int,string>();



    public static void Initialize()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            charactersNames.Add(0,  "Darkiel");
            charactersNames.Add(1,  "Urania");
            charactersNames.Add(2,  "Neva");
            charactersNames.Add(3,  "Slava");
            charactersNames.Add(4,  "Asami");
            charactersNames.Add(5,  "Caio");
            charactersNames.Add(6,  "Eirina");
            charactersNames.Add(7,  "Gweyir");
            charactersNames.Add(8,  "Anna");
            charactersNames.Add(9,  "Elissa");
            charactersNames.Add(10, "Eileen");
            charactersNames.Add(11, "Aryna");
            charactersNames.Add(12, "Rokorid");
            charactersNames.Add(13, "Molly");
            charactersNames.Add(14, "Haiku");
            charactersNames.Add(15, "Sara");
            charactersNames.Add(16, "Rurdeth");
            charactersNames.Add(17, "Vulen");
            charactersNames.Add(18, "Nina");
            charactersNames.Add(19, "Edea");
            charactersNames.Add(20, "Thessalia");
            charactersNames.Add(21, "Viessa");
            charactersNames.Add(22, "Yuna");
            charactersNames.Add(23, "Eorwyn");
            charactersNames.Add(24, "Elaiza");
            charactersNames.Add(25, "Sophie");
            charactersNames.Add(26, "Ruth");
            charactersNames.Add(27, "Kuro");
            charactersNames.Add(28, "Darka");
            charactersNames.Add(29, "Ramiel");
        }
        else
        {
            //Already initialized.
        }
    }

    public static Sprite LoadFullSpriteForId(int id)
    {
        Sprite sprite = Resources.Load<Sprite>("Characters/" + charactersNames[id]+"/Full");
        return sprite;
    }
    public static Sprite LoadIconSpriteForId(int id)
    {
        Sprite sprite = Resources.Load<Sprite>("Characters/" + charactersNames[id] + "/icon");
        return sprite;
    }
}
