using System.Collections.Generic;
using System;

using UnityEngine.Events;
using UnityEngine;
public abstract class BaseEventManager
{
    public abstract void ClearAll();
}
/// <summary>
/// 事件管理器（支持优先级）
/// </summary>
/// <typeparam name="T">事件类型枚举</typeparam>
public class EventManager<T> : BaseEventManager where T : Enum
{
    // 事件字典：存储所有注册的事件
    private Dictionary<T, BaseEventInfo> dic_Event = new Dictionary<T, BaseEventInfo>();
    // ====================== Event管理器实现 ======================
    #region 事件监听管理
    /// <summary>
    /// 注册无参事件监听
    /// </summary>
    /// <param name="name">事件名称</param>
    /// <param name="action">委托方法</param>
    /// <param name="priority">优先级（默认0）</param>
    public void AddEventListener(T name, UnityAction action, int priority = 5)
    {
        if (!dic_Event.TryGetValue(name, out var info))
        {
            // 创建新事件类型
            var newInfo = new EventInfo();
            newInfo.AddListener(action, priority);
            dic_Event.Add(name, newInfo);
            return;
        }

        // 类型检查并添加监听
        if (info is EventInfo eventInfo)
            eventInfo.AddListener(action, priority);
        else
            Debug.LogError($"事件类型不匹配: {name}");
    }

    /// <summary>
    /// 注册单参数事件监听
    /// </summary>
    public void AddEventListener<TP1>(T name, UnityAction<TP1> action, int priority = 5)
    {
        if (!dic_Event.TryGetValue(name, out var info))
        {
            var newInfo = new EventInfo<TP1>();
            newInfo.AddListener(action, priority);
            dic_Event.Add(name, newInfo);
            return;
        }

        if (info is EventInfo<TP1> eventInfo)
            eventInfo.AddListener(action, priority);
        else
            Debug.LogError($"事件类型不匹配: {name}");
    }

