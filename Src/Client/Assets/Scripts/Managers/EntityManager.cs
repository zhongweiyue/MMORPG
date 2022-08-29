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
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @event);
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

        internal void OnEntitySync(NEntitySync data)
        {
            Entity entity = null;
            entityDict.TryGetValue(data.Id, out entity);
            if (entity != null)
            {
                if (data.Entity != null)
                {
                    entity.EntityData = data.Entity;
                }
                if (entityNotifyDict.ContainsKey(data.Id))
                {
                    entityNotifyDict[entity.entityId].OnEntityChanged(entity);
                    entityNotifyDict[entity.entityId].OnEntityEvent(data.Event);
                }
            }
        }
    }
}

