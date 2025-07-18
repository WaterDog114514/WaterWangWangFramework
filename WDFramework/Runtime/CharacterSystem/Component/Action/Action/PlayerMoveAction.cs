//using UnityEngine;
//using UnityEngine.EventSystems;

///// <summary>
///// ����ƶ���Ϊ
///// </summary>
//public class PlayerMoveAction : MoveAction
//{
//    //��б�ӽ��£�����ƫ��ϵ��
//    private float VeticalViewOffset = 1.5f;
//    //��������ϵ��
//    private float ReductionFactor = 0.2f;
//    protected DirectionKeyInputInfo HorizontalInputInfo;
//    protected DirectionKeyInputInfo VerticalInputInfo;
//    protected DirectionKeyInputInfo RunInputInfo;
//    /// <summary>
//    /// �ƶ����� �����ĸ������ƶ�
//    /// </summary>
//    protected Vector3 MoveDirection;
//    public PlayerMoveAction(BaseCharacter behavior) : base(behavior)
//    {
//        //��һ��
//        HorizontalInputInfo = InputManager.Instance.GetKeyInfo(E_InputEvent.MoveHorizontal) as DirectionKeyInputInfo;
//        VerticalInputInfo = InputManager.Instance.GetKeyInfo(E_InputEvent.MoveVertical) as DirectionKeyInputInfo;
//        RunInputInfo = InputManager.Instance.GetKeyInfo(E_InputEvent.Run) as DirectionKeyInputInfo;
//    }

//    public override void ActionEnd()
//    {
//        base.ActionEnd();
//    }

//    public override void ActionStart()
//    {
//        base.ActionStart();
//    }

//    public override void ActionUpdate()
//    {

//        //��Ҫ��һ���Ż�
//        MoveDirection = new Vector3(-HorizontalInputInfo.Value, 0, VerticalInputInfo.Value * VeticalViewOffset);
//        if (p_MoveSpeed <= 0 || MoveDirection == Vector3.zero) return;
//        //������ֵ �����+���ܼ�
//        float InputMoveSpeed = Mathf.Max(Mathf.Abs(HorizontalInputInfo.Value), Mathf.Abs(VerticalInputInfo.Value)) + RunInputInfo.Value;

//        eventManager.TriggerEvent(E_CharacterEvent.SetAnimationValue,E_AnimationHash.MoveSpeed, InputMoveSpeed);
//        //ʵ���ƶ�
//        controller.Move(MoveDirection * p_MoveSpeed * InputMoveSpeed * ReductionFactor);

//    }

//    public override bool EvaluateEnterCondition()
//    {
//        bool can = !eventManager.TriggerRequest<E_CharacterStateName, bool>(E_CharacterEvent.ContainState, E_CharacterStateName.Attacking);
//        Debug.Log(can);

//        return can;
//    }
//}
