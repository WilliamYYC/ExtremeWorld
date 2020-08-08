using Common;
using Common.Utils;
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
    public  class ChatService:Singleton<ChatService>,IDisposable
    {
        public ChatService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ChatRequest>(this.OnChat);
        }

        

        public void Dispose()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<ChatRequest>(this.OnChat);
        }

        public void Init()
        {
            ChatManager.Instance.Init();
        }

        private void OnChat(NetConnection<NetSession> sender, ChatRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnChat : chacater:{0}  channel:{1}  toid:{2}  toNmae:{3}", character.Id, request.Message.Channel, request.Message.ToId, request.Message.ToName);

            if (request.Message.Channel == ChatChannel.Private)
            {
                var chat = SessionManager.Instance.GetSession(request.Message.ToId);
                if (chat == null)
                {
                    sender.Session.Response.Chat = new ChatResponse();
                    sender.Session.Response.Chat.Result = Result.Failed;
                    sender.Session.Response.Chat.Errormsg = "对方不在线";
                    sender.SendResponse();
                }
                else
                {
                    if (chat.Session.Response.Chat == null)
                    {
                        chat.Session.Response.Chat = new ChatResponse();
                    }
                    request.Message.FromId = character.Id;
                    request.Message.FromName = character.Name;
                    chat.Session.Response.Chat.Result = Result.Success;
                    chat.Session.Response.Chat.privateMessages.Add(request.Message);
                    chat.SendResponse();

                    if (sender.Session.Response.Chat == null)
                    {
                        sender.Session.Response.Chat = new ChatResponse();
                    }
                    sender.Session.Response.Chat.Result = Result.Success;
                    sender.Session.Response.Chat.privateMessages.Add(request.Message);
                    sender.SendResponse();
                }
            }
            else
            {
                sender.Session.Response.Chat = new ChatResponse();
                sender.Session.Response.Chat.Result = Result.Success;
                ChatManager.Instance.AddMessage(character, request.Message);
                sender.SendResponse();
            }
            
            
        }
    }
}
