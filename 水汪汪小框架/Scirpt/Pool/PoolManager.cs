using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Dictionary<string, Pool> PoolDic = new Dictionary<string, Pool>();

    private Dictionary<string, PoolObjMaxSizeinfo> PoolGroup = new Dictionary<string, PoolObjMaxSizeinfo>();

    /// <summary>
    /// 程序启动时候，如果开启可视化，则创建root
    /// </summary>
    public PoolManager()
    {
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
            PoolGroup.Add(key, SettingData.dataDic[key]);
        }
    }

    //创建物体时候检测
    public void FirstCreate_PoolCheck(Obj obj)
    {
        if (!PoolDic.ContainsKey(obj.PoolIdentity))
        {
          //  CreateNewPool(obj);
        }

    }

    //新创建一个池子
    public void CreateNewPool(string Group,string Identity)
    {
        Pool pool = null;
        switch (Group)
        {
            case "Circulate":
                pool = new CircuPool();
                break;
            case "Expansion":
            case "Fixed":
                break;
        }
    }

    /// <summary>
    /// 拿东西的方法（若没有东西会自动创建)
    /// </summary>
    /// <param name="name">抽屉容器的名字</param>
    /// <returns>从缓存池中取出的对象</returns>
    public Obj GetObj(string PoolIdentity)
    {
        Obj obj = null;
        //有了的方法
        if (PoolDic.ContainsKey(PoolIdentity))
        {
            obj = PoolDic[PoolIdentity].QuitPool();
            return obj;
        }

        return obj;
    }

    /// <summary>
    /// 将使用中的对象“销毁”，加入缓存池
    /// </summary>
    /// <param name="obj"></param>
    public void DestroyObj(Obj obj)
    {
        if (!PoolDic.ContainsKey(obj.PoolIdentity))
        {
            Debug.LogWarning("移除的不是缓存池对象！！");
            return;
        }
        PoolDic[obj.PoolIdentity].EnterPool(obj);
    }
    /// <summary>
    /// 过场景时候清除数据
    /// </summary>
    public void Clear()
    {
        PoolDic.Clear();
        root = null;
    }


    /// <summary>
    /// 将对象压入缓存池时候进行的操作
    /// </summary>
    private void PullObjToPoolOperation(Obj obj)
    {

    }
}
