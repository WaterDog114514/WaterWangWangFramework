using UnityEngine;

public abstract class MoveAction : BaseAction
{
    /// <summary>
    /// ��ɫ������
    /// </summary>
    protected float p_MoveSpeed => eventManager.TriggerRequest<E_CharacterAttributeType, float>(E_CharacterEvent.GetFloatAttribute, E_CharacterAttributeType.MoveSpeed);
    public MoveAction(BaseCharacter behavior) : base(behavior)
    {
        
    }



    public override void ActionStart()
    {
        //�ƶ�����
        eventManager.TriggerEvent(E_CharacterEvent.SetAnimationBool, E_AnimationHash.Moving,true);
        //�ƶ�״̬
        eventManager.TriggerEvent(E_CharacterEvent.EnterState, E_CharacterStateName.Moving);

    }
    //״̬�����ж���ţͷ�������� �Ҳۣ�
    public override void ActionEnd()
    {
        eventManager.TriggerEvent(E_CharacterEvent.RemoveState, E_CharacterStateName.Moving);
        //�ر��ƶ�����
        eventManager.TriggerEvent(E_CharacterEvent.SetAnimationBool, E_AnimationHash.Moving, false);

    }

}
