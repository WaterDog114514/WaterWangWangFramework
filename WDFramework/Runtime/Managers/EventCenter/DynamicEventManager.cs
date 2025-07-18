using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��̬ö��������¼�������
/// </summary>
/// <typeparam name="T">��̬ö�����ͣ��̳���DynamicEnumBase��</typeparam>
public class DynamicEventManager<T> where T : DynamicEnum<T>
{
    // �¼��ֵ䣺�洢����ע����¼�
    private Dictionary<string, BaseEventInfo> dic_Event = new Dictionary<string, BaseEventInfo>();
    // ====================== �¼��������� ======================
    //enumValue�Ļ�ȡ
    /*
       1. ��ȡ����ֵ������Ϊnull��
        var eventValue = GameEvent.Get("PlayerDeath");
       2. ����ö����ʱԤ����ֵ
        public class GameEvent : DynamicEnum<GameEvent> {
        public static readonly GameEvent PlayerDeath = new("PlayerDeath");
        public static readonly GameEvent LevelComplete = new("LevelComplete");
}
     
     */
    /// <summary>
    /// ע���޲��¼�����
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
            Debug.LogError($"�¼����Ͳ�ƥ��: {enumValue.Name}");
    }

    /// <summary>
    /// ע�ᵥ�����¼�����
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
            Debug.LogError($"�¼����Ͳ�ƥ��: {enumValue.Name}");
    }

    /// <summary>
    /// ע��˫�����¼�����
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
            Debug.LogError($"�¼����Ͳ�ƥ��: {enumValue.Name}");
    }

    // ====================== �¼����� ======================

    /// <summary>
    /// �����޲��¼�
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
    /// �����������¼�
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
    /// ����˫�����¼�
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

    // ====================== �¼��Ƴ� ======================

    /// <summary>
    /// �Ƴ��޲��¼�����
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
    /// �Ƴ��������¼�����
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
    /// �Ƴ�˫�����¼�����
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
    // ====================== ������� ======================
    // ====================== ������ʵ�� ======================
    #region �����������
    /// <summary>
    /// ע���޲��������
    /// </summary>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <param name="enumValue">ö��ֵ</param>
    /// <param name="func">���󷽷�</param>
    /// <param name="priority">���ȼ���Ĭ��5��</param>
    public void AddRequestListener<TResult>(T enumValue, Func<TResult> func, int priority = 5)
    {
        string key = enumValue.Name;
        if (!dic_Event.TryGetValue(key, out var info))
        {
            // ��������������
            var newInfo = new RequestInfo<TResult>();
            newInfo.AddListener(func, priority);
            dic_Event.Add(key, newInfo);
            return;
        }

        // ���ͼ�鲢��Ӽ���
        if (info is RequestInfo<TResult> requestInfo)
            requestInfo.AddListener(func, priority);
        else
            Debug.LogError($"�������Ͳ�ƥ��: {enumValue.Name}");
    }

    /// <summary>
    /// ע�ᵥ�����������
    /// </summary>
    /// <typeparam name="TParam">��������</typeparam>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <param name="enumValue">ö��ֵ</param>
    /// <param name="func">���󷽷�</param>
    /// <param name="priority">���ȼ���Ĭ��5��</param>
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
            Debug.LogError($"�������Ͳ�ƥ��: {enumValue.Name}");
    }
    #endregion

    #region ���󴥷�
    /// <summary>
    /// �����޲�����
    /// </summary>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <param name="enumValue">ö��ֵ</param>
    /// <returns>���һ���������ķ���ֵ</returns>
    public TResult TriggerRequest<TResult>(T enumValue)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is RequestInfo<TResult> requestInfo)
        {
            // �����ȼ�˳��ִ�У��������һ���������Ľ��
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
    /// ��������������
    /// </summary>
    /// <typeparam name="TParam">��������</typeparam>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <param name="enumValue">ö��ֵ</param>
    /// <param name="param">����ֵ</param>
    /// <returns>���һ���������ķ���ֵ</returns>
    public TResult TriggerRequest<TParam, TResult>(T enumValue, TParam param)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is RequestInfo<TParam, TResult> requestInfo)
        {
            // �����ȼ�˳��ִ�У��������һ���������Ľ��
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

    #region �����Ƴ�
    /// <summary>
    /// �Ƴ��޲��������
    /// </summary>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <param name="enumValue">ö��ֵ</param>
    /// <param name="func">Ҫ�Ƴ������󷽷�</param>
    public void RemoveRequestListener<TResult>(T enumValue, Func<TResult> func)
    {
        string key = enumValue.Name;
        if (dic_Event.TryGetValue(key, out var info) && info is RequestInfo<TResult> requestInfo)
        {
            requestInfo.RemoveListener(func);
            // �޼���ʱ����
            if (requestInfo.funcs.Count == 0)
                dic_Event.Remove(key);
        }
    }

    /// <summary>
    /// �Ƴ��������������
    /// </summary>
    /// <typeparam name="TParam">��������</typeparam>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <param name="enumValue">ö��ֵ</param>
    /// <param name="func">Ҫ�Ƴ������󷽷�</param>
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
    /// ��������¼�
    /// </summary>
    public void ClearAll()
    {
        dic_Event.Clear();
    }

    /// <summary>
    /// ���ָ���¼�
    /// </summary>
    public void Clear(T enumValue)
    {
        dic_Event.Remove(enumValue.Name);
    }
}