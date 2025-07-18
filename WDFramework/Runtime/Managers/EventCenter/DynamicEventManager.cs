using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 动态枚举适配的事件管理器
/// </summary>
/// <typeparam name="T">动态枚举类型（继承自DynamicEnumBase）</typeparam>
public class DynamicEventManager<T> where T : DynamicEnum<T>
{
    // 事件字典：存储所有注册的事件
    private Dictionary<string, BaseEventInfo> dic_Event = new Dictionary<string, BaseEventInfo>();
    // ====================== 事件监听管理 ======================
    //enumValue的获取
    /*
       1. 获取已有值（可能为null）
        var eventValue = GameEvent.Get("PlayerDeath");
       2. 定义枚举类时预定义值
        public class GameEvent : DynamicEnum<GameEvent> {
        public static readonly GameEvent PlayerDeath = new("PlayerDeath");
        public static readonly GameEvent LevelComplete = new("LevelComplete");
}
     
     */
    /// <summary>
    /// 注册无参事件监听
    /// </summary>
    public void AddEventListener(T enumValue, UnityAction action, int priority = 5)
    {
        string key = enumValue.Name;
        if (!dic_Event.TryGetValue(key, out var info))
        {
            var newInfo = new EventInfo();
            newInfo.AddListener(action, priority);
            dic_Event.Add(key, newInfo);
            return;
        }

        if (info is EventInfo eventInfo)
            eventInfo.AddListener(action, priority);
        else
            Debug.LogError($"事件类型不匹配: {enumValue.Name}");
    }

    /// <summary>
    /// 注册单参数事件监听
    /// </summary>
    public void AddEventListener<TP1>(T enumValue, UnityAction<TP1> action, int priority = 5)
    {
        string key = enumValue.Name;
        if (!dic_Event.TryGetValue(key, out var info))
        {
            var newInfo = new EventInfo<TP1>();
            newInfo.AddListener(action, priority);
            dic_Event.Add(key, newInfo);
            return;
        }

        if (info is EventInfo<TP1> eventInfo)
            eventInfo.AddListener(action, priority);
        else
            Debug.LogError($"事件类型不匹配: {enumValue.Name}");
    }

    /// <summary>
    /// 注册双参数事件监听
    /// </summary>
    public void AddEventListener<TP1, TP2>(T enumValue, UnityAction<TP1, TP2> action, int priority = 5)
    {
        string key = enumValue.Name;
        if (!dic_Event.TryGetValue(key, out var info))
        {
            var newInfo = new EventInfo<TP1, TP2>();
            newInfo.AddListener(action, priority);
            dic_Event.Add(key, newInfo);
            return;
        }

        if (info is EventInfo<TP1, TP2> eventInfo)
            eventInfo.AddListener(action, priority);
        else
            Debug.LogError($"事件类型不匹配: {enumValue.Name}");
    }

    // ====================== 事件触发 ======================

