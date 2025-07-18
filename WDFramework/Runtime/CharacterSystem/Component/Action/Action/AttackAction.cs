using UnityEngine;

public class AttackAction : BaseAction
{
    public AttackAction(BaseCharacter behavior) : base(behavior)
    {

    }

    public override void ActionEnd()
    {
        eventManager.TriggerEvent(E_CharacterEvent.RemoveState, E_CharacterStateName.Attacking);

    }

    public override void ActionStart()
    {
        //¹¥»÷¶¯»­
        eventManager.TriggerEvent(E_CharacterEvent.SetAnimationTrigger,E_AnimationHash.NormalAttack1);
        //½øÈë¹¥»÷×´Ì¬
        eventManager.TriggerEvent(E_CharacterEvent.EnterState, E_CharacterStateName.Attacking);
        TimerManager.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, 1F, ExitState);
    }

    public override void ActionUpdate()
    {
    }

    public override bool EvaluateEnterCondition()
    {
        return true;
    }
}
