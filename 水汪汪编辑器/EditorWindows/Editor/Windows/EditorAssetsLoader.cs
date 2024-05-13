using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// 编辑器专用资源加载模块
/// </summary>
public class EditorAssetsLoader
{
    public static EditorAssetsLoader Instance { get; private set; } = new EditorAssetsLoader();
    /// <summary>
    /// 通过id，获取到场景和资源Asset中的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T FindWithID<T>(int id) where T : Object
    {
        //先获Asset中的取预设体对象
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T prefab = AssetDatabase.LoadAssetAtPath<T>(path);
            if (prefab != null && prefab.GetInstanceID() == id)
            {
                return prefab;
            }
        }
        //再通过场景上去遍历
        //只有Gameobject才能放到场景上呀
        if (typeof(T) != typeof(GameObject)) return null;
        GameObject[] sceneGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in sceneGameObjects)
        {
            if (obj.GetInstanceID() == id)
            {
                return obj as T;
            }
        }
        return null;
    }
}
