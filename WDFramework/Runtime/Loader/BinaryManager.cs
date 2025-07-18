using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// ���������л�������
/// </summary>
public static class BinaryManager
{
    [SerializeField]
    /// <summary>
    /// �洢��������ݵ�perisistent�ļ�����
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fileName"></param>
    public static void SaveToPersistent(object obj, string fileName, string path = null)
    {
        if (path == null)
            path = Application.persistentDataPath + "/";
        //���ж�·���ļ�����û��
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        SaveToPath(obj, path + "/" + fileName);
    }
    public static void SaveToPath(object obj, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception("���л�ʱ��pathΪnull");
        }
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }
    }

    /// <summary>
    /// ͨ�����ͼ��أ���ȡ2��������ת���ɶ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T Load<T>(string path) where T : class
    {
        return Load(typeof(T), path) as T;
    }
    /// <summary>
    /// ͨ��Type���ͼ���,��ȡ2��������ת���ɶ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static object Load(Type loadType, string path)
    {
        //���ж�·���ļ���û��
        if (!File.Exists(path))
        {
            Debug.LogError($"���л�����ʧ�ܣ������ڴ�·���µ��ļ�{path}");
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
    /// ��Persistent�ļ����и������ֶ�ȡ
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
    /// �Ӷ��������ݷ����л�Ϊ����
    /// </summary>
    /// <typeparam name="T">Ŀ����������</typeparam>
    /// <param name="data">����������</param>
    /// <returns>���ط����л���Ķ���</returns>
    public static T LoadFromBinary<T>(byte[] data) where T : class
    {
        if (data == null || data.Length == 0)
            return null;

        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(memoryStream) as T; // ���ط����л���Ķ���
        }
    }
    /// <summary>
    /// �� UnityEngine.Object ���л�Ϊ����������
    /// </summary>
    /// <param name="obj">Ҫ���л��� Unity ����</param>
    /// <returns>���ض���������</returns>
    private static byte[] ObjectToBinary(UnityEngine.Object obj)
    {
        object SerializableObj = obj;
        if (SerializableObj == null)
            return null;

        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, SerializableObj);
            return memoryStream.ToArray(); // �������л���Ķ���������
        }
    }
    /// <summary>
    /// ���Cloneһ������
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
    /// �Ƿ�����ȿ�¡
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


    #region ͨ��type���ط��ͽṹ


    // ���淺������
    private static readonly Dictionary<string, Type> GenericTypeCache = new();

    // ���� MethodInfo
    private static readonly Dictionary<string, MethodInfo> MethodCache = new();

    // ���� PropertyInfo
    private static readonly Dictionary<string, PropertyInfo> PropertyCache = new();

    /// <summary>
    /// ��̬���ؿ��ŷ������ͣ�֧��˫���Ͳ�����
    /// </summary>
    /// <param name="genericTypeDefinition">���ŷ������ͣ��� typeof(ExcelConfigurationContainer<,>)</param>
    /// <param name="typeArguments">���Ͳ������ͣ��� typeof(int), typeof(EnemyConfig)��</param>
    /// <param name="path">�ļ�·��</param>
    /// <returns>���صĶ���</returns>
    public static object LoadOpenGeneric(Type genericTypeDefinition, Type[] typeArguments, string path)
    {
        if (genericTypeDefinition == null || typeArguments == null || typeArguments.Length == 0)
        {
            Debug.LogError("�������Ͷ�����������Ϊ��");
            return null;
        }

        // 1. ��ȡ���췺������
        var genericType = GetOrCreateGenericType(genericTypeDefinition, typeArguments);

        // 2. ���� Load ����
        var loadMethod = GetOrCreateMethodInfo(typeof(BinaryManager), nameof(Load), new[] { typeof(Type), typeof(string) });
        return loadMethod.Invoke(null, new object[] { genericType, path });
    }

    /// <summary>
    /// ��̬���ص��������ͣ�֧�ֵ����Ͳ�����
    /// </summary>
    /// <param name="genericTypeDefinition">���ŷ������ͣ��� typeof(List<>))</param>
    /// <param name="typeArgument">���Ͳ������ͣ��� typeof(int)��</param>
    /// <param name="path">�ļ�·��</param>
    /// <returns>���صĶ���</returns>
    public static object LoadSingleGeneric(Type genericTypeDefinition, Type typeArgument, string path)
    {
        return LoadOpenGeneric(genericTypeDefinition, new[] { typeArgument }, path);
    }

    /// <summary>
    /// ��ȡ���췺������
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
    /// ��ȡ�򴴽� MethodInfo
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
            Debug.LogError($"δ�ҵ�����: {methodName}");
            return null;
        }

        MethodCache[cacheKey] = method;
        return method;
    }

    // �������з������ֲ���...
    #endregion
}
