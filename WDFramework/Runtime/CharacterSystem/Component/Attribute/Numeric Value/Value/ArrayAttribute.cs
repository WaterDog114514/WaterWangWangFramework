using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �������ͣ����ڴ洢���������
/// </summary>
public class ArrayAttribute<T> : CharacterAttribute where T : struct, IComparable
{
    public T[] Value;
    public ArrayAttribute(T[] value)
    {
        Value = value;
    }
    /// <summary>
    /// ������������ͨ���������������е�ֵ
    /// </summary>
    /// <param name="index">��������</param>
    /// <returns>�����е�ֵ</returns>
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Value.Length)
            {
                throw new IndexOutOfRangeException($"���� {index} �������鷶Χ [0, {Value.Length - 1}]");
            }
            return Value[index];
        }
        set
        {
            if (index < 0 || index >= Value.Length)
            {
                throw new IndexOutOfRangeException($"���� {index} �������鷶Χ [0, {Value.Length - 1}]");
            }
            Value[index] = value;
        }
    }
}
