using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    class GuildServcie:Singleton<GuildServcie>,IDisposable
    {
        public UnityAction OnGuildUpdate;
        public UnityAction<bool> OnGuildCreateResult;

        public UnityAction<List<NGuildInfo>> OnGuildListResult;


        public void Init()
        { }
        public GuildServcie()
        {
            MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuildResponse);
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);
            MessageDistributer.Instance.Subscribe<CreateGuildResponse>(this.OnCreateGuildResponse);
            MessageDistributer.Instance.Subscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }

        

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuildResponse);
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);
            MessageDistributer.Instance.Unsubscribe<CreateGuildResponse>(this.OnCreateGuildResponse);
            MessageDistributer.Instance.Unsubscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }


        public void sendGuildCreate(string guildName, string guildNotice, int gold)
        {
            Debug.Log("sendGuildCreate");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildCreate = new CreateGuildRequest();
            message.Request.guildCreate.GuildName = guildName;
            message.Request.guildCreate.Notice = guildNotice;
            message.Request.guildCreate.Gold = gold;
            NetClient.Instance.SendMessage(message);
        }

       

        private void OnCreateGuildResponse(object sender, CreateGuildResponse message)
        {
            Debug.LogFormat("OnCreateGuildResponse: {0}", message.Result);
            if (OnGuildCreateResult != null)
            {
                OnGuildCreateResult(message.Result == Result.Success);
            }

            if (message.Result == Result.Success)
            {
                GuildManager.Instance.Init(message.guildInfo);
                MessageBox.Show(string.Format("{0}公会创建成功", message.guildInfo.GuildName), "公会");
            }
            else
                MessageBox.Show(string.Format("{0}公会创建失败", message.guildInfo.GuildName), "公会");
        }


        public  void sendJoinGuildRequest(int guildID)
        {
            Debug.Log("sendJoinGuildRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinReq = new GuildJoinRequest();
            message.Request.guildJoinReq.Apply = new NGuildApplyInfo();
            message.Request.guildJoinReq.Apply.GuildId = guildID;
            NetClient.Instance.SendMessage(message);
        }

        public  void sendJoinGuildResponse(bool accept, GuildJoinRequest request)
        {
            Debug.Log("sendJoinGuildResponse");
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.guildJoinRes = new GuildJoinResponse();
            message.Response.guildJoinRes.Result = Result.Success;
            message.Response.guildJoinRes.Apply = request.Apply;
            message.Response.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }

        void OnGuildJoinRequest(object sender, GuildJoinRequest message)
        {
            var confirm = MessageBox.Show(string.Format("{0}申请加入公会", message.Apply.Name),"申请入会", MessageBoxType.Confirm,"接受","拒绝");

            confirm.OnYes = () =>
            {
                sendJoinGuildResponse(true, message);
            };

            confirm.OnNo = () =>
            {
                sendJoinGuildResponse(false, message);
            };
        }


        private void OnGuildJoinResponse(object sender, GuildJoinResponse message)
        {
             Debug.LogFormat("OnGuildJoinResponse  : {0} ", message.Result);
            if (message.Result == Result.Success)
            {
                MessageBox.Show("加入公会成功","公会");
            }
            else
                MessageBox.Show("加入公会失败", "公会");
        }


        void OnGuildResponse(object sender, GuildResponse message)
        {
            Debug.LogFormat("OnGuildResponse  : {0}  {1} :  {2}",message.Result , message.Guild.Id, message.Guild.GuildName);
           
            GuildManager.Instance.Init(message.Guild);

            if (OnGuildUpdate !=null)
            {
                this.OnGuildUpdate();
            }
        }


        public void sendGuildListRequest()
        {
            Debug.Log("sendGuildListRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildList = new GuildListRequest();
            NetClient.Instance.SendMessage(message);
        }

         void OnGuildList(object sender, GuildListResponse message)
        {
            if (OnGuildListResult != null)
            {
                this.OnGuildListResult(message.Guilds);
            }
        }

        public void sendGuildLeaveRequest(int characterId)
        {
            Debug.Log("sendGuildLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildLeave = new GuildLeaveRequest();
            message.Request.guildLeave.characterId = characterId;
            message.Request.guildLeave.GuildId = User.Instance.CurrentCharacter.Guild.Id;
            NetClient.Instance.SendMessage(message);
        }
 

        void OnGuildLeave(object sender, GuildLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                GuildManager.Instance.Init(null);
                MessageBox.Show("离开公会成功", "公会");
            }
            else
                MessageBox.Show("离开公会失败", "公会",MessageBoxType.Error);
        }


        public void sendGuidJoinApply(bool accept, NGuildApplyInfo info)
        {
            Debug.Log("sendGuidJoinApply");
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.guildJoinRes = new GuildJoinResponse();
            message.Response.guildJoinRes.Result = Result.Success;
            message.Response.guildJoinRes.Apply = info;
            message.Response.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }

        public void sendGuildAdminCommand(GuildAdminCommand command, int characterId)
        {
            Debug.Log("sendGuildAdminCommand");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildAdmin = new GuildAdminRequest();
            message.Request.guildAdmin.Command = command;
            message.Request.guildAdmin.Target = characterId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildAdmin(object sender, GuildAdminResponse message)
        {
            Debug.LogFormat("OnGuildAdmin: {0} : {1}", message.Command, message.Result);
            MessageBox.Show(string.Format("执行操作: {0} 结果: {1}:{2}", message.Command, message.Result, message.Errormsg));
        }
    }
}
