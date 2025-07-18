using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BUFFState
{
    /// <summary>
    /// 通过读表，给每个BUFF确定唯一id，避免重复buff同时叠加
    /// </summary>
    public int BuffID;
    public BUFFState()
    {

    }
    /// <summary>
    /// 一个BUFF可能有多个增益，数组存储
    /// </summary>
    public IBuffEffect[] BuffEffects;
    /// <summary>
    /// Buff计时器
    /// </summary>
    protected TimerObj timer;
    /// <summary>
    /// 应用Buff状态
    /// </summary>
    public virtual void ApplyBuffEffect()
    {
        for (int i = 0; i < BuffEffects.Length; i++)
        {
            BuffEffects[i].ApplyEffect();
        }
    }
    /// <summary>
    /// 移除BUFF状态
    /// </summary>
    public virtual void RemoveBuffEffect()
    {
        for (int i = 0; i < BuffEffects.Length; i++)
        {
            BuffEffects[i].RemoveEffect();
        }
        timer.Destroy();
    }

}
