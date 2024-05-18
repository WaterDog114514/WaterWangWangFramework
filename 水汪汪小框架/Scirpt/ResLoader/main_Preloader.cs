using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 预加载核心
/// </summary>
public class main_Preloader
{
    /// <summary>
    /// 总需要加载资源的数量
    /// </summary>
    public int TotalResNum;

    /// <summary>
    /// 已加载资源数量
    /// </summary>
    public int LoadedResNum;
    /// <summary>
    /// 正在加载的任务名
    /// </summary>
    public string CurrentTaskName;
    public List<PreLoadTask> preloadResTasks = new List<PreLoadTask>();

    /// <summary>
    /// 开始进行预加载
    /// </summary>
    public void StartLoad()
    {

        if (preloadResTasks.Count == 0) Debug.LogError("预加载任务为0，请添加预加载任务后再执行");
        MonoManager.Instance.StartCoroutine(ReallyPreLoadRes());
    }
    /// <summary>
    /// 清除所有加载任务
    /// </summary>
    private void ClearAllTasks()
    {
        //释放所有加载记录的信息
        preloadResTasks.Clear();
        TotalResNum = 0;
        LoadedResNum = 0;
        CurrentTaskName = null;
        TempPath.Clear();
    }

    /// <summary>
    /// 预加载资源 一般是加载场景时候调用 只有预加载完毕才能加载新场景
    /// </summary>
    public IEnumerator ReallyPreLoadRes()
    {
        //先统计所有要加载资源的数量
        TotalResNum += preloadResTasks.Count;
        Coroutine currentCoroutine = null;
        //先根据总预加载任务分出小分支一个同类的加载任务
        for (int j = 0; j < preloadResTasks.Count; j++)
        {
            //进度更新之大类

            PreLoadTask preLoadTask = preloadResTasks[j];
            //回调资源
            Res[] LoadedRes = new Res[preLoadTask.TaskList.Count];
            //再根据总任务中的异步任务加载
            for (int i = 0; i < preLoadTask.TaskList.Count; i++)
            {

                AsyncLoadTask task = preLoadTask.TaskList[i];
                //加载完成不必加载
                if (task.isFinish) continue;
                currentCoroutine = task.StartAsyncLoad();
                yield return currentCoroutine;
                //记录加载后的回调资源
                LoadedRes[i] = task.ResInfo;
                //单资源完成增加
                LoadedResNum++;
                //进度条更新逻辑小类进度，使用事件中心
                Debug.Log($"加载进度{LoadedResNum}/{TotalResNum}");
            }
            //加载完一个预加载大任务就会调用一下
            preLoadTask.callback?.Invoke(LoadedRes);

        }



        //加载完毕，清除所有任务
        ClearAllTasks();
        //回调一下预加载好的资源哦
    }
    /// <summary>
    /// 创建预加载任务
    /// </summary>
    public void CreatePreLoadTask(PreLoadTask task)
    {
        preloadResTasks.Add(task);
    }
    //暂时性的路径存储，防止重复加载
    private List<string> TempPath = new List<string>();

    public void CreatePreloadTaskFromPaths(string[] paths, UnityAction<Res[]> callback = null)
    {
        PreLoadTask preLoadTask = new PreLoadTask();
        for (int i = 0; i < paths.Length; i++)
        {
            // 现在你可以使用nameFieldValue
            string path = paths[i];
            //根据方式加载
            if (IsABOrResLoadFromPath(path))
            {
                string abName = path.Substring(0, path.IndexOf('/'));
                string resName = path.Replace(abName + "/", null);
                AsyncLoadTask task = ResLoader.Instance.CreateAB_Async<UnityEngine.Object>(abName, resName, null);
                preLoadTask.TaskList.Add(task);
            }
            else if (IsABOrResLoadFromPath(path))
            {
                AsyncLoadTask task = ResLoader.Instance.CreateRes_Async<UnityEngine.Object>(path, null);
                preLoadTask.TaskList.Add(task);
            }

        }
        preLoadTask.callback = callback;
        CreatePreLoadTask(preLoadTask);

    }

    /// <summary>
    ///从路径判断是AB包还是Res加载  
    /// </summary>
    /// <param name="path"></param>
    /// <returns>true为AB包，false为Res</returns>
    private bool IsABOrResLoadFromPath(string path)
    {
       string temp = path.Substring(0,3);
        if (temp == "Res")
        {
            return false;
        }
        return true;    
    }
}


public class PreLoadTask
{
    public UnityAction<Res[]> callback;
    public List<AsyncLoadTask> TaskList = new List<AsyncLoadTask>();
}