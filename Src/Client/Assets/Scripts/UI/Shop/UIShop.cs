﻿using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow
{
    public Text title;
    public Text money;
    public GameObject shopItem;
    ShopDefine shop;
    public Transform[] itemRoot;

    void Start()
    {
        StartCoroutine(InitItems());
    }

    IEnumerator InitItems() 
    {
        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            if (kv.Value.Status > 0) //在售物品，小于0是下架物品
            {
                GameObject go = Instantiate(shopItem, itemRoot[0]);
                UIShopItem ui = go.GetComponent<UIShopItem>();
                ui.SetShopItem(kv.Key, kv.Value, this);
            }
        }
        yield return null;
    }

    public void SetShop(ShopDefine shop) 
    {
        this.shop = shop;
        title.text = shop.Name;
        money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    private UIShopItem selectedItem;
    public void SelectShopItem(UIShopItem item) 
    {
        if (selectedItem != null) 
        {
            selectedItem.Selected = false;
        }
        selectedItem = item;
    }

    public void OnClickBuy() 
    {
        if (selectedItem == null) 
        {
            MessageBox.Show("请选择要购买的道具", "购买提示");
            return;
        }
        if (!ShopManager.Instance.BuyItem(shop.ID, selectedItem.ShopItemID)) 
        {
        
        }
    }
}