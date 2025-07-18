using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WDFramework
{
    /// <summary>
    /// 循环对象池：如果池子已满，放东西会直接深度销毁
    /// 取东西如果没有空闲对象，那么就会取最后一个
    /// </summary>
    public class CircuPool : Pool
    {
        public CircuPool(int MaxCount, string Identity) : base(MaxCount, Identity)
        {
        }

        public override Obj Operation_QuitObjPoolNoFree()
        {
            if (usingQueue.Count <= 0)
            {
                Debug.LogError("错误！使用池数量为0");
                return null;
            }
            //让正在使用的第一个来服用
            Obj obj = usingQueue[0];
            //调用进出池子的方法，这是很符合规矩的
            obj.EnterPoolCallback?.Invoke();
            obj.QuitPoolCallback?.Invoke();
            //刷新池子排序，把它放到末尾
            usingQueue.Remove(obj);
            usingQueue.Add(obj);
            return obj;
        }
        public override void Operation_EnterObjPoolFull(Obj obj)
        {
            //循环池 直接删除外来人口
            obj.DeepDestroy();
        }

        public override void Operation_FirstCreateRecord(Obj obj)
        {
            //满了，不予以记录
            if(IsFull) { return; }
            usingQueue.Add(obj);
        }
    }
}
