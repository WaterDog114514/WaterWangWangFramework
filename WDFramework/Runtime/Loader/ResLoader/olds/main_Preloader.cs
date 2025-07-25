//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;


///// <summary>
///// 预加载核心
///// </summary>
//namespace WDFramework
//{
//    /// <summary>
//    /// AB包预加载模块
//    /// </summary>
//   internal class main_Preloader
//    {
//        /// <summary>
//        /// 总需要加载资源的数量
//        /// </summary>
//        public int TotalResNum;
//        /// <summary>
//        /// 已加载资源数量
//        /// </summary>
//        public int LoadedResNum;
//        /// <summary>
//        /// 正在加载的任务名
//        /// </summary>
//        public string CurrentTaskName;
//        public List<PreLoadTask> preloadResTasks = new List<PreLoadTask>();
//        /// <summary>
//        /// 开始进行预加载
//        /// </summary>
//        public void StartLoad()
//        {

//            if (preloadResTasks.Count == 0) Debug.LogError("预加载任务为0，请添加预加载任务后再执行");
//            UpdateSystem.Instance.StartCoroutine(ReallyPreLoadRes());
//        }
//        /// <summary>
//        /// 清除所有加载任务
//        /// </summary>
//        private void ClearAllTasks()
//        {
//            //释放所有加载记录的信息
//            preloadResTasks.Clear();
//            TotalResNum = 0;
//            LoadedResNum = 0;
//            CurrentTaskName = null;
//            TempPath.Clear();
//        }

//        /// <summary>
//        /// 预加载资源 一般是加载场景时候调用 只有预加载完毕才能加载新场景
//        /// </summary>
//        public IEnumerator ReallyPreLoadRes()
//        {
//            //先统计所有要加载资源的数量
//            TotalResNum += preloadResTasks.Count;
//            Coroutine currentCoroutine = null;
//            //先根据总预加载任务分出小分支一个同类的加载任务
//            for (int j = 0; j < preloadResTasks.Count; j++)
//            {
//                //进度更新之大类

//                PreLoadTask preLoadTask = preloadResTasks[j];
//                //回调资源
//                Res[] LoadedRes = new Res[preLoadTask.TaskList.Count];
//                //再根据总任务中的异步任务加载
//                for (int i = 0; i < preLoadTask.TaskList.Count; i++)
//                {

//                    AsyncLoadTask task = preLoadTask.TaskList[i];
//                    //加载完成不必加载
//                    if (task.isFinish) continue;
//                    currentCoroutine = task.StartAsyncLoad();
//                    yield return currentCoroutine;
//                    //记录加载后的回调资源
//                    LoadedRes[i] = task.ResInfo;
//                    //单资源完成增加
//                    LoadedResNum++;
//                    //进度条更新逻辑小类进度，使用事件中心
//                    Debug.Log($"加载进度{LoadedResNum}/{TotalResNum}");
//                }
//                //加载完一个预加载大任务就会调用一下
//                preLoadTask.callback?.Invoke(LoadedRes);
//            }



//            //加载完毕，清除所有任务
//            ClearAllTasks();
//            //回调一下预加载好的资源哦
//        }
//        /// <summary>
//        /// 创建预加载任务
//        /// </summary>
//        public void CreatePreLoadTask(PreLoadTask task)
//        {
//            preloadResTasks.Add(task);
//        }
//        //暂时性的路径存储，防止重复加载
//        private List<string> TempPath = new List<string>();
//        /// <summary>
//        /// 创建AB包的预加载任务
//        /// </summary>
//        /// <param name="paths">AB包名和资源名</param>
//        /// <param name="callback"></param>
//        public void CreatePreloadTaskFromPaths((E_ABPackName, string)[] paths, UnityAction<Res[]> callback = null)
//        {
//            PreLoadTask preLoadTask = new PreLoadTask();
//            foreach (var path in paths)
//            {
//                // 现在你可以使用nameFieldValue
//                    string abName = path.Item1.ToString();
//                    string resName = path.Item2;
//                    AsyncLoadTask task = ResLoader.Instance.CreateAB_Async<UnityEngine.Object>((E_ABPackName)Enum.Parse(typeof(E_ABPackName), abName), resName, null);
//                    preLoadTask.TaskList.Add(task);

//            }
//            //回调
//            preLoadTask.callback = callback;
//            CreatePreLoadTask(preLoadTask);

//        }
//    }
//   internal class PreLoadTask
//    {
//        public UnityAction<Res[]> callback;
//        public List<AsyncLoadTask> TaskList = new List<AsyncLoadTask>();
//    }
//}


