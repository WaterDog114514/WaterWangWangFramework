using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 有系统管控的系统单利，会在构造时候自动加入系统管理器
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ManagedMonoSingleton<T> : MonoBehaviour, ISystem where T : MonoBehaviour
{
    private static T instance;
    //用于加锁的对象
    protected static readonly object lockObj = new object();
    protected void Awake()
    {
        Initialized();
    }
    /// <summary>
    /// 初始化方法，当此系统刚刚创建时候会调用
    /// </summary>
    public abstract void Initialized();
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
                throw new InvalidOperationException("请通过 GameSystemManager 注册系统");
            }
        }
    }
    private static GameObject obj;
    public static T  ConstructSystem()
    {
        Debug.LogWarning("第一次可行性测试，确定没问题后删LOG");
        //动态创建 动态挂载
        //在场景上创建空物体
        obj = new GameObject();
        //得到T脚本的类名 为对象改名 这样再编辑器中可以明确的看到该
        //单例模式脚本对象依附的GameObject
        obj.name = typeof(T).ToString();
        //动态挂载对应的 单例模式脚本
        instance = obj.AddComponent<T>();
       
        //过场景时不移除对象 保证它在整个游戏生命周期中都存在
        DontDestroyOnLoad(obj);
        return instance;
    }

    public virtual void DestorySystem()
    {
        //销毁物体
        Destroy(obj);
        instance = null;
    }
}
