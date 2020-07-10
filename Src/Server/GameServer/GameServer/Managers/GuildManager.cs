using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class GuildManager:Singleton<GuildManager>
    {
        public Dictionary<int, Guild> guilds = new Dictionary<int, Guild>();
        public HashSet<string> guildNames = new HashSet<string>();

        public void Init()
        {
            this.guilds.Clear();
            foreach (var guild in DBService.Instance.Entities.Guilds)
            {
                this.AddGuild(new Guild(guild));
            }
        }

        public void AddGuild(Guild guild)
        {
            this.guilds.Add(guild.Id, guild);
            this.guildNames.Add(guild.Name);
            guild.timestamp = TimeUtil.timestamp;
        }

        public Guild GetGuild(int guildId)
        {
            if (guildId == 0)
            {
                return null;
            }
            Guild guild = null;
            this.guilds.TryGetValue(guildId, out guild);
            return guild;
        }

        public bool CheckNameExisted(string guildName)
        {
            return this.guildNames.Contains(guildName);
        }

        public bool CreateGuild(string guildName, string notice, Character leader)
        {
            DateTime now = DateTime.Now;
            TGuild dbguild = DBService.Instance.Entities.Guilds.Create();
            dbguild.Name = guildName;
            dbguild.Notice = notice;
            dbguild.LeaderID = leader.Id;
            dbguild.LeaderName = leader.Name;
            dbguild.CreateTime = now;
            DBService.Instance.Entities.Guilds.Add(dbguild);

            Guild guild = new Guild(dbguild);
            guild.AddMember(leader.Id, leader.Name,leader.Data.Class, leader.Data.Level, GuildTitle.President);
            leader.Guild = guild;
            DBService.Instance.Save();
            leader.Data.GuildId = dbguild.Id;
            DBService.Instance.Save();
            this.AddGuild(guild);

            return true;
        }

        internal List<NGuildInfo> GetGuildInfos()
        {
            List<NGuildInfo> result = new List<NGuildInfo>();
            foreach (var kv in this.guilds)
            {
                result.Add(kv.Value.GuildInfo(null));
            }
            return result;
        }
    }
}
