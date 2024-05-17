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

    //用来存储抽屉中的对象 记录没有正在使用的对象
    private Queue<Obj> poolQueue = new Queue<Obj>();
    //数量
    public int Count => poolQueue.Count;
    public int maxCount;
    /// <summary>
    /// 是否有空闲的对象
    /// </summary>
    public abstract bool IsHaveFreeObj { get; }


    /// <summary>
    /// 只有第一次创建抽屉时候才调用
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="volume">抽屉对象</param>
    public Pool()
    {
       
    }
    /// <summary>
    /// 从抽屉中取出对象，并移除抽屉中的对象
    /// </summary>
    /// <returns>想要的对象数据</returns>

    public Obj QuitPool()
    {
        Obj obj = null;
        //根据池子类型取出不同逻辑
        //循环：如果无空闲了，从第一个取出
        if (IsHaveFreeObj)
        {
            obj = poolQueue.Dequeue();
            recordQuitPool(obj);
        }
        obj = NoFreeObjOperate();

        obj.QuitPoolCallback?.Invoke();
        //如果柜子里没有东西，且使用中的物体超过最大数量上限时候
        return obj;
    }
    /// <summary>
    /// 将使用完的对象放入抽屉
    /// </summary>
    /// <param name="obj"></param>
    public void EnterPool(Obj obj)
    {
        recordEnterPoolObj(obj);
        poolQueue.Enqueue(obj);
        obj.EnterPoolCallback?.Invoke();
        //在开发时候，应该可看见的放入抽屉容器，好观察
#if UNITY_EDITOR
      //  if (obj is GameObj)
           // (obj as GameObj).transform.SetParent(VolumeTransform);
#endif

    }
    /// <summary>
    /// 记录已经有一个物体进入池子，变为空闲状态
    /// </summary>
    public abstract void recordEnterPoolObj(Obj obj);
    /// <summary>
    /// 记录已经有一个物体已经出池子
    /// </summary>
    public abstract void recordQuitPool(Obj obj);
    /// <summary>
    /// 没有空闲物体的处理逻辑
    /// </summary>
    public abstract Obj NoFreeObjOperate();
}

/// <summary>
/// 循环对象池
/// </summary>
public class CircuPool : Pool
{
    //用来记录使用中的对象的 
    private List<Obj> usingQueue = new List<Obj>();

    public CircuPool()
    {
    }

    public override bool IsHaveFreeObj => usingQueue.Count < maxCount;

    public override Obj NoFreeObjOperate()
    {
        if (usingQueue.Count <= 0)
        {
            Debug.LogError("错误！使用池数量为0");
            return null;
        }
        //让正在使用的第一个来服用
        Obj obj = usingQueue[0];
        //刷新顺序，把它放到末尾
        usingQueue.Remove(obj);
        usingQueue.Add(obj);
        return obj;
    }

    public override void recordEnterPoolObj(Obj obj)
    {
        usingQueue.Remove(obj);
    }

    public override void recordQuitPool(Obj obj)
    {
        usingQueue.Add(obj);
    }
}