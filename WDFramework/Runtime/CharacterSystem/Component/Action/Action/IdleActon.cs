using UnityEngine;

public  class IdleAction : BaseAction
{

    public IdleAction(BaseCharacter behavior) : base(behavior)
    {
        
    }

    //״̬�����ж���ţͷ�������� �Ҳۣ�

    public override void ActionStart()
    {
      //  eventManager.TriggerEvent(E_CharacterEvent.EnterState, E_CharacterStateName.Idle);
        //���ö���
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
