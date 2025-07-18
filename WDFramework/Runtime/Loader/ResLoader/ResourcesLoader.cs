using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace WDFramework
{
    public partial class ResLoader : Singleton<ResLoader>, IFrameworkSystem
    {
        /// <summary>
        /// ��Դ��Ϣ��
        /// </summary>
        public class ResourcesLoader : BaseResLoadModuel
        {
            public ResourcesLoader(Dictionary<string, Res> dic_LoadedRes) : base(dic_LoadedRes)
            {
            }
            /// <summary>
            /// ���ڼ�����Դ�е�����
            /// </summary>
            private Dictionary<string, UnityAction> dic_LoadResCallback;
            protected override void initializedLoader()
            {
                dic_LoadResCallback = new Dictionary<string, UnityAction>();
            }
            public T LoadResourcesSync<T>(string path) where T : UnityEngine.Object
            {
                var reskey = $"Res_{path}";
                //�Ѿ����ع���Դ�Ĵ�����
                if (dic_LoadedRes.ContainsKey(reskey))
                {
                    return dic_LoadedRes[reskey].GetAsset<T>();
                }
                //δ������Դ�ͼ���
                var resInfo = new Res(typeof(T));
                resInfo.Asset = Resources.Load<T>(path);
                //�����ֵ�
                dic_LoadedRes[reskey] = resInfo;
                return resInfo.GetAsset<T>();
            }
            public IEnumerator LoadResourcesAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
            {
                var reskey = $"Res_{path}";
                //�Ѿ����ع���Դ�Ĵ�����
                if (dic_LoadedRes.ContainsKey(reskey))
                {
                    //������ڼ����У���ô��ӽ��ص��б�
                    if (dic_LoadResCallback.ContainsKey(reskey))
                    {
                        dic_LoadResCallback[reskey] += () => { callback.Invoke(dic_LoadedRes[reskey].GetAsset<T>()); };
                    }
                    //���������ɣ�ֱ�ӻص�
                    else
                    {
                        callback?.Invoke(dic_LoadedRes[reskey].GetAsset<T>());
                    }
                    yield break;
                }
                //��������Դ
                var resInfo = new Res(typeof(T));
                //��������
                dic_LoadedRes.Add(reskey, null);
                //������ػص�����
                dic_LoadResCallback[reskey] = () => { callback.Invoke(resInfo.GetAsset<T>()); };
                //ֱ��ͬ������ ���Ҽ�¼��Դ��Ϣ ���ֵ��� �����´�ֱ��ȡ������
                ResourceRequest rq = Resources.LoadAsync<T>(path);
                yield return rq;
                resInfo.Asset = rq.asset as T;
                //�ص����й�������񲢽�����
                dic_LoadResCallback[reskey]?.Invoke();
                //�����ֵ�
                dic_LoadedRes[reskey] = resInfo;
            }
        }
    }
}