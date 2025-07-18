using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���ܹؼ��ڵ�/����
/// </summary>
public abstract class SkillNode
{
    /// <summary>
    /// ���践��״̬��Ӧʹ������״̬���μ�¼����
    /// </summary>
    public abstract void execute(SkillRunningData data);
    /// <summary>
    /// �жϻ����ӽڵ���û��ֱ�ӷ�����ɣ����ǻ������ڽ��м�ʱ��
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public abstract bool IsHaveChildNode(SkillRunningData data);
}
/// <summary>
/// �������Ӽ��ܽڵ�
/// </summary>
public abstract class SingleSkillNode : SkillNode
{
    public SkillNode nextNode;
    public override bool IsHaveChildNode(SkillRunningData data)
    {
        if (nextNode != null) return true;
        //�ؼ��㣬�Ƿ��ڼ�ʱ�����ڼ�ʱ��ֱ�����
        else
        {
            if (data.isTiming)
                data.SkillState = E_SkillState.Running;
            else
                data.SkillState = E_SkillState.Finished;

            return false;
        }
    }
}
/// <summary>
/// ������Ӽ��ܽڵ�
/// </summary>
public abstract class MultipleSkillNode : SkillNode
{
    public List<SkillNode> childrenNodes = new List<SkillNode>();
    public override bool IsHaveChildNode(SkillRunningData data)
    {
        if (childrenNodes.Count > 0) return true;
        //�ؼ��㣬�Ƿ��ڼ�ʱ�����ڼ�ʱ��ֱ�����
        else
        {
            if (data.isTiming)
                data.SkillState = E_SkillState.Running;
            else
                data.SkillState = E_SkillState.Finished;

            return false;
        }
    }
}
