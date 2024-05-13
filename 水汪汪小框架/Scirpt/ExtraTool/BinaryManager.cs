using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryManager : Singleton_UnMono<BinaryManager>
{

    /// <summary>
    /// 存储类对象数据
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fileName"></param>
    public void Save(object obj, string fileName, string path = null)
    {
        if (path == null)
            path = Application.persistentDataPath + "/";
        //先判断路径文件夹有没有
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        using (FileStream fs = new FileStream(path + "/" + fileName, FileMode.OpenOrCreate, FileAccess.Write))
        {

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }
    }

    /// <summary>
    /// 读取2进制数据转换成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T Load<T>( string path) where T : class
    {
        //先判断路径文件有没有
        if (!File.Exists(path))
        {
            Debug.LogError($"序列化加载失败，不存在此路径下的文件{path}");
            return default(T);
        }

        T obj = default(T);

        using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs) as T;
            fs.Close();
        }
        return obj;
    }
    /// <summary>
    /// 直接从persistent加载文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T LoadFromName<T>(string fileName) where T : class
    {
        string path = Application.persistentDataPath + "/" + fileName;
        return Load<T>(path);   
    }
}
