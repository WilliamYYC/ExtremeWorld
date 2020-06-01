using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;
using Common.Data;
using Managers;
using JetBrains.Annotations;

public class MapTools
{

    [MenuItem("Map Tools/Export Teleports")]


    public static void ExportTeleports()
    {
        DataManager.Instance.Load();

        Scene Current = EditorSceneManager.GetActiveScene();
        string CurrentScene = Current.name;

        if (Current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前的场景", "确定");
            return;

        }

        List<TeleportObject> AllTeleportObjects = new List<TeleportObject>();

        foreach (var map in DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";

            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogWarningFormat("scene {0} is not existed", sceneFile);
                continue;
            }

            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);

            TeleportObject[] teleportObjects = GameObject.FindObjectsOfType<TeleportObject>();

            foreach (var teleport in teleportObjects)
            {
                if (!DataManager.Instance.Teleporters.ContainsKey(teleport.ID))
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图 : {0} 中配置的 teleport: {1}不存在", map.Value.Resource, teleport.ID), "确定");
                    return;
                }
                TeleporterDefine def = DataManager.Instance.Teleporters[teleport.ID];
                if (def.MapID != map.Value.ID)
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图 : {0} 中配置的 teleport: {1} mapid :{2} 错误", map.Value.Resource, teleport.ID, def.MapID), "确定");
                    return;
                }

                def.Position = GameObjectTool.WorldToLogicN(teleport.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(teleport.transform.forward);
            }

        }

        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + CurrentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "传送点导出完成","确定");

    }
}
