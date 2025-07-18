using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 角色行为基类
/// </summary>
public abstract class BaseAction
{
    public BaseCharacter BehaviorCharacter;
    protected CharacterController controller;
    protected EventManager<E_CharacterEvent> eventManager;
    protected CharacterActionDriver driver;
    public BaseAction(BaseCharacter behavior)
    {
        BehaviorCharacter = behavior;
        controller = BehaviorCharacter.GetComponent<CharacterController>();
        eventManager = BehaviorCharacter.eventManager;
    }
    //行为生命周期
    public abstract void ActionStart();
    public abstract void ActionUpdate();
    public abstract void ActionEnd();
    /// <summary>
    /// 判断是否满足进入此行为的条件
    /// </summary>
    /// <returns></returns>
    public abstract bool EvaluateEnterCondition();
    /// <summary>
    /// 主动结束状态
    /// </summary>
    protected virtual void ExitState()
    {
      BehaviorCharacter.GetCharacterComponent<CharacterActionDriver>().EndCurrentStateToIdle();
    }
}
