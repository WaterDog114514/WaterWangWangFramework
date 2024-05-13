using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 异步加载任务，只有异步加载才需要通过任务进行加载
/// </summary>
public class AsyncLoadTask
{
    public float LoadProcess;
    public Res ResInfo;
    protected bool IsStartedLoad = false;
    /// <summary>
    /// 判断某资源进度是否完成
    /// </summary>
    public bool isFinish => LoadProcess == 1 ? true : false;
    /// <summary>
    /// 直接得到某加载成功后的资源本身
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetAsset<T>() where T : UnityEngine.Object => ResInfo.GetAsset<T>();

    /// <summary>
    /// 指令队列，加载时或加载前存储指令，加载完毕后会调用
    /// </summary>
    private CommandQueue commandQueue = new CommandQueue();
    public void AddCallbackCommand(UnityAction callback)
    {
        AddCallbackCommand(new Command(callback));
    }
    public void AddCallbackCommand(ICommand command)
    {
        if (commandQueue == null)
        {
            Debug.LogError("已完成加载任务，不能再添加回调命令了");
            return;
        }
        commandQueue.AddCommand(command);
    }

    /// <summary>
    /// 包括回调，改进度，资源赋值等
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resInfo"></param>
    public void FinishTask(Res resInfo)
    {
        LoadProcess = 1;
        ResInfo = resInfo;
        //清空命令
        commandQueue.ExecuteCommands();
        commandQueue = null;
    }

    private Func<Coroutine> LoadOperation;
    /// <summary>
    /// 初始化协程加载操作，有了这个，才能通过委托控制什么时候开始执行任务
    /// </summary>
    /// <param name="operation"></param>
    public void IntiLoadOperation(IEnumerator operation)
    {
        LoadOperation += () =>
        {
            return MonoManager.Instance.StartCoroutine(operation);
        };
    }
    public void StartAsyncLoad()
    {
        if (LoadOperation == null)
        {
            Debug.LogError("任务开启失败，不存在要加载的资源");
            return;
        }
        LoadOperation?.Invoke();
    }
}