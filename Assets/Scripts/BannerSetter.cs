using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerSetter : Singleton<BannerSetter>
{
    [SerializeField]
    Image bkgnd;


    [SerializeField]
    List<Sprite> images;

    [SerializeField]
    Sprite newbie;

    [SerializeField]
    GameObject main;


    public void ShowBkgnd()
    {
        main.SetActive(true);
    }
    public void HideBkgnd()
    {
        main.SetActive(false);
    }
    public void SetRandomBanner()
    {
        int number = Random.Range(0, images.Count);
        bkgnd.sprite = images[number];
    }
    public void SetNewbie()
    {
        bkgnd.sprite = newbie;
    }
}
