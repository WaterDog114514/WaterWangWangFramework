using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// ��ϵͳ�ܿص�ϵͳ���������ڹ���ʱ���Զ�����ϵͳ������
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ManagedSingleton<T> : ISystem where T : class, new()
{
    private static T instance;
    public ManagedSingleton()
    {
        Debug.Log($"{typeof(T).Name} ϵͳ��ʼ��");
        instance = this as T;
        Initialized();
    }

    /// <summary>
    /// ��ʼ������������ϵͳ�ոմ���ʱ������
    /// </summary>
    public abstract void Initialized();
    //���ڼ����Ķ���
    protected static readonly object lockObj = new object();
    //���Եķ�ʽ
    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                lock (lockObj)
                {
                    return instance;
                }
            }
            else
            {
                // �׳��쳣����ʽ�����������������
                throw new InvalidOperationException($"��ͨ�� GameSystemManager ע��ϵͳ,��ǰ����ϵͳΪ��{typeof(T).Name}");
            }
        }
    }

    public virtual void DestorySystem()
    {
        instance = null;
    }
}
