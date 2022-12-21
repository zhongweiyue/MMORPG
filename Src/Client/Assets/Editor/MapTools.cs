﻿using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MapTools 
{
    [MenuItem("MapTools/Export Teleporters")]
    public static void ExportTeleporters()
    {
        DataManager.Instance.Load();
        Scene current = EditorSceneManager.GetActiveScene();
        string currentScene = current.name;
        if (current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "确定");
            return;
        }
        List<TeleporterObject> allTeleporters = new List<TeleporterObject>();
        foreach (var map in DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogFormat("Scene {0} not exited", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);//编辑器模式下一个个打开场景
            TeleporterObject[] teleporters = GameObject.FindObjectsOfType<TeleporterObject>();
            foreach (var teleporter in teleporters)
            {
                if (!DataManager.Instance.Teleporters.ContainsKey(teleporter.ID))
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图: {0} 中配置的 teleporter:[{1}]中不存在",map.Value.Resource,teleporter.ID), "确定");
                    return;
                }
                TeleporterDefine teleporterDef = DataManager.Instance.Teleporters[teleporter.ID];
                if (teleporterDef.MapID != map.Value.ID)
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图: {0} 中配置的 teleporter:[{1}] mapID:[{2}]错误", map.Value.Resource, teleporter.ID,teleporterDef.MapID), "确定");
                    return;
                }
                teleporterDef.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                teleporterDef.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
            }
        }
        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "传送点导出完成", "确定");
    }

    [MenuItem("MapTools/Export SpawnPoints")]
    public static void ExportSpawnPoints()
    {
        DataManager.Instance.Load();
        Scene current = EditorSceneManager.GetActiveScene();
        string currentScene = current.name;
        if (current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "确定");
            return;
        }
        if (DataManager.Instance.SpawnPoints == null) 
        {
            DataManager.Instance.SpawnPoints = new Dictionary<int, Dictionary<int, SpawnPointDefine>>();
        }

        foreach (var map in DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogFormat("Scene {0} not exited", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);//编辑器模式下一个个打开场景
            SpawnPoint[] spawnpoints = GameObject.FindObjectsOfType<SpawnPoint>();
            if (!DataManager.Instance.SpawnPoints.ContainsKey(map.Value.ID)) 
            {
                DataManager.Instance.SpawnPoints[map.Value.ID] = new Dictionary<int, SpawnPointDefine>();
            }

            foreach (var sp in spawnpoints)
            {
                if (!DataManager.Instance.SpawnPoints[map.Value.ID].ContainsKey(sp.ID))
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图: {0} 中配置的 teleporter:[{1}]中不存在", map.Value.Resource, sp.ID), "确定");
                    return;
                }
                SpawnPointDefine def = DataManager.Instance.SpawnPoints[map.Value.ID][sp.ID];
                def.ID = sp.ID;
                def.MapID = map.Value.ID;
                def.Position = GameObjectTool.WorldToLogicN(sp.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(sp.transform.forward);
            }
        }
        DataManager.Instance.SaveSpawnPoints();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "刷怪点导出完成", "确定");
    }

    [MenuItem("MapTools/Generate NavData")]
    public static void GenerateNavData() 
    {
        Material red = new Material(Shader.Find("Standard"));
        red.color = Color.red;
        red.SetColor("_Color", Color.red);
        red.enableInstancing = true;
        GameObject go = GameObject.Find("MinimapBoundingBox");
        if (go != null) 
        {
            GameObject root = new GameObject("Root");
            BoxCollider bound = go.GetComponent<BoxCollider>();
            float step = 1f;
            for (float x = bound.bounds.min.x; x < bound.bounds.max.x; x += step) 
            {
                for (float z = bound.bounds.min.z; z < bound.bounds.max.z; z += step)
                {
                    for (float y = bound.bounds.min.y; y < bound.bounds.max.y+5f; y += step)
                    {
                        var pos = new Vector3(x, y, z);
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(pos, out hit, 0.5f, NavMesh.AllAreas)) 
                        {
                            if (hit.hit) 
                            {
                                //mapData[x][y][z]= 1;
                                var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                box.name = "Hit" + hit.mask;
                                box.GetComponent<MeshRenderer>().sharedMaterial = red;
                                box.transform.SetParent(root.transform, true);
                                box.transform.position = pos;
                                box.transform.localScale = Vector3.one * 0.9f;
                            }
                        }
                    }
                }
            }
        }
    }
}
