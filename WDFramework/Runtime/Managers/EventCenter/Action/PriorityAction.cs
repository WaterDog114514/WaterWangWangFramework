using UnityEngine.Events;

/// <summary>
/// 优先级动作结构体（无参数）
/// </summary>
public struct PriorityAction
{
    public int priority;          // 优先级数值（越小越先执行）
    public UnityAction action;    // 对应的委托方法

    public PriorityAction(int p, UnityAction a)
    {
        priority = p;
        action = a;
    }
}

/// <summary>
/// 优先级动作结构体（单参数）
/// </summary>
public struct PriorityAction<TP1>
{
    public int priority;              // 优先级数值
    public UnityAction<TP1> action;   // 带参数的委托方法

    public PriorityAction(int p, UnityAction<TP1> a)
    {
        priority = p;
        action = a;
    }
}

/// <summary>
/// 优先级动作结构体（双参数）
/// </summary>
public struct PriorityAction<TP1, TP2>
{
    public int priority;                  // 优先级数值
    public UnityAction<TP1, TP2> action;  // 带双参数的委托方法

    public PriorityAction(int p, UnityAction<TP1, TP2> a)
    {
        priority = p;
        action = a;
    }
}
