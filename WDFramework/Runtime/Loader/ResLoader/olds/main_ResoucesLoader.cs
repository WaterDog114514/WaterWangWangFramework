using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace WDFramework
{
    /// <summary>
    /// Resouces资源加载核心
    /// </summary>
    internal class main_ResoucesLoader
    {
        /// <summary>
        /// 同步加载资源 基本上用来加载配置文件，无需任务方式调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        public T LoadSync<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 异步加载某资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerator reallyLoadAsync<T>(string path, AsyncLoadTask task) where T : UnityEngine.Object
        {
            Res resInfo = new Res(typeof(T));
            task.ResInfo = resInfo;
            //直接同步加载 并且记录资源信息 到字典中 方便下次直接取出来用
            ResourceRequest rq = Resources.LoadAsync<T>(path);
            //回调
            while (!rq.isDone)
            {
                task.LoadProcess = rq.progress;
                yield return null;
            }
            yield return rq;

            resInfo.Asset = rq.asset as T;
            //回调并结束了
            task.FinishTask(resInfo);
        }
    }

}
