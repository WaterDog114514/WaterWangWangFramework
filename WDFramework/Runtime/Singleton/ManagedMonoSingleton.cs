using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// ��ϵͳ�ܿص�ϵͳ���������ڹ���ʱ���Զ�����ϵͳ������
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ManagedMonoSingleton<T> : MonoBehaviour, ISystem where T : MonoBehaviour
{
    private static T instance;
    //���ڼ����Ķ���
    protected static readonly object lockObj = new object();
    protected void Awake()
    {
        Initialized();
    }
    /// <summary>
    /// ��ʼ������������ϵͳ�ոմ���ʱ������
    /// </summary>
    public abstract void Initialized();
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
                throw new InvalidOperationException("��ͨ�� GameSystemManager ע��ϵͳ");
            }
        }
    }
    private static GameObject obj;
    public static T  ConstructSystem()
    {
        Debug.LogWarning("��һ�ο����Բ��ԣ�ȷ��û�����ɾLOG");
        //��̬���� ��̬����
        //�ڳ����ϴ���������
        obj = new GameObject();
        //�õ�T�ű������� Ϊ������� �����ٱ༭���п�����ȷ�Ŀ�����
        //����ģʽ�ű�����������GameObject
        obj.name = typeof(T).ToString();
        //��̬���ض�Ӧ�� ����ģʽ�ű�
        instance = obj.AddComponent<T>();
       
        //������ʱ���Ƴ����� ��֤����������Ϸ���������ж�����
        DontDestroyOnLoad(obj);
        return instance;
    }

    public virtual void DestorySystem()
    {
        //��������
        Destroy(obj);
        instance = null;
    }
}
