using System;
using UnityEngine;
//一群可以二进制序列化的玩意，替代Unity类专用来序列化
[Serializable]
public struct SerializableVector2
{
    public Vector2 vector2
    {
        get => new Vector2(x, y);
        set
        {
            x = value.x;
            y = value.y;
        }
    }
    public float x, y;
    public SerializableVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}