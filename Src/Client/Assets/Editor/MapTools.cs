using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;
using Common.Data;
using Managers;
using JetBrains.Annotations;
using UnityEngine.AI;

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



    [MenuItem("Map Tools/Export SpawnPoint")]
    public static void ExportSpawnPoint()
    {
        DataManager.Instance.Load();

        Scene Current = EditorSceneManager.GetActiveScene();
        string CurrentScene = Current.name;

        if (Current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前的场景", "确定");
            return;

        }

        if (DataManager.Instance.SpawnPoints ==null)
        {
            DataManager.Instance.SpawnPoints = new Dictionary<int, Dictionary<int, SpawnPointDefine>>();
        }
       

        foreach (var map in DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";

            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogWarningFormat("scene {0} is not existed", sceneFile);
                continue;
            }

            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);

            SpawnPoint[] SpawnPointObjects = GameObject.FindObjectsOfType<SpawnPoint>();
            if (!DataManager.Instance.SpawnPoints.ContainsKey(map.Value.ID))
            {
                DataManager.Instance.SpawnPoints[map.Value.ID] = new Dictionary<int, SpawnPointDefine>();
            }
            foreach (var spawnPoint in SpawnPointObjects)
            {
                if (!DataManager.Instance.SpawnPoints[map.Value.ID].ContainsKey(spawnPoint.ID))
                {
                    DataManager.Instance.SpawnPoints[map.Value.ID][spawnPoint.ID] = new SpawnPointDefine();
                }
                SpawnPointDefine def = DataManager.Instance.SpawnPoints[map.Value.ID][spawnPoint.ID];
                def.ID = spawnPoint.ID;
                def.MapID = map.Value.ID;
                def.Position = GameObjectTool.WorldToLogicN(spawnPoint.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(spawnPoint.transform.forward);
            }

        }

        DataManager.Instance.SaveSpawnPoints();
        EditorSceneManager.OpenScene("Assets/Levels/" + CurrentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "怪物刷新点导出完成", "确定");

    }


    [MenuItem("Map Tools/Generate NavData")]
    public static void GenerateNavData()
    {
        Material material = new Material(Shader.Find("Particles/Alpha Blended"));
        material.color = Color.red;
        material.SetColor("_TintColor", Color.red);
        material.enableInstancing = true;
        GameObject go = GameObject.Find("UIWorldBounds");

        if (go !=null)
        {
            GameObject root = new GameObject("Root");
            BoxCollider boxCollider = go.GetComponent<BoxCollider>();
            float step = 1f;
            for (float x = boxCollider.bounds.min.x; x < boxCollider.bounds.max.x; x+=step)
            {
                for (float z = boxCollider.bounds.min.z; z < boxCollider.bounds.max.z; z += step)
                {
                    for (float y= boxCollider.bounds.max.y; y < boxCollider.bounds.max.y + 5f; y+= step)
                    {
                        var pos = new Vector3(x, y, z);
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(pos, out hit, 0.5f, NavMesh.AllAreas))
                        {
                            if (hit.hit)
                            {
                                var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                box.name = "Hit" + hit;
                                box.GetComponent<MeshRenderer>().sharedMaterial = material;
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
