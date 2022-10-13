using Common;
using Common.Data;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class Spawner
    {
        public SpawnRuleDefine Define { get; set; }

        private Map Map;

        private float spawnTime = 0;//刷新时间
        private float unspawnTime = 0;//消失时间
        private bool spawned = false;//是否生成过了
        private SpawnPointDefine spawnPoint = null;

        public Spawner(SpawnRuleDefine define, Map map) 
        {
            this.Define = define;
            this.Map = map;
            if (DataManager.Instance.SpawnPoints.ContainsKey(Map.ID)) 
            {
                if (DataManager.Instance.SpawnPoints[Map.ID].ContainsKey(this.Define.SpawnPoint))
                {
                    spawnPoint = DataManager.Instance.SpawnPoints[Map.ID][this.Define.SpawnPoint];
                }
                else 
                {
                    Log.ErrorFormat("SpawnRule[{0}] SpawnPoint[{1}] not exited",this.Define.ID,this.Define.SpawnPoint);
                }
            }
        }

        public void Update() 
        {
            if (CanSpawn()) 
            {
                Spawn();
            }
        }

        bool CanSpawn() 
        {
            if (this.spawned)
                return false;
            if (this.unspawnTime + this.Define.SpawnPeriod > Time.time)
                return false;
            return true;
        }

        public void Spawn() 
        {
            this.spawned = true;
            Log.InfoFormat("Map[{0}] Spawn[{1}:Mon:{2},Lv:{3}] At Point:{4}",this.Define.MapID,this.Define.ID,this.Define.SpawnMonID,this.Define.SpawnLevel,this.Define.SpawnPoint);
            Map.monsterManager.Create(this.Define.SpawnMonID, this.Define.SpawnLevel, this.spawnPoint.Position, this.spawnPoint.Direction);
        }
    }
}
