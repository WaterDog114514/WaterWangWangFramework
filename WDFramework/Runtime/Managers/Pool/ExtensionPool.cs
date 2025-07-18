using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WDFramework
{
    //���ݳأ����˻��Զ�����
    public class ExtensionPool : Pool
    {
        public ExtensionPool(int MaxCount, string Identity) : base(MaxCount, Identity)
        {
        }

        public override Obj Operation_QuitObjPoolNoFree()
        {
            //���Զ����ݣ����ǻᴫnull������
            maxCount++;
            return null;
        }

        public override void Operation_EnterObjPoolFull(Obj obj)
        {
            //���ݲ���
            maxCount++;
            poolQueue.Enqueue(obj);
            //�ص��������
            obj.EnterPoolCallback?.Invoke();
        }

        public override void Operation_FirstCreateRecord(Obj obj)
        {
            //���ˣ������Լ�¼
            if (IsFull) { return; }
            usingQueue.Add(obj);
        }
    }
}
