using UnityEngine;
/// <summary>
/// 技能释放信息
/// </summary>
public class SkillCastInfo : DataObj
{
    /// <summary>
    /// 目标对象
    /// </summary>
    public Object TargetObj;
    /// <summary>
    /// 施法者对象
    /// </summary>
    public Object CasterObj;
    /// <summary>
    /// 位置信息
    /// </summary>
    public Vector3 TargetPosition;
}
