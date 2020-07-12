using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }

        public MapDefine CurrentMapData { get; set; }

        public PlayerInputController CurrentCharacterObject { get; set; }

        public NTeamInfo TeamInfo { get; set; }
        public void AddGold(int gold)
        {
            this.CurrentCharacter.Gold += gold;
        }

        public int CurrentRide = 0;
        public void Ride(int id)
        {
            if (CurrentRide != id)
            {
                CurrentRide = id;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
            }
            else
            {
                CurrentRide =0;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, 0);
            }
        }
    }
}
