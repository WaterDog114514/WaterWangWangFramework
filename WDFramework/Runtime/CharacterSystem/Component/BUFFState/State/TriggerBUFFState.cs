using UnityEngine;
using WDFramework;
/// <summary>
/// 触发性BUFF状态，每隔一段时间就会触发一次
/// </summary>
public class TriggerBUFFState : BUFFState
{
    /// <summary>
    /// 触发次数
    /// </summary>
    public int TriggerNums;
    /// <summary>
    /// 间隔时间
    /// </summary>
    public float IntervalTime;
    public override void ApplyBuffEffect()
    {
        timer = ObjectManager.Instance.CreateDataObject<TimerObj>().AddFinishCallback(RemoveBuffEffect).SetIntervalCallback(IntervalTime,base.ApplyBuffEffect);
        timer.ReStart(TriggerNums*IntervalTime);
    }
}
