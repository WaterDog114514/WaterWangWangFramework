using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 编辑器中加载AB包
/// </summary>
public class main_EditorABLoader 
{
    private FrameworkSettingData settingData;



    //用于放置需要打包进AB包中的资源路径 
    private string rootPath => settingData.abLoadSetting.ABEditorLoadPath;

    public main_EditorABLoader()
    {
        settingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
    }

    //1.加载单个资源的
    public T LoadEditorRes<T>(string path) where T : Object
    {
#if UNITY_EDITOR
        //后缀名，需自己完善，因为加载路径 需要填写后缀名一起
        string suffixName = getSuffixName<T>();
        T res = AssetDatabase.LoadAssetAtPath<T>(rootPath + path + suffixName);
        return res;
#else
        return null;
#endif
    }
    //待完成 获取某路径所有资源
    public Object[] LoadEditorAllRes(string path)
    {
#if UNITY_EDITOR
         //后缀名，需自己完善，因为加载路径 需要填写后缀名一起
        //  string suffixName = getSuffixName<T>();
       // T res = AssetDatabase.LoadAllAssetsAtPath<T>(rootPath + path + suffixName);
        return null;
#else
        return null;
#endif
    }
    private string getSuffixName<T>()
    {
        //预设体、纹理（图片）、材质球、音效等等
        if (typeof(T) == typeof(GameObject))
           return".prefab";
        else if (typeof(T) == typeof(Material))
            return ".mat";
        else if (typeof(T) == typeof(Texture))
            return ".png";
        else if (typeof(T) == typeof(AudioClip))
            return  ".mp3";
        return null;
    }
    //2.加载图集相关资源的
    public Sprite LoadSprite(string path, string spriteName)
    {
#if UNITY_EDITOR
        //加载图集中的所有子资源 
        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
        //遍历所有子资源 得到同名图片返回
        foreach (var item in sprites)
        {
            if (spriteName == item.name)
                return item as Sprite;
        }
        return null;
#else
        return null;
#endif
    }

    //加载图集文件中的所有子图片并返回给外部
    public Dictionary<string, Sprite> LoadSprites(string path)
    {
#if UNITY_EDITOR
        Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
        foreach (var item in sprites)
        {
            spriteDic.Add(item.name, item as Sprite);
        }
        return spriteDic;
#else
        return null;
#endif
    }


}
