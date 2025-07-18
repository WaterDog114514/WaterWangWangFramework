using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ϸ������չ����
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
