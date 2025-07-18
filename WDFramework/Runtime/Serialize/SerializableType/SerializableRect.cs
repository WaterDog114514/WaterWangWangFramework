using System;
using UnityEngine;
/// <summary>
///可序列化的Rect类
/// </summary>
[Serializable]
public struct SerializableRect
{
    public float x;
    public float y;
    public float width;
    public float height;
    [NonSerialized]
    public Rect _rect;
    public SerializableRect(float x, float y, float width, float height) : this()
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public Rect rect
    {
        get
        {
            if(_rect == default) _rect = new Rect(x,y,width,height);
            return _rect;
        }
        set
        {
            x = value.x;
            y = value.y;
            width = value.width;
            height = value.height;
            _rect = new Rect(x, y, width, height);
        }
    }


}