    /// <summary>
    /// 触发无参事件
    /// </summary>
    public void TriggerEvent(T enumValue)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is EventInfo eventInfo)
        {
            foreach (var item in eventInfo.actions)
                item.action?.Invoke();
        }
    }

    /// <summary>
    /// 触发单参数事件
    /// </summary>
    public void TriggerEvent<TP1>(T enumValue, TP1 arg1)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is EventInfo<TP1> eventInfo)
        {
            foreach (var item in eventInfo.actions)
                item.action?.Invoke(arg1);
        }
    }

    /// <summary>
    /// 触发双参数事件
    /// </summary>
    public void TriggerEvent<TP1, TP2>(T enumValue, TP1 arg1, TP2 arg2)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is EventInfo<TP1, TP2> eventInfo)
        {
            foreach (var item in eventInfo.actions)
                item.action?.Invoke(arg1, arg2);
        }
    }

    // ====================== 事件移除 ======================

    /// <summary>
    /// 移除无参事件监听
    /// </summary>
    public void RemoveEventListener(T enumValue, UnityAction action)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is EventInfo eventInfo)
        {
            eventInfo.RemoveListener(action);
            if (eventInfo.actions.Count == 0)
                dic_Event.Remove(key);
        }
    }

    /// <summary>
    /// 移除单参数事件监听
    /// </summary>
    public void RemoveEventListener<TP1>(T enumValue, UnityAction<TP1> action)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is EventInfo<TP1> eventInfo)
        {
            eventInfo.RemoveListener(action);
            if (eventInfo.actions.Count == 0)
                dic_Event.Remove(key);
        }
    }

    /// <summary>
    /// 移除双参数事件监听
    /// </summary>
    public void RemoveEventListener<TP1, TP2>(T enumValue, UnityAction<TP1, TP2> action)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is EventInfo<TP1, TP2> eventInfo)
        {
            eventInfo.RemoveListener(action);
            if (eventInfo.actions.Count == 0)
                dic_Event.Remove(key);
        }
    }
    // ====================== 请求添加 ======================
    // ====================== 请求功能实现 ======================
    #region 请求监听管理
    /// <summary>
    /// 注册无参请求监听
    /// </summary>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="enumValue">枚举值</param>
    /// <param name="func">请求方法</param>
    /// <param name="priority">优先级（默认5）</param>
    public void AddRequestListener<TResult>(T enumValue, Func<TResult> func, int priority = 5)
    {
        string key = enumValue.Name;
        if (!dic_Event.TryGetValue(key, out var info))
        {
            // 创建新请求类型
            var newInfo = new RequestInfo<TResult>();
            newInfo.AddListener(func, priority);
            dic_Event.Add(key, newInfo);
            return;
        }

        // 类型检查并添加监听
        if (info is RequestInfo<TResult> requestInfo)
            requestInfo.AddListener(func, priority);
        else
            Debug.LogError($"请求类型不匹配: {enumValue.Name}");
    }

    /// <summary>
    /// 注册单参数请求监听
    /// </summary>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="enumValue">枚举值</param>
    /// <param name="func">请求方法</param>
    /// <param name="priority">优先级（默认5）</param>
    public void AddRequestListener<TParam, TResult>(T enumValue, Func<TParam, TResult> func, int priority = 5)
    {
        string key = enumValue.Name;
        if (!dic_Event.TryGetValue(key, out var info))
        {
            var newInfo = new RequestInfo<TParam, TResult>();
            newInfo.AddListener(func, priority);
            dic_Event.Add(key, newInfo);
            return;
        }

        if (info is RequestInfo<TParam, TResult> requestInfo)
            requestInfo.AddListener(func, priority);
        else
            Debug.LogError($"请求类型不匹配: {enumValue.Name}");
    }
    #endregion

    #region 请求触发
    /// <summary>
    /// 触发无参请求
    /// </summary>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="enumValue">枚举值</param>
    /// <returns>最后一个监听器的返回值</returns>
    public TResult TriggerRequest<TResult>(T enumValue)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is RequestInfo<TResult> requestInfo)
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
    /// <param name="enumValue">枚举值</param>
    /// <param name="param">参数值</param>
    /// <returns>最后一个监听器的返回值</returns>
    public TResult TriggerRequest<TParam, TResult>(T enumValue, TParam param)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is RequestInfo<TParam, TResult> requestInfo)
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
    /// <param name="enumValue">枚举值</param>
    /// <param name="func">要移除的请求方法</param>
    public void RemoveRequestListener<TResult>(T enumValue, Func<TResult> func)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is RequestInfo<TResult> requestInfo)
        {
            requestInfo.RemoveListener(func);
            // 无监听时清理
            if (requestInfo.funcs.Count == 0)
                dic_Event.Remove(key);
        }
    }

    /// <summary>
    /// 移除单参数请求监听
    /// </summary>
    /// <typeparam name="TParam">参数类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="enumValue">枚举值</param>
    /// <param name="func">要移除的请求方法</param>
    public void RemoveRequestListener<TParam, TResult>(T enumValue, Func<TParam, TResult> func)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is RequestInfo<TParam, TResult> requestInfo)
        {
            requestInfo.RemoveListener(func);
            if (requestInfo.funcs.Count == 0)
                dic_Event.Remove(key);
        }
    }
    #endregion

    /// <summary>
    /// 清空所有事件
    /// </summary>
    public void ClearAll()
    {
        dic_Event.Clear();
    }

    /// <summary>
    /// 清除指定事件
    /// </summary>
    public void Clear(T enumValue)
    {
        dic_Event.Remove(enumValue.Name);
    }
}