
using System;

/// <summary>
/// 优先级请求结构体（无参数，带返回值）
/// </summary>
public struct PriorityFunc<TResult>
{
    public int priority;          // 优先级数值（越小越先执行）
    public Func<TResult> func;    // 对应的请求方法

    public PriorityFunc(int p, Func<TResult> f)
    {
        priority = p;
        func = f;
    }
}

/// <summary>
/// 优先级请求结构体（单参数，带返回值）
/// </summary>
public struct PriorityFunc<TParam, TResult>
{
    public int priority;              // 优先级数值
    public Func<TParam, TResult> func; // 带参数的请求方法

    public PriorityFunc(int p, Func<TParam, TResult> f)
    {
        priority = p;
        func = f;
    }
}