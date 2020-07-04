using Common;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class TeamServices:Singleton<TeamServices>
    {
        public void Init()
        {
            TeamManager.Instance.Init();
        }
        public TeamServices()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamLeaveRequest>(this.OnTeamLeave);
        }

        void OnTeamInviteRequest(NetConnection<NetSession> sender, TeamInviteRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteRequest: fromID:{0}  fromName:{1} toId:{2} toName{3}", message.FromId, message.FromName, message.ToId, message.ToName);

            //队伍已经存在了
            if (character.Team != null)
            {
                //不是队长不能邀请队员加入队伍
                if (character.Team.Leader.Id != message.FromId)
                {
                    sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                    sender.Session.Response.teamInviteRes.Errormsg = "只有队长才能邀请队员";
                    sender.Session.Response.teamInviteRes.Result = Result.Failed;
                    sender.SendResponse();
                    return;
                }
            }
            
            //获取目标用户的session
            var target = SessionManager.Instance.GetSession(message.ToId);
            //目标用户不存在 发送失败响应
            if (target == null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Errormsg = "目标用户不在线";
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.SendResponse();
                return;
            }
            //目标用户已经有队伍了 发送失败响应
            if (target.Session.Character.Team !=null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Errormsg = "目标用户已经有队伍了";
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.SendResponse();
                return;
            }
            Log.InfoFormat("ForwardTeamInviteRequest: fromID:{0}  fromName:{1} toId:{2} toName{3}", message.FromId, message.FromName, message.ToId, message.ToName);
            target.Session.Response.teamInviteReq = message;
            target.SendResponse();
        }

        void OnTeamInviteResponse(NetConnection<NetSession> sender, TeamInviteResponse message)
        {
            Character character = sender.Session.Character;

            Log.InfoFormat("OnTeamInviteResponse: CharacterId:{0}  Result:{1} fromID:{2} ToID{3}", character.Id, message.Result, message.Request.FromId, message.Request.ToId);
            sender.Session.Response.teamInviteRes = message;

            
            
            if (message.Result == Result.Success)
            {
                var requester = SessionManager.Instance.GetSession(message.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.teamInviteRes.Errormsg = "邀请组队的人不在线了";
                    sender.Session.Response.teamInviteRes.Result = Result.Failed;
                }
                else
                {
                    TeamManager.Instance.AddTeamMembers(requester.Session.Character, character);
                    requester.Session.Response.teamInviteRes = message;
                    requester.Session.Response.teamInviteRes.Result = Result.Success;
                    requester.Session.Response.teamInviteRes.Errormsg = "组队成功";
                    
                    requester.SendResponse();
                }
            }
            sender.SendResponse();
        }

        void OnTeamLeave(NetConnection<NetSession> sender, TeamLeaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamLeave, character :{0}  TeamID :{1}:{2]", character.Id, message.TeamId,message.characterId);

            sender.Session.Response.teamLeave = new TeamLeaveResponse();
            sender.Session.Response.teamLeave.characterId = message.characterId;

            sender.Session.Response.teamLeave.Result = Result.Success;

            character.Team.Leave(character);

            sender.SendResponse();
        }
    }
}
