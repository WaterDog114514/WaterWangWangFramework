using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// ��ֵ���͵����ԣ����ڴ洢������ߡ�������ֵ
/// </summary>
public class NumericAttribute<T> : CharacterAttribute where T : struct, IComparable
{
    /// <summary>
    /// ������ֵ�����ɳ�
    /// </summary>
    public T BaseValue { get; set; }
    /// <summary>
    /// ���ռӹ���õ���ֵ
    /// </summary>
    public T Value { get; set; }
    /// <summary>
    /// ����Ч��ֵ
    /// </summary>
    public List<AttributeChangeEffect<T>> ChangeEffects { get; set; }
    private  INumericOperations<T> Operation;

    public NumericAttribute(T baseValue = default)
    {
        BaseValue = baseValue;
        Value = baseValue;
        ChangeEffects = new List<AttributeChangeEffect<T>>();
        if (typeof(T) == typeof(int))
        {
            Operation = (INumericOperations<T>)IntOperations.instance;
        }
        else if (typeof(T) == typeof(float))
        {
            Operation = (INumericOperations<T>)FloatOperations.instance;
        }
    }
    /// <summary>
    /// ��������Ч��
    /// </summary>
    public void UpdateGainEffects()
    {
        T changeValue = default(T);
        T multiplicationValue = Operation.One;

        foreach (var effect in ChangeEffects)
        {
            if (effect == null) continue;
            if (effect.type == AttributeChangeEffectType.Change)
                changeValue = Operation.Add(changeValue, effect.value);
            else if (effect.type == AttributeChangeEffectType.Multiplication)
                multiplicationValue = Operation.Multiply(multiplicationValue, effect.value);
        }

        Value = Operation.Multiply(Operation.Add(BaseValue, changeValue), multiplicationValue);
    }

    public AttributeChangeEffect<T> AddChangeEffect(AttributeChangeEffectType type, T value)
    {
        var effect = new AttributeChangeEffect<T>(value, type);
        ChangeEffects.Add(effect);
        return effect;
    }
    /// <summary>
    /// ���������ֵ����
    /// </summary>
    /// <param name="effect"></param>
    public void AddChangeEffect(AttributeChangeEffect<T> effect)
    {
        ChangeEffects.Add(effect);
        UpdateGainEffects();
    }
    /// <summary>
    /// �Ƴ�ĳ��ֵ����Ч��
    /// </summary>
    /// <param name="effect"></param>
    public void RemoveChangeEffect(AttributeChangeEffect<T> effect)
    {
        ChangeEffects.Remove(effect);
        UpdateGainEffects();
    }
}

//��ֵ������� ��֤float��int�ܲ���
public interface INumericOperations<T>
{
    T Add(T a, T b);
    T Multiply(T a, T b);
    public T One { get; }

}
public class IntOperations : INumericOperations<int>
{
    public static IntOperations instance = new IntOperations();
    public int One => 1;

    public int Add(int a, int b) => a + b;
    public int Multiply(int a, int b) => a * b;
}
public class FloatOperations : INumericOperations<float>
{
    public static FloatOperations instance = new FloatOperations();
    public float One => 1;
    public float Add(float a, float b) => a + b;
    public float Multiply(float a, float b) => a * b;
}