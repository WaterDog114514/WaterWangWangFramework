// ====================== 请求结构定义 ======================
using System;
using System.Collections.Generic;

/// <summary>
/// 无参请求信息类（带返回值）
/// </summary>
/// <typeparam name="TResult">返回值类型</typeparam>
public class RequestInfo<TResult> : BaseEventInfo
{
    public List<PriorityFunc<TResult>> funcs = new List<PriorityFunc<TResult>>(); // 存储优先级请求的列表

    /// <summary>
    /// 添加请求监听（自动排序）
    /// </summary>
    /// <param name="func">请求方法</param>
    /// <param name="priority">优先级</param>
    public void AddListener(Func<TResult> func, int priority)
    {
        var item = new PriorityFunc<TResult>(priority, func);
        // 找到第一个比当前优先级大的位置插入
        int index = funcs.FindIndex(f => f.priority > priority);
        if (index == -1)
            funcs.Add(item);
        else
            funcs.Insert(index, item);
    }

    /// <summary>
    /// 移除指定请求监听
    /// </summary>
    public void RemoveListener(Func<TResult> func)
    {
        funcs.RemoveAll(f => f.func.Equals(func));
    }
}

/// <summary>
/// 单参数请求信息类（带返回值）
/// </summary>
/// <typeparam name="TParam">参数类型</typeparam>
/// <typeparam name="TResult">返回值类型</typeparam>
public class RequestInfo<TParam, TResult> : BaseEventInfo
{
    public List<PriorityFunc<TParam, TResult>> funcs = new List<PriorityFunc<TParam, TResult>>();

    public void AddListener(Func<TParam, TResult> func, int priority)
    {
        var item = new PriorityFunc<TParam, TResult>(priority, func);
        int index = funcs.FindIndex(f => f.priority > priority);
        if (index == -1)
            funcs.Add(item);
        else
            funcs.Insert(index, item);
    }

    public void RemoveListener(Func<TParam, TResult> func)
    {
        funcs.RemoveAll(f => f.func.Equals(func));
    }
}
