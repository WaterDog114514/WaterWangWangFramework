using UnityEngine;
/// <summary>
/// 框架层系统
///提供游戏核心且通用功能，不直接参与游戏逻辑，
///通常全局唯一且生命周期与游戏进程一致
/// </summary>
public interface IFrameworkSystem : ISystem
{
    /// <summary>
    /// 初始化系统，在游戏进程刚启动时候调用
    /// </summary>
   void InitializedSystem();
}
