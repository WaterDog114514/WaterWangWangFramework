using LitJson;
using System;
using Newtonsoft;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// 该插件需要配合水汪汪编辑器使用
/// </summary>
public enum JsonType
{
    JsonNet,
    JsonUtlity,
    LitJson,
}

/// <summary>
/// Json数据管理类 主要用于进行 Json的序列化存储到硬盘 和 反序列化从硬盘中读取到内存中
/// </summary>
public class JsonManager
{
    private static JsonManager instance = new JsonManager();
    public static JsonManager Instance => instance;
    /// <summary>
    /// 存储文件路径，由配置文件设置，也由水汪汪存档编辑器初始化
    /// </summary>
    public string SavePath;
    private JsonManager()
    {

    }
    /// <summary>
    ///  存储Json数据，并存储到persistentDataPath里
    /// </summary>
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        //保存到默认路径
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        Save(data, path, type);
    }
    /// <summary>
    /// 存储json到指定路径
    /// </summary>
    /// <param name="data">对象</param>
    /// <param name="path">路劲</param>
    /// <param name="type"></param>
    public void SaveDataToPath(object data, string path, JsonType type = JsonType.LitJson)
    {
        //确定存储路径
        //序列化 得到Json字符串
        Save(data, path, type);
    }
    //保存的真正过程
    private void Save(object data, string path, JsonType type)
    {
        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
            case JsonType.JsonNet:
                jsonStr = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                break;
        }
        //把序列化的Json字符串 存储到指定路径的文件中
        File.WriteAllText(path, jsonStr);
    }
    //读取指定文件中的 Json数据 反序列化
    public T LoadData<T>(string fileName, string path = null, JsonType type = JsonType.LitJson) where T : new()
    {
        //首先先判断 是不是从指定目录读取文件
        if (path == null)
            path = Application.streamingAssetsPath + "/" + fileName + ".json";
        //先判断 是否存在这个文件
        //如果不存在默认文件 就从 读写文件夹中去寻找
        if (!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";
        //如果读写文件夹中都还没有 那就返回一个默认对象
        if (!File.Exists(path))
            return new T();

        //进行反序列化
        string jsonStr = File.ReadAllText(path);
        //数据对象
        T data = default(T);
        if (jsonStr != null)
        {
            switch (type)
            {
                case JsonType.JsonUtlity:
                    data = JsonUtility.FromJson<T>(jsonStr);
                    break;
                case JsonType.LitJson:
                    data = JsonMapper.ToObject<T>(jsonStr);
                    break;
                case JsonType.JsonNet:
                    data = JsonConvert.DeserializeObject<T>(jsonStr, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    break;
            }
        }
        //把对象返回出去
        return data;
    }
    /// <summary>
    /// 从Application.streamingAssetsPath 里加载json
    /// </summary>
    /// <param name="LoadType"></param>
    /// <param name="fileName"></param>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public object LoadData(Type LoadType, string fileName, string path = null, JsonType type = JsonType.LitJson)
    {
        //首先先判断 是不是从指定目录读取文件
        if (path == null)
            path = Application.streamingAssetsPath + "/" + fileName + ".json";
        //先判断 是否存在这个文件
        //如果不存在默认文件 就从 读写文件夹中去寻找
        if (!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";
        //如果读写文件夹中都还没有 那就返回一个默认对象
        if (!File.Exists(path))
            return Activator.CreateInstance(LoadType);

        //进行反序列化
        string jsonStr = File.ReadAllText(path);
        //数据对象
        object data = null;
        if (jsonStr != null)
        {
            switch (type)
            {
                case JsonType.JsonUtlity:
                    data = JsonUtility.FromJson(jsonStr, LoadType);
                    break;
                case JsonType.LitJson:
                    data = JsonMapper.ToObject(jsonStr, LoadType);
                    break;
            }
        }
        //把对象返回出去
        return data;
    }

    /// <summary>
    /// 从指定路径泛型加载json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public T LoadDataFromPath<T>(string path, JsonType type = JsonType.LitJson) where T : new()
    {
        //首先先判断 是不是从指定目录读取文件
        if (path == null)
        {
            Debug.LogError("目标路径为空！！");
            return default(T);
        }
        //进行反序列化
        string jsonStr = File.ReadAllText(path);
        //数据对象
        T data = default(T);
        if (jsonStr != null)
        {
            switch (type)
            {
                case JsonType.JsonUtlity:
                    data = JsonUtility.FromJson<T>(jsonStr);
                    break;
                case JsonType.LitJson:
                    data = JsonMapper.ToObject<T>(jsonStr);
                    break;
                case JsonType.JsonNet:
                    data = JsonConvert.DeserializeObject<T>(jsonStr, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    break;
            }
        }
        //把对象返回出去
        return data;
    }
    /// <summary>
    /// 从指定路径非泛型版加载json
    /// </summary>
    /// <param name="LoadType"></param>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public object LoadDataFromPath(Type LoadType, string path, JsonType type = JsonType.LitJson)
    {
        //首先先判断 是不是从指定目录读取文件
        if (path == null)
        {
            Debug.LogError("目标路径为空！！");
            return Activator.CreateInstance(LoadType);
        }
        //进行反序列化
        string jsonStr = File.ReadAllText(path);
        //数据对象
        object data = default(object);
        if (jsonStr != null)
        {
            switch (type)
            {
                case JsonType.JsonUtlity:
                    data = JsonUtility.FromJson(jsonStr, LoadType);
                    break;
                case JsonType.LitJson:
                    data = JsonMapper.ToObject(jsonStr, LoadType);
                    break;
                case JsonType.JsonNet:
                    data = JsonConvert.DeserializeObject(jsonStr, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    break;
            }
        }
        //把对象返回出去
        return data;
    }
    /// <summary>
    /// 从已读取到的json内容中加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public T LoadDataFromText<T>(string content, JsonType type = JsonType.LitJson)
    {
        if (content == null || content == "") { return default(T); }
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(content);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(content);
                break;
            case JsonType.JsonNet:
                data = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                break;
        }
        return data;
    }
    /// <summary>
    /// 获取序列化后的文本内容（特殊情况用）
    /// </summary>
    /// <param name="data"></param>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetSerializableTextContent(object data,  JsonType type)
    {
        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
            case JsonType.JsonNet:
                jsonStr = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                break;
        }
        return jsonStr; 
    }
}
