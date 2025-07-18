/// <summary>
/// 角色单位的事件，如进行攻击，受伤，加属性等
/// </summary>
public enum E_CharacterEvent
{
    Attack,
    GetHit,
    AddAttribute,
    GetIntAttribute,
    GetIntArrayAttribute,
    GetFloatAttribute,
    GetFloatArrayAttribute,
    GetStringAttribute,

    //状态判断
    EnterState,
    ContainState,
    RemoveState,
    //设置动画参数
     SetAnimationValue,
     SetAnimationTrigger,
     SetAnimationBool,
    //动画特殊处理
    SetMoveAnimationValue
    
   
 
}