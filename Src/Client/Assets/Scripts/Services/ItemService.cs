using Network;
using System;
using SkillBridge.Message;
using Common.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Managers;

namespace Services
{
    class ItemService:Singleton<ItemService>,IDisposable
    {
        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Subscribe<ItemEquipResponse>(this.OnItemEquip);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Unsubscribe<ItemEquipResponse>(this.OnItemEquip);
        }

        public void SendItemBuy(int shopId,int shopItemId)
        {
            Debug.Log("SendItemBuy");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.ShopId = shopId;
            message.Request.itemBuy.ShopItemId = shopItemId;

            NetClient.Instance.SendMessage(message);
        }


        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            MessageBox.Show("购买结果: "+message.Result +"\n"+ message.Errormsg,"购买完成");
        }


        Item pendingEquip = null;
        bool isEquip;

        public bool SendItemEquip(Item Equip , bool isEquip )
        {
            if (pendingEquip != null)
            {
                return false;
            }
            Debug.Log("SendItemEquip");
            //装备未装备
            pendingEquip = Equip;
            this.isEquip = isEquip;
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemEquip = new ItemEquipRequest();
            message.Request.itemEquip.isEquip = isEquip;
            message.Request.itemEquip.Slot = (int)Equip.EquipInfo.Slot;
            message.Request.itemEquip.itemId = Equip.itemId;

            NetClient.Instance.SendMessage(message);
            return true;
        }

        private void OnItemEquip(object sender, ItemEquipResponse message)
        {
            if (message.Result == Result.Success)
            {
                if (pendingEquip != null)
                {
                    //是否装备,装备了就脱下 ，未装备就船上
                    if (this.isEquip)
                    {
                        EquipManager.Instance.OnEquipItem(pendingEquip);
                    }
                    else
                    {
                        EquipManager.Instance.OnUnEquipItem(pendingEquip.EquipInfo.Slot);
                    }
                    pendingEquip = null;
                }
            }
        }
    }
}
