using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using WDEditor;
[Serializable]
/// <summary>
/// ����������
/// </summary>
public abstract class BaseWindowData
{
    /// <summary>
    /// ������
    /// </summary>
    public abstract string Title { get;}
    /// <summary>
    /// �Ƿ�ʹ�ñ�����ɫ
    /// </summary>
    public bool isUseBlackground;
    /// <summary>
    /// ��ǰ���ڴ�С ��¼�������´δ򿪻���һ����
    /// </summary>
    public SerializableVector2 currentWindowSize;
    [HideInInspector]
    public bool isFirstCreated = true;
    // ��һ�δ�����ʼ������ ���಻�ܵ�
    public void IntiFirst()
    {
        
        //ֻ�ܵ�һ�δ���ʱ������
        if (!isFirstCreated) return;
        isFirstCreated = false;
        //ִ�е�һ�δ����߼�
        IntiWinSize();
        IntiFirstCreate();

    }
    /// <summary>
    /// ����ʱ�����ֵ����window��ʼ������
    /// </summary>
    public virtual void IntiLoad()
    {

    }
    /// <summary>
    /// ��ʼ������data��ֵֻ��������д
    /// </summary>
    public virtual void IntiFirstCreate()
    {


    }
    //��ʼ�����ڴ�С
    private void IntiWinSize()
    {
        //Ĭ�����ô��ڴ�С
        currentWindowSize.vector2 = new Vector2(768,512);
    }
}
