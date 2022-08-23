using Common;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


class EntityManager:Singleton<EntityManager>
{
    private int index = 0;
    public List<Entity> AllEntityList = new List<Entity>();
    public Dictionary<int, List<Entity>> MapEntityDict = new Dictionary<int, List<Entity>>();//地图中所有在线玩家实体

    public void AddEntity(int mapId,Entity entity)
    {
        AllEntityList.Add(entity);
        //加入管理器生成唯一id
        entity.EntityData.Id = ++index;
        List<Entity> entityList = null;
        if (!MapEntityDict.TryGetValue(mapId, out entityList))
        {
            entityList = new List<Entity>();
            MapEntityDict[mapId] = entityList;
        }
        entityList.Add(entity);
    }

    public void RemoveEntity(int mapId, Entity entity)
    {
        AllEntityList.Remove(entity);
        MapEntityDict[mapId].Remove(entity);
    }
}

