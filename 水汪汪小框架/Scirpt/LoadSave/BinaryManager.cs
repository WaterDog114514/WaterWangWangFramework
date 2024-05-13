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
    public T Load<T>(string fileName, string path = null) where T : class
    {
        if (path == null)
            path = Application.persistentDataPath + "/";

        //先判断路径文件夹有没有
        if (!Directory.Exists(path)||!File.Exists(path+"/"+fileName) )
          { 
            Debug.LogError($"序列化加载失败，不存在此路径下的文件{path+fileName}");
            return default(T);
        }

        T obj = default(T);

        using (FileStream fs = File.Open(path + "/" + fileName, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs) as T;
            fs.Close();
        }
        return obj;
    }
}
