using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    TextMeshProUGUI txt_charName;
    [SerializeField]
    TextMeshProUGUI txt_count;

    int myId;
    public int ID { get { return myId; } }
    
    private void Awake()
    {
        image= GetComponent<Image>();
    }
    public void Initialize(int id,Sprite sprite, string name, string count,bool show)
    {
        myId = id;
        image.sprite = sprite;
        txt_charName.text = name;
        txt_count.text = count;
        txt_count.gameObject.SetActive(show);
        txt_charName.gameObject.SetActive(show);
    }

    public void OnSelected()
    {
        CharacterSelectionManager.Instance.OnCharacterSelected(myId);
    }

    public void ShowAsSelected()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
    public void ShowAsNotSelected()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
    }
}
