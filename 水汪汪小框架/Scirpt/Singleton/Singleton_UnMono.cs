using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 非mono的单例模式基类，已实现线程锁
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton_UnMono<T> where T:class,new()
{
    private static T instance;
    //用于加锁的对象
    protected static readonly object lockObj = new object();
    //属性的方式
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}
