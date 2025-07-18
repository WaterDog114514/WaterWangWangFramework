using System;
using UnityEngine;
//һȺ���Զ��������л������⣬���Unity��ר�������л�
[Serializable]
public class SerializableColor
{
    public float r = 255;
    public float b = 255;
    public float g = 255;
    public float a = 255;

    public SerializableColor(float r, float b, float g, float a)
    {
        this.r = r;
        this.b = b;
        this.g = g;
        this.a = a;
    }

    public Color color
    {
        get => new Color(r, g, b, a);
        set
        {
            r = value.r;
            g = value.g;
            b = value.b;
            a = value.a;
        }
    }
}