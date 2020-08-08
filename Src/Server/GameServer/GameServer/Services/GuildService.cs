using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class GuildService :Singleton<GuildService>
    {

        public void Init()
        {
            GuildManager.Instance.Init();
        }
        public GuildService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildLeaveRequest>(this.OnGuildLeave);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildListRequest>(this.OnGuildList);
            
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<CreateGuildRequest>(this.OnGuildCreate);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildAdminRequest>(this.OnGuildAdmin);
        }

       

        void OnGuildCreate(NetConnection<NetSession> sender, CreateGuildRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildCreate : guildName: {0}  character: {0}:{1}", message.GuildName, character.Id, character.Name);
            
            sender.Session.Response.guildCreate = new CreateGuildResponse();
            if (character.Guild !=null)
            {
                //已经有公会了
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "已经有公会了";
                sender.SendResponse();
                return;
            }
            if (GuildManager.Instance.CheckNameExisted(message.GuildName))
            {
                //已经该有公会了
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "公会名称已存在";
                sender.SendResponse();
                return;
            }
            
            if (sender.Session.Character.Gold >= message.Gold)
            {
                GuildManager.Instance.CreateGuild(message.GuildName, message.Notice, character);
            }
            else
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "公会创建资金不足";
                sender.SendResponse();
                return;
            }
            
            sender.Session.Response.guildCreate.guildInfo = character.Guild.GuildInfo(character);
            sender.Session.Response.guildCreate.Result = Result.Success;
            sender.SendResponse();

        }

         void OnGuildList(NetConnection<NetSession> sender, GuildListRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildList : character: {0}:{1}", character.Id, character.Name);
            sender.Session.Response.guildList = new GuildListResponse();
            sender.Session.Response.guildList.Guilds.AddRange(GuildManager.Instance.GetGuildInfos());
            sender.Session.Response.guildList.Result = Result.Success;
            sender.SendResponse();
        }



        private void OnGuildJoinRequest(NetConnection<NetSession> sender, GuildJoinRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinRequest : guildId: {0}  character: {0}:{1}", message.Apply.GuildId, character.Id, character.Name);
            var guild = GuildManager.Instance.GetGuild(message.Apply.GuildId);
            if (guild == null)
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "公会不存在";
                sender.SendResponse();
                return;
            }
            message.Apply.characterId = character.Data.ID;
            message.Apply.Name = character.Data.Name;
            message.Apply.Class = character.Data.Class;
            message.Apply.Level = character.Data.Level;

            if (guild.JoinApply(message.Apply))
            {
                var leader = SessionManager.Instance.GetSession(guild.Data.LeaderID);
                if (leader != null)
                {
                    leader.Session.Response.guildJoinReq = message;
                    leader.SendResponse();
                }
            }
            else
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "请勿重复申请";
                sender.SendResponse();
            }
        }
        

        private void OnGuildJoinResponse(NetConnection<NetSession> sender, GuildJoinResponse message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinResponse : guildId: {0}  character: {0}:{1}", message.Apply.GuildId, character.Id, character.Name);
            var guild = GuildManager.Instance.GetGuild(message.Apply.GuildId);

            if (message.Result == Result.Success)
            {
                guild.JoinApprove(message.Apply);
            }

            var requester = SessionManager.Instance.GetSession(message.Apply.characterId);
            if (requester != null)
            {
                requester.Session.Character.Guild = guild;
                requester.Session.Response.guildJoinRes = message;
                requester.Session.Response.guildJoinRes.Result = Result.Success;
                requester.Session.Response.guildJoinRes.Errormsg = "加入公会成功";
                requester.SendResponse();
            }
        }
         void OnGuildLeave(NetConnection<NetSession> sender, GuildLeaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildLeave : character: {0}:{1}", character.Id, character.Name);
            sender.Session.Response.guildLeave = new GuildLeaveResponse();

            character.Guild.Leave(message.characterId);
            sender.Session.Response.guildLeave.characterId = character.Id;
            sender.Session.Response.guildLeave.Result = Result.Success;
            DBService.Instance.Save();
            sender.SendResponse();
        }


        private void OnGuildAdmin(NetConnection<NetSession> sender, GuildAdminRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildAdmin : character: {0}:{1}", character.Id, character.Name);

            sender.Session.Response.guildAdmin = new GuildAdminResponse();
            if (character.Guild == null)
            {
                sender.Session.Response.guildAdmin.Result = Result.Failed;
                sender.Session.Response.guildAdmin.Errormsg = "你没有公会退出什么玩意儿";
                sender.SendResponse();
                return;
            }

            character.Guild.ExcuteCommand(message.Command, character.Id ,message.Target);

            var target = SessionManager.Instance.GetSession(message.Target);
            if (target !=null)
            {
                target.Session.Response.guildAdmin = new GuildAdminResponse();
                target.Session.Response.guildAdmin.Result = Result.Success;
                target.Session.Response.guildAdmin.Command = message;
                target.SendResponse();
            }
            sender.Session.Response.guildAdmin.Result = Result.Success;
            sender.Session.Response.guildAdmin.Command = message;
            sender.SendResponse();
        }
    }
}
