using Common;
using GameServer.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


public class MapService:Singleton<MapService>
{
    public MapService()
    {

    }

    public void Init()
    {
        MapManager.Instance.Init();
    }
}

