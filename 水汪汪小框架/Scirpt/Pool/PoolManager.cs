using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池(对象池)模块 管理器
/// </summary>

public class PoolManager : Singleton_UnMono<PoolManager>
{
    /// <summary>
    /// GetObj的时候从哪里拿对象 AB包和Resources选一个
    /// </summary>
    public enum ObjFrom
    {
        AB, Res
    }

    /// <summary>
    /// 放抽屉的柜子
    /// </summary>
    public Transform root;
    private Dictionary<string, Pool> PoolDic = new Dictionary<string, Pool>();
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
    /// <summary>
    /// 拿东西的方法（若没有东西会自动创建)
    /// </summary>
    /// <param name="name">抽屉容器的名字</param>
    /// <returns>从缓存池中取出的对象</returns>
    public Obj GetObj(string PoolIdentity, ObjFrom from = ObjFrom.AB)
    {
        Obj obj = null;
        //有了的方法
        if (PoolDic.ContainsKey(PoolIdentity))
        {
            obj = PoolDic[PoolIdentity].QuitPool();
            return obj;
        }

        //待写  需要读资源逻辑操作来判断加载类型

//      //第一次创建池子操作
//#if UNITY_EDITOR
        
//        Transform Volume = new GameObject(obj.name).transform;
//        Volume.SetParent(root);
//        Volume.name = obj.transform.name;
//        PoolDic.Add(name, new CircuPool(obj, Volume));
//#else
//            PoolDic.Add(name, new CircuPool(obj, null));
//#endif
        //没有的时候 通过资源加载 去实例化出一个Obj
        switch (from)
        {
            case ObjFrom.AB:
                break;
            case ObjFrom.Res:
               // obj = Obj.CreateObj(Resources.Load<Object>(name));
                break;
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
