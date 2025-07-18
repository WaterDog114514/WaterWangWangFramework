using UnityEngine;
/// <summary>
/// 延迟执行节点
/// </summary>
public class SkillDelayNode : SingleSkillNode
{
    public float delayTime;
    public override void execute(SkillRunningData data)
    {
        if (!IsHaveChildNode(data)) return;
        //开始计时
        data.SkillState = E_SkillState.Running;
        TimerManager.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, delayTime, () => {
            data.SkillState = E_SkillState.Finished;
            nextNode.execute(data); 
        });

    }
}
