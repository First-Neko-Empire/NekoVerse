using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum Characters
{
    Kuro, Urania, Asami, Darkiel, Neva, Slava, Caio, Eirina, Gweyir, Anna,
    Elissa, Eileen, Aryna, Rokorid, Molly, Haiku, Sara, Rurdeth,
    Vulen, Nina, Edea, Thessalia, Viessa, Yuna, Eorvyn, Elaiza,
    Sophie, Ruth, Darka, Ramiel, NullCharacter
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
            charactersNames.Add(0,  "Kuro");
            charactersNames.Add(1,  "Urania");
            charactersNames.Add(2,  "Asami");
            charactersNames.Add(3,  "Darkiel");
            charactersNames.Add(4,  "Neva");
            charactersNames.Add(5,  "Slava");
            charactersNames.Add(6,  "Caio");
            charactersNames.Add(7, "Eirina");
            charactersNames.Add(8,  "Gweyir");
            charactersNames.Add(9,  "Anna");
            charactersNames.Add(10, "Elissa");
            charactersNames.Add(11, "Eileen");
            charactersNames.Add(12, "Aryna");
            charactersNames.Add(13, "Rokorid");
            charactersNames.Add(14, "Molly");
            charactersNames.Add(15, "Haiku");
            charactersNames.Add(16, "Sara");
            charactersNames.Add(17, "Rurdeth");
            charactersNames.Add(18, "Vulen");
            charactersNames.Add(19, "Nina");
            charactersNames.Add(20, "Edea");
            charactersNames.Add(21, "Thessalia");
            charactersNames.Add(22, "Viessa");
            charactersNames.Add(23, "Yuna");
            charactersNames.Add(24, "Eorvyn");
            charactersNames.Add(25, "Elaiza");
            charactersNames.Add(26, "Sophie");
            charactersNames.Add(27, "Ruth");
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
