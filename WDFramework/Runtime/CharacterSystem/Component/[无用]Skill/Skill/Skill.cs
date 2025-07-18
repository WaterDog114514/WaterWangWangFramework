using UnityEngine;
using WDFramework;

public  class Skill :DataObj
{
    /// <summary>
    /// ���ܵ�Ψһ��ID���������������жϳ����߼�������
    /// </summary>
    public int SkillID;
    /// <summary>
    /// ��������
    /// </summary>
    private SkillPipe pipe;

    public SkillRunningData RunningData;
    /// <summary>
    /// ��ʼ�ͷż���
    /// </summary>
    public void CastSkill(GameObj caster)
    {
        if(b_IsWaitSkillEnd&&RunningData.SkillState== E_SkillState.Running) return;
        //��ʼִ����
        RunningData.SkillState = E_SkillState.Running;
        pipe.StartFirstSkillNode(RunningData);
    }
    /// <summary>
    /// �ȴ�д�������ܸպð󶨵��������ϳ�ʼ����
    /// </summary>
    /// <param name="caster"></param>
    public void BindingSkill(GameObj caster)
    {
        RunningData = ObjectManager.Instance.CreateDataObject<SkillRunningData>();
        RunningData.SkillCaster = caster;
        RunningData.SkillState = E_SkillState.Running;
    }

    //�Ƿ���Ҫ�ȴ��ͷŽ��������ͷ�
    private bool b_IsWaitSkillEnd;
}
