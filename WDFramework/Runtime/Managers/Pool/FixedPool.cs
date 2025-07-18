using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDFramework
{
    /// <summary>
    /// ���ݳأ�����������˻�ֱ������Ҫ����Ķ���
    /// </summary>
    public class FixedPool : Pool
    {
        public FixedPool(int MaxCount, string Identity) : base(MaxCount, Identity)
        {
        }

        //��������
        public override void Operation_EnterObjPoolFull(Obj obj)
        {
            obj.DeepDestroy();
        }

        public override void Operation_FirstCreateRecord(Obj obj)
        {
            //���ˣ������Լ�¼
            if (IsFull) { return; }
            usingQueue.Add(obj);
        }

        public override Obj Operation_QuitObjPoolNoFree()
        {
            Debug.LogWarning($"���ݳ�{Identity}�Ѿ�û�п��ж����ˣ�����null����");
            return null;
        }

    }
}


