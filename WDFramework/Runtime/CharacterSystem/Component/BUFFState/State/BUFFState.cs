using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BUFFState
{
    /// <summary>
    /// ͨ��������ÿ��BUFFȷ��Ψһid�������ظ�buffͬʱ����
    /// </summary>
    public int BuffID;
    public BUFFState()
    {

    }
    /// <summary>
    /// һ��BUFF�����ж�����棬����洢
    /// </summary>
    public IBuffEffect[] BuffEffects;
    /// <summary>
    /// Buff��ʱ��
    /// </summary>
    protected TimerObj timer;
    /// <summary>
    /// Ӧ��Buff״̬
    /// </summary>
    public virtual void ApplyBuffEffect()
    {
        for (int i = 0; i < BuffEffects.Length; i++)
        {
            BuffEffects[i].ApplyEffect();
        }
    }
    /// <summary>
    /// �Ƴ�BUFF״̬
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
