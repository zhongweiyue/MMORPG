﻿using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


public class MapService:Singleton<MapService>
{
    public MapService()
    {
        MessageDistributer<NetConnection<Network.NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
        MessageDistributer<NetConnection<Network.NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleporter);
    }

    public void Init()
    {
        MapManager.Instance.Init();
    }

    private void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest request)
    {
        Character character = sender.Session.Character;
        Log.InfoFormat("OnMapEntitySync: CharacterID:{0}: {1}, EntityId:{2}, Event:{3}, Entity:{4}", character.Id, character.Info.Name, request.entitySync.Id, request.entitySync.Event, request.entitySync.Entity.String());
        MapManager.Instance[character.Info.mapId].UpdateEntity(request.entitySync);
    }

    internal void SendEntityUpdate(NetConnection<NetSession>conn, NEntitySync entity)
    {

        conn.Session.Response.mapEntitySync = new MapEntitySyncResponse();
        conn.Session.Response.mapEntitySync.entitySyncs.Add(entity);
        conn.SendResponse();
    }

    private void OnMapTeleporter(NetConnection<NetSession> sender, MapTeleportRequest request)
    {
        Character character = sender.Session.Character;
        Log.InfoFormat("OnMapTeleporter: characterID:{0}:{1} TeleporterID:{2}", character.Id, character.Data, request.teleporterId);
        if (!DataManager.Instance.Teleporters.ContainsKey(request.teleporterId))
        {
            Log.WarningFormat("Source TeleporterID [{0}] not exited ",request.teleporterId);
            return;
        }
        TeleporterDefine source = DataManager.Instance.Teleporters[request.teleporterId];
        if (source.LinkTo == 0 || !DataManager.Instance.Teleporters.ContainsKey(source.LinkTo))
        {
            Log.WarningFormat("Source TeleporterID [{0}] LinkTo ID [{1}] not exited ", request.teleporterId, source.LinkTo);
        }
        TeleporterDefine target = DataManager.Instance.Teleporters[source.LinkTo];
        MapManager.Instance[source.MapID].CharacterLeave(character);
        character.Position = target.Position;
        character.Direction = target.Direction;
        MapManager.Instance[target.MapID].CharacterEnter(sender, character);
    }
}

