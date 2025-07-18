using UnityEngine;
/// <summary>
/// �ӳ�ִ�нڵ�
/// </summary>
public class SkillDelayNode : SingleSkillNode
{
    public float delayTime;
    public override void execute(SkillRunningData data)
    {
        if (!IsHaveChildNode(data)) return;
        //��ʼ��ʱ
        data.SkillState = E_SkillState.Running;
        TimerManager.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, delayTime, () => {
            data.SkillState = E_SkillState.Finished;
            nextNode.execute(data); 
        });

    }
}
