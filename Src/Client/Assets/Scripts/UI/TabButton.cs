using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public Sprite activeImage;
    public Sprite normalImage;
    public TabView tabView;
    public int tabIndex = 0;
    public bool selected = false;
    private Image tabImage;
    void Start()
    {
        tabImage = GetComponent<Image>();
        normalImage = tabImage.sprite;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Select(bool select)
    {
        tabImage.overrideSprite = select ? activeImage : normalImage;
    }

    void OnClick() 
    {
        tabView.SelectTab(tabIndex);
    }

}
