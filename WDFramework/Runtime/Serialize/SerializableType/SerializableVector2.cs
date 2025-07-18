using System;
using UnityEngine;
//һȺ���Զ��������л������⣬���Unity��ר�������л�
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