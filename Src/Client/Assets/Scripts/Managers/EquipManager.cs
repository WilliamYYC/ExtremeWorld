using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    class EquipManager : Singleton<EquipManager>
    {
        public delegate void OnEquipChangeHandler();

        public event OnEquipChangeHandler OnEquipChanged;

        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];

        byte[] Data;

        unsafe public void  Init(byte[] data)
        {
            this.Data = data;
            this.ParseEquipData(data);
        }

        public bool Contains(int equipId)
        {
            for (int i = 0; i < this.Equips.Length; i++)
            {
                if (Equips[i] !=null  &&  Equips[i].itemId == equipId)
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
        //从byte数组中解析装备信息
        unsafe void ParseEquipData(byte[] data)
        {
            fixed(byte* pt = data)
            {
                for (int i = 0; i < this.Equips.Length; i++)
                {
                    int itemId = *(int*)(pt + i * sizeof(int));
                    if (itemId > 0)
                    {
                        Equips[i] = ItemManager.Instance.items[itemId];
                    }
                    else
                        Equips[i] = null;
                }
                
            }
        }
        
        unsafe public byte[] GetEquipData()
        {
            fixed (byte* pt = this.Data)
            {
                for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
                {
                    int* itemID = (int*)(pt + i * sizeof(int));
                    if (Equips[i] == null)
                    {
                        *itemID = 0;
                    }
                    else
                    {
                        *itemID = Equips[i].itemId;
                    }
                }
            }
            return this.Data;
        }

        //穿装备
        public void EquipItem(Item Equip)
        {
            ItemService.Instance.SendItemEquip(Equip, true);
        }
        //脱装备
        public void UnEquipItem(Item Equip)
        {
            ItemService.Instance.SendItemEquip(Equip, false);
        }


        public void OnEquipItem(Item pendingEquip)
        {
            //已经穿上相同的装备return
            if (this.Equips[(int)pendingEquip.EquipInfo.Slot] != null && this.Equips[(int)pendingEquip.EquipInfo.Slot].itemId == pendingEquip.itemId)
            {
                return;
            }
            this.Equips[(int)pendingEquip.EquipInfo.Slot] = ItemManager.Instance.items[pendingEquip.itemId];

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
