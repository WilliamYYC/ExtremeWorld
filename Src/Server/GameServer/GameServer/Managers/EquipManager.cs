using Common;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class EquipManager : Singleton<EquipManager>
    {
        public Result EquipItem(NetConnection<NetSession> sender, int slot, int itemId, bool isEquip)
        {
            //判断是否存在该装备
            Character character = sender.Session.Character;
            if (!character.itemManager.items.ContainsKey(itemId))
            {
                return Result.Failed;
            }

            UpdateEquip(character.Data.Equips, slot, itemId, isEquip);

            DBService.Instance.Save();
            return Result.Success;
        }

        unsafe void UpdateEquip(byte[] equipData, int slot, int itemId, bool isEquip)
        {
            fixed(byte *pt = equipData)
            {
                int* slotId = (int*)(pt + slot * sizeof(int));

                if (isEquip)
                {
                    *slotId = itemId;
                }
                else
                    *slotId = 0;
            }
        }
    }
}
