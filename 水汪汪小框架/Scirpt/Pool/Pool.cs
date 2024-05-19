using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池类型
/// </summary>
public enum PoolType
{
    /// <summary>
    /// 循环使用版本的抽屉，会从使用中的对象的第一个取出来使用
    /// </summary>
    Circulate,
    /// <summary>
    /// 扩容的抽屉，会从使用中的对象的第一个取出来使用
    /// </summary>
    Expansion,
    /// <summary>
    /// 定容池，固定的容量，当无空闲且满了，不会进行任何操作
    /// </summary>
    Fixed

}
//我们只需要处理没有空闲 记录使用逻辑 取消记录使用逻辑
public abstract class Pool
{
    //开发时监听专用
#if UNITY_EDITOR
    public Queue<Obj> PoolQueue => poolQueue;
    public List<Obj> UsingQueue => usingQueue;

#endif


    //该池子同类的预设体信息，不够用时候会根据此来创建
    protected string Identity;
    protected Type ObjType;
    //用来存储抽屉中的对象 记录没有正在使用的对象
    protected Queue<Obj> poolQueue = new Queue<Obj>();
    //数量
    public int Count => poolQueue.Count;
    public int maxCount;
    /// <summary>
    /// 是否有空闲的对象
    /// </summary>
    //用来记录使用中的对象的 
    protected List<Obj> usingQueue = new List<Obj>();
    public bool IsHaveFreeObj => usingQueue.Count < maxCount && poolQueue.Count > 0;
    //满了吗
    public bool IsFull => usingQueue.Count + poolQueue.Count >= maxCount;
    /// <summary>
    /// 只有第一次创建抽屉时候才调用
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="volume">抽屉对象</param>
    public Pool(int MaxCount, Obj Prefab)
    {
        this.maxCount = MaxCount;
        Identity = Prefab.PoolIdentity;
        ObjType = Prefab.GetType();
    }
    /// <summary>
    /// 从抽屉中取出对象，并移除抽屉中的对象
    /// </summary>
    /// <returns>想要的对象数据</returns>

    public Obj Operation_QuitPool()
    {
        Obj obj = null;
        //根据池子是否有空闲对象，来进行操作
        if (IsHaveFreeObj)
        {
            //有就直接出，然后记录一下
            obj = poolQueue.Dequeue();
            usingQueue.Add(obj);
        }
        //没有空闲对象，就进行无空闲的操作
        else
        {
            //爆满了，抛给子类来处理
            if (usingQueue.Count >= maxCount)
                obj = Operation_QuitObjPoolFull();
            //对象池对象为0，但是使用没有爆满情况，就需要创建了
            else
            {
                obj = Operation_CreatePoolObj();
                usingQueue.Add(obj);
            }
        }
        //操作成功后，进行回调出池子操作
        obj.QuitPoolCallback?.Invoke();
        //如果柜子里没有东西，且使用中的物体超过最大数量上限时候
        return obj;
    }
    /// <summary>
    /// 将使用完的对象放入抽屉
    /// </summary>
    /// <param name="obj"></param>
    public void Operation_EnterPool(Obj obj)
    {
        //外来人口想要进池子，就先判断是不是爆满
        if (!usingQueue.Contains(obj) && IsFull)
        {
            //给子类处理爆满入栈操作
            Operation_EnterObjPoolFull(obj);
        }
        else
        {
            //记录一下
            usingQueue.Remove(obj);
            poolQueue.Enqueue(obj);
            //回调进入操作
            obj.EnterPoolCallback?.Invoke();
        }

    }


    // 没有空闲物体的处理逻辑
    public abstract Obj Operation_QuitObjPoolFull();
    /// <summary>
    /// 满了放东西操作
    /// </summary>
    /// <param name="obj"></param>
    public abstract void Operation_EnterObjPoolFull(Obj obj);
    //当需要创建时候就要这个了哦
    public Obj Operation_CreatePoolObj()
    {
        if (ObjType.IsSubclassOf(typeof(DataObj))) return ObjectManager.Instance.CreateDataObject(ObjType);
        else return ObjectManager.Instance.CreateGameObject(
            PrefabLoaderManager.Instance.GetPrefabInfoFromName(Identity)
            );
    }


}
