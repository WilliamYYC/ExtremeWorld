using Common;
using Common.Utils;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
     class ChatManager:Singleton<ChatManager>
    {
        public Dictionary<int, List<ChatMessage>> locals = new Dictionary<int, List<ChatMessage>>();//本地消息
        public List<ChatMessage> systems = new List<ChatMessage>();//系统
        public List<ChatMessage> worlds = new List<ChatMessage>();//世界
        public Dictionary<int, List<ChatMessage>> teams = new Dictionary<int, List<ChatMessage>>();//队伍消息
        public Dictionary<int, List<ChatMessage>> guilds = new Dictionary<int, List<ChatMessage>>();//公会消息
        public void Init()
        { 
        }

        public void AddMessage(Character from, ChatMessage message)
        {
            message.FromId = from.Id;
            message.FromName = from.Name;
            message.Time = TimeUtil.timestamp;

            switch (message.Channel)
            {
                case ChatChannel.Local:
                    this.AddLocalMessage(from.Info.mapId, message);
                    break;
                case ChatChannel.World:
                    this.AddWorldMessage(message);
                    break;
                case ChatChannel.System:
                    this.AddSystemMessage(message);
                    break;
                case ChatChannel.Team:
                    this.AddTeamMessage(from.Team.Id, message);
                    break;
                case ChatChannel.Guild:
                    this.AddGuildMessage(from.Guild.Id, message);
                    break;
            }
        }

      

        public void AddLocalMessage(int mapId, ChatMessage message)
        {
            if (!locals.TryGetValue(mapId, out List<ChatMessage> messages))
            {
                messages = new List<ChatMessage>();
                this.locals[mapId] = messages;
            }
            messages.Add(message);
        }

        public void AddWorldMessage(ChatMessage message)
        {
            this.worlds.Add(message);
        }

        public void AddSystemMessage(ChatMessage message)
        {
            this.systems.Add(message);
        }

        public void AddTeamMessage(int teamId, ChatMessage message)
        {
            if (!teams.TryGetValue(teamId, out List<ChatMessage> messages))
            {
                messages = new List<ChatMessage>();
                this.teams[teamId] = messages;
            }
            messages.Add(message);
        }

        public void AddGuildMessage(int guildId, ChatMessage message)
        {
            if (!guilds.TryGetValue(guildId, out List<ChatMessage> messages))
            {
                messages = new List<ChatMessage>();
                this.guilds[guildId] = messages;
            }
            messages.Add(message);
        }

        internal int GetLocalMessages(int mapId, int localIdx, List<ChatMessage> result)
        {
            if (!locals.TryGetValue(mapId, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetMessage(localIdx, result, messages);
        }

        internal int GetWorldMessages(int worldIdx, List<ChatMessage> result)
        {
            return GetMessage(worldIdx, result, this.worlds);
        }

        internal int GetSystemMessages(int systemIdx, List<ChatMessage> result)
        {
            return GetMessage(systemIdx, result, this.systems);
        }

        internal int GetTeamMessages(int teamId ,int teamIdx, List<ChatMessage> result)
        {
            if (!teams.TryGetValue(teamId, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetMessage(teamIdx, result, messages);
        }

        internal int GetGuildMessages(int guildId, int guildIdx, List<ChatMessage> result)
        {
            if (!guilds.TryGetValue(guildId, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetMessage(guildIdx, result, messages);
        }

        public int GetMessage(int idx, List<ChatMessage> result, List<ChatMessage> messages)
        {
            if (idx == 0)
            {
                if (messages.Count > GameDefine.MaxChatRecordNums)
                {
                    idx = messages.Count - GameDefine.MaxChatRecordNums;
                }
            }

            for (; idx < messages.Count; idx++)
            {
                result.Add(messages[idx]);
            }
                        
            return idx;
        }
    }
}
