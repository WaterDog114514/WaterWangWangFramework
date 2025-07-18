using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// ��mono�ĵ���ģʽ���࣬����ϵͳ�������ܿأ�һ�����������������̽���
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T:class,new()
{
    private static T instance;
    //���ڼ����Ķ���
    protected static readonly object lockObj = new object();
    //���Եķ�ʽ
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}
