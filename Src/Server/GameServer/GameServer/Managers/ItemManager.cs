﻿using Common;
using GameServer;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


class ItemManager
{
    Character Owner;
    public Dictionary<int, Item> ItemsDict = new Dictionary<int, Item>();
    public ItemManager(Character owner)
    {
        this.Owner = owner;
        foreach (var item in owner.Data.Items)
        {
            ItemsDict.Add(item.ItemID, new Item(item));
        }
    }

    public bool UseItem(int itemId, int count = 1)
    {
        Log.InfoFormat("[{0}]UseItem[{1}:{2}]", Owner.Data.ID, itemId, count);
        Item item = null;
        if (ItemsDict.TryGetValue(itemId, out item))
        {
            if (item.Count < count)
            {
                return false;
            }
            //TODO 使用物品逻辑
            item.Remove(count);
            return true;
        }
        return false;
    }

    public bool HasItem(int itemId)
    {
        Item item = null;
        if (ItemsDict.TryGetValue(itemId, out item))
        {
            return item.Count > 0;
        }
        return false;
    }

    public Item GetItem(int itemId)
    {
        Item item = null;
        ItemsDict.TryGetValue(itemId, out item);
        Log.InfoFormat("[{0}]GetItem[{1}:{2}]", Owner.Data.ID, itemId, item);
        return item;
    }


    public bool AddItem(int itemId,int count)
    {
        Item item = null;
        if (ItemsDict.TryGetValue(itemId, out item))
        {
            item.Add(count);
        }
        else
        {
            TCharacterItem dbItem = new TCharacterItem();
            dbItem.CharacterID = Owner.Data.ID;
            dbItem.Owner = Owner.Data;//TCharacter
            dbItem.ItemID = itemId;
            dbItem.ItemCount = count;
            Owner.Data.Items.Add(dbItem);
            item = new Item(dbItem);
            ItemsDict.Add(itemId, item);
        }
        Log.InfoFormat("[{0}]AddItem[{1}] addCount:{2}", Owner.Data.ID, item, count);
        //DBService.Instance.Save();
        return true;
    }

    public bool RemoveItem(int itemId,int count)
    {
        if (!ItemsDict.ContainsKey(itemId))
        {
            return false;
        }
        Item item = ItemsDict[itemId];
        if (item.Count < count)
        {
            return false;
        }
        item.Remove(count);
        Log.InfoFormat("[{0}]RemoveItem[{1}] removeCount:{2}", Owner.Data.ID, item, count);
        //DBService.Instance.Save();
        return true;
    }

    public void GetItemInfos(List<NItemInfo> list)
    {
        foreach (var item in ItemsDict)
        {
            list.Add(new NItemInfo() { Id = item.Value.ItemID, Count = item.Value.Count });
        }
    }
}

