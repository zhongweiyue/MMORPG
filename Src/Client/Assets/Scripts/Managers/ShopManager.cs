using Common.Data;
using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    private UIShop uiShop;
    public void Init() 
    {
        NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop, OnOpenShop);
    }

    private bool OnOpenShop(NpcDefine npc) 
    {
        ShowShop(npc.Param);
        return true;
    }

    public void ShowShop(int shopId) 
    {
        ShopDefine shop;
        if (DataManager.Instance.Shops.TryGetValue(shopId, out shop)) 
        {
            uiShop = UIManager.Instance.Show<UIShop>();
            if (uiShop != null) 
            {
                uiShop.SetShop(shop);
            }
        }
    }

    public bool BuyItem(int shopId, int shopItemId) 
    {
        ItemService.Instance.SendBuyItem(shopId, shopItemId);
        return true;
    }

    public void RefreshGold()
    {
        if (uiShop != null)
        {
            uiShop.money.text = User.Instance.CurrentCharacter.Gold.ToString();
        }
    }
}
