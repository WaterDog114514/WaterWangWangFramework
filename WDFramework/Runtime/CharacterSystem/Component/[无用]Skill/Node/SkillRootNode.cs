using UnityEngine;
/// <summary>
/// �����ܽڵ㣬ͬʱ����CDһ��
/// </summary>
public class SkillRootNode : SingleSkillNode
{
    public float CDTime = 0;
    /// <summary>
    /// ����CD�Ƿ�ת��
    /// </summary>
    public bool isFinishCD = true;
    public override void execute(SkillRunningData data)
    {
        //CDΪ0������CD����
        if (CDTime == 0)
        {
            nextNode.execute(data);
            return;
        }
        //�м�����ȴCD���һ�����ȴ��
        if (!isFinishCD && CDTime > 0)
        {
            data.SkillState = E_SkillState.Finished;
            return;
        }
        nextNode.execute(data);
        //�����ͷųɹ�����������ȴ
        StartRecordCD();
    }

    /// <summary>
    /// ���¼�ʱCD
    /// </summary>
    private void StartRecordCD()
    {
        isFinishCD = false;
        TimerManager.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, CDTime, () => { isFinishCD = true; });
    }
}
