using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
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
        StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
    }

    bool OnItemNotify(NStatus status) 
    {
        if (status.Action == StatusAction.Add) 
        {
            AddItem(status.Id, status.Value);
        }
        if (status.Action == StatusAction.Delete) 
        {
            RemoveItem(status.Id, status.Value);
        }
        return true;
    }
    private void AddItem(int itemId, int count)
    {
        Item item = null;
        if (ItemsDict.TryGetValue(itemId, out item))
        {
            item.Count += count;
        }
        else 
        {
            item = new Item(itemId, count);
            ItemsDict.Add(itemId, item);
        }
        BagManager.Instance.AddItem(itemId, count);
    }

    private void RemoveItem(int itemId, int count)
    {
        if (!ItemsDict.ContainsKey(itemId)) {
            return;
        }
        Item item = ItemsDict[itemId];
        if (item.Count < count) 
        {
            return;
        }
        item.Count -= count;
        BagManager.Instance.RemoveItem(itemId, count);
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
