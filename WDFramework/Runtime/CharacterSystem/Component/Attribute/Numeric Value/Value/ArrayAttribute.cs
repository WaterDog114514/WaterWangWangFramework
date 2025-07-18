using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 数组类型，用于存储数组的属性
/// </summary>
public class ArrayAttribute<T> : CharacterAttribute where T : struct, IComparable
{
    public T[] Value;
    public ArrayAttribute(T[] value)
    {
        Value = value;
    }
    /// <summary>
    /// 索引器，用于通过索引访问数组中的值
    /// </summary>
    /// <param name="index">数组索引</param>
    /// <returns>数组中的值</returns>
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Value.Length)
            {
                throw new IndexOutOfRangeException($"索引 {index} 超出数组范围 [0, {Value.Length - 1}]");
            }
            return Value[index];
        }
        set
        {
            if (index < 0 || index >= Value.Length)
            {
                throw new IndexOutOfRangeException($"索引 {index} 超出数组范围 [0, {Value.Length - 1}]");
            }
            Value[index] = value;
        }
    }
}
