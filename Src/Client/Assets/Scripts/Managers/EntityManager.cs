using Entities;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    interface IEntityNotify
    {
        void OnEntityRemoved();
    }

    class EntityManager : Singleton<EntityManager>
    {
        Dictionary<int, Entity> entityDict = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> entityNotifyDict = new Dictionary<int, IEntityNotify>();

        public void RegisterEntityChangeNotify(int entityId, IEntityNotify notify)
        {
            entityNotifyDict[entityId] = notify;
        }

        public void AddEntity(Entity entity)
        {
            entityDict[entity.entityId]=entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            entityDict.Remove(entity.Id);
            if (entityNotifyDict.ContainsKey(entity.Id))
            {
                entityNotifyDict[entity.Id].OnEntityRemoved();
                entityNotifyDict.Remove(entity.Id);
            }
        }
    }
}

