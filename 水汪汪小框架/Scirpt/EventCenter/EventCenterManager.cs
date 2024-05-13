using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件中心管理器  
/// 只能用于对象与模块间 或 模块与模块间的信息交流
/// </summary>
class EventCenterManager : Singleton_UnMono<EventCenterManager>
{
    /// <summary>
    /// 游戏事件的集合，比如怪物死亡，游戏胜利等
    /// </summary>
    private Dictionary<E_GameEvent, BaseEventInfo> dic_GameEvent = new Dictionary<E_GameEvent, BaseEventInfo>();
    /// <summary>
    /// 输入事件集合，比如攻击按键事件，防御按键事件
    /// </summary>
    private Dictionary<E_InputEvent, BaseEventInfo> dic_InputEvent= new Dictionary<E_InputEvent, BaseEventInfo>();
    //触发游戏时间
    public void TriggerGameEvent(E_GameEvent name)
    {
        if (dic_GameEvent.ContainsKey(name))
        {
            (dic_GameEvent[name] as EventInfo).Event?.Invoke();
        }
    }
    
    //触发游戏时间
    public void TriggerGameEvent<T>(E_GameEvent name, T parameter)
    {
        if (dic_GameEvent.ContainsKey(name))
        {
            (dic_GameEvent[name] as EventInfo<T>).Event?.Invoke(parameter);
        }
    }

    //输入事件触发
    public void TriggerInputEvent(E_InputEvent name)
    {
        if (dic_InputEvent.ContainsKey(name))
        {
            (dic_InputEvent[name] as EventInfo).Event?.Invoke();
        }
    }
    //触发输入按键事件
    public void TriggerInputEvent<T>(E_InputEvent name, T parameter)
    {
        if (dic_InputEvent.ContainsKey(name))
        {
            (dic_InputEvent[name] as EventInfo<T>).Event?.Invoke(parameter);
        }
    }

    public void AddGameEventListener(E_GameEvent name, UnityAction fun)
    {
        if (!dic_GameEvent.ContainsKey(name))
        {
            dic_GameEvent.Add(name, new EventInfo(fun));
        }
        else
        {
            (dic_GameEvent[name] as EventInfo).Event += fun;
        }
    }
    public void AddGameEventListener<T>(E_GameEvent name, UnityAction<T> fun)
    {
        if (!dic_GameEvent.ContainsKey(name))
        {
            dic_GameEvent.Add(name, new EventInfo<T>(fun));
        }
        else
        {
            (dic_GameEvent[name] as EventInfo<T>).Event += fun;
        }
    }

    public void AddInputEventListener(E_InputEvent name, UnityAction fun)
    {
        if (!dic_InputEvent.ContainsKey(name))
        {
            dic_InputEvent.Add(name, new EventInfo(fun));
        }
        else
        {
            (dic_InputEvent[name] as EventInfo).Event += fun;
        }
    }
    public void AddInputEventListener<T>(E_InputEvent name, UnityAction<T> fun)
    {
        if (!dic_InputEvent.ContainsKey(name))
        {
            dic_InputEvent.Add(name, new EventInfo<T>(fun));
        }
        else
        {
            (dic_InputEvent[name] as EventInfo<T>).Event += fun;
        }
    }

    public void RemoveGameEventListener(E_GameEvent name, UnityAction fun)
    {
        if (dic_GameEvent.ContainsKey(name))
            (dic_GameEvent[name] as EventInfo).Event -= fun;
    }
    public void RemoveGameEventListener<T>(E_GameEvent name, UnityAction<T> fun)
    {
        if (dic_GameEvent.ContainsKey(name))
            (dic_GameEvent[name] as EventInfo<T>).Event -= fun;
    }

    public void RemoveInputEventListener(E_InputEvent name, UnityAction fun)
    {
        if (dic_InputEvent.ContainsKey(name))
            (dic_InputEvent[name] as EventInfo).Event -= fun;
    }
    public void RemoveInputEventListener<T>(E_InputEvent name, UnityAction<T> fun)
    {
        if (dic_InputEvent.ContainsKey(name))
            (dic_InputEvent[name] as EventInfo<T>).Event -= fun;
    }




    /// <summary>
    /// 清空所有事件的监听
    /// </summary>
    public void ClearAll()
    {
        dic_GameEvent.Clear();
        dic_InputEvent.Clear();
    }
    /// <summary>
    /// 清除指定某一个事件的所有监听
    /// </summary>
    public void Clear(E_GameEvent name)
    {
        if (dic_GameEvent.ContainsKey(name))
            dic_GameEvent.Remove(name);
    }
    public void Clear(E_InputEvent name)
    {
        if (dic_InputEvent.ContainsKey(name))
            dic_InputEvent.Remove(name);
    }
}