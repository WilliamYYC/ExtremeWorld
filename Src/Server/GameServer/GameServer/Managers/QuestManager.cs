using Common;
using Common.Data;
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
    class QuestManager 
    {
        Character Owner;

        public QuestManager(Character owner)
        {
            this.Owner = owner;
;
        }

        //获取当前角色身上的所有任务
        public  void GetQuestInfos(List<NQuestInfo> list)
        {
            foreach (var quest in this.Owner.Data.Quests)
            {
                list.Add(GetQuestInfo(quest));
            }
        }

        public NQuestInfo GetQuestInfo(TCharacterQuests quest)
        {
            return new NQuestInfo()
            {
                QuestId = quest.QuestID,
                QuestGuid = quest.Id,
                Status = (QuestStatus)quest.Status,
                Targets = new int[3]
                {
                    quest.Target1,
                    quest.Target2,
                    quest.Target3
                }
            };
        }


        public Result SubmitQuest(NetConnection<NetSession> sender, int questID)
        {
            Character character = sender.Session.Character;
            QuestDefine questDefine;
            if (DataManager.Instance.Quests.TryGetValue(questID, out questDefine))
            {
                var dbQuest = character.Data.Quests.Where(q => q.QuestID == questID).FirstOrDefault();
                if (dbQuest !=null)
                {
                    if (dbQuest.Status != (int)QuestStatus.Completed)
                    {
                        sender.Session.Response.questSubmit.Errormsg = "任务未完成";
                        return Result.Failed;
                    }
                    dbQuest.Status = (int)QuestStatus.Finished;
                    sender.Session.Response.questSubmit.Quest = GetQuestInfo(dbQuest);
                    DBService.Instance.Save();
                    //处理奖励的物品
                    if (questDefine.RewardGold > 0)
                    {
                        character.Gold += questDefine.RewardGold;
                    }

                    if (questDefine.RewardExp > 0)
                    {
                        //character.EXP += questDefine.RewardExp;
                    }

                    if (questDefine.RewardItem1 > 0)
                    {
                        character.itemManager.AddItem(questDefine.RewardItem1, questDefine.RewardItem1Count);
                    }

                    if (questDefine.RewardItem2 > 0)
                    {
                        character.itemManager.AddItem(questDefine.RewardItem2, questDefine.RewardItem2Count);
                    }
                    if (questDefine.RewardItem3 > 0)
                    {
                        character.itemManager.AddItem(questDefine.RewardItem3, questDefine.RewardItem3Count);
                    }
                    DBService.Instance.Save();
                    return Result.Success;

                }
                else
                {
                    sender.Session.Response.questAccept.Errormsg = "任务不存在[2]";
                    return Result.Failed;
                }
            }
            else
            {
                sender.Session.Response.questAccept.Errormsg = "任务不存在[1]";
                return Result.Failed;
            }
        }

        public Result AcceptQuest(NetConnection<NetSession> sender, int questID)
        {
            Character character = sender.Session.Character;
            QuestDefine questDefine;
            if (DataManager.Instance.Quests.TryGetValue(questID, out questDefine))
            {
                var dbQuest = DBService.Instance.Entities.CharacterQuests.Create();
                dbQuest.QuestID = questDefine.ID;
                //如果任务没有目标直接完成
                if (questDefine.Target1 == QuestTarget.None)
                {
                    dbQuest.Status = (int)QuestStatus.Completed;
                }
                else 
                {
                    dbQuest.Status = (int)QuestStatus.InProgress;
                }
                sender.Session.Response.questAccept.Quest = GetQuestInfo(dbQuest);
                character.Data.Quests.Add(dbQuest);
                DBService.Instance.Save();
                return Result.Success;
            }
            else
            {
                sender.Session.Response.questAccept.Errormsg = "任务不存在";
                return Result.Failed;
            }
        }
    }
}
