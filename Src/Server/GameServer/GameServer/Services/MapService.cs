using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            //MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapCharacterEnterRequest>(this.OnCharacterEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleport);
        }

       
        public void Init()
        {
            MapManager.Instance.Init();
        }


       // void OnCharacterEnter(NetConnection<NetSession> sender, MapCharacterEnterRequest request)
       // {
       //
       // }

        void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnMapEntitySync: characeterId{0}:{1} Entity.Id: {2}  Evt :{3} Entity: {4}", character.Id, character.Info.Name, request.entitySync.Id,request.entitySync.Event,request.entitySync.Entity.String());
            MapManager.Instance[character.Info.mapId].UpdateEntity(request.entitySync);
        }

        internal void SendMapEntityUpdate(NetConnection<NetSession> conn, NEntitySync entity)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapEntitySync = new MapEntitySyncResponse();

            message.Response.mapEntitySync.entitySyncs.Add(entity);
            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }


         void OnMapTeleport(NetConnection<NetSession> sender, MapTeleportRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnMapTeleport character ID{0} :{1}   TeleportID:{2}",character.Id, character.Data, request.teleporterId);

            if (!DataManager.Instance.Teleporters.ContainsKey(request.teleporterId))
            {
                Log.WarningFormat("source TeleportID {0}is not existes", request.teleporterId);
                return;
            }
            TeleporterDefine SourceTeleporterDefine = DataManager.Instance.Teleporters[request.teleporterId];

            if (SourceTeleporterDefine.LinkTo == 0 || !DataManager.Instance.Teleporters.ContainsKey(SourceTeleporterDefine.LinkTo))
            {
                Log.WarningFormat("source TeleportID {0} linkto Id{1}is not existes", request.teleporterId, SourceTeleporterDefine.LinkTo);
            }


            TeleporterDefine TargetTeleporterDefine = DataManager.Instance.Teleporters[SourceTeleporterDefine.LinkTo];

            MapManager.Instance[SourceTeleporterDefine.MapID].CharacterLeave(character);
            character.Position = TargetTeleporterDefine.Position;
            character.Direction = TargetTeleporterDefine.Direction;
            MapManager.Instance[TargetTeleporterDefine.MapID].CharacterEnter(sender, character);
        }

    }
}