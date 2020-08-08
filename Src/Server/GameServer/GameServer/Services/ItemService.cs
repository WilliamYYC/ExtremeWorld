﻿using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class ItemService :Singleton<ItemService>
    {
        public ItemService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemEquipRequest>(this.OnItemEquip);
        }

        public void Init() { }


        void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemBuy: character {0} Shop{1} ShopItem{2}", character.Id, message.ShopId, message.ShopItemId);
            var result = ShopManager.Instance.BuyItem(sender , message.ShopId, message.ShopItemId);
            sender.Session.Response.itemBuy = new ItemBuyResponse();
            sender.Session.Response.itemBuy.Result = result;
            sender.SendResponse();
        }

         void OnItemEquip(NetConnection<NetSession> sender, ItemEquipRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemEquip: character {0} slot{1} Item{2} Equip {3}", character.Id, message.Slot, message.itemId, message.isEquip);
            var result = EquipManager.Instance.EquipItem(sender, message.Slot, message.itemId, message.isEquip);
            sender.Session.Response.itemEquip = new ItemEquipResponse();
            sender.Session.Response.itemEquip.Result = result;
            sender.SendResponse();
        }
        
    }
}
