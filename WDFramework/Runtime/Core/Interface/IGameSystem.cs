using UnityEngine;
/// <summary>
///游戏系统，受到游戏系统管理器的管控
/// </summary>
public interface IGameSystem : ISystem
{
    /// <summary>
    /// 当此系统被销毁时执行的逻辑
    /// </summary>
    void DestorySystem();
}
