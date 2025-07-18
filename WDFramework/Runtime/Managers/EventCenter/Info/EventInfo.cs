// ====================== �¼��ṹ���� ======================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �޲����¼���Ϣ��
/// </summary>
public class EventInfo : BaseEventInfo
{
    public List<PriorityAction> actions = new List<PriorityAction>();  // �洢���ȼ��������б�

    /// <summary>
    /// ��Ӽ������Զ�����
    /// </summary>
    /// <param name="action">ί�з���</param>
    /// <param name="priority">���ȼ�</param>
    public void AddListener(UnityAction action, int priority)
    {
        var item = new PriorityAction(priority, action);
        // �ҵ���һ���ȵ�ǰ���ȼ����λ�ò���
        int index = actions.FindIndex(a => a.priority > priority);
        if (index == -1)
            actions.Add(item);
        else
            actions.Insert(index, item);
    }

    /// <summary>
    /// �Ƴ�ָ������
    /// </summary>
    public void RemoveListener(UnityAction action)
    {
        actions.RemoveAll(a => a.action.Equals(action));
    }
}

/// <summary>
/// �������¼���Ϣ��
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
/// ˫�����¼���Ϣ��
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