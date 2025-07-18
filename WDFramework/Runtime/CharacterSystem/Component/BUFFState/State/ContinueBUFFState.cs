using UnityEngine;
using WDFramework;
/// <summary>
/// �г���ʱ���BUFF״̬
/// </summary>
public class ContinueBUFFState : BUFFState
{
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public float Totaltime;
    public override void ApplyBuffEffect()
    {
        base.ApplyBuffEffect();
        timer = ObjectManager.Instance.CreateDataObject<TimerObj>().AddFinishCallback(RemoveBuffEffect);
        timer.ReStart(Totaltime);
    }
}
