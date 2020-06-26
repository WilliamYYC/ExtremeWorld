using Network;
using System;
using SkillBridge.Message;
using Common.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    class ItemService:Singleton<ItemService>,IDisposable
    {
        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
        }

        
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
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

    }
}
