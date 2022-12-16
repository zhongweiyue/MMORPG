using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRideItem : ListView.ListViewItem
{
    public Image icon;
    public Text title;
    public Text level;

    public Image background;
    public Sprite normalBg;
    public Sprite selectBg;

    public Item item;
    private void Start()
    {
        
    }
    public override void onSelected(bool selected)
    {
        background.overrideSprite = selected ? selectBg : normalBg;
    }

    public void SetRideItem(Item item, UIRide owner) 
    {
        this.item = item;
        if (title != null) title.text = this.item.Define.Name;
        if (level != null) level.text = this.item.Define.Level.ToString();
        if (icon != null) icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }
}
