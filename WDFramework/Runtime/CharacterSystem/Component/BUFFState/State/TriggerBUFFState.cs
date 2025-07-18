using UnityEngine;
using WDFramework;
/// <summary>
/// ������BUFF״̬��ÿ��һ��ʱ��ͻᴥ��һ��
/// </summary>
public class TriggerBUFFState : BUFFState
{
    /// <summary>
    /// ��������
    /// </summary>
    public int TriggerNums;
    /// <summary>
    /// ���ʱ��
    /// </summary>
    public float IntervalTime;
    public override void ApplyBuffEffect()
    {
        timer = ObjectManager.Instance.CreateDataObject<TimerObj>().AddFinishCallback(RemoveBuffEffect).SetIntervalCallback(IntervalTime,base.ApplyBuffEffect);
        timer.ReStart(TriggerNums*IntervalTime);
    }
}
