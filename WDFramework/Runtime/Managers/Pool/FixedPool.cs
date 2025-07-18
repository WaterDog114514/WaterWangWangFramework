using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDFramework
{
    /// <summary>
    /// 定容池：如果池子满了会直接销毁要放入的对象
    /// </summary>
    public class FixedPool : Pool
    {
        public FixedPool(int MaxCount, string Identity) : base(MaxCount, Identity)
        {
        }

        //满了销毁
        public override void Operation_EnterObjPoolFull(Obj obj)
        {
            obj.DeepDestroy();
        }

        public override void Operation_FirstCreateRecord(Obj obj)
        {
            //满了，不予以记录
            if (IsFull) { return; }
            usingQueue.Add(obj);
        }

        public override Obj Operation_QuitObjPoolNoFree()
        {
            Debug.LogWarning($"定容池{Identity}已经没有空闲对象了，返回null操作");
            return null;
        }

    }
}


