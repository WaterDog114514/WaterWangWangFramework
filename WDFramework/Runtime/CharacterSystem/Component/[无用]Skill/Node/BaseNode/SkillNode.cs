using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 技能关键节点/步骤
/// </summary>
public abstract class SkillNode
{
    /// <summary>
    /// 无需返回状态，应使用运行状态传参记录即可
    /// </summary>
    public abstract void execute(SkillRunningData data);
    /// <summary>
    /// 判断还有子节点吗？没有直接返回完成，除非还有正在进行计时的
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public abstract bool IsHaveChildNode(SkillRunningData data);
}
/// <summary>
/// 单个孩子技能节点
/// </summary>
public abstract class SingleSkillNode : SkillNode
{
    public SkillNode nextNode;
    public override bool IsHaveChildNode(SkillRunningData data)
    {
        if (nextNode != null) return true;
        //关键点，是否在计时，不在计时就直接完成
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
/// 多个孩子技能节点
/// </summary>
public abstract class MultipleSkillNode : SkillNode
{
    public List<SkillNode> childrenNodes = new List<SkillNode>();
    public override bool IsHaveChildNode(SkillRunningData data)
    {
        if (childrenNodes.Count > 0) return true;
        //关键点，是否在计时，不在计时就直接完成
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
