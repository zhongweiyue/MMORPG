using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Item
    {
        public int Id;
        public int Count;
        public ItemDefine Define;
        public Item(NItemInfo item)
        {
            Id = item.Id;
            Count = item.Count;
            Define = DataManager.Instance.Items[item.Id];
        }

        public override string ToString()
        {
            return string.Format("Id:{0},Count:{1}", Id, Count);
        }
    }
}

