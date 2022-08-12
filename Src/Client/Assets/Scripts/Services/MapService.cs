using Common.Data;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class MapService : Singleton<MapService>, IDisposable
{
    public int CurrentMapId { get; private set; }

    public MapService()
    {
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.MapCharacterEnterResponse>(this.OnMapCharacterEnter);
    }
    public void Init()
    {

    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MapCharacterEnterResponse>(this.OnMapCharacterEnter);
    }


    private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
    {
        Debug.LogFormat("OnMapCharacterEnter MapId:{0},CharacterCount:{1}", response.mapId, response.Characters.Count);
        foreach (var cha in response.Characters)
        {
            if (User.Instance.CurrentCharacter.Id == cha.Id)
            {
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
}
