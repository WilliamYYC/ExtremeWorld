﻿using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class BagService:Singleton<BagService>
    {

        public BagService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<BagSaveRequest>(this.OnBagSave);
        }

        public void Init()
        {

        }
         void OnBagSave(NetConnection<NetSession> sender, BagSaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("BagSaveRequest  Character{0}  Unlocked {1}", character.Id, request.Bag.Unlocked);
            if (request.Bag != null)
            {
                character.Data.Bag.Items = request.Bag.Items;
                DBService.Instance.Save();
            }
            
        }
    }
}
