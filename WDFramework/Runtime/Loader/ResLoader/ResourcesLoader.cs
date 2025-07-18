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
        /// 资源信息类
        /// </summary>
        public class ResourcesLoader : BaseResLoadModuel
        {
            public ResourcesLoader(Dictionary<string, Res> dic_LoadedRes) : base(dic_LoadedRes)
            {
            }
            /// <summary>
            /// 正在加载资源中的任务
            /// </summary>
            private Dictionary<string, UnityAction> dic_LoadResCallback;
            protected override void initializedLoader()
            {
                dic_LoadResCallback = new Dictionary<string, UnityAction>();
            }
            public T LoadResourcesSync<T>(string path) where T : UnityEngine.Object
            {
                var reskey = $"Res_{path}";
                //已经加载过资源的处理方法
                if (dic_LoadedRes.ContainsKey(reskey))
                {
                    return dic_LoadedRes[reskey].GetAsset<T>();
                }
                //未加载资源就加载
                var resInfo = new Res(typeof(T));
                resInfo.Asset = Resources.Load<T>(path);
                //加入字典
                dic_LoadedRes[reskey] = resInfo;
                return resInfo.GetAsset<T>();
            }
            public IEnumerator LoadResourcesAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
            {
                var reskey = $"Res_{path}";
                //已经加载过资源的处理方法
                if (dic_LoadedRes.ContainsKey(reskey))
                {
                    //如果还在加载中，那么添加进回调列表
                    if (dic_LoadResCallback.ContainsKey(reskey))
                    {
                        dic_LoadResCallback[reskey] += () => { callback.Invoke(dic_LoadedRes[reskey].GetAsset<T>()); };
                    }
                    //如果加载完成，直接回调
                    else
                    {
                        callback?.Invoke(dic_LoadedRes[reskey].GetAsset<T>());
                    }
                    yield break;
                }
                //创建新资源
                var resInfo = new Res(typeof(T));
                //挂起留名
                dic_LoadedRes.Add(reskey, null);
                //挂起加载回调任务
                dic_LoadResCallback[reskey] = () => { callback.Invoke(resInfo.GetAsset<T>()); };
                //直接同步加载 并且记录资源信息 到字典中 方便下次直接取出来用
                ResourceRequest rq = Resources.LoadAsync<T>(path);
                yield return rq;
                resInfo.Asset = rq.asset as T;
                //回调所有挂起的任务并结束了
                dic_LoadResCallback[reskey]?.Invoke();
                //加入字典
                dic_LoadedRes[reskey] = resInfo;
            }
        }
    }
}