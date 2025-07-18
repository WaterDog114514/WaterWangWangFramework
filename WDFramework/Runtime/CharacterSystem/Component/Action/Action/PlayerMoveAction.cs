//using UnityEngine;
//using UnityEngine.EventSystems;

///// <summary>
///// 玩家移动行为
///// </summary>
//public class PlayerMoveAction : MoveAction
//{
//    //在斜视角下，纵向偏移系数
//    private float VeticalViewOffset = 1.5f;
//    //移速消减系数
//    private float ReductionFactor = 0.2f;
//    protected DirectionKeyInputInfo HorizontalInputInfo;
//    protected DirectionKeyInputInfo VerticalInputInfo;
//    protected DirectionKeyInputInfo RunInputInfo;
//    /// <summary>
//    /// 移动方向 朝着哪个方向移动
//    /// </summary>
//    protected Vector3 MoveDirection;
//    public PlayerMoveAction(BaseCharacter behavior) : base(behavior)
//    {
//        //绑定一波
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

//        //需要进一步优化
//        MoveDirection = new Vector3(-HorizontalInputInfo.Value, 0, VerticalInputInfo.Value * VeticalViewOffset);
//        if (p_MoveSpeed <= 0 || MoveDirection == Vector3.zero) return;
//        //按键热值 方向键+疾跑键
//        float InputMoveSpeed = Mathf.Max(Mathf.Abs(HorizontalInputInfo.Value), Mathf.Abs(VerticalInputInfo.Value)) + RunInputInfo.Value;

//        eventManager.TriggerEvent(E_CharacterEvent.SetAnimationValue,E_AnimationHash.MoveSpeed, InputMoveSpeed);
//        //实际移动
//        controller.Move(MoveDirection * p_MoveSpeed * InputMoveSpeed * ReductionFactor);

//    }

//    public override bool EvaluateEnterCondition()
//    {
//        bool can = !eventManager.TriggerRequest<E_CharacterStateName, bool>(E_CharacterEvent.ContainState, E_CharacterStateName.Attacking);
//        Debug.Log(can);

//        return can;
//    }
//}
