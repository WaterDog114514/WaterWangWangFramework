using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有游戏对象管理器，记录着所有实例化后的游戏对象，仅仅包括GameObject和非Mono的数据对象
/// </summary>
public class ObjectMgr : Singleton_UnMono<ObjectMgr>
{
    private Dictionary<int, Obj> dicObj = new Dictionary<int, Obj>();


    private int tempID;
    private void AddObjToDic(Obj obj)
    {
        tempID++;
        obj.ID = tempID;
        dicObj.Add(obj.ID, obj);
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
        return(CreateGameObject(id));
    }
    public GameObj CreateGameObject(PrefabInfo info)
    {
        GameObject gameObj = Object.Instantiate(info.res);
        GameObj obj = new GameObj(gameObj);
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
        dicObj.Remove(obj.ID);
    }
}

