using UnityEngine.Events;

/// <summary>
/// 用于子类事件信息的父类 
/// 可以通过声明多个参数的T进行拓展，这样你就能传更多的参数了
/// </summary>
abstract class BaseEventInfo
{

}
/// <summary>
/// 无参无返回值委托信息类
/// </summary>
class EventInfo : BaseEventInfo
{
    public UnityAction Event;
    public EventInfo(UnityAction _event)
    {
        Event += _event;
    }
}
/// <summary>
/// 有一个参数的委托信息类
/// </summary>
class EventInfo<T> : BaseEventInfo
{
    public EventInfo(UnityAction<T> _event)
    {
        Event += _event;
    }
    public UnityAction<T> Event;
}