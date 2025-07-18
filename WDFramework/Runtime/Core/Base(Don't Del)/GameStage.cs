using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 生命周期枚举
/// </summary>

public abstract class GameStage
{
    /// <summary>
    /// 协程调度者，用来调用其他协程
    /// </summary>
    protected MonoBehaviour waitExecutor => UpdateSystem.Instance;
    /// <summary>
    /// 下一个自动进入阶段
    /// </summary>
    public abstract GameStage nextAutoChangeStage { get; }
    /// <summary>
    /// 整个阶段操作——自主性（支持协程)
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator StageOperator();
    public static bool operator ==(GameStage left, GameStage right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.GetType() == right.GetType();
    }

    public static bool operator !=(GameStage left, GameStage right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        return this.GetType() == obj.GetType();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}