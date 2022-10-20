using Common.Data;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using System.Text;

public class MapService : Singleton<MapService>, IDisposable
{
    public int CurrentMapId { get; set; }

    public MapService()
    {
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.MapCharacterEnterResponse>(this.OnMapCharacterEnter);
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.MapEntitySyncResponse>(this.OnMapEntitySync);
    }
    public void Init()
    {

    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MapCharacterEnterResponse>(this.OnMapCharacterEnter);
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MapEntitySyncResponse>(this.OnMapEntitySync);
    }


    private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
    {
        Debug.LogFormat("OnMapCharacterEnter MapId:{0},CharacterCount:{1}", response.mapId, response.Characters.Count);
        foreach (var cha in response.Characters)
        {
            if (User.Instance.CurrentCharacter == null ||( cha.Type == CharacterType.Player&&User.Instance.CurrentCharacter.Id == cha.Id))
            {
                //当前角色切换地图
                User.Instance.CurrentCharacter = cha;
            }
            CharacterManager.Instance.AddCharacter(cha);
        }
        if (CurrentMapId != response.mapId)
        {
            EnterMap(response.mapId);
            CurrentMapId = response.mapId;
        }
    }

    private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
    {
        Debug.LogFormat("OnMapCharacterLeave CharacterID:{0}", response.entityId);
        if (response.entityId != User.Instance.CurrentCharacter.EntityId)
        {
            CharacterManager.Instance.RemoveCharacter(response.entityId);
        }
        else
        {
            CharacterManager.Instance.Clear();
        }
    }

    private void EnterMap(int mapId)
    {
        if (DataManager.Instance.Maps.ContainsKey(mapId))
        {
            MapDefine map = DataManager.Instance.Maps[mapId];
            User.Instance.currentMapData = map;
            SceneManager.Instance.LoadScene(map.Resource);
        }
        else
        {
            Debug.LogFormat("EnterMap MapID:{0} is not exit", mapId);
        }
    }

    public void SendMapEntitySync(EntityEvent entityEvent,NEntity entity)
    {
        Debug.LogFormat("MapEntityUpdateRequest: ID:{0}, Pos:{1}, DIR:{2}, SPEED:{3}", entity.Id, entity.Position.String(), entity.Direction.String(), entity.Speed);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.mapEntitySync = new MapEntitySyncRequest();
        message.Request.mapEntitySync.entitySync = new NEntitySync()
        {
            Id = entity.Id,
            Event = entityEvent,
            Entity = entity
        };
        NetClient.Instance.SendMessage(message);
    }

    private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("MapEntityUpdateResponse: Entity:{0}", response.entitySyncs.Count);
        sb.AppendLine();
        foreach (var entity in response.entitySyncs)
        {
            EntityManager.Instance.OnEntitySync(entity);
            sb.AppendFormat("[{0}]event:{1} entity:{2}", entity.Id, entity.Event, entity.Entity.String());
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

    public void SendMapTeleport(int teleporterID)
    {
        Debug.LogFormat("MapTeleporterRequest:teleporterID:{0}",teleporterID);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.mapTeleport = new MapTeleportRequest();
        message.Request.mapTeleport.teleporterId = teleporterID;
        NetClient.Instance.SendMessage(message);
    }
}