    /// <summary>
    /// 注册双参数事件监听
    /// </summary>
    public void AddEventListener<TP1, TP2>(T name, UnityAction<TP1, TP2> action, int priority = 5)
    {
        if (!dic_Event.TryGetValue(name, out var info))
        {
            var newInfo = new EventInfo<TP1, TP2>();
            newInfo.AddListener(action, priority);
            dic_Event.Add(name, newInfo);
            return;
        }

        if (info is EventInfo<TP1, TP2> eventInfo)
            eventInfo.AddListener(action, priority);
        else
            Debug.LogError($"事件类型不匹配: {name}");
    }
    // ====================== Event管理器实现 ======================
    #endregion
    #region 事件触发
    /// <summary>
    /// 触发无参事件
    /// </summary>
    public void TriggerEvent(T name)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is EventInfo eventInfo)
        {
            // 按优先级顺序执行
            foreach (var item in eventInfo.actions)
                item.action?.Invoke();
        }
    }

    /// <summary>
    /// 触发单参数事件
    /// </summary>
    public void TriggerEvent<TP1>(T name, TP1 arg1)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is EventInfo<TP1> eventInfo)
        {
            foreach (var item in eventInfo.actions)
                item.action?.Invoke(arg1);
        }
    }

    /// <summary>
    /// 触发双参数事件
    /// </summary>
    public void TriggerEvent<TP1, TP2>(T name, TP1 arg1, TP2 arg2)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is EventInfo<TP1, TP2> eventInfo)
        {
            foreach (var item in eventInfo.actions)
                item.action?.Invoke(arg1, arg2);
        }
    }
    #endregion
    #region 事件移除
    /// <summary>
    /// 移除无参事件监听
    /// </summary>
    public void RemoveEventListener(T name, UnityAction action)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is EventInfo eventInfo)
        {
            eventInfo.RemoveListener(action);
            // 无监听时清理
            if (eventInfo.actions.Count == 0)
                dic_Event.Remove(name);
        }
    }

    /// <summary>
    /// 移除单参数事件监听
    /// </summary>
    public void RemoveEventListener<TP1>(T name, UnityAction<TP1> action)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is EventInfo<TP1> eventInfo)
        {
            eventInfo.RemoveListener(action);
            if (eventInfo.actions.Count == 0)
                dic_Event.Remove(name);
        }
    }

    /// <summary>
    /// 移除双参数事件监听
    /// </summary>
    public void RemoveEventListener<TP1, TP2>(T name, UnityAction<TP1, TP2> action)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is EventInfo<TP1, TP2> eventInfo)
        {
            eventInfo.RemoveListener(action);
            if (eventInfo.actions.Count == 0)
                dic_Event.Remove(name);
        }
    }
    #endregion
    // ====================== EventManager 请求功能实现 ======================
    #region 请求监听管理
    /// <summary>
    /// 注册无参请求监听
    /// </summary>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="name">请求名称</param>
    /// <param name="func">请求方法</param>
    /// <param name="priority">优先级（默认0）</param>
    public void AddRequestListener<TResult>(T name, Func<TResult> func, int priority = 5)
    {
        if (!dic_Event.TryGetValue(name, out var info))
        {
            // 创建新请求类型
            var newInfo = new RequestInfo<TResult>();
            newInfo.AddListener(func, priority);
            dic_Event.Add(name, newInfo);
            return;
        }

        // 类型检查并添加监听
        if (info is RequestInfo<TResult> requestInfo)
            requestInfo.AddListener(func, priority);
        else
            Debug.LogError($"请求类型不匹配: {name}");
    }

    /// <summary>
    /// 注册单参数请求监听
    /// </summary>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    public void AddRequestListener<TParam, TResult>(T name, Func<TParam, TResult> func, int priority = 5)
    {
        if (!dic_Event.TryGetValue(name, out var info))
        {
            var newInfo = new RequestInfo<TParam, TResult>();
            newInfo.AddListener(func, priority);
            dic_Event.Add(name, newInfo);
            return;
        }

        if (info is RequestInfo<TParam, TResult> requestInfo)
            requestInfo.AddListener(func, priority);
        else
            Debug.LogError($"请求类型不匹配: {name}");
    }
    #endregion
    #region 请求触发
    /// <summary>
    /// 触发无参请求
    /// </summary>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="name">请求名称</param>
    /// <returns>最后一个监听器的返回值</returns>
    public TResult TriggerRequest<TResult>(T name)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is RequestInfo<TResult> requestInfo)
        {
            // 按优先级顺序执行，返回最后一个监听器的结果
            TResult result = default;
            foreach (var item in requestInfo.funcs)
            {
                result = item.func.Invoke();
            }
            return result;
        }
        return default;
    }

    /// <summary>
    /// 触发单参数请求
    /// </summary>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="name">请求名称</param>
    /// <param name="param">参数值</param>
    /// <returns>最后一个监听器的返回值</returns>
    public TResult TriggerRequest<TParam, TResult>(T name, TParam param)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is RequestInfo<TParam, TResult> requestInfo)
        {
            // 按优先级顺序执行，返回最后一个监听器的结果
            TResult result = default;
            foreach (var item in requestInfo.funcs)
            {
                result = item.func.Invoke(param);
            }
            return result;
        }
        return default;
    }
    #endregion
    #region 请求移除
    /// <summary>
    /// 移除无参请求监听
    /// </summary>
    /// <typeparam name="TResult">返回值类型</typeparam>
    public void RemoveRequestListener<TResult>(T name, Func<TResult> func)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is RequestInfo<TResult> requestInfo)
        {
            requestInfo.RemoveListener(func);
            // 无监听时清理
            if (requestInfo.funcs.Count == 0)
                dic_Event.Remove(name);
        }
    }

    /// <summary>
    /// 移除单参数请求监听
    /// </summary>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    public void RemoveRequestListener<TParam, TResult>(T name, Func<TParam, TResult> func)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is RequestInfo<TParam, TResult> requestInfo)
        {
            requestInfo.RemoveListener(func);
            if (requestInfo.funcs.Count == 0)
                dic_Event.Remove(name);
        }
    }
    #endregion

    /// <summary>
    /// 清空所有事件
    /// </summary>
    public override void ClearAll()
    {
        dic_Event.Clear();
    }

    /// <summary>
    /// 清除指定事件
    /// </summary>
    public void Clear(T name)
    {
        dic_Event.Remove(name);
    }
}