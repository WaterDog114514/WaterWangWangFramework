using System;
using UnityEngine;



/// <summary>
/// 属性增益效果
/// </summary>
public class AttributeChangeEffect<T> where T : struct, IComparable
{

    /// <summary>
    /// 增益增
    /// </summary>
    public T value;
    /// <summary>
    /// 增益类型
    /// </summary>
    public AttributeChangeEffectType type;
    //初始化
    public AttributeChangeEffect(T value, AttributeChangeEffectType type)
    {
        this.value = value;
        this.type = type;
    }
}
/// <summary>
/// 属性增益类型规范
/// </summary>
public enum AttributeChangeEffectType
{
    /// <summary>
    /// 倍乘
    /// </summary>
    Multiplication,
    /// <summary>
    /// 加减
    /// </summary>
    Change
}