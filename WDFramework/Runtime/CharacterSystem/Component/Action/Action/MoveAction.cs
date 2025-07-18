using UnityEngine;

public abstract class MoveAction : BaseAction
{
    /// <summary>
    /// 角色的移速
    /// </summary>
    protected float p_MoveSpeed => eventManager.TriggerRequest<E_CharacterAttributeType, float>(E_CharacterEvent.GetFloatAttribute, E_CharacterAttributeType.MoveSpeed);
    public MoveAction(BaseCharacter behavior) : base(behavior)
    {
        
    }



    public override void ActionStart()
    {
        //移动动画
        eventManager.TriggerEvent(E_CharacterEvent.SetAnimationBool, E_AnimationHash.Moving,true);
        //移动状态
        eventManager.TriggerEvent(E_CharacterEvent.EnterState, E_CharacterStateName.Moving);

    }
    //状态名和行动名牛头不对马嘴 我槽！
    public override void ActionEnd()
    {
        eventManager.TriggerEvent(E_CharacterEvent.RemoveState, E_CharacterStateName.Moving);
        //关闭移动动画
        eventManager.TriggerEvent(E_CharacterEvent.SetAnimationBool, E_AnimationHash.Moving, false);

    }

}
