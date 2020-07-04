using Common;
using GameServer.Entities;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class TeamManager:Singleton<TeamManager>
    {
        public List<Team> TeamList = new List<Team>();
        public Dictionary<int, Team> TeamDictionary = new Dictionary<int, Team>();
        public void Init()
        {

        }
        //开放一个通过charcaterId获取队伍信息的接口
        public Team GetTeamByCharcaterId(int charcaterId)
        {
            Team team = null;
            this.TeamDictionary.TryGetValue(charcaterId, out team);
            return team;
        }
        public void AddTeamMembers(Character leader, Character member)
        {
            //判断队长有没有队伍
            if (leader.Team == null)
            {
                leader.Team = CreateTeam(leader);
            }
            leader.Team.AddMember(member);
        }


        //在内存中创建队伍
        public Team CreateTeam(Character leader)
        {
            Team team = null;
            //如果有已经创建但是为null的队伍,使用该队伍
            for (int i = 0; i < TeamList.Count; i++)
            {
                team = TeamList[i];
                if (team.members.Count == 0)
                {
                    team.AddMember(leader);
                    return team;
                }
            }
            //创建一个新的队伍
            team = new Team(leader);
            this.TeamList.Add(team);
            team.Id = this.TeamList.Count;
            return team;
        }
    }
}
