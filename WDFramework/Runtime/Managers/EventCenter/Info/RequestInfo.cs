// ====================== ����ṹ���� ======================
using System;
using System.Collections.Generic;

/// <summary>
/// �޲�������Ϣ�ࣨ������ֵ��
/// </summary>
/// <typeparam name="TResult">����ֵ����</typeparam>
public class RequestInfo<TResult> : BaseEventInfo
{
    public List<PriorityFunc<TResult>> funcs = new List<PriorityFunc<TResult>>(); // �洢���ȼ�������б�

    /// <summary>
    /// �������������Զ�����
    /// </summary>
    /// <param name="func">���󷽷�</param>
    /// <param name="priority">���ȼ�</param>
    public void AddListener(Func<TResult> func, int priority)
    {
        var item = new PriorityFunc<TResult>(priority, func);
        // �ҵ���һ���ȵ�ǰ���ȼ����λ�ò���
        int index = funcs.FindIndex(f => f.priority > priority);
        if (index == -1)
            funcs.Add(item);
        else
            funcs.Insert(index, item);
    }

    /// <summary>
    /// �Ƴ�ָ���������
    /// </summary>
    public void RemoveListener(Func<TResult> func)
    {
        funcs.RemoveAll(f => f.func.Equals(func));
    }
}

/// <summary>
/// ������������Ϣ�ࣨ������ֵ��
/// </summary>
/// <typeparam name="TParam">��������</typeparam>
/// <typeparam name="TResult">����ֵ����</typeparam>
public class RequestInfo<TParam, TResult> : BaseEventInfo
{
    public List<PriorityFunc<TParam, TResult>> funcs = new List<PriorityFunc<TParam, TResult>>();

    public void AddListener(Func<TParam, TResult> func, int priority)
    {
        var item = new PriorityFunc<TParam, TResult>(priority, func);
        int index = funcs.FindIndex(f => f.priority > priority);
        if (index == -1)
            funcs.Add(item);
        else
            funcs.Insert(index, item);
    }

    public void RemoveListener(Func<TParam, TResult> func)
    {
        funcs.RemoveAll(f => f.func.Equals(func));
    }
}
