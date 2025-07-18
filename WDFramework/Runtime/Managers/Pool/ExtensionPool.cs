using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WDFramework
{
    //扩容池，满了会自动拓容
    public class ExtensionPool : Pool
    {
        public ExtensionPool(int MaxCount, string Identity) : base(MaxCount, Identity)
        {
        }

        public override Obj Operation_QuitObjPoolNoFree()
        {
            //会自动扩容，但是会传null给外面
            maxCount++;
            return null;
        }

        public override void Operation_EnterObjPoolFull(Obj obj)
        {
            //扩容操作
            maxCount++;
            poolQueue.Enqueue(obj);
            //回调进入操作
            obj.EnterPoolCallback?.Invoke();
        }

        public override void Operation_FirstCreateRecord(Obj obj)
        {
            //满了，不予以记录
            if (IsFull) { return; }
            usingQueue.Add(obj);
        }
    }
}
