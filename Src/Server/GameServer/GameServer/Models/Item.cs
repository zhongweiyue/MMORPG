using GameServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class Item
    {
        TCharacterItem dbItem;
        public int ItemID;
        public int Count;
        public Item(TCharacterItem item)
        {
            dbItem = item;
            ItemID = (short)item.ItemID;
            Count = (short)item.ItemCount;
        }

        public void Add(int count)
        {
            Count += count;
            dbItem.ItemCount = Count;
        }

        public void Remove(int count)
        {
            Count -= count;
            dbItem.ItemCount = Count;
        }

        public bool Use(int count = 1)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format("ID:{0},Count:{1}", ItemID, Count);
        }
    }
}

