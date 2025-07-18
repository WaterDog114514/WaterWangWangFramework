// ====================== 事件结构定义 ======================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 无参数事件信息类
/// </summary>
public class EventInfo : BaseEventInfo
{
    public List<PriorityAction> actions = new List<PriorityAction>();  // 存储优先级动作的列表

    /// <summary>
    /// 添加监听（自动排序）
    /// </summary>
    /// <param name="action">委托方法</param>
    /// <param name="priority">优先级</param>
    public void AddListener(UnityAction action, int priority)
    {
        var item = new PriorityAction(priority, action);
        // 找到第一个比当前优先级大的位置插入
        int index = actions.FindIndex(a => a.priority > priority);
        if (index == -1)
            actions.Add(item);
        else
            actions.Insert(index, item);
    }

    /// <summary>
    /// 移除指定监听
    /// </summary>
    public void RemoveListener(UnityAction action)
    {
        actions.RemoveAll(a => a.action.Equals(action));
    }
}

/// <summary>
/// 单参数事件信息类
/// </summary>
public class EventInfo<TP1> : BaseEventInfo
{
    public List<PriorityAction<TP1>> actions = new List<PriorityAction<TP1>>();

    public void AddListener(UnityAction<TP1> action, int priority)
    {
        var item = new PriorityAction<TP1>(priority, action);
        int index = actions.FindIndex(a => a.priority > priority);
        if (index == -1)
            actions.Add(item);
        else
            actions.Insert(index, item);
    }

    public void RemoveListener(UnityAction<TP1> action)
    {
        actions.RemoveAll(a => a.action.Equals(action));
    }
}

/// <summary>
/// 双参数事件信息类
/// </summary>
public class EventInfo<TP1, TP2> : BaseEventInfo
{
    public List<PriorityAction<TP1, TP2>> actions = new List<PriorityAction<TP1, TP2>>();

    public void AddListener(UnityAction<TP1, TP2> action, int priority)
    {
        var item = new PriorityAction<TP1, TP2>(priority, action);
        int index = actions.FindIndex(a => a.priority > priority);
        if (index == -1)
            actions.Add(item);
        else
            actions.Insert(index, item);
    }

    public void RemoveListener(UnityAction<TP1, TP2> action)
    {
        actions.RemoveAll(a => a.action.Equals(action));
    }


}