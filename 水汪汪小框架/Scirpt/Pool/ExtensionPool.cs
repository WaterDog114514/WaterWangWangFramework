using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionPool : Pool
{
    public ExtensionPool(int MaxCount, Obj Prefab) : base(MaxCount, Prefab)
    {
    }

    public override Obj Operation_QuitObjPoolFull()
    {
        maxCount++;
        Debug.Log("已经扩容，当前容量:" + maxCount);
        Obj obj = Operation_CreatePoolObj();
        usingQueue.Add(obj);
        return obj;
    }

    public override void Operation_EnterObjPoolFull(Obj obj)
    {
        //扩容操作
        maxCount++;
        poolQueue.Enqueue(obj);
        //回调进入操作
        obj.EnterPoolCallback?.Invoke();
    }
}
