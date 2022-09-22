﻿using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class BagManager : Singleton<BagManager>
{
    public int Unlocked;
    public BagItem[] Items;

    NBagInfo Info;

    unsafe public void Init(NBagInfo info) 
    {
        this.Info = info;
        this.Unlocked = info.Unlocked;
        Items = new BagItem[this.Unlocked];
        if (info.Items != null && info.Items.Length >= this.Unlocked)
        {
            Analyze(info.Items);
        }
        else 
        {
            Info.Items = new byte[sizeof(BagItem) * this.Unlocked];
            Reset();
        }
    }

    public void Reset() 
    {
        int i = 0;//当前格子索引
        foreach (var kv in ItemManager.Instance.ItemsDict)
        {
            if (kv.Value.Count <= kv.Value.Define.StackLimit)
            {
                Items[i].ItemId = (ushort)kv.Key;
                Items[i].Count = (ushort)kv.Value.Count;
            }
            else 
            {
                int count = kv.Value.Count;
                while (count > kv.Value.Define.StackLimit) 
                {
                    Items[i].ItemId = (ushort)kv.Key;
                    Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                    i++;
                    count -= kv.Value.Define.StackLimit;
                }
                Items[i].ItemId = (ushort)kv.Key;
                Items[i].Count = (ushort)count;
            }
            i++;//新物品放在新格子
        }
    }

    unsafe void Analyze(byte[] data) 
    {
        fixed (byte* pt = data) 
        {
            for (int i = 0; i < Unlocked; i++) 
            {
                BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                Items[i] = *item;
            }
        }
    }

    unsafe public NBagInfo GetBagInfo() 
    {
        fixed (byte* pt = Info.Items) 
        {
            for (int i = 0; i < Unlocked; i++)
            {
                BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                *item = Items[i];
            }
        }
        return Info;
    }

}