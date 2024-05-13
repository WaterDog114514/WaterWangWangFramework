using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// 配置文件加载器，专门加载配置文件
/// </summary>
public class SettingDataLoader : Singleton_UnMono<SettingDataLoader>
{
    /// <summary>
    /// 已经加载过的数据文件，有的话直接从里面拿
    /// </summary>
    private Dictionary<string, BaseSettingData> dic_LoadedData = new Dictionary<string, BaseSettingData>();
    /// <summary>
    /// 只有在编辑器模式下才能保存
    /// </summary>
    public void SaveData<T>(T data) where T : BaseSettingData
    {
#if UNITY_EDITOR
        //再次检查
        if (!Directory.Exists(data.DirectoryPath))
        {
            Directory.CreateDirectory(data.DirectoryPath);
        }
        JsonManager.Instance.SaveDataToPath(data, data.DirectoryPath + data.DataName + ".json", JsonType.JsonNet);
        //卸载资源，防止BUG
        Resources.UnloadAsset(Resources.Load<TextAsset>(data.DataName));
#endif
    }

    public T LoadData<T>() where T : BaseSettingData, new()
    {

        //加载过直接拿了直接润了
        if (dic_LoadedData.ContainsKey(typeof(T).Name))
            return dic_LoadedData[typeof(T).Name] as T;

        //第一次加载
        T data = new T();
        TextAsset asset = Resources.Load<TextAsset>(data.DataName);
        if (asset != null)
        {
            data = JsonManager.Instance.LoadDataFromText<T>(asset.text, JsonType.JsonNet);
        }
        else
        {
            Debug.LogWarning("已第一次创建数据文件："+typeof(T).Name);
        }
        return data;

    }


}

public abstract class BaseSettingData
{
    public BaseSettingData()
    {
        IntiValue();
    }

    //存在Resources里，方便游打包后读取
    [JsonIgnore]
    public abstract string DirectoryPath { get; }// =>
    [JsonIgnore]
    public abstract string DataName { get; } //=> 
    /// <summary>
    /// 第一次初始化方法
    /// </summary>
    public abstract void IntiValue();
}