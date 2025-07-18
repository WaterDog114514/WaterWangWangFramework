
using System;

/// <summary>
/// ���ȼ�����ṹ�壨�޲�����������ֵ��
/// </summary>
public struct PriorityFunc<TResult>
{
    public int priority;          // ���ȼ���ֵ��ԽСԽ��ִ�У�
    public Func<TResult> func;    // ��Ӧ�����󷽷�

    public PriorityFunc(int p, Func<TResult> f)
    {
        priority = p;
        func = f;
    }
}

/// <summary>
/// ���ȼ�����ṹ�壨��������������ֵ��
/// </summary>
public struct PriorityFunc<TParam, TResult>
{
    public int priority;              // ���ȼ���ֵ
    public Func<TParam, TResult> func; // �����������󷽷�

    public PriorityFunc(int p, Func<TParam, TResult> f)
    {
        priority = p;
        func = f;
    }
}