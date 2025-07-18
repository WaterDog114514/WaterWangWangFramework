using System;
using UnityEngine;

/// <summary>
/// 属性BUFF的修改影响效果
/// </summary>
/// <typeparam name="T"></typeparam>
public class AttributeBUFFStateEffect<T> : DataObj, IBuffEffect where T : struct, IComparable
{
    /// <summary>
    /// 属性修改
    /// </summary>
    public AttributeChangeEffect<T> ChangeEffect;
    /// <summary>
    /// 应用的属性
    /// </summary>
    public NumericAttribute<T> ApplyNumericAttribute;

    public void ApplyEffect()
    {
        if (ApplyNumericAttribute.ChangeEffects.Contains(ChangeEffect)) return;
        ApplyNumericAttribute.AddChangeEffect(ChangeEffect);
    }

    public void RemoveEffect()
    {
        ApplyNumericAttribute.RemoveChangeEffect(ChangeEffect);
    }
}