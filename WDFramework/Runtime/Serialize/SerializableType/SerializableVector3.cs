using System;
using UnityEngine;
//һȺ���Զ��������л������⣬���Unity��ר�������л�

[Serializable]
public struct SerializableVector3
{
    public float x, y, z;
    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3 vector3
    {
        get => new Vector3(x, y, z);
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }
}