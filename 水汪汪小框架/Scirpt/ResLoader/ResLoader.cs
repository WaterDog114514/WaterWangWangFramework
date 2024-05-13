using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 资源加载的核心类，封装了各个子模块的加载
/// </summary>
public class ResLoader : Singleton_UnMono<ResLoader>
{
    //所有子模块
    private main_ResoucesLoader resourcesLoader;
    private main_ABLoader abLoader;
    private main_EditorABLoader editorLoader;
    private main_Preloader preloader;
    //核心设置配置文件
    private FrameworkSettingData settingData;
    /// <summary>
    /// 所有的异步加载任务，包括已经执行和未执行的
    /// </summary>
    private Dictionary<string, AsyncLoadTask> dic_LoadedTask = new Dictionary<string, AsyncLoadTask>();
    /// <summary>
    /// 已经加载过的资源
    /// </summary>
    private Dictionary<string, Res> dic_LoadedRes = new Dictionary<string, Res>();
    public ResLoader()
    {
        settingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
        //初始化所有必备加载子模块
        resourcesLoader = new main_ResoucesLoader();
        abLoader = new main_ABLoader();
        preloader = new main_Preloader();
#if UNITY_EDITOR
        editorLoader = new main_EditorABLoader();
#endif
    }

    #region AB包加载相关
    /// <summary>
    /// 通过类型异步加载AB包资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callback"></param>
    /// <returns></returns>M
    public AsyncLoadTask LoadAB_Async<T>(string abName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
    {

        string key = $"AB_{abName}_{resName}";
        //先任务是否第一次加载完成了吗
        if (!IsFirstAsyncLoad(key, callback))
            return dic_LoadedTask[key];

#if UNITY_EDITOR
        if (settingData.abLoadSetting.IsDebugABLoad)
        {
            EditorLoadABRes(abName, resName, callback);
            return dic_LoadedTask[key];
        }
#endif

        //第一次就新建任务
        AsyncLoadTask task = new AsyncLoadTask();
        task.IntiLoadOperation(abLoader.ReallyLoadAsync<T>(abName, resName, task));
        task.AddCallbackCommand(new LoadResAsyncCommand<T>(callback, task));
        //加载资源占位
        dic_LoadedRes.Add(key, null);
        //异步加载完成记录
        task.AddCallbackCommand(() =>
        {
            dic_LoadedRes[key] = task.ResInfo;
        });
        //记录到字典中
        dic_LoadedTask.Add(key, task);
        return task;
    }
    /// <summary>
    /// 通过名字异步加载AB包资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public AsyncLoadTask LoadAB_Async(string abName, string resName, UnityAction<UnityEngine.Object> callback)
    {
        return LoadAB_Async<UnityEngine.Object>(abName, resName, callback);
    }


    /// <summary>
    /// (开始任务后立即加载)通过类型进行同步加载AB包
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public T LoadAB_Sync<T>(string abName, string resName) where T : UnityEngine.Object
    {
        string key = $"AB_{abName}_{resName}";
        if (dic_LoadedRes.ContainsKey(key))
        {
            if (dic_LoadedRes[key] != null)
                return dic_LoadedRes[key].GetAsset<T>();
            else Debug.LogError("获取资源失败，可能是正在进行异步加载中！，请通过异步获取");
        }
#if UNITY_EDITOR
        //编辑器调试加载
        if (settingData.abLoadSetting.IsDebugABLoad)
        {
            return EditorLoadABRes<T>(abName, resName);
        }
#endif
        //加载操作
        T res = abLoader.ReallyLoadSync<T>(abName, resName);
        Res ResInfo = new Res(typeof(T));
        ResInfo.Asset = res;
        dic_LoadedRes.Add(key, ResInfo);
        return res;

    }
    /// <summary>
    /// 通过名字进行同步加载AB包
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object LoadAB_Sync(string abName, string resName)
    {
        return LoadAB_Sync<UnityEngine.Object>(abName, resName);
    }

    #endregion

    #region Resources加载模块
    /// <summary>
    /// 异步加载Resources 按类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public AsyncLoadTask LoadRes_Async<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
    {
        //先任务是否第一次加载完成了吗
        if (!IsFirstAsyncLoad(path, callback))
        {
            return dic_LoadedTask[path];
        }

        //第一次就新建任务
        AsyncLoadTask task = new AsyncLoadTask();
        task.IntiLoadOperation(resourcesLoader.reallyLoadAsync<T>(path, task));
        task.AddCallbackCommand(new LoadResAsyncCommand<T>(callback, task));
        //加载资源占位
        dic_LoadedRes.Add(path, null);
        //异步加载完成记录
        task.AddCallbackCommand(() =>
        {
            dic_LoadedRes[path] = task.ResInfo;
        });
        dic_LoadedTask.Add(path, task);
        return task;
    }
    /// <summary>
    /// 同步快速加载配置文件等，需要非常迅速，所以不用Task了，直接完成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T LoadRes_Sync<T>(string path) where T : UnityEngine.Object
    {
        if (dic_LoadedRes.ContainsKey(path))
        {
            if (dic_LoadedRes[path] != null)
                return dic_LoadedRes[path].GetAsset<T>();
            else Debug.LogError("获取资源失败，可能是正在进行异步加载中！，请通过异步获取");
        }
        return resourcesLoader.LoadSync<T>(path);
    }
    #endregion

    #region 预加载模块相关
    /// <summary>
    /// 开始进行预加载  (需要先创建预加载任务！！)
    /// </summary>
    public void StartPreload()
    {
        preloader.StartLoad();
    }
    /// <summary>
    /// 创建预加载任务
    /// </summary>
    public void CreatePreloadTask(string taskName, PreLoadInfo[] infos, Type type = null)
    {
        PreloadResTask task = new PreloadResTask() { ResInfos = infos, taskName = taskName };
        preloader.CreatePreLoadTask(task, type);
    }
    public void CreatePreloadTask(PreloadResTask task, Type type = null)
    {
        preloader.CreatePreLoadTask(task, type);
    }
    /// <summary>
    /// 加载某AB包中所有有关类型资源
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="ABName"></param>
    /// <param name="type">按类型加载，null为加载某AB包中所有资源</param>
    public void CreatePreloadABTask(string taskName, string ABName, Type type = null)
    {
        preloader.waitLoadAssetBundle.Add(MonoManager.Instance.StartCoroutine(preloader.ReallyCreatePreloadABTask(taskName, ABName, type, abLoader)));
    }


    #endregion


    /// <summary>
    /// 判断是不是第一次执行异步加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private bool IsFirstAsyncLoad<T>(string key, UnityAction<T> callback = null) where T : UnityEngine.Object
    {
        //先判断是否是第一次加载某任务
        if (!dic_LoadedTask.ContainsKey(key)) return true;
        //创建任务用于判断
        AsyncLoadTask task = dic_LoadedTask[key];
        //编辑器逻辑
#if UNITY_EDITOR
        //if (settingData.abLoadSetting.IsDebugABLoad && !task.isFinish )
        //{
        //    if (!task.isFinish) (task as SyncLoadTask).AddOperation(() =>
        //    {
        //        callback(task.getRes<T>());
        //    });
        //    return task;
        //}
#endif
        if (!task.isFinish)
        {
            task.AddCallbackCommand(new LoadResAsyncCommand<T>(callback, task));
        }
        //加载了就直接调用
        else
        {
            callback?.Invoke(dic_LoadedTask[key].GetAsset<T>());
        }
        return false;
    }
    //使用编辑器加载时候记录资源
    private T EditorLoadABRes<T>(string abName, string resName, UnityAction<T> callback = null) where T : UnityEngine.Object
    {
        AsyncLoadTask task = new AsyncLoadTask();
        T res = editorLoader.LoadEditorRes<T>(abName + "/" + resName);

        //直接秒速完成任务
        Res resInfo = new Res(typeof(T));
        resInfo.Asset = res;
        callback?.Invoke(res);
        task.FinishTask(resInfo);
        dic_LoadedRes.Add($"AB_{abName}_{resName}", resInfo);
        dic_LoadedTask.Add($"AB_{abName}_{resName}", task);
        return res;
    }



}