using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace WDFramework
{
    /// <summary>
    /// 资源加载的核心类，封装了各个子模块的加载
    /// </summary>
    public partial class ResLoader : Singleton<ResLoader>, IFrameworkSystem
    {
        //所有子模块
        private ResourcesLoader resourcesLoader;
        private ABPackLoader abLoader;
        //核心设置配置文件
        /// <summary>
        /// 已经加载过的资源
        /// </summary>
        private Dictionary<string, Res> dic_LoadedRes = new Dictionary<string, Res>();
        public void InitializedSystem()
        {
            //初始化所有必备加载子模块
            resourcesLoader = new ResourcesLoader(dic_LoadedRes);
            abLoader = new ABPackLoader(dic_LoadedRes);
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <returns>任务进度，实时跟踪</returns>
        public TaskProcess LoadABPack(E_ABPackName ABPackName, UnityAction callback = null)
        {
            //创建加载进度
            var taskProcess = TaskProcess.CreateTaskProcess(-1);
            UpdateSystem.Instance.StartCoroutine(abLoader.LoadABPack(ABPackName, taskProcess, callback));
            return taskProcess;
        }
        //从已加载的ab包中得到包中资源
        public T GetABPackRes<T>(E_ABPackName ABPackName, string resName) where T : UnityEngine.Object
        {
            var res = abLoader.GetABPackRes<T>(ABPackName, resName);
            return res.GetAsset<T>();
        }
        //从已加载的ab包中得到包中资源-异步
        public void GetABPackResAsync<T>(E_ABPackName ABPackName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            UpdateSystem.Instance.StartCoroutine(abLoader.GetABPackResAsync<T>(ABPackName, resName, callback));
        }
        /// <summary>
        /// 卸载AB包
        /// </summary>
        public void DeleteABPack(E_ABPackName ABPackName)
        {
            UpdateSystem.Instance.StartCoroutine(abLoader.UnLoadAssetBundle(ABPackName.ToString()));
        }
        public void LoadResourcesAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
            resourcesLoader.LoadResourcesAsync<T>(path, callback);

        }
        /// <summary>
        /// 同步快速加载配置文件等，直接完成
        /// </summary>
        public T LoadResourcesSync<T>(string path) where T : UnityEngine.Object
        {
            return resourcesLoader.LoadResourcesSync<T>(path);
        }
    }
}

