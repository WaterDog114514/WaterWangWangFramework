using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有游戏对象管理器，记录着所有实例化后的游戏对象，仅仅包括GameObject和非Mono的数据对象
/// </summary>
public class ObjectManager : Singleton_UnMono<ObjectManager>
{
   // private Dictionary<int, Obj> dicObj = new Dictionary<int, Obj>();
    private int tempID;

    //创建预设体对象
    public GameObj CreateGameObject(string prefabName)
    {
        PrefabInfo prefabInfo = PrefabLoaderManager.Instance.GetPrefabInfoFromName(prefabName);
        return CreateGameObject(prefabInfo);
    }
    public GameObj CreateGameObject(int id)
    {
        PrefabInfo prefabInfo = PrefabLoaderManager.Instance.GetPrefabInfoFromID(id);
        return (CreateGameObject(id));
    }
    public GameObj CreateGameObject(PrefabInfo info)
    {
        //原始创建
        GameObject gameObj = UnityEngine.Object.Instantiate(info.res);
        GameObj obj = new GameObj(gameObj);
        //只有对象池约束才要
        if (info is PoolPrefabInfo)
        {
            gameObj.name = (info as PoolPrefabInfo).identity;
            obj.PoolGroup = (info as PoolPrefabInfo).PoolGroup;
        }
        gameObj.AddComponent<GameObjectInstance>().Inti(obj);
        return obj;
    }

    /// <summary>
    /// 创建一般数据对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T CreateDataObject<T>() where T : DataObj, new()
    {
        T obj = new T();
        return obj;
    }
    public DataObj CreateDataObject(Type type)
    {
        if (!type.IsSubclassOf(typeof(DataObj)))
        {
            Debug.LogError($"实例化错误！{type.Name}不继承自DataObj类");
            return null;
        }
        DataObj obj = Activator.CreateInstance(type) as DataObj;
        return obj;
    }

    /// <summary>
    /// 浅销毁，将其放入对象池而已
    /// </summary>
    /// <param name="obj"></param>
    public void DestroyObj(Obj obj)
    {
        PoolManager.Instance.DestroyObj(obj);
    }
    /// <summary>
    /// 深度销毁，将其完全从内存中移除
    /// </summary>
    public void ReallyDestroyObj(Obj obj)
    {
        if (obj is GameObj)
        {
            obj?.DestroyCallback();
        }
    }
}

