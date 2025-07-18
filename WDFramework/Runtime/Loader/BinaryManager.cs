using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// 二进制序列化辅助类
/// </summary>
public static class BinaryManager
{
    [SerializeField]
    /// <summary>
    /// 存储类对象数据到perisistent文件夹中
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fileName"></param>
    public static void SaveToPersistent(object obj, string fileName, string path = null)
    {
        if (path == null)
            path = Application.persistentDataPath + "/";
        //先判断路径文件夹有没有
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        SaveToPath(obj, path + "/" + fileName);
    }
    public static void SaveToPath(object obj, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception("序列化时候path为null");
        }
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }
    }

    /// <summary>
    /// 通过泛型加载，读取2进制数据转换成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T Load<T>(string path) where T : class
    {
        return Load(typeof(T), path) as T;
    }
    /// <summary>
    /// 通过Type类型加载,读取2进制数据转换成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static object Load(Type loadType, string path)
    {
        //先判断路径文件有没有
        if (!File.Exists(path))
        {
            Debug.LogError($"序列化加载失败，不存在此路径下的文件{path}");
            return default;
        }
        object obj = null;
        using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs);
            fs.Close();
        }
        return obj;
    }

    /// <summary>
    /// 在Persistent文件夹中根据名字读取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T LoadFromPersistent<T>(string fileName) where T : class
    {
        string path = Application.persistentDataPath + "/" + fileName;
        return Load<T>(path);
    }
    /// <summary>
    /// 从二进制数据反序列化为对象
    /// </summary>
    /// <typeparam name="T">目标对象的类型</typeparam>
    /// <param name="data">二进制数据</param>
    /// <returns>返回反序列化后的对象</returns>
    public static T LoadFromBinary<T>(byte[] data) where T : class
    {
        if (data == null || data.Length == 0)
            return null;

        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(memoryStream) as T; // 返回反序列化后的对象
        }
    }
    /// <summary>
    /// 将 UnityEngine.Object 序列化为二进制数据
    /// </summary>
    /// <param name="obj">要序列化的 Unity 对象</param>
    /// <returns>返回二进制数组</returns>
    private static byte[] ObjectToBinary(UnityEngine.Object obj)
    {
        object SerializableObj = obj;
        if (SerializableObj == null)
            return null;

        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, SerializableObj);
            return memoryStream.ToArray(); // 返回序列化后的二进制数组
        }
    }
    /// <summary>
    /// 深度Clone一个对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T DeepClone<T>(T obj)
    {
        if (obj == null)
            return default;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(memoryStream);
        }
    }

    /// <summary>
    /// 非泛型深度克隆
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static object DeepClone(Type type, object obj)
    {
        if (obj == null || type == null)
            return null;

        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(memoryStream);
        }
    }


    #region 通过type加载泛型结构


    // 缓存泛型类型
    private static readonly Dictionary<string, Type> GenericTypeCache = new();

    // 缓存 MethodInfo
    private static readonly Dictionary<string, MethodInfo> MethodCache = new();

    // 缓存 PropertyInfo
    private static readonly Dictionary<string, PropertyInfo> PropertyCache = new();

    /// <summary>
    /// 动态加载开放泛型类型（支持双泛型参数）
    /// </summary>
    /// <param name="genericTypeDefinition">开放泛型类型（如 typeof(ExcelConfigurationContainer<,>)</param>
    /// <param name="typeArguments">泛型参数类型（如 typeof(int), typeof(EnemyConfig)）</param>
    /// <param name="path">文件路径</param>
    /// <returns>加载的对象</returns>
    public static object LoadOpenGeneric(Type genericTypeDefinition, Type[] typeArguments, string path)
    {
        if (genericTypeDefinition == null || typeArguments == null || typeArguments.Length == 0)
        {
            Debug.LogError("泛型类型定义或参数类型为空");
            return null;
        }

        // 1. 获取或构造泛型类型
        var genericType = GetOrCreateGenericType(genericTypeDefinition, typeArguments);

        // 2. 调用 Load 方法
        var loadMethod = GetOrCreateMethodInfo(typeof(BinaryManager), nameof(Load), new[] { typeof(Type), typeof(string) });
        return loadMethod.Invoke(null, new object[] { genericType, path });
    }

    /// <summary>
    /// 动态加载单泛型类型（支持单泛型参数）
    /// </summary>
    /// <param name="genericTypeDefinition">开放泛型类型（如 typeof(List<>))</param>
    /// <param name="typeArgument">泛型参数类型（如 typeof(int)）</param>
    /// <param name="path">文件路径</param>
    /// <returns>加载的对象</returns>
    public static object LoadSingleGeneric(Type genericTypeDefinition, Type typeArgument, string path)
    {
        return LoadOpenGeneric(genericTypeDefinition, new[] { typeArgument }, path);
    }

    /// <summary>
    /// 获取或构造泛型类型
    /// </summary>
    private static Type GetOrCreateGenericType(Type genericTypeDefinition, Type[] typeArguments)
    {
        var cacheKey = $"{genericTypeDefinition.FullName}_{string.Join(",", typeArguments.Select(t => t.FullName))}";
        if (GenericTypeCache.TryGetValue(cacheKey, out var cachedType))
        {
            return cachedType;
        }

        var genericType = genericTypeDefinition.MakeGenericType(typeArguments);
        GenericTypeCache[cacheKey] = genericType;
        return genericType;
    }

    /// <summary>
    /// 获取或创建 MethodInfo
    /// </summary>
    private static MethodInfo GetOrCreateMethodInfo(Type type, string methodName, Type[] parameterTypes)
    {
        var cacheKey = $"{type.FullName}_{methodName}_{string.Join(",", parameterTypes.Select(t => t.FullName))}";
        if (MethodCache.TryGetValue(cacheKey, out var cachedMethod))
        {
            return cachedMethod;
        }

        var method = type.GetMethod(methodName, parameterTypes);
        if (method == null)
        {
            Debug.LogError($"未找到方法: {methodName}");
            return null;
        }

        MethodCache[cacheKey] = method;
        return method;
    }

    // 其他现有方法保持不变...
    #endregion
}
