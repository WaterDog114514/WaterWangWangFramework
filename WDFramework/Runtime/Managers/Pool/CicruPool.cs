using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WDFramework
{
    /// <summary>
    /// ѭ������أ���������������Ŷ�����ֱ���������
    /// ȡ�������û�п��ж�����ô�ͻ�ȡ���һ��
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
                Debug.LogError("����ʹ�ó�����Ϊ0");
                return null;
            }
            //������ʹ�õĵ�һ��������
            Obj obj = usingQueue[0];
            //���ý������ӵķ��������Ǻܷ��Ϲ�ص�
            obj.EnterPoolCallback?.Invoke();
            obj.QuitPoolCallback?.Invoke();
            //ˢ�³������򣬰����ŵ�ĩβ
            usingQueue.Remove(obj);
            usingQueue.Add(obj);
            return obj;
        }
        public override void Operation_EnterObjPoolFull(Obj obj)
        {
            //ѭ���� ֱ��ɾ�������˿�
            obj.DeepDestroy();
        }

        public override void Operation_FirstCreateRecord(Obj obj)
        {
            //���ˣ������Լ�¼
            if(IsFull) { return; }
            usingQueue.Add(obj);
        }
    }
}
