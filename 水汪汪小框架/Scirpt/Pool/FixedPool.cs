using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPool : Pool {
    public FixedPool(int MaxCount, Obj Prefab) : base(MaxCount, Prefab)
    {
    }

    public override void Operation_EnterObjPoolFull(Obj obj)
    {
        ObjectManager.Instance.ReallyDestroyObj(obj);
    }

    public override Obj Operation_QuitObjPoolFull()
    {
        Debug.LogWarning($"定容池{Identity}已经没有空闲对象了，返回null操作");
        return null;
    }

}


