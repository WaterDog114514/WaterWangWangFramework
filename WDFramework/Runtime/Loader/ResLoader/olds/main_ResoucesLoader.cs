using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace WDFramework
{
    /// <summary>
    /// Resouces��Դ���غ���
    /// </summary>
    internal class main_ResoucesLoader
    {
        /// <summary>
        /// ͬ��������Դ �������������������ļ�����������ʽ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        public T LoadSync<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// �첽����ĳ��Դ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerator reallyLoadAsync<T>(string path, AsyncLoadTask task) where T : UnityEngine.Object
        {
            Res resInfo = new Res(typeof(T));
            task.ResInfo = resInfo;
            //ֱ��ͬ������ ���Ҽ�¼��Դ��Ϣ ���ֵ��� �����´�ֱ��ȡ������
            ResourceRequest rq = Resources.LoadAsync<T>(path);
            //�ص�
            while (!rq.isDone)
            {
                task.LoadProcess = rq.progress;
                yield return null;
            }
            yield return rq;

            resInfo.Asset = rq.asset as T;
            //�ص���������
            task.FinishTask(resInfo);
        }
    }

}
