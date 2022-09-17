using Common.Data;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<int, Item> ItemsDict = new Dictionary<int, Item>();
    internal void Init(List<NItemInfo>items)
    {
        ItemsDict.Clear();
        foreach (var info in items)
        {
            Item item = new Item(info);
            ItemsDict.Add(info.Id, item);
            Debug.LogFormat("ItemManager:Init [{0}]", item);
        }
    }
    public ItemDefine GetItem(int itemId)
    {
        return null;
    }

    public bool UseItem(int itemId)
    {
        return false;
    }

    public bool UseItem(ItemDefine item)
    {
        return false;
    }

}
