using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Managers
{
    public enum NpcQuestStatus
    {
        None,//无任务
        Complete,//拥有已完成可提交任务
        Available,//拥有可接受任务
        Incomplete,//拥有未完成任务

    }
    class QuestManager:Singleton<QuestManager>
    {
        //所有有效的任务
        public List<NQuestInfo> questInfos;
        public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();

        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();
        public UnityAction<Quest> OnQuestStatusChanged;
        public void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;
            allQuests.Clear();
            this.npcQuests.Clear();
            InitQuests();
        }

         void InitQuests()
        {
            //初始化已有任务
            foreach (var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                
                this.allQuests[quest.Info.QuestId] = quest;
            }
            //检查可用任务
            this.checkAvailableQuest();

            //往Npc身上添加任务
            foreach (var kv in allQuests)
            {
                this.AddNpcQuest(kv.Value.questDefine.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.questDefine.SubmitNPC, kv.Value);
            }
          
        }
        //检查可用任务
        void checkAvailableQuest()
        {
            //初始化可用任务
            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class)
                {
                    //不符合职业
                    continue;
                }
                if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level)
                {
                    //等级太低
                    continue;
                }
                if (this.allQuests.ContainsKey(kv.Key))
                {
                    //已存在该任务
                    continue;
                }

                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    if (this.allQuests.TryGetValue(kv.Value.PreQuest, out preQuest))
                    {
                        if (preQuest.Info == null)
                        {
                            continue;//前置任务未接取
                        }
                        if (preQuest.Info.Status != QuestStatus.Finished)
                        {
                            continue;//前置任务未完成
                        }
                    }
                    else
                    {
                        continue;//前置任务没接
                    }
                }
                Quest quest = new Quest(kv.Value);
                this.allQuests[quest.questDefine.ID] = quest;
            }
        }

        public void OnQuestSubmited(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.questDefine.DialogFinish);
        }

        public  void OnQuestAccepted(NQuestInfo info)
        {
            var quest =this.RefreshQuestStatus(info);
            MessageBox.Show(quest.questDefine.DialogAccept);
        }

        public  void AddNpcQuest(int NPC, Quest quest)
        {
            if (!this.npcQuests.ContainsKey(NPC))
            {
                this.npcQuests[NPC] = new Dictionary<NpcQuestStatus, List<Quest>>();
            }
            List < Quest > availables;
            List<Quest> completes;
            List<Quest> incompletes;

            if (!this.npcQuests[NPC].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                this.npcQuests[NPC][NpcQuestStatus.Available] = availables;
            }

            if (!this.npcQuests[NPC].TryGetValue(NpcQuestStatus.Complete, out completes))
            {
                completes = new List<Quest>();
                this.npcQuests[NPC][NpcQuestStatus.Complete] = completes;
            }

            if (!this.npcQuests[NPC].TryGetValue(NpcQuestStatus.Incomplete, out incompletes))
            {
                incompletes = new List<Quest>();
                this.npcQuests[NPC][NpcQuestStatus.Incomplete] = incompletes;
            }

            if (quest.Info == null)
            {
                if (NPC == quest.questDefine.AcceptNPC && !this.npcQuests[NPC][NpcQuestStatus.Available].Contains(quest))
                {
                    this.npcQuests[NPC][NpcQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if (NPC == quest.questDefine.SubmitNPC && quest.Info.Status == QuestStatus.Completed)
                {
                    if (!this.npcQuests[NPC][NpcQuestStatus.Complete].Contains(quest))
                    {
                        this.npcQuests[NPC][NpcQuestStatus.Complete].Add(quest);
                    }
                   
                }

                if (NPC == quest.questDefine.SubmitNPC && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!this.npcQuests[NPC][NpcQuestStatus.Incomplete].Contains(quest))
                    {
                        this.npcQuests[NPC][NpcQuestStatus.Incomplete].Add(quest);
                    }
                }
            }


        }

        //获取任务状态
        public NpcQuestStatus GetQuestStatusByNpc(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (this.npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count >0)
                {
                    return NpcQuestStatus.Complete;
                }
                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return NpcQuestStatus.Available;
                }
                if (status[NpcQuestStatus.Incomplete].Count>0)
                {
                    return NpcQuestStatus.Incomplete;
                }
            }
            return NpcQuestStatus.None;
        }


        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (this.npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                {
                    return showQuestDialog(status[NpcQuestStatus.Complete].First());
                }
                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return showQuestDialog(status[NpcQuestStatus.Available].First());
                }
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                {
                    return showQuestDialog(status[NpcQuestStatus.Incomplete].First());
                }
            }
            return false;
        }

        bool showQuestDialog(Quest quest)
        {
            if (quest.Info == null || quest.Info.Status == QuestStatus.Completed)
            {
               UIQuestDialog dlg =  UIManagers.Instance.Show<UIQuestDialog>();
                dlg.SetQuest(quest);
                dlg.OnClose += OnQuestDialogClose;
                return true;
            }
            if (quest.Info != null || quest.Info.Status == QuestStatus.InProgress)
            {
                if (!string.IsNullOrEmpty(quest.questDefine.DialogIncomplete))
                {
                    MessageBox.Show(quest.questDefine.DialogIncomplete);
                }
            }
            return true;
        }

         void OnQuestDialogClose(UIWindows sender, UIWindows.WinowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIWindows.WinowResult.Yes)
            {
                if (dlg.quest.Info == null)
                {
                    QuestService.Instance.SendQusetAccept(dlg.quest);
                }
                else if (dlg.quest.Info.Status == QuestStatus.Completed)
                {
                    QuestService.Instance.SendQuestSubmit(dlg.quest);
                }
            }
            else if (result == UIWindows.WinowResult.No)
            {
                MessageBox.Show(dlg.quest.questDefine.DialogDeny);
            }
        }

        Quest RefreshQuestStatus(NQuestInfo quest)
        {
            this.npcQuests.Clear();
            Quest result;
            if (this.allQuests.ContainsKey(quest.QuestId))
            {
                this.allQuests[quest.QuestId].Info = quest;
                result = this.allQuests[quest.QuestId];
            }
            else
            {
                result = new Quest(quest);
                this.allQuests[quest.QuestId] = result;
            }

            checkAvailableQuest();

            foreach (var kv in this.allQuests)
            {
                this.AddNpcQuest(kv.Value.questDefine.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.questDefine.SubmitNPC, kv.Value);
            }

            if (OnQuestStatusChanged != null)
            {
                OnQuestStatusChanged(result);
            }

            return result;
        }

    }
}
