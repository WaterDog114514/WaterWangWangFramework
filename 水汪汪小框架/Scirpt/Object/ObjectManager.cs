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
    private PoolManager poolManager;
    public ObjectManager()
    {
        poolManager = new PoolManager();
    }


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
        //给予id
        GiveObjID(obj);
        //只有对象池约束才要
        if (info is PoolPrefabInfo)
        {
            gameObj.name = (info as PoolPrefabInfo).identity;
            obj.PoolGroup = (info as PoolPrefabInfo).PoolGroup;
        }
        gameObj.AddComponent<GameObjectInstance>().Inti(obj);
        return obj;
    }

    //从对象池中获得物体
    public GameObj GetGameObjFromPool(PrefabInfo info)
    {
        return poolManager.GetGameObj(info);
    }
    public GameObj GetGameObjFromPool(int id)
    {
        return poolManager.GetGameObj(PrefabLoaderManager.Instance.GetPrefabInfoFromID(id));
    }
    public GameObj GetGameObjFromPool(string PrefabName)
    {
        return poolManager.GetGameObj(PrefabLoaderManager.Instance.GetPrefabInfoFromName(PrefabName));
    }

    //获取数据对象从对象池之中
    public T getDataObjFromPool<T>() where T : DataObj
    { 
        return poolManager.GetDataObj(typeof(T)) as T;
    }

    public DataObj getDataObjFromPool(Type type)  
    {
        return poolManager.GetDataObj(type);
    }

    /// <summary>
    /// 创建一般数据对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T CreateDataObject<T>() where T : DataObj, new()
    {
        return CreateDataObject(typeof(T)) as T; 
    }
    public DataObj CreateDataObject(Type type)
    {
        if (!type.IsSubclassOf(typeof(DataObj)))
        {
            Debug.LogError($"实例化错误！{type.Name}不继承自DataObj类");
            return null;
        }
        DataObj obj = Activator.CreateInstance(type) as DataObj;
        //给予ID
        GiveObjID(obj);
        return obj;
    }

    private int CurrentNewObjID=0;
    /// <summary>
    /// 给予新对象唯一ID，一般创建时候使用
    /// </summary>
    private void GiveObjID(Obj obj)
    {
       obj.ID = CurrentNewObjID++;
    }

    /// <summary>
    /// 浅销毁，将其放入对象池而已
    /// </summary>
    /// <param name="obj"></param>
    public void DestroyObj(Obj obj)
    {
        poolManager.DestroyObj(obj);
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

