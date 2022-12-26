using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers 
{
    public class NavManager: Singleton<NavManager>
    {
        private NpcDefine npcDefine;
        private bool navState;
        public bool NavState 
        {
            get { return navState; }
            set { navState = value; }
        }

        private bool loadSceneComplete;
        public bool LoadSceneComplete 
        {
            get { return loadSceneComplete; }
            set { loadSceneComplete = value; }
        }

        private int navTargetNpc = 0;
        public int NavTargetNpc
        {
            get{ return navTargetNpc; }
            set { navTargetNpc = value; }
        }
        public Vector3 GetNpcPos(int npcId)
        {
            npcDefine = NpcManager.Instance.GetNpcDefine(npcId);
            if (npcDefine.Map == User.Instance.CurrentCharacter.mapId)
            {
                return NpcManager.Instance.GetNpcPosition(npcId);
            }
            else
            {
                TeleporterDefine teleporterDefine;
                if (DataManager.Instance.Teleporters.TryGetValue(GetNavTeleporterID(), out teleporterDefine))
                {
                    return GameObjectTool.LogicToWorld(teleporterDefine.Position);
                }
                return Vector3.zero;
            }
        }

        private int GetNavTeleporterID()
        {
            int teleporterId = 0;
            foreach (var teleporter in DataManager.Instance.Teleporters)
            {
                if (teleporter.Value.MapID == User.Instance.CurrentCharacter.mapId && DataManager.Instance.Teleporters[teleporter.Value.LinkTo].MapID == npcDefine.Map)
                {
                    teleporterId = teleporter.Key;
                    break;
                }
            }
            return teleporterId;
        }
    }
}

