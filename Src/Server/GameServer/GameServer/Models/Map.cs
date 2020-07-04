using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        //地图中的角色 ,已CharacterId为key
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();

        

        SpawnManger SpawnManger = new SpawnManger();
        public MonsterManager MonsterManager = new MonsterManager();
        //刷怪管理器
        internal Map(MapDefine define)
        {
            this.Define = define;
            this.SpawnManger.Init(this);
            this.MonsterManager.Init(this);
        }

        internal void Update()
        {
            SpawnManger.Update();
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);

            character.Info.mapId = this.ID;

            this.MapCharacters[character.Id] = new MapCharacter(conn, character);

            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;

            foreach (var kv in this.MapCharacters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                if (kv.Value.character != character)
                {
                    this.AddCharacterEnterMap(kv.Value.connection, character.Info);
                }
            }
            foreach(var kv in this.MonsterManager.Monsters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.Info);
            }
            conn.SendResponse();
        }

         void AddCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo info)
        {
            if (conn.Session.Response.mapCharacterEnter == null)
            {
                conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            }
            conn.Session.Response.mapCharacterEnter.Characters.Add(info);
            conn.SendResponse();
        }



        internal void CharacterLeave(Character character)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1}",this.Define.ID,character.Id);
            
            foreach (var kv in this.MapCharacters)
            {
                this.SendCharacterLeaveMap(kv.Value.connection, character);
            }
            this.MapCharacters.Remove(character.Id);
        }

         void SendCharacterLeaveMap(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("SendCharacterLeaveMap To {0}:{1} : Map:{2} character:{3}:{4} ", conn.Session.Character.Id, conn.Session.Character.Info.Name, this.Define.ID, character.Id,character.Data.Name);
            conn.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            conn.Session.Response.mapCharacterLeave.entityId = character.entityId;

            conn.SendResponse();
        }


        internal void UpdateEntity(NEntitySync entity)
        {
            foreach (var kv in this.MapCharacters)
            {
                if (kv.Value.character.entityId == entity.Id)
                {
                    kv.Value.character.Position = entity.Entity.Position;
                    kv.Value.character.Direction = entity.Entity.Direction;
                    kv.Value.character.Speed = entity.Entity.Speed;
                }
                else 
                {
                    MapService.Instance.SendMapEntityUpdate(kv.Value.connection, entity);
                }
            }
        }

        //怪物进入地图
        internal void MonsterEnter(Monster monster)
        {
            Log.InfoFormat("MonsterEnter: map{0} monster{1}", this.Define.ID, monster.Id);
            foreach (var item in this.MapCharacters)
            {
                this.AddCharacterEnterMap(item.Value.connection, monster.Info);
            }
        }
    }
}
