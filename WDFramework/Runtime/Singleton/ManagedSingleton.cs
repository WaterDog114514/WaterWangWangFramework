using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 有系统管控的系统单利，会在构造时候自动加入系统管理器
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ManagedSingleton<T> : ISystem where T : class, new()
{
    private static T instance;
    public ManagedSingleton()
    {
        Debug.Log($"{typeof(T).Name} 系统初始化");
        instance = this as T;
        Initialized();
    }

    /// <summary>
    /// 初始化方法，当此系统刚刚创建时候会调用
    /// </summary>
    public abstract void Initialized();
    //用于加锁的对象
    protected static readonly object lockObj = new object();
    //属性的方式
    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                lock (lockObj)
                {
                    return instance;
                }
            }
            else
            {
                // 抛出异常或隐式创建（根据设计需求）
                throw new InvalidOperationException($"请通过 GameSystemManager 注册系统,当前启动系统为：{typeof(T).Name}");
            }
        }
    }

    public virtual void DestorySystem()
    {
        instance = null;
    }
}
