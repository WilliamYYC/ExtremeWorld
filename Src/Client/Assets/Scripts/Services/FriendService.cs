using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    class FriendService : Singleton<FriendService>, IDisposable
    {
        public UnityAction OnFriendUpdate;
        public void Init()
        { }

        public FriendService()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }

        //发送请求
        public void SendFriendAddRequest(int friendID, string friendName)
        {
            Debug.Log("SendFriendAddRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddReq = new FriendAddRequest();
            message.Request.friendAddReq.FromId = User.Instance.CurrentCharacter.Id;
            message.Request.friendAddReq.FromName = User.Instance.CurrentCharacter.Name;
            message.Request.friendAddReq.ToId = friendID;
            message.Request.friendAddReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);
        }

        public void SendFriendAddResponse(bool Accept, FriendAddRequest request)
        {
            Debug.Log("SendFriendAddResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddRes = new FriendAddResponse();
            message.Request.friendAddRes.Result = Accept ? Result.Success : Result.Failed;
            message.Request.friendAddRes.Errormsg = Accept ? "对方接受了你的请求" : "对方拒绝了你的请求";
            message.Request.friendAddRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }

        public  void SendFriendRemoveRequest(int characterId, int friendId)
        {
            Debug.Log("SendFriendRemoveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendRemove = new FriendRemoveRequest();
            message.Request.friendRemove.Id = characterId;
            message.Request.friendRemove.friendId = friendId;
            NetClient.Instance.SendMessage(message);
        }


        //请求的响应
         void OnFriendRemove(object sender, FriendRemoveResponse response)
        {
            if (response.Result == Result.Success)
            {
                MessageBox.Show("好友删除成功");
            }
            else
            {
                MessageBox.Show("好友删除失败");
            }
        }

         void OnFriendList(object sender, FriendListResponse response)
        {
            Debug.Log("OnFriendList");
            FriendManager.Instance.allFriends = response.Friends;
            if (this.OnFriendUpdate!=null)
            {
                this.OnFriendUpdate();
            }
        }

         void OnFriendAddResponse(object sender, FriendAddResponse response)
        {
            if (response.Result == Result.Success)
            {
                MessageBox.Show(response.Request.ToName + "接受了你的好友请求","添加好友成功");
            }
            else
            {
                MessageBox.Show(response.Errormsg, "添加好友失败");
            }
        }

         void OnFriendAddRequest(object sender, FriendAddRequest request)
        {
            var func = MessageBox.Show(string.Format("{0}请求加你为好友", request.FromName), "好友请求", MessageBoxType.Confirm,"接受","拒绝");
            func.OnYes = () => { this.SendFriendAddResponse(true, request); };

            func.OnNo = () => { this.SendFriendAddResponse(false, request); };
        }
    }
}
