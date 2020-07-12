using System;
using Network;
using UnityEngine;
using Models;
using SkillBridge.Message;
using Common.Data;
using Managers;

namespace Services
{
    class MapService: Singleton<MapService>, IDisposable
    {
        public int CurrentMapId = 0;
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }


        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        

        public void Init()
        {

        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse message)
        {
            
            Debug.LogFormat("OnMapCharacterEnter: map: {0} count: {1}", message.mapId, message.Characters.Count);

            foreach (var kv in message.Characters)
            {
                if (User.Instance.CurrentCharacter == null ||  (kv.Type == CharacterType.Player && kv.Id == User.Instance.CurrentCharacter.Id))
                {
                    User.Instance.CurrentCharacter = kv;
                }
                CharacterManager.Instance.AddCharacter(kv);
            }
            if (CurrentMapId != message.mapId)
            {
                this.EnterMap(message.mapId);
                CurrentMapId = message.mapId;
            }
        }

       

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse message)
        {
            Debug.LogFormat("OnMapCharacterLeave:charID:{0}", message.entityId);
           //如果退出用户是自己删除所有信息,不是就删除退出的用户信息
            if (message.entityId != User.Instance.CurrentCharacter.EntityId)
            {
                CharacterManager.Instance.RemoveCharacter(message.entityId);
            }
            else
            {
                CharacterManager.Instance.clear();
            }
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine mpdefine = DataManager.Instance.Maps[mapId];
                Models.User.Instance.CurrentMapData = mpdefine;
                SceneManager.Instance.LoadScene(mpdefine.Resource);
            }
            else
                Debug.LogFormat("EnterMap: map :{0} is not exist", mapId);
        }

        public void SendMapEntitySync(EntityEvent entityEvent, NEntity entity, int param)
        {
            Debug.LogFormat("MapEntityUpdateRequest： ID {0} POS {1} DIR {2} SPEED {3}", entity.Id, entity.Position, entity.Direction, entity.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Event = entityEvent,
                Entity = entity,
                Param = param
            };
            NetClient.Instance.SendMessage(message);
        }

        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.AppendFormat("MapEntitySyncResponse: Entitise: {0}", response.entitySyncs.Count);
            builder.AppendLine();

            foreach (var entity in response.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);
                builder.AppendFormat("        [{0}] evt{1}  entity{2} ", entity.Id, entity.Event, entity.Entity.String());
                builder.AppendLine();
            }
            Debug.LogFormat(builder.ToString());
        }



        internal void sendMapTeleport(int TeLeportId)
        {
            Debug.LogFormat("MapTeleportRequest： teleporterIdID {0}", TeLeportId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = TeLeportId;
            NetClient.Instance.SendMessage(message);
        }
    }
}
