using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ItemManager
    {
        Character Owner;

        public Dictionary<int, Item> items = new Dictionary<int, Item>();


        public ItemManager(Character owner)
        {
            this.Owner = owner;
            //把角色的所有道具添加到items管理器中
            foreach (var item in owner.Data.Items)
            {
                this.items.Add(item.ItemID, new Item(item));
            }
        }

        public bool UseItem(int itemid, int count = 1)
        {
            Log.InfoFormat("{0}  UseItem ID:{1} Count {2} ",this.Owner.Data.ID,itemid,count);
            Item item = null;
            if (this.items.TryGetValue(itemid, out item))
            {
                if (item.Count < count)
                {
                    return false;
                }
                //TODO:增加使用逻辑

                item.Remove(count);
                return true;
            }
            return false;
        }

        //判断是否有item
        public bool HasItem(int itemid)
        {
            Item item = null;
            if (this.items.TryGetValue(itemid, out item))
            {
                return item.Count > 0;
            }
            return false;
        }
        //获取item
        public Item GetItem(int itemid)
        {
            Item item = null;
            this.items.TryGetValue(itemid, out item);
            Log.InfoFormat("{0}  GetItem {1}  {2} ", this.Owner.Data.ID, itemid, item);
            return item;
        }

        //添加item
        public bool AddItem(int itemid, int count)
        {
            Item item = null;
            //添加item,如果管理器中存在加count,不存在创建item添加到数据库和管理器
            if (this.items.TryGetValue(itemid, out item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItem dbcharItem = new TCharacterItem();
                dbcharItem.CharacterID = Owner.Data.ID;
                dbcharItem.Owner = Owner.Data;
                dbcharItem.ItemID = itemid;
                dbcharItem.ItemCount = count;
                Owner.Data.Items.Add(dbcharItem);
                Item item1 = new Item(dbcharItem);
                this.items.Add(itemid, item1);
            }
            Log.InfoFormat("{0}  AddItem ID:{1} AddCount {2} ", this.Owner.Data.ID, itemid, count);
            //DBService.Instance.Save();
            return true;
        }

        //删除item

        public  bool RemoveItem(int itemid, int count)
        {
            if (!items.ContainsKey(itemid))
            {
                return false;
            }
            Item item = items[itemid];
            if (item.Count < count)
            {
                return false;
            }
            item.Remove(count);
            Log.InfoFormat("{0}  RemoveItem ID:{1} RemoveCount {2} ", this.Owner.Data.ID, itemid, count);
            //DBService.Instance.Save();
            return true;
        }
        public void GetItemInfos(List<NItemInfo> list)
        {
            foreach (var item in this.items)
            {
                list.Add(new NItemInfo() { Id = item.Value.ItemID, Count = item.Value.Count });
            }
        }
    }
}
