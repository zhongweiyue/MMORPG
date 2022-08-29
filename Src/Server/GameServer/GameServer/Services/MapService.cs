using Common;
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
        NetMessage message = new NetMessage();
        message.Response = new NetMessageResponse();
        message.Response.mapEntitySync = new MapEntitySyncResponse();
        message.Response.mapEntitySync.entitySyncs.Add(entity);
        byte[] data = PackageHandler.PackMessage(message);
        conn.SendData(data, 0, data.Length);

    }

}

