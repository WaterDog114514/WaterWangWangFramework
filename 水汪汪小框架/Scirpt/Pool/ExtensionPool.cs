using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionPool : Pool
{
    public ExtensionPool(int MaxCount, Obj Prefab) : base(MaxCount, Prefab)
    {
    }

    public override Obj Operation_PoolFull()
    {
        //扩容操作
        maxCount++;
        Debug.Log("已经扩容，当前容量:"+maxCount);
        Obj obj = Operation_CreatePoolObj();
        usingQueue.Add(obj);
        return obj;
    }


}
