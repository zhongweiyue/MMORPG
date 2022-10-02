using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class EquipManager : Singleton<EquipManager>
    {
        public delegate void OnEquipChangeHanlder();
        public event OnEquipChangeHanlder OnEquipChanged;
        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];//7个装备槽
        byte[] Data;

        unsafe public void Init(byte[] data)
        {
            this.Data = data;
            ParseEquipData(data);
        }

        public bool Contains(int equipId)
        {
            for (int i = 0; i < this.Equips.Length; i++)
            {
                if (Equips[i] != null && Equips[i].Id == equipId)
                {
                    return true;
                }
            }
            return false;
        }

        public Item GetEquip(EquipSlot slot)
        {
            return Equips[(int)slot];
        }

        unsafe void ParseEquipData(byte[] data)
        {
            fixed (byte* pt = this.Data)
            {
                for (int i = 0; i < Equips.Length; i++)
                {
                    int itemId = *(int*)(pt + i * sizeof(int));
                    if (itemId > 0)
                    {
                        Equips[i] = ItemManager.Instance.ItemsDict[itemId];
                    }
                    else
                    {
                        Equips[i] = null;
                    }
                }
            }
        }

        unsafe public byte[] GetEquipData()
        {
            fixed (byte* pt = Data)
            {
                for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
                {
                    int* itemid = (int*)(pt + i * sizeof(int));
                    if (Equips[i] == null)
                    {
                        *itemid = 0;
                    }
                    else
                    {
                        *itemid = Equips[i].Id;
                    }
                }
            }
            return this.Data;
        }

        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, true);
        }

        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, false);
        }

        public void OnEquipItem(Item equip)
        {
            if (Equips[(int)equip.EquipInfo.Slot] != null && Equips[(int)equip.EquipInfo.Slot].Id == equip.Id)
            {
                return;
            }
            Equips[(int)equip.EquipInfo.Slot] = ItemManager.Instance.ItemsDict[equip.Id];
            if (OnEquipChanged != null)
            {
                OnEquipChanged();
            }
        }

        public void OnUnEquipItem(EquipSlot slot)
        {
            if (this.Equips[(int)slot] != null)
            {
                this.Equips[(int)slot] = null;
                if (OnEquipChanged != null)
                {
                    OnEquipChanged();
                }
            }
        }
    }
}
