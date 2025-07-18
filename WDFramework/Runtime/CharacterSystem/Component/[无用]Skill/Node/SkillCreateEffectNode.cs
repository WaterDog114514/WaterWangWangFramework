using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 创建技能特效的目标节点
/// </summary>
public class SkillCreateEffectNode : SingleSkillNode
{
    /// <summary>
    /// 技能特效路径
    /// </summary>
    public GameObj EffectObj;
    //附加动作
    public UnityAction<GameObj> CreateEffectAction;

    public override void execute(SkillRunningData data)
    {
        if (!IsHaveChildNode(data)) return;
        //逻辑
    }
}
