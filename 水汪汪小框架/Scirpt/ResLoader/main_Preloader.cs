using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 预加载核心
/// </summary>
class main_Preloader
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
    public List<PreloadResTask> preloadResTasks = new List<PreloadResTask>();
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
        preloadResTasks.Clear();
        TotalResNum = 0;
        LoadedResNum = 0;
        CurrentTaskName = null;
        waitLoadAssetBundle.Clear();

    }

    /// <summary>
    /// 预加载资源 一般是加载场景时候调用 只有预加载完毕才能加载新场景
    /// </summary>
    public IEnumerator ReallyPreLoadRes()
    {
        //对于加载整个包，先等加载完毕
        foreach (var wait in waitLoadAssetBundle)
        {
            yield return wait;
        }

        //先统计所有要加载资源的数量
        foreach (var task in preloadResTasks)
        {
            TotalResNum += task.ResInfos.Length;
        }

        //先根据每个任务分配自己的协程
        foreach (var task in preloadResTasks)
        {
            //设置当前任务名
            CurrentTaskName = task.taskName;
            //先分配协程
            DistributeCoroutine(task);
            //根据单个任务的协程进行记录
            foreach (var coroutine in task.coroutines)
            {
                yield return coroutine;
                //单资源完成增加
                LoadedResNum++;
                Debug.Log($"加载进度{LoadedResNum}/{TotalResNum}");
                //进度条更新逻辑，使用事件中心
                // do do do

            }
        }

        //加载完毕，清除所有任务
        ClearAllTasks();

    }
    /// <summary>
    /// 创建预加载任务
    /// </summary>
    public void CreatePreLoadTask(PreloadResTask task, Type type = null)
    {
        task.LoadType = type;
        preloadResTasks.Add(task);
    }
    public List<Coroutine> waitLoadAssetBundle = new List<Coroutine>();
    //加载整个AB包中所有指定类型资源
    public IEnumerator ReallyCreatePreloadABTask(string taskName, string ABName, Type type, main_ABLoader abLoader)
    {

        string[] AllResName = null;
        //异步获取AB包中所有的资源名
        yield return MonoManager.Instance.StartCoroutine(abLoader.getABAllResName(ABName, (names) => { 
            
            AllResName = names;
        
        }));
        if (AllResName.Length == 0)
        {
            Debug.LogWarning("此AB资源包中资源数量为0！！");
            yield break;
        }
        PreLoadInfo[] infos = new PreLoadInfo[AllResName.Length];
        for (int i = 0; i < AllResName.Length; i++)
        {
            infos[i] = new PreLoadInfo() { ABName = ABName, ResName = AllResName[i] };
        }
        PreloadResTask task = new PreloadResTask() { taskName = taskName, ResInfos = infos };
        CreatePreLoadTask(task, type);
    }
    /// <summary>
    /// 根据一个任务的包名，资源名，给这个任务匹配自己的加载协程
    /// </summary>
    private void DistributeCoroutine(PreloadResTask task)
    {
        task.coroutines = new Coroutine[task.ResInfos.Length];
        //加载资源任务
        for (int i = 0; i < task.coroutines.Length; i++)
        {
            //分配每一个任务的协程
           //  task.coroutines[i] = ResLoader.Instance.LoadAB_Async(task.ResInfos[i].ABName, task.ResInfos[i].ResName, null);
        }
    }
}
/// <summary>
/// 按类型进行分类预加载
/// </summary>
public class PreloadResTask
{
    //加载类型
    public Type LoadType;
    public Coroutine[] coroutines;
    /// <summary>
    /// 任务名
    /// </summary>
    public string taskName;
    /// <summary>
    /// 所加载的资源们的信息
    /// </summary>
    public PreLoadInfo[] ResInfos;

}
public class PreLoadInfo
{
    public string ABName;
    public string ResName;
}