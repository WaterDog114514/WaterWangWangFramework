using UnityEngine;

public  class IdleAction : BaseAction
{

    public IdleAction(BaseCharacter behavior) : base(behavior)
    {
        
    }

    //状态名和行动名牛头不对马嘴 我槽！

    public override void ActionStart()
    {
      //  eventManager.TriggerEvent(E_CharacterEvent.EnterState, E_CharacterStateName.Idle);
        //闲置动画
        eventManager.TriggerEvent(E_CharacterEvent.SetAnimationBool, E_AnimationHash.Idle,true);

    }
    public override void ActionEnd()
    {
      //  eventManager.TriggerEvent(E_CharacterEvent.RemoveState, E_CharacterStateName.Idle);
        eventManager.TriggerEvent(E_CharacterEvent.SetAnimationBool, E_AnimationHash.Idle,false);
    }



    public override void ActionUpdate()
    {
    }

    public override bool EvaluateEnterCondition()
    {
        return true;
    }
}
