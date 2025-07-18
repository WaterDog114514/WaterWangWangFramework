using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// 模块系统设置的配置文件加载器，专门加载配置文件
/// 存储位置为StreamingAssets
/// </summary>
public class SystemSettingLoader : Singleton<SystemSettingLoader>
{
    /// <summary>
    /// 存储文件夹位置
    /// </summary>
    private string DirectoryPath => Application.streamingAssetsPath + "\\SettingData";
    /// <summary>
    /// 已经加载过的数据文件，有的话直接从里面拿
    /// </summary>
    private Dictionary<string, BaseSettingData> dic_LoadedData = new Dictionary<string, BaseSettingData>();
    /// <summary>
    /// 只有在编辑器模式下才能保存
    /// </summary>
#if UNITY_EDITOR

    public void SaveData<T>(T data) where T : BaseSettingData
    {
        //PC端存储方法
        //检验文件夹是否存在
        if (!Directory.Exists(DirectoryPath))
            Directory.CreateDirectory(DirectoryPath);
        //再次检查
        BinaryManager.SaveToPath(data, Path.Combine(DirectoryPath, $"{typeof(T).Name}.settingdata"));
    }
#endif
    public T LoadData<T>() where T : BaseSettingData, new()
    {

        //加载过直接拿了直接润了
        if (dic_LoadedData.ContainsKey(typeof(T).Name))
            return dic_LoadedData[typeof(T).Name] as T;
        //PC端加载
#if UNITY_STANDALONE_WIN
        //第一次加载
        var LoadPath = Path.Combine(DirectoryPath, $"{typeof(T).Name}.settingdata");
        //先判断有没有路径
        if (!File.Exists(LoadPath))
        {
            Debug.LogWarning($"配置加载失败，不存在该路径{LoadPath}，已创建默认配置");
            var defaultData = Activator.CreateInstance(typeof(T)) as T;
            return defaultData;
        }
        //存在再进行加载
        T data = BinaryManager.Load<T>(LoadPath);
        return data;
#endif
        //安卓端加载 等待写

    }


}
