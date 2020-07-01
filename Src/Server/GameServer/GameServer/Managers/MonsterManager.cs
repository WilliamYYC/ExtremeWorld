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
    class MonsterManager
    {
        private Map Map;

        public Dictionary<int, Monster> Monsters = new Dictionary<int, Monster>();
        public  void Init(Map map)
        {
            this.Map = map;
        }

        //创建怪物
        public Monster Create(int SpawnMonId, int SpawnLevel, NVector3 position, NVector3 direction)
        {
            Monster mon = new Monster(SpawnMonId, SpawnLevel, position, direction);

            EntityManager.Instance.AddEntity(this.Map.ID, mon);
            mon.Info.Id = mon.entityId;
            mon.Info.mapId = this.Map.ID;
            Monsters[mon.Id] = mon;

            this.Map.MonsterEnter(mon);
            return mon;
        }

    }
}
