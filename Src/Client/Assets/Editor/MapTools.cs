﻿using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
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
}