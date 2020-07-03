using Common;
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
    class FriendService : Singleton<FriendService>
    {
        public void Init()
        {

        }
        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemove);
        }
        
        void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRequest: fromID:{0}  fromName:{1} toId:{2} toName{3}", message.FromId, message.FromName, message.ToId, message.ToName);

            //=0说明没有传入ID， 使用名称进行查找
            if (message.ToId ==0)
            {
                foreach (var cha in CharacterManager.Instance.characters)
                {
                    if (cha.Value.Data.Name == message.ToName)
                    {
                        message.ToId = cha.Key;
                        break;
                    }
                }
            }

            NetConnection<NetSession> friends = null;
            if (message.ToId >0)
            {
                if (character.FriendManager.GetFriendInfo(message.ToId) != null)
                {
                    sender.Session.Response.friendAddRes = new FriendAddResponse();
                    sender.Session.Response.friendAddRes.Errormsg = "已经是好友了";
                    sender.Session.Response.friendAddRes.Result = Result.Failed;

                    sender.SendResponse();
                    return;
                }
                friends = SessionManager.Instance.GetSession(message.ToId);
            }

            if (friends ==null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Errormsg = "好友不存在或者不在线";
                sender.Session.Response.friendAddRes.Result = Result.Failed;

                sender.SendResponse();
                return;
            }
            Log.InfoFormat("ForwardRequest: fromID:{0}  fromName:{1} toId:{2} toName{3}", message.FromId, message.FromName, message.ToId, message.ToName);
            friends.Session.Response.friendAddReq = message;
            friends.SendResponse();
        }

        //收到加好友的响应
        void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse message)
        {
            Character character = sender.Session.Character;

            Log.InfoFormat("OnFriendAddResponse: CharacterId:{0}  Result:{1} fromID:{2} ToID{3}", character.Id, message.Result, message.Request.FromId, message.Request.ToId);
            sender.Session.Response.friendAddRes = message;
            var requester = SessionManager.Instance.GetSession(message.Request.FromId);
            if (message.Result == Result.Success)
            {
                if (requester == null)
                {
                    sender.Session.Response.friendAddRes.Errormsg = "请求者已离线";
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                }
                else
                {
                    character.FriendManager.AddFriend(requester.Session.Character);
                    requester.Session.Character.FriendManager.AddFriend(character);
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddRes = message;
                    requester.Session.Response.friendAddRes.Result = Result.Success;
                    requester.Session.Response.friendAddRes.Errormsg = "添加好友成功";
                    requester.SendResponse();
                }
                sender.SendResponse();
            }
            else
            {
                if (requester != null)
                {
                    requester.Session.Response.friendAddRes = message;
                    requester.Session.Response.friendAddRes.Result = Result.Failed;
                    requester.Session.Response.friendAddRes.Errormsg = "被拒绝了";
                    requester.SendResponse();
                }
            }
            

        }
        void OnFriendRemove(NetConnection<NetSession> sender, FriendRemoveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendRemove, character :{0}  friendRelationId :{1}", character.Id, message.Id);

            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.Id = message.Id;

            //删除自己的好友
            if (character.FriendManager.RemoveFriendById(message.Id))
            {
                //删除好友中的自己
                sender.Session.Response.friendRemove.Result = Result.Success;
                var friend = SessionManager.Instance.GetSession(message.friendId);
                if (friend != null)
                {
                    friend.Session.Character.FriendManager.RemoveFriendByFriendId(character.Id);
                }
                else
                {
                    this.RemoveFriend(message.friendId, character.Id);
                }
            }
            else
                sender.Session.Response.friendRemove.Result = Result.Failed;


            DBService.Instance.Save();
            sender.SendResponse();
        }

         void RemoveFriend(int CharacterId, int friendId)
        {
            var removeItem = DBService.Instance.Entities.CharacterFriends.FirstOrDefault(v => v.CharacterID == CharacterId && v.FriendID == friendId);
            if (removeItem !=null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
        }
    }
}
