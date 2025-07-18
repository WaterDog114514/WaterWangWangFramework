using System;
using UnityEngine;



/// <summary>
/// ��������Ч��
/// </summary>
public class AttributeChangeEffect<T> where T : struct, IComparable
{

    /// <summary>
    /// ������
    /// </summary>
    public T value;
    /// <summary>
    /// ��������
    /// </summary>
    public AttributeChangeEffectType type;
    //��ʼ��
    public AttributeChangeEffect(T value, AttributeChangeEffectType type)
    {
        this.value = value;
        this.type = type;
    }
}
/// <summary>
/// �����������͹淶
/// </summary>
public enum AttributeChangeEffectType
{
    /// <summary>
    /// ����
    /// </summary>
    Multiplication,
    /// <summary>
    /// �Ӽ�
    /// </summary>
    Change
}