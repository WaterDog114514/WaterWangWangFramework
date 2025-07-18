using System;
using System.Collections.Generic;


namespace WDFramework
{


    //����ֻ��Ҫ����û�п��� ��¼ʹ���߼� ȡ����¼ʹ���߼�
    public abstract class Pool
    {
        //����ʱ����ר��
#if UNITY_EDITOR
        public Queue<Obj> PoolQueue => poolQueue;
        public List<Obj> UsingQueue => usingQueue;

#endif
        /// <summary>
        /// ���������
        /// </summary>
        public enum E_PoolType
        {
            /// <summary>
            /// ѭ��ʹ�ð汾�ĳ��룬���ʹ���еĶ���ĵ�һ��ȡ����ʹ��
            /// </summary>
            Circulate,
            /// <summary>
            /// ���ݵĳ��룬���ʹ���еĶ���ĵ�һ��ȡ����ʹ��
            /// </summary>
            Expansion,
            /// <summary>
            /// ���ݳأ��̶������������޿��������ˣ���������κβ���
            /// </summary>
            Fixed

        }
        //�ó���ͬ���Ԥ������Ϣ��������ʱ�����ݴ�������
        protected string Identity;
        //�����洢�����еĶ��� ��¼û������ʹ�õĶ���
        protected Queue<Obj> poolQueue = new Queue<Obj>();
        //����
        public int Count => poolQueue.Count;
        public int maxCount;
        /// <summary>
        /// �Ƿ��п��еĶ���
        /// </summary>
        //������¼ʹ���еĶ���� 
        protected List<Obj> usingQueue = new List<Obj>();
        public bool IsHaveFreeObj => usingQueue.Count < maxCount && poolQueue.Count > 0;
        /// <summary>
        /// �����𣬵�ʹ�óغͿ��гؼ������������������˵������
        /// </summary>
        public bool IsFull => usingQueue.Count + poolQueue.Count >= maxCount;
        /// <summary>
        /// ֻ�е�һ�δ�������ʱ��ŵ���
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="volume">�������</param>
        public Pool(int MaxCount,string Identity)
        {
            this.maxCount = MaxCount;
            this.Identity = Identity;
        }
        /// <summary>
        /// �ӳ�����ȡ�����󣬲��Ƴ������еĶ���
        /// </summary>
        /// <returns>��Ҫ�Ķ�������</returns>

        public Obj Operation_QuitPool()
        {
            Obj obj = null;
            //���ݳ����Ƿ��п��ж��������в���
            if (IsHaveFreeObj)
            {
                //�о�ֱ�ӳ���Ȼ���¼һ��
                obj = poolQueue.Dequeue();
                usingQueue.Add(obj);
            }
            //û�п��ж��󣬾ͽ����޿��еĲ���
            else
            {
                //�����ˣ��׸�����������
                if (usingQueue.Count >= maxCount)
                    obj = Operation_QuitObjPoolNoFree();
                //����ض���Ϊ0������ʹ��û�б����������ʱ��ȡ����
                else
                {
                  return null;
                }
            }
            //�����ɹ��󣬽��лص������Ӳ���
            obj?.QuitPoolCallback?.Invoke();
            //���������û�ж�������ʹ���е����峬�������������ʱ��
            return obj;
        }
        /// <summary>
        /// ��ʹ����Ķ���������
        /// </summary>
        /// <param name="obj"></param>
        public void Operation_EnterPool(Obj obj)
        {
            //�����˿���Ҫ�����ӣ������ж��ǲ��Ǳ���
            if (!usingQueue.Contains(obj) && IsFull)
            {
                //�����ദ������ջ����
                Operation_EnterObjPoolFull(obj);
            }
            else
            {
                //��¼һ��
                usingQueue.Remove(obj);
                poolQueue.Enqueue(obj);
                //�ص��������
                obj.EnterPoolCallback?.Invoke();
            }
        }
        /// <summary>
        /// ȡ������ʱ��û�п��п�������Ĵ����߼�
        /// </summary>
        /// <returns></returns>
        public abstract Obj Operation_QuitObjPoolNoFree();
        /// <summary>
        /// ���˷Ŷ�������
        /// </summary>
        /// <param name="obj"></param>
        public abstract void Operation_EnterObjPoolFull(Obj obj);
        /// <summary>
        /// ��һ�δ���ʱ��ļ�¼����������������ֱ�Ӱ�����¼���������
        /// </summary>
        public abstract void Operation_FirstCreateRecord(Obj obj);
    }

}