using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 全局事件中心管理器（支持优先级）
/// 用于处理全局事件和请求
/// 适用于模块与系统间或系统与系统间的信息交流
/// </summary>
public class EventCenterSystem : Singleton<EventCenterSystem>
{
    /// <summary>
    /// 事件库容纳器
    /// Key: 事件枚举类型
    /// Value: 对应的事件管理器
    /// </summary>
    private Dictionary<Type, BaseEventManager> dic_EventManager = new Dictionary<Type, BaseEventManager>();

    /// <summary>
    /// 初始化事件中心
    /// </summary>
    public EventCenterSystem()
    {
        Init();
    }

    /// <summary>
    /// 初始化默认事件库
    /// </summary>
    private void Init()
    {
        dic_EventManager[typeof(E_FrameworkEvent)] = new EventManager<E_FrameworkEvent>();
        dic_EventManager[typeof(E_InputEvent)] = new EventManager<E_InputEvent>();
        dic_EventManager[typeof(E_GameEvent)] = new EventManager<E_GameEvent>();
    }
    /// <summary>
    /// 注册新的事件库
    /// </summary>
    public void RegisterEventContainer<T>() where T :Enum
    {
        if(dic_EventManager.ContainsKey(typeof(T)))
        {
            Debug.LogWarning($"已包含{typeof(T).Name}事件库，无需重复注册");
            return;
        }    
        dic_EventManager[typeof(T)] = new EventManager<T>();
    }
    #region 事件处理（支持优先级）
    /// <summary>
    /// 添加无参数事件监听（支持优先级）
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <param name="name">事件名称</param>
    /// <param name="action">监听方法</param>
    /// <param name="priority">优先级（默认0，越小越先执行）</param>
    public void AddEventListener<T>(T name, UnityAction action, int priority = 0) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.AddEventListener(name, action, priority);
        }
    }

    /// <summary>
    /// 添加带单参数的事件监听（支持优先级）
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <param name="name">事件名称</param>
    /// <param name="action">监听方法</param>
    /// <param name="priority">优先级（默认0，越小越先执行）</param>
    public void AddEventListener<T, TParam>(T name, UnityAction<TParam> action, int priority = 0) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.AddEventListener(name, action, priority);
        }
    }

    /// <summary>
    /// 添加带双参数的事件监听（支持优先级）
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TParam1">参数1类型</typeparam>
    /// <typeparam name="TParam2">参数2类型</typeparam>
    /// <param name="name">事件名称</param>
    /// <param name="action">监听方法</param>
    /// <param name="priority">优先级（默认0，越小越先执行）</param>
    public void AddEventListener<T, TParam1, TParam2>(T name, UnityAction<TParam1, TParam2> action, int priority = 0) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.AddEventListener(name, action, priority);
        }
    }
    /// <summary>
    /// 触发无参全局事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    public void TriggerEvent<T>(T name) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.TriggerEvent(name);
        }
    }

    /// <summary>
    /// 触发带单参数的事件
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <param name="name">事件名称</param>
    /// <param name="parameter">参数值</param>
    public void TriggerEvent<T, TParam>(T name, TParam parameter) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.TriggerEvent(name, parameter);
        }
    }

    /// <summary>
    /// 触发带双参数的事件
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TParam1">参数1类型</typeparam>
    /// <typeparam name="TParam2">参数2类型</typeparam>
    /// <param name="name">事件名称</param>
    /// <param name="parameter1">参数1值</param>
    /// <param name="parameter2">参数2值</param>
    public void TriggerEvent<T, TParam1, TParam2>(T name, TParam1 parameter1, TParam2 parameter2) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.TriggerEvent(name, parameter1, parameter2);
        }
    }

    /// <summary>
    /// 移除无参数事件监听
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <param name="name">事件名称</param>
    /// <param name="action">事件方法</param>
    public void RemoveEventListener<T>(T name, UnityAction action) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.RemoveEventListener(name,action);
        }
    }
    /// <summary>
    /// 移除有一个参数事件监听
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <param name="name">事件名称</param>
    /// <param name="action">事件方法</param>
    public void RemoveEventListener<T, par1>(T name, UnityAction<par1> action) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.RemoveEventListener(name, action);
        }
    }
    /// <summary>
    /// 移除有2个参数事件监听
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <param name="name">事件名称</param>
    /// <param name="action">事件方法</param>
    public void RemoveEventListener<T, par1,par2>(T name, UnityAction<par1,par2> action) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.RemoveEventListener(name, action);
        }
    }

    #endregion

    #region 请求处理（支持优先级）
    /// <summary>
    /// 添加无参数请求监听（支持优先级）
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="name">请求名称</param>
    /// <param name="func">请求方法</param>
    /// <param name="priority">优先级（默认0，越小越先执行）</param>
    public void AddRequestListener<T, TResult>(T name, Func<TResult> func, int priority = 0) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.AddRequestListener(name, func, priority);
        }
    }

    /// <summary>
    /// 添加带单参数的请求监听（支持优先级）
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="name">请求名称</param>
    /// <param name="func">请求方法</param>
    /// <param name="priority">优先级（默认0，越小越先执行）</param>
    public void AddRequestListener<T, TParam, TResult>(T name, Func<TParam, TResult> func, int priority = 0) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.AddRequestListener(name, func, priority);
        }
    }

    /// <summary>
    /// 触发无参数请求
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="name">请求名称</param>
    /// <returns>最后一个监听器的返回值</returns>
    public TResult TriggerRequest<T, TResult>(T name) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            return eventManager.TriggerRequest<TResult>(name);
        }
        return default;
    }

    /// <summary>
    /// 触发带单参数的请求
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="name">请求名称</param>
    /// <param name="parameter">参数值</param>
    /// <returns>最后一个监听器的返回值</returns>
    public TResult TriggerRequest<T, TParam, TResult>(T name, TParam parameter) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            return eventManager.TriggerRequest<TParam, TResult>(name, parameter);
        }
        return default;
    }

    /// <summary>
    /// 移除无参数请求监听
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="name">请求名称</param>
    /// <param name="func">请求方法</param>
    public void RemoveRequestListener<T, TResult>(T name, Func<TResult> func) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.RemoveRequestListener(name, func);
        }
    }

    /// <summary>
    /// 移除带单参数的请求监听
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="name">请求名称</param>
    /// <param name="func">请求方法</param>
    public void RemoveRequestListener<T, TParam, TResult>(T name, Func<TParam, TResult> func) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.RemoveRequestListener(name, func);
        }
    }
    #endregion

    #region 事件清理
    /// <summary>
    /// 清除指定事件的所有监听
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <param name="name">事件名称</param>
    public void ClearEventListener<T>(T name) where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.Clear(name);
        }
    }

    /// <summary>
    /// 清空指定事件库的所有事件监听
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    public void ClearEvent<T>() where T : Enum
    {
        if (TryGetEventManager<T>(out var eventManager))
        {
            eventManager.ClearAll();
        }
    }

    /// <summary>
    /// 清空所有事件库的所有事件监听
    /// </summary>
    public void ClearAllEvent()
    {
        foreach (var eventManager in dic_EventManager.Values)
        {
            eventManager.ClearAll();
        }
    }
    #endregion

    #region 辅助方法
    /// <summary>
    /// 尝试获取指定类型的事件管理器
    /// </summary>
    /// <typeparam name="T">事件枚举类型</typeparam>
    /// <param name="eventManager">返回的事件管理器</param>
    /// <returns>是否成功获取</returns>
    private bool TryGetEventManager<T>(out EventManager<T> eventManager) where T : Enum
    {
        if (dic_EventManager.TryGetValue(typeof(T), out var baseEventManager))
        {
            eventManager = baseEventManager as EventManager<T>;
            return true;
        }

        Debug.LogError($"找不到事件库：{typeof(T).Name}");
        eventManager = null;
        return false;
    }
    #endregion
}