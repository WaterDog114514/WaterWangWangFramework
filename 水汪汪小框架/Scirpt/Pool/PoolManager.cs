using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 缓存池(对象池)模块 管理器
/// </summary>
public class PoolManager : Singleton_UnMono<PoolManager>
{
    /// <summary>
    /// 放抽屉的柜子
    /// </summary>
    public Transform root;
    //抽屉管理，只有当使用编辑器开发采用哦
    private Dictionary<string, Transform> dic_VolumeTransform = new Dictionary<string, Transform>();
    /// <summary>
    /// 所有池子管理
    /// </summary>
    private Dictionary<string, Pool> dic_Pool = new Dictionary<string, Pool>();
    /// <summary>
    /// 对象池预设，创建池子时候专用
    /// </summary>
    private Dictionary<string, PoolObjMaxSizeinfo> dic_PoolGroup = new Dictionary<string, PoolObjMaxSizeinfo>();
    private FrameworkSettingData SettingData;
    /// <summary>
    /// 程序启动时候，如果开启可视化，则创建root
    /// </summary>
    public PoolManager()
    {
        IntiManager();
        SettingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
#if UNITY_EDITOR
        if (root == null)
        {
            root = new GameObject("PoolRoot").transform;
        }
#endif
    }
    private void IntiManager()
    {
        //初始化加载配置操作
        PoolObjMaxSizeinfoContainer SettingData = GameExcelDataLoader.Instance.GetDataContainer<PoolObjMaxSizeinfoContainer>();
        foreach (var key in SettingData.dataDic.Keys)
        {
            dic_PoolGroup.Add(key, SettingData.dataDic[key]);
        }
    }
    //新创建一个池子
    public Pool CreateNewPool(Obj obj)
    {
        //没有名字和分组对象的组就创建默认的即可
        if (dic_Pool.ContainsKey(obj.PoolIdentity))
        {
            Debug.LogError($"唯一对象池{obj.PoolIdentity}已存在，还试图创建它，请保证对象的池标记唯一性");
            return null;
        }
        //開始创建
        Pool pool = null;
        if (obj.PoolGroup == null || obj.PoolGroup == "" || !dic_PoolGroup.ContainsKey(obj.PoolGroup))
        {

            Debug.LogWarning($"{obj.PoolIdentity}没有对象池预设，已为他创建默认的预设的对象池，默认对象池上限为{SettingData.loadPrefabSetting.DefaultGroupPoolSize}");

            //创建对象池
            pool = new CircuPool(SettingData.loadPrefabSetting.DefaultGroupPoolSize, obj);
        }
        //有预设的情况下
        else
        {
            //先获取预设
            PoolObjMaxSizeinfo info = dic_PoolGroup[obj.PoolGroup];
            switch (info.PoolType)
            {
                case "Circulate":
                    pool = new CircuPool(info.GroupLimit, obj);
                    break;
                case "Expansion":
                    pool = new ExtensionPool(info.GroupLimit, obj);
                    break;
                case "Fixed":
                    pool = new FixedPool(info.GroupLimit, obj);
                    break;
            }
        }
        dic_Pool.Add(obj.PoolIdentity, pool);
        return pool;

    }
    public GameObj GetGameObj(int id)
    {
        return GetGameObj(PrefabLoaderManager.Instance.GetPrefabInfoFromID(id));
    }
    public GameObj GetGameObj(string prefabName)
    {
        return GetGameObj(PrefabLoaderManager.Instance.GetPrefabInfoFromName(prefabName));
    }

    /// <summary>
    /// 拿东西的方法（若没有东西会自动创建)
    /// </summary>
    /// <param name="name">抽屉容器的名字</param>
    /// <returns>从缓存池中取出的对象</returns>
    public GameObj GetGameObj(PrefabInfo info)
    {
        GameObj obj = null;
        if (info is UnLimitedPrefabInfo)
        {
            Debug.LogError($"获取对象失败，该对象{info.res.name}不受对象池约束");
            return null;
        }
        //有了的方法 直接从池子拿
        if (dic_Pool.ContainsKey((info as PoolPrefabInfo).identity))
        {
            obj = dic_Pool[(info as PoolPrefabInfo).identity].Operation_QuitPool() as GameObj;
        }
        //没有就先创建物体，然后根据物体创建对象池
        else
        {
            obj = ObjectManager.Instance.CreateGameObject(info);
            Pool pool = CreateNewPool(obj);
            //放取一波记录一下
            pool.Operation_EnterPool(obj);
            obj = pool.Operation_QuitPool() as GameObj;
        }
        return obj;
    }
    //安全的得到物体,取到物体才执行操作
    public void GetGameObjSafety(PrefabInfo info, UnityAction<GameObj> callback)
    {
        GameObj Obj = GetGameObj(info);
        if(Obj != null) callback?.Invoke(Obj);
    }
    public void GetGameObjSafety(string prefabName, UnityAction<GameObj> callback)
    {
        GameObj Obj = GetGameObj(PrefabLoaderManager.Instance.GetPrefabInfoFromName(prefabName));
        if (Obj != null) callback?.Invoke(Obj);
    }
    public DataObj GetDataObj(Type type)
    {
        DataObj obj = null;
        //有了的方法
        if (dic_Pool.ContainsKey(type.Name))
        {
            obj = dic_Pool[type.Name].Operation_QuitPool() as DataObj;
        }
        //没有就通过全局对象管理器来创建，它会创建池子一起的
        else
        {
            obj = ObjectManager.Instance.CreateDataObject(type);
        }
        return obj;
    }

    /// <summary>
    /// 将使用中的对象“销毁”，加入缓存池
    /// </summary>
    /// <param name="obj"></param>
    public void DestroyObj(Obj obj)
    {
        if (!dic_Pool.ContainsKey(obj.PoolIdentity))
        {
            Debug.LogWarning("移除的不是缓存池对象！！");
            return;
        }
        dic_Pool[obj.PoolIdentity].Operation_EnterPool(obj);
    }
    /// <summary>
    /// 过场景时候清除数据
    /// </summary>
    public void Clear()
    {
        dic_Pool.Clear();
        root = null;
    }

    public void DEMOTEST()
    {

    }
}
