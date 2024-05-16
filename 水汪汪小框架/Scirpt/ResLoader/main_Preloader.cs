using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

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
    public List<AsyncLoadTask> preloadResTasks = new List<AsyncLoadTask>();

    /// <summary>
    /// 开始进行预加载
    /// </summary>
    public void StartLoad()
    {
        //先检查看看有哪些任务已经自己加载了的？
        foreach (var task in preloadResTasks)
        {

        }

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
        //先根据每个任务分配自己的协程
        foreach (var task in preloadResTasks)
        {
            //加载完成不必加载
            if (task.isFinish) continue;

            currentCoroutine = task.StartAsyncLoad();
            yield return currentCoroutine;
            //单资源完成增加
            LoadedResNum++;
            Debug.Log($"加载进度{LoadedResNum}/{TotalResNum}");
            //进度条更新逻辑，使用事件中心
        }

        //加载完毕，清除所有任务
        ClearAllTasks();

    }
    /// <summary>
    /// 创建预加载任务
    /// </summary>
    public void CreatePreLoadTask(AsyncLoadTask task)
    {
        preloadResTasks.Add(task);
    }

    //暂时性的路径存储，防止重复加载
    private List<string> TempPath = new List<string>();

    //加载整个AB包中所有指定类型资源
    /// <summary>
    /// 根据一个任务的包名，资源名，给这个任务匹配自己的加载协程
    /// </summary>

    public void PreloadFromExcel<T>(string ResPathName = "Res" +
        "Path", E_LoadType loadType = E_LoadType.AB) where T : DataBaseContainer
    {
        T container = GameExcelDataLoader.Instance.GetDataContainer<T>();
        // 反射获取dataDic字段
        var dataDicField = typeof(T).GetField("dataDic");
        if (dataDicField == null)
        {
            Debug.LogError($"读取错误，请不要在{typeof(T).Name}中改dataDic属性名");
            return;
        }
        // 获取dataDic的值
        var dataDicValue = dataDicField.GetValue(container) as IDictionary;

        // 获取字典的值类型（类型2）
        var valueType = dataDicField.FieldType.GetGenericArguments()[1];

        // 通过类型2得到名为name的字段
        var nameField = valueType.GetField(ResPathName);
        if (nameField == null)
        {
            Debug.LogError($"读取错误，数据对象类中{valueType.Name}不存在{ResPathName}的字段");
            return;
        }

        // 遍历字典，获取名为name的字段的值
        foreach (DictionaryEntry pair in dataDicValue)
        {
            object valueObject = pair.Value;
            object nameFieldValue = nameField.GetValue(valueObject);
            // 现在你可以使用nameFieldValue
            string path = nameFieldValue.ToString();
            //防止重复创建同路径任务
            if (TempPath.Contains(path))
                continue;
            else
                TempPath.Add(path);

            //根据方式加载
            if (loadType == E_LoadType.AB)
            {
                Debug.Log(path);
                string abName = path.Substring(0, path.IndexOf('/'));
                string resName = path.Replace(abName + "/", null);
                AsyncLoadTask task = ResLoader.Instance.CreateAB_Async<UnityEngine.Object>(abName, resName, null);
                CreatePreLoadTask(task);
            }
            else if (loadType == E_LoadType.Res)
            {
                AsyncLoadTask task = ResLoader.Instance.CreateRes_Async<UnityEngine.Object>(path, null);
                CreatePreLoadTask(task);
            }
        }
    }
    /// <summary>
    /// 加载方式 是AB包加载，还是Res加载
    /// </summary>
    public enum E_LoadType
    {
        AB,
        Res
    }
}
