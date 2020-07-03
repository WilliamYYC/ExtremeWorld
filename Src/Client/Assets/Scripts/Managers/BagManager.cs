using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Managers
{
    public class BagManager : Singleton<BagManager>
    {

        public int Unlocked;

        public BagItem[] items;

        NBagInfo Info;

        unsafe public void Init(NBagInfo info)
        {
            this.Info = info;
            this.Unlocked = info.Unlocked;
            items = new BagItem[this.Unlocked];

            if (info.Items != null && info.Items.Length >= this.Unlocked)
            {
                Analyze(info.Items);
            }
            else
            {
                info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }


        public void Reset()
        {
            int i = 0;
            foreach (var kv in ItemManager.Instance.items)
            {
                if (kv.Value.count <= kv.Value.itemDefine.StackLimit)
                {
                    this.items[i].itemId = (ushort)kv.Key;
                    this.items[i].Count = (ushort)kv.Value.count;
                }
                else
                {
                    int count = kv.Value.count;
                    while (count > kv.Value.itemDefine.StackLimit)
                    {
                        this.items[i].itemId = (ushort)kv.Key;
                        this.items[i].Count = (ushort)kv.Value.itemDefine.StackLimit;
                        i++;
                        count -= kv.Value.itemDefine.StackLimit;
                    }

                    this.items[i].itemId = (ushort)kv.Key;
                    this.items[i].Count = (ushort)count;
                }
                i++;
            }
        }

     

        unsafe void Analyze(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    items[i] = *item;
                }
            }
        }

        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = Info.Items)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = items[i];
                }
            }
            return this.Info;
        }


        public void RemoveItem(int itemid, int count)
        {
            

        }

        public void AddItem(int itemid, int count)
        {
            ushort addCount = (ushort)count;
            for (int i = 0; i < this.items.Length; i++)
            {
                if (this.items[i].itemId == itemid)
                {
                    ushort canAdd = (ushort)(DataManager.Instance.Items[itemid].StackLimit - this.items[i].Count);
                    if (canAdd >= addCount)
                    {
                        this.items[i].Count += addCount;
                        addCount = 0;
                        break;
                    }
                    else
                    {
                        this.items[i].Count += canAdd;
                        addCount -= canAdd;
                    }
                }
            }

            if (addCount > 0)
            {
                for (int i = 0; i < this.items.Length; i++)
                {
                    if (this.items[i].itemId == 0)
                    {
                        this.items[i].itemId = (ushort)itemid;
                        this.items[i].Count = addCount;
                        break;
                    }
                }
            }
        }
    }
}

