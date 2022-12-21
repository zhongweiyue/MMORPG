﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavPathRenderer : MonoSingleton<NavPathRenderer>
{
    LineRenderer pathRender;
    NavMeshPath path;

    private void Start()
    {
        pathRender = GetComponent<LineRenderer>();
        pathRender.enabled = false;
    }

    public void SetPath(NavMeshPath path, Vector3 target) 
    {
        this.path = path;
        if (this.path == null)
        {
            pathRender.enabled = false;
            pathRender.positionCount = 0;
        }
        else 
        {
            pathRender.enabled = true;
            pathRender.positionCount = path.corners.Length + 1;
            pathRender.SetPositions(path.corners);
            pathRender.SetPosition(pathRender.positionCount - 1, target);//设置终点
            for (int i = 0; i < pathRender.positionCount; i++) //所有的点都往上挪一些
            {
                pathRender.SetPosition(i, pathRender.GetPosition(i) + Vector3.up * 0.2f);
            }
        }
    }
}
