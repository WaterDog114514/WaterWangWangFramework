using UnityEngine;
/// <summary>
/// 根技能节点，同时计算CD一起
/// </summary>
public class SkillRootNode : SingleSkillNode
{
    public float CDTime = 0;
    /// <summary>
    /// 技能CD是否转好
    /// </summary>
    public bool isFinishCD = true;
    public override void execute(SkillRunningData data)
    {
        //CD为0就是无CD技能
        if (CDTime == 0)
        {
            nextNode.execute(data);
            return;
        }
        //有技能冷却CD的且还在冷却中
        if (!isFinishCD && CDTime > 0)
        {
            data.SkillState = E_SkillState.Finished;
            return;
        }
        nextNode.execute(data);
        //技能释放成功，需重置冷却
        StartRecordCD();
    }

    /// <summary>
    /// 重新计时CD
    /// </summary>
    private void StartRecordCD()
    {
        isFinishCD = false;
        TimerManager.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, CDTime, () => { isFinishCD = true; });
    }
}
