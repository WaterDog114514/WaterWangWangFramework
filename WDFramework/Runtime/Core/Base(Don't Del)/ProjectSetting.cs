using System;
using UnityEngine;

/// <summary>
/// 项目设定初始化类
/// 负责项目层设定的初始化
/// </summary>
public abstract class InitializedProjectSetting
{
    protected GameStageManager GSManager => GameStageManager.Instance;
    /// <summary>
    /// 初始化项目层设定
    /// </summary>
   public abstract void RegisterProjectSetting();
}