using UnityEngine;
/// <summary>
/// 技能节点运行数据，用于节点之间传递
/// </summary>
public class SkillRunningData : DataObj
{
    /// <summary>
    /// 技能施法者
    /// </summary>
    public GameObj SkillCaster;
    /// <summary>
    /// 整体技能流程执行状态
    /// </summary>
    public E_SkillState SkillState;
    /// <summary>
    /// 是否正在计时
    /// </summary>
    public bool isTiming = false;
}

public enum E_SkillState
{
    Finished,
    Running
}