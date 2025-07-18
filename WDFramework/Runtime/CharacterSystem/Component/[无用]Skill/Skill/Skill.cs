using UnityEngine;
using WDFramework;

public  class Skill :DataObj
{
    /// <summary>
    /// 技能的唯一性ID，可以用来进入判断池子逻辑来复用
    /// </summary>
    public int SkillID;
    /// <summary>
    /// 技能流程
    /// </summary>
    private SkillPipe pipe;

    public SkillRunningData RunningData;
    /// <summary>
    /// 开始释放技能
    /// </summary>
    public void CastSkill(GameObj caster)
    {
        if(b_IsWaitSkillEnd&&RunningData.SkillState== E_SkillState.Running) return;
        //开始执行了
        RunningData.SkillState = E_SkillState.Running;
        pipe.StartFirstSkillNode(RunningData);
    }
    /// <summary>
    /// 等待写，当技能刚好绑定到人物身上初始化用
    /// </summary>
    /// <param name="caster"></param>
    public void BindingSkill(GameObj caster)
    {
        RunningData = ObjectManager.Instance.CreateDataObject<SkillRunningData>();
        RunningData.SkillCaster = caster;
        RunningData.SkillState = E_SkillState.Running;
    }

    //是否需要等待释放结束才能释放
    private bool b_IsWaitSkillEnd;
}
