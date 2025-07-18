using UnityEngine.Events;

/// <summary>
/// ���ȼ������ṹ�壨�޲�����
/// </summary>
public struct PriorityAction
{
    public int priority;          // ���ȼ���ֵ��ԽСԽ��ִ�У�
    public UnityAction action;    // ��Ӧ��ί�з���

    public PriorityAction(int p, UnityAction a)
    {
        priority = p;
        action = a;
    }
}

/// <summary>
/// ���ȼ������ṹ�壨��������
/// </summary>
public struct PriorityAction<TP1>
{
    public int priority;              // ���ȼ���ֵ
    public UnityAction<TP1> action;   // ��������ί�з���

    public PriorityAction(int p, UnityAction<TP1> a)
    {
        priority = p;
        action = a;
    }
}

/// <summary>
/// ���ȼ������ṹ�壨˫������
/// </summary>
public struct PriorityAction<TP1, TP2>
{
    public int priority;                  // ���ȼ���ֵ
    public UnityAction<TP1, TP2> action;  // ��˫������ί�з���

    public PriorityAction(int p, UnityAction<TP1, TP2> a)
    {
        priority = p;
        action = a;
    }
}
