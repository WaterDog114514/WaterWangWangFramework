using System;
using UnityEngine;

/// <summary>
/// ����BUFF���޸�Ӱ��Ч��
/// </summary>
/// <typeparam name="T"></typeparam>
public class AttributeBUFFStateEffect<T> : DataObj, IBuffEffect where T : struct, IComparable
{
    /// <summary>
    /// �����޸�
    /// </summary>
    public AttributeChangeEffect<T> ChangeEffect;
    /// <summary>
    /// Ӧ�õ�����
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