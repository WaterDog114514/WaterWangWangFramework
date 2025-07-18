
/// <summary>
/// 节点进出口的类型
/// </summary>
public enum E_NodePortType
{
    /// <summary>
    /// 多口，允许多个节点连入，或者同时连接多个节点
    /// </summary>
    Mulit,
    /// <summary>
    /// 出口，仅允许单个节点连入，或者同时连接单个节点
    /// </summary>
    Single,
    /// <summary>
    /// 无口，不允许别的节点连接，或者自己不能连接别的节点
    /// </summary>
    None
}