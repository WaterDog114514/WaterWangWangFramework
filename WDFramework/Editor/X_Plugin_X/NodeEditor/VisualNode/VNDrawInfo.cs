using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 绘制信息类，用于存储节点在编辑器中绘制的信息
/// </summary>
[Serializable]
public class VNDrawInfo
{
    public Vector2 Position
    {
        get => _position.vector2;
        set => _position.vector2 = value;
    }

    //进口出口的中心位置
    [NonSerialized]
    public Vector2 EnterPortCenterPos;
    [NonSerialized]
    public Vector2 ExitPortCenterPos;

    //绘制的节点所在位置
    public SerializableVector2 _position;
    //绘制的名字颜色
    public SerializableColor TitleColor = new SerializableColor(0, 0, 0, 1);
    //标题块颜色
    public SerializableColor TitleAreaColor = new SerializableColor(0, 0, 0, 1);
    //背景颜色                              
    public SerializableColor BackgroundColor = new SerializableColor(0, 0, 0, 1);
    //出口进口节点方位
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