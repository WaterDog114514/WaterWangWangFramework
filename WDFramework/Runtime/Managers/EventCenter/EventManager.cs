using System.Collections.Generic;
using System;

using UnityEngine.Events;
using UnityEngine;
public abstract class BaseEventManager
{
    public abstract void ClearAll();
}
/// <summary>
/// �¼���������֧�����ȼ���
/// </summary>
/// <typeparam name="T">�¼�����ö��</typeparam>
public class EventManager<T> : BaseEventManager where T : Enum
{
    // �¼��ֵ䣺�洢����ע����¼�
    private Dictionary<T, BaseEventInfo> dic_Event = new Dictionary<T, BaseEventInfo>();
    // ====================== Event������ʵ�� ======================
    #region �¼���������
    /// <summary>
    /// ע���޲��¼�����
    /// </summary>
    /// <param name="name">�¼�����</param>
    /// <param name="action">ί�з���</param>
    /// <param name="priority">���ȼ���Ĭ��0��</param>
    public void AddEventListener(T name, UnityAction action, int priority = 5)
    {
        if (!dic_Event.TryGetValue(name, out var info))
        {
            // �������¼�����
            var newInfo = new EventInfo();
            newInfo.AddListener(action, priority);
            dic_Event.Add(name, newInfo);
            return;
        }

        // ���ͼ�鲢��Ӽ���
        if (info is EventInfo eventInfo)
            eventInfo.AddListener(action, priority);
        else
            Debug.LogError($"�¼����Ͳ�ƥ��: {name}");
    }

    /// <summary>
    /// ע�ᵥ�����¼�����
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
            Debug.LogError($"�¼����Ͳ�ƥ��: {name}");
    }

    /// <summary>
    /// ע��˫�����¼�����
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
            Debug.LogError($"�¼����Ͳ�ƥ��: {name}");
    }
    // ====================== Event������ʵ�� ======================
    #endregion
    #region �¼�����
    /// <summary>
    /// �����޲��¼�
    /// </summary>
    public void TriggerEvent(T name)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is EventInfo eventInfo)
        {
            // �����ȼ�˳��ִ��
            foreach (var item in eventInfo.actions)
                item.action?.Invoke();
        }
    }

    /// <summary>
    /// �����������¼�
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
    /// ����˫�����¼�
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
    #region �¼��Ƴ�
    /// <summary>
    /// �Ƴ��޲��¼�����
    /// </summary>
    public void RemoveEventListener(T name, UnityAction action)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is EventInfo eventInfo)
        {
            eventInfo.RemoveListener(action);
            // �޼���ʱ����
            if (eventInfo.actions.Count == 0)
                dic_Event.Remove(name);
        }
    }

    /// <summary>
    /// �Ƴ��������¼�����
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
    /// �Ƴ�˫�����¼�����
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
    // ====================== EventManager ������ʵ�� ======================
    #region �����������
    /// <summary>
    /// ע���޲��������
    /// </summary>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <param name="name">��������</param>
    /// <param name="func">���󷽷�</param>
    /// <param name="priority">���ȼ���Ĭ��0��</param>
    public void AddRequestListener<TResult>(T name, Func<TResult> func, int priority = 5)
    {
        if (!dic_Event.TryGetValue(name, out var info))
        {
            // ��������������
            var newInfo = new RequestInfo<TResult>();
            newInfo.AddListener(func, priority);
            dic_Event.Add(name, newInfo);
            return;
        }

        // ���ͼ�鲢��Ӽ���
        if (info is RequestInfo<TResult> requestInfo)
            requestInfo.AddListener(func, priority);
        else
            Debug.LogError($"�������Ͳ�ƥ��: {name}");
    }

    /// <summary>
    /// ע�ᵥ�����������
    /// </summary>
    /// <typeparam name="TParam">��������</typeparam>
    /// <typeparam name="TResult">����ֵ����</typeparam>
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
            Debug.LogError($"�������Ͳ�ƥ��: {name}");
    }
    #endregion
    #region ���󴥷�
    /// <summary>
    /// �����޲�����
    /// </summary>
    /// <typeparam name="TResult">����ֵ����</typeparam>
    /// <param name="name">��������</param>
    /// <returns>���һ���������ķ���ֵ</returns>
    public TResult TriggerRequest<TResult>(T name)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is RequestInfo<TResult> requestInfo)
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
    /// <param name="name">��������</param>
    /// <param name="param">����ֵ</param>
    /// <returns>���һ���������ķ���ֵ</returns>
    public TResult TriggerRequest<TParam, TResult>(T name, TParam param)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is RequestInfo<TParam, TResult> requestInfo)
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
    public void RemoveRequestListener<TResult>(T name, Func<TResult> func)
    {
        if (dic_Event.TryGetValue(name, out var info) && info is RequestInfo<TResult> requestInfo)
        {
            requestInfo.RemoveListener(func);
            // �޼���ʱ����
            if (requestInfo.funcs.Count == 0)
                dic_Event.Remove(name);
        }
    }

    /// <summary>
    /// �Ƴ��������������
    /// </summary>
    /// <typeparam name="TParam">��������</typeparam>
    /// <typeparam name="TResult">����ֵ����</typeparam>
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
    /// ��������¼�
    /// </summary>
    public override void ClearAll()
    {
        dic_Event.Clear();
    }

    /// <summary>
    /// ���ָ���¼�
    /// </summary>
    public void Clear(T name)
    {
        dic_Event.Remove(name);
    }
}