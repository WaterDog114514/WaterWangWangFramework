using System;
/// <summary>
/// 有更新行为的
/// </summary>
public interface IUpdate
{
}
/// <summary>
/// mono固定帧触发
/// </summary>
public interface IMonoFixedUpdate : IUpdate
{
    void MonoFixedUpdate();
}
/// <summary>
/// mono帧更新触发
/// </summary>
public interface IMonoUpdate : IUpdate
{
    void MonoUpdate();
}
/// <summary>
/// mono帧更新触发
/// </summary>
public interface IMonoLastUpdate : IUpdate
{
    void MonoLastUpdate();
}
