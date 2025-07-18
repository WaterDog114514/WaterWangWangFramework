using UnityEngine;

/// <summary>
/// 四叉树物体
/// </summary>
public class QuadObject : MonoBehaviour
{
    /// <summary>
    /// 引用到实际的游戏物体
    /// </summary>
    public GameObject GameObj;

    /// <summary>
    /// 表示一个物体在四叉树中的座标，以及碰撞大小
    /// </summary>
    public SerializableRect ObjRect;

    /// <summary>
    /// 是否是动态物体，动态物体需要频繁更新
    /// </summary>
    public bool IsDynamic;
    public QuadObject(GameObject gameObject, Rect bounds, bool isDynamic)
    {
        GameObj = gameObject;
        ObjRect.rect = bounds;
        IsDynamic = isDynamic;
    }
}

