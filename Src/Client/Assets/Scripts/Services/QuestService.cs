using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    public class QuestService : Singleton<QuestService>, IDisposable
    {

        public QuestService()
        {
            MessageDistributer.Instance.Subscribe<QuestAcceptResponse>(this.OnAcceptQuest);
            MessageDistributer.Instance.Subscribe<QuestSubmitResponse>(this.OnSubmitQuest);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<QuestAcceptResponse>(this.OnAcceptQuest);
            MessageDistributer.Instance.Unsubscribe<QuestSubmitResponse>(this.OnSubmitQuest);
        }


        public bool SendQusetAccept(Quest quest)
        {
            Debug.Log("SendQusetAccept");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questAccept = new QuestAcceptRequest();
            message.Request.questAccept.QuestId = quest.questDefine.ID;
            NetClient.Instance.SendMessage(message);
            return true;
        }

        public bool SendQuestSubmit(Quest quest)
        {
            Debug.Log("SendQuestSubmit");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questSubmit = new QuestSubmitRequest();
            message.Request.questSubmit.QuestId = quest.questDefine.ID;
            NetClient.Instance.SendMessage(message);
            return true;
        }

        private void OnSubmitQuest(object sender, QuestSubmitResponse message)
        {
            Debug.LogFormat("OnSubmitQuest :{0} , ERROR: {1}", message.Result, message.Errormsg);

            if (Result.Success == message.Result)
            {
                QuestManager.Instance.OnQuestSubmited(message.Quest);
            }
            else
            {
                MessageBox.Show("任务完成失败", "错误", MessageBoxType.Error);
            }
        }

        private void OnAcceptQuest(object sender, QuestAcceptResponse message)
        {
            Debug.LogFormat("OnAcceptQuest :{0} , ERROR: {1}", message.Result, message.Errormsg);

            if (Result.Success == message.Result)
            {
                QuestManager.Instance.OnQuestAccepted(message.Quest);
            }
            else
            {
                MessageBox.Show("任务接受失败","错误",MessageBoxType.Error);
            }
        }

       
    }
}
