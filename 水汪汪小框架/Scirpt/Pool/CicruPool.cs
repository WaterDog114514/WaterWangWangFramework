using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 循环对象池
/// </summary>
public class CircuPool : Pool
{
    public CircuPool(int MaxCount, Obj Prefab) : base(MaxCount, Prefab)
    {
    }

    public override Obj Operation_QuitObjPoolFull()
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

    public override void Operation_EnterObjPoolFull(Obj obj)
    {
        //循环池 直接删除外来人口
        ObjectManager.Instance.ReallyDestroyObj(obj);
    }
}