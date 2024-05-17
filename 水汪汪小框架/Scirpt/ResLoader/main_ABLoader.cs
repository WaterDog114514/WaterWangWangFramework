using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 加载资源模式，是通过异步还是同步进行加载
/// </summary>
public enum E_LoadModel
{
    Async,
    Sync
}
class main_ABLoader
{
    private FrameworkSettingData settingData;


    public Dictionary<string, AssetBundle> dic_Bundle = new Dictionary<string, AssetBundle>();
    /// <summary>
    /// 主包名，根据 ab 包获取
    /// </summary>
    private string MainName => settingData.abLoadSetting.ABMainName;
    /// <summary>
    /// 已加载到的主包
    /// </summary>
    private AssetBundle mainAB = null;
    /// <summary>
    /// 主包依赖获取配置文件
    /// </summary>
    private AssetBundleManifest manifest = null;
    /// <summary>
    /// AB资源包存储目录，在游戏目录的地址，通过框架总设置进行设置
    /// </summary>
    private string ABPath
    {
        get
        {
            if (settingData.abLoadSetting.IsStreamingABLoad)
                return Application.streamingAssetsPath + "/";
            return settingData.abLoadSetting.ABLoadPath;
        }
    }
    public main_ABLoader()
    {
        settingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
        LoadMainBundle();
    }
    //加载主包，只用调用一次即可
    private void LoadMainBundle()
    {
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(ABPath + MainName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }
    /// <summary>
    /// 同步加载依赖包
    /// </summary>
    /// <param name="abName"></param>
    private void LoadDependenciesAsset_Sync(string abName)
    {
        if (mainAB == null) LoadMainBundle();
        string[] dependencies = manifest.GetAllDependencies(abName);
        //遍历所有的依赖包名
        foreach (string dependencyName in dependencies)
        {
            //加载过这个依赖包了，就跳过到下一个
            if (dic_Bundle.ContainsKey(dependencyName)) continue;
            //同步加载依赖包
            AssetBundle ab = AssetBundle.LoadFromFile(ABPath + dependencyName);
            dic_Bundle.Add(dependencyName, ab);
        }

    }

    /// <summary>
    /// 异步加载依赖包
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    private IEnumerator LoadDependenciesAsset_Async(string abName)
    {
        if (mainAB == null) LoadMainBundle();
        string[] dependencies = manifest.GetAllDependencies(abName);

        //遍历所有的依赖包名
        foreach (string dependencyName in dependencies)
        {
            //加载过这个依赖包了，就跳过到下一个
            if (dic_Bundle.ContainsKey(dependencyName))
            {
                //同时加载时候，防止卡Null
                while (dic_Bundle[dependencyName] == null)
                    yield return null;
                
                continue;
            }
            //一开始异步加载 就记录 如果此时的记录中的值 是null 那证明这个ab包正在被异步加载
            dic_Bundle.Add(dependencyName, null);
            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(ABPath + dependencyName);
            yield return req;
            //异步加载结束后 再替换之前的null  这时 不为null 就证明加载结束了
            dic_Bundle[dependencyName] = req.assetBundle;

        }
    }

    //真正同步加载 多合一
    public T ReallyLoadSync<T>(string abName, string resName) where T : UnityEngine.Object
    {
        //加载依赖包
        LoadDependenciesAsset_Sync(abName);
        //判断是否加载了AB包
        AssetBundle ab = null;
        if (!dic_Bundle.ContainsKey(abName))
        {
            //加载AB包
            ab = AssetBundle.LoadFromFile(ABPath + abName);
            dic_Bundle.Add(abName, ab);
        }

        else
        {
            ab = dic_Bundle[abName];
        }
        //加载包中资源
        Res resInfo = new Res(typeof(T));
        //加载完成逻辑
        T res = ab.LoadAsset<T>(resName);
        resInfo.Asset = res;
        //完成任务啦
        return res;
    }
    /// <summary>
    /// 真正的异步加载 二合一
    /// </summary>
    /// <typeparam name="T">加载好的资源</typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callback">加载回调</param>
    /// <returns></returns>
    public IEnumerator ReallyLoadAsync<T>(string abName, string resName, AsyncLoadTask task) where T : UnityEngine.Object
    {
        Res resInfo = null;
        //异步加载，先等异步加载依赖包完毕后再加载
        yield return MonoManager.Instance.StartCoroutine(LoadDependenciesAsset_Async(abName));

        //第一次加载某AB包↓
        if (!dic_Bundle.ContainsKey(abName))
        {
            //第一次进行异步加载，先给dic分空间
            dic_Bundle.Add(abName, null);
            //加载AB包中的资源逻辑
            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(ABPath + abName);
            yield return req;
            //异步加载结束后 再替换之前的null  这时 不为null 就证明加载结束了
            dic_Bundle[abName] = req.assetBundle;
            Debug.Log("矩阵完成");
        }
        //同时加载时候，防止卡Null
        while (dic_Bundle[abName] == null)
            yield return null;


        //根据所要加载的东西分配不同唯一的资源信息类

        //重要！！！
        //重要等写！！！
        // ResInfo对接，读取配置表相关操作
        //重要！！！
        //重要！！！

        //创建命令  预加载或回调为null，就不要调用啦
        //创建资源信息
        AssetBundleRequest abq = dic_Bundle[abName].LoadAssetAsync<T>(resName);


        resInfo = new Res(typeof(T));
        //加载进度更新逻辑
        while (!abq.isDone)
        {
            task.LoadProcess = abq.progress;
            yield return null;
        }

        //完成加载
        yield return abq;

        Debug.Log(abq.asset);
        resInfo.Asset = abq.asset as T;
        task.FinishTask(resInfo);
        //参数传入和调用

    }

    /// <summary>
    /// 卸载指定AB包
    /// </summary>
    public void UnLoadAssetBundle(string abName)
    {
        MonoManager.Instance.StartCoroutine(ReallyUnLoadAssetBundle(abName));
    }
    /// <param name="abName"></param>
    private IEnumerator ReallyUnLoadAssetBundle(string abName)
    {
        if (dic_Bundle.ContainsKey(abName))
        {
            //有资源加载就等到加载结束
            while (dic_Bundle[abName] == null)
            {
                yield return null;
            }
            dic_Bundle[abName].Unload(false);
            dic_Bundle.Remove(abName);
            yield break;
        }
        Debug.LogError("资源卸载失败，不存在此AB包");
    }

    /// <summary>
    /// 预加载某资源包中所有的资源名
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    public IEnumerator getABAllResName(string abName, UnityAction<string[]> callback)
    {
        if (!dic_Bundle.ContainsKey(abName))
        {
            //第一次进行异步加载，先给dic分空间
            dic_Bundle.Add(abName, null);
            //加载AB包中的资源逻辑
            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(ABPath + abName);
            yield return req;
            //异步加载结束后 再替换之前的null  这时 不为null 就证明加载结束了
            dic_Bundle[abName] = req.assetBundle;
        }
        callback(dic_Bundle[abName].GetAllAssetNames());

    }



}


/// <summary>
/// 预加载而用 加载一个AB包所有资源
/// </summary>
/// <typeparam name = "T" ></ typeparam >
/// < param name="abName"></param>
/// <returns></returns>
//public IEnumerator ReallyLoadAllAssetAsync<T>(string abName, AsyncLoadTask task, UnityAction<AsyncLoadTask[]> callback) where T : UnityEngine.Object
//{
//    异步加载，先等异步加载依赖包完毕后再加载
//    yield return MonoManager.Instance.StartCoroutine(LoadDependenciesAsset_Async(abName));

//    if (!dic_Bundle.ContainsKey(abName))
//    {
//        第一次进行异步加载，先给dic分空间
//        dic_Bundle.Add(abName, null);
//        加载AB包中的资源逻辑
//        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(ABPath + abName);
//        yield return req;
//        异步加载结束后 再替换之前的null  这时 不为null 就证明加载结束了
//        dic_Bundle[abName] = req.assetBundle;
//    }
//    加载包中所有资源


//    AssetBundleRequest abq = dic_Bundle[abName].LoadAllAssetsAsync<T>();
//    加载进度更新逻辑
//    while (!abq.isDone)
//    {
//        task.LoadProcess = abq.progress;
//        Debug.Log(abq.progress);
//        yield return null;
//    }
//    yield return abq;

//    T[] loadRes = new T[abq.allAssets.Length];
//    先把资源装进去嘛
//    for (int i = 0; i < loadRes.Length; i++)
//    {
//        loadRes[i] = abq.allAssets[i] as T;
//    }

//    给所有加载好的资源分配任务，让其存储到外部
//    AsyncLoadTask[] tasks = new AsyncLoadTask[loadRes.Length];


//    for (int i = 0; i < tasks.Length; i++)
//    {
//        Res<T> resInfo = new Res<T>();
//        resInfo.asset = loadRes[i];
//        tasks[i] = new AsyncLoadTask();
//        分任务执行完毕
//        tasks[i].FinishTask(resInfo);
//    }

//    总任务完成操作
//    Res<T[]> TotalInfo = new Res<T[]>();
//    TotalInfo.asset = loadRes;
//    task.FinishTask(TotalInfo);
//    重要！！！
//    重要等写！！！
//     ResInfo对接，读取配置表相关操作
//    重要！！！
//    重要！！！

//    这里回调，会把他们装进dic里了
//    callback(tasks);
//}