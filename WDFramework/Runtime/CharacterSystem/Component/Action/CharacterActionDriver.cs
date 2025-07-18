using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色行为持有者 行为作出者
/// </summary>
public class CharacterActionDriver : CharacterComponent
{
    protected Dictionary<E_ActionName, BaseAction> dic_actions = new Dictionary<E_ActionName, BaseAction>();

    public CharacterActionDriver(BaseCharacter baseCharacter) : base(baseCharacter)
    {

    }

    public override void IntializeComponent()
    {
        //输入测试

    }
    protected BaseAction CurrentAction;
    public override void UpdateComponent()
    {
        if (CurrentAction != null)
            CurrentAction.ActionUpdate();
    }
    public void StartNewAction(E_ActionName actionName)
    {
        BaseAction action = dic_actions[actionName];
        //只有不是重复当前行为，且满足进入条件才能切换行为
        if (CurrentAction != action && action.EvaluateEnterCondition())
        {
            Debug.Log($"切换状态{actionName}");
            CurrentAction?.ActionEnd();
            CurrentAction = action;
            action.ActionStart();
        }
    }

    protected void AddAction(E_ActionName name, BaseAction action)
    {
        if (!dic_actions.ContainsKey(name))
        {
            dic_actions.Add(name, action);
        }
    }
    /// <summary>
    /// 结束当前状态，转为Idle状态
    /// </summary>
    public void EndCurrentStateToIdle()
    {
        StartNewAction(E_ActionName.Idle);
    }
}
