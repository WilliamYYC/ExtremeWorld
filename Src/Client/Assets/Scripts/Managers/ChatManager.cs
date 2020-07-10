using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    public class ChatManager:Singleton<ChatManager>
    {
        public enum LocalChannel 
        {
            All = 0,//综合
            Local = 1,
            World = 2,
            Team = 3,
            Guild = 4,
            Private = 5,//私聊
        }

        private ChatChannel[] ChannelFilter = new ChatChannel[6]
        {
            ChatChannel.Local | ChatChannel.World | ChatChannel.Team| ChatChannel.Guild| ChatChannel.Private |ChatChannel.System,
            ChatChannel.Local,
            ChatChannel.World,
            ChatChannel.Team,
            ChatChannel.Guild,
            ChatChannel.Private
        };

       

        public LocalChannel displayChannel;
        public LocalChannel sendChannel;
        public List<ChatMessage>[] Messages = new List<ChatMessage>[6]
            { 
                new List<ChatMessage>(),
                new List<ChatMessage>(),
                new List<ChatMessage>(),
                new List<ChatMessage>(),
                new List<ChatMessage>(),
                new List<ChatMessage>()
            };

        public Action OnChat { internal set; get; }
        public ChatChannel SendChannel 
        {
            get {
                switch (sendChannel)
                {
                    case LocalChannel.Local: return ChatChannel.Local;
                    case LocalChannel.World: return ChatChannel.World;
                    case LocalChannel.Team: return ChatChannel.Team;
                    case LocalChannel.Guild: return ChatChannel.Guild;
                    case LocalChannel.Private: return ChatChannel.Private;
                }
                return ChatChannel.Local;
            }
        }

        public int PrivateID = 0;
        public string PrivateName = "";


        public void Init() { }

        public void SendChat(string content, int ToId= 0, string toName="")
        {
            ChatService.Instance.sendChatMessage(this.SendChannel, content, ToId, toName);
        }
        public void startPrivateChat(int targetId, string targetName)
        {
            this.PrivateID = targetId;
            this.PrivateName = targetName;

            this.sendChannel = LocalChannel.Private;

            if (this.OnChat !=null)
            {
                this.OnChat();
            }

        }

        //设置发送频道
        public bool setSendChannel(LocalChannel localChannel)
        {
            if (localChannel == LocalChannel.Team)
            {
                if (User.Instance.TeamInfo == null)
                {
                    this.AddSystemMessage("你没有加入任何队伍");
                    return false;
                }
            }
            if (localChannel == LocalChannel.Guild)
            {
                if (User.Instance.CurrentCharacter.Guild == null)
                {
                    this.AddSystemMessage("你没有加入任何公会");
                    return false;
                }
            }
            this.sendChannel = localChannel;
            Debug.LogFormat("set  Channel: {0}", this.sendChannel);
            return true;
        }
        //把服务器的消息添加到对应的频道中
        internal void AddMessage(ChatChannel channel, List<ChatMessage> messages)
        {
            for (int i = 0; i < this.Messages.Length; i++)
            {
                if ((this.ChannelFilter[i] & channel) == channel)
                {
                    this.Messages[i].AddRange(messages);
                }
            }
            if (this.OnChat !=null)
            {
                this.OnChat();
            }
        }

        //添加系统消息
        public void AddSystemMessage(string message, string from ="")
        {
            this.Messages[(int)LocalChannel.All].Add(new ChatMessage()
            {
                Channel = ChatChannel.System,
                Message = message,
                FromName = from
            }) ;
            if (this.OnChat !=null)
            {
                this.OnChat();
            }
        }

        public string GetCurrentMessage()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var message in this.Messages[(int)displayChannel])
            {
                sb.AppendLine(FormatMessage(message));
            }
            return sb.ToString();
        }

        private string FormatMessage(ChatMessage message)
        {
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    return string.Format("[本地] {0} {1}", FormatFromPlayer(message), message.Message);
                    break;
                case ChatChannel.World:
                    return string.Format("<color=cyan>[世界] {0} {1}</color>", FormatFromPlayer(message), message.Message);
                    break;
                case ChatChannel.System:
                    return string.Format("<color=yellow>[系统] {0}</color>",  message.Message);
                    break;
                case ChatChannel.Private:
                    return string.Format("<color=cyan>[私聊] {0} {1}</color>", FormatFromPlayer(message), message.Message);
                    break;
                case ChatChannel.Team:
                    return string.Format("<color=cyan>[队伍] {0} {1}</color>", FormatFromPlayer(message), message.Message);
                    break;
                case ChatChannel.Guild:
                    return string.Format("<color=cyan>[公会] {0} {1}</color>", FormatFromPlayer(message), message.Message);
                    break;

            }
            return "";
        }

        private string FormatFromPlayer(ChatMessage message)
        {
            if (message.FromId == User.Instance.CurrentCharacter.Id)
            {
                return "<a name=\"\" class =\"player\">[你]</a>";
            }
            else
                return string.Format("<a name=\"c:{0} : {1}\" class =\"player\">[{1}]</a>", message.FromId, message.FromName);
        }
    }
}
