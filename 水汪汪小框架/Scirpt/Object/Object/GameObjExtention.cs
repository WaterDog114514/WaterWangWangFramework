using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏物体拓展方法
/// </summary>
public static class GameObjExtensionMethod
{
    public static GameObj SetParent(this GameObj obj, Transform Parent)
    {

        obj.transform.parent = Parent;
        return obj;
    }

    public static GameObj SetPosition(this GameObj obj, Vector3 pos)
    {

        obj.transform.position = pos;
        return obj;
    }

    public static GameObj SetRotation(this GameObj obj,Quaternion quaternion )
    {

        obj.transform.rotation = quaternion;
        return obj;
    }

    public static GameObj SetScale(this GameObj obj, Vector3 Scale)
    {
    
        obj.transform.localScale = Scale;
        return obj;
    }
}
