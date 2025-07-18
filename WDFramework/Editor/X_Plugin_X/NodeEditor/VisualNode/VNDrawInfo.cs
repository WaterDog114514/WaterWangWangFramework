using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ������Ϣ�࣬���ڴ洢�ڵ��ڱ༭���л��Ƶ���Ϣ
/// </summary>
[Serializable]
public class VNDrawInfo
{
    public Vector2 Position
    {
        get => _position.vector2;
        set => _position.vector2 = value;
    }

    //���ڳ��ڵ�����λ��
    [NonSerialized]
    public Vector2 EnterPortCenterPos;
    [NonSerialized]
    public Vector2 ExitPortCenterPos;

    //���ƵĽڵ�����λ��
    public SerializableVector2 _position;
    //���Ƶ�������ɫ
    public SerializableColor TitleColor = new SerializableColor(0, 0, 0, 1);
    //�������ɫ
    public SerializableColor TitleAreaColor = new SerializableColor(0, 0, 0, 1);
    //������ɫ                              
    public SerializableColor BackgroundColor = new SerializableColor(0, 0, 0, 1);
    //���ڽ��ڽڵ㷽λ
    public E_PortPos EnterPortPos;
    public E_PortPos ExitPortPos;
    public enum E_PortPos
    {
        left,
        right,
        top,
        bottom,
    }
}