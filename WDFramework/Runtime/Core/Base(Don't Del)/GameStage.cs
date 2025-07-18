using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ��������ö��
/// </summary>

public abstract class GameStage
{
    /// <summary>
    /// Э�̵����ߣ�������������Э��
    /// </summary>
    protected MonoBehaviour waitExecutor => UpdateSystem.Instance;
    /// <summary>
    /// ��һ���Զ�����׶�
    /// </summary>
    public abstract GameStage nextAutoChangeStage { get; }
    /// <summary>
    /// �����׶β������������ԣ�֧��Э��)
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