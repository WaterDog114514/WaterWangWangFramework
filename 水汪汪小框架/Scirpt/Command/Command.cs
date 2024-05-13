using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 无参命令
/// </summary>
public class Command : ICommand
{
    private UnityAction action;
    public Command(UnityAction action)
    {
        this.action = action;
    }

    public void Execute()
    {
        action?.Invoke();
    }
}
/// <summary>
/// 单个参数命令
/// </summary>
/// <typeparam name="T"></typeparam>
public class Command<T> : ICommand
{
    public T parameter;
    private UnityAction<T> action;
    public Command(UnityAction<T> action, T paramter)
    {
        this.parameter = paramter;
        this.action = action;
    }

    public void Execute()
    {
        action?.Invoke(parameter);
    }
}
/// <summary>
/// 两个参数命令
/// </summary>
/// <typeparam name="T"></typeparam>
public class Command<T1, T2> : ICommand
{
    public T1 parameter1;
    public T2 parameter2;
    private UnityAction<T1, T2> action;

    public Command(UnityAction<T1, T2> action, T1 paramter1, T2 paramter2)
    {
        this.action = action;
        this.parameter1 = paramter1;
        this.parameter2 = paramter2;
    }
    public void Execute()
    {
        action?.Invoke(parameter1, parameter2);
    }
}


/// <summary>
/// 异步加载资源回调命令
/// </summary>
/// <typeparam name="T"></typeparam>
public class LoadResAsyncCommand<T> : ICommand   where T : UnityEngine.Object  
{
    public AsyncLoadTask task;
    private UnityAction<T> action;
    public LoadResAsyncCommand(UnityAction<T> action, AsyncLoadTask task)
    {
        this.task = task;
        this.action = action;
    }

    public void Execute()
    {
        action?.Invoke(task.GetAsset<T>());
    }
}