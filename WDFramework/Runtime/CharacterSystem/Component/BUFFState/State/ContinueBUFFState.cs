using UnityEngine;
using WDFramework;
/// <summary>
/// 有持续时间的BUFF状态
/// </summary>
public class ContinueBUFFState : BUFFState
{
    /// <summary>
    /// 持续时间
    /// </summary>
    public float Totaltime;
    public override void ApplyBuffEffect()
    {
        base.ApplyBuffEffect();
        timer = ObjectManager.Instance.CreateDataObject<TimerObj>().AddFinishCallback(RemoveBuffEffect);
        timer.ReStart(Totaltime);
    }
}
