using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏对象实例 方便检测
/// </summary>
public class GameObjectInstance : MonoBehaviour
{
    public GameObj gameObj { private set; get; }
    public void Inti(GameObj obj)
    {
        gameObj = obj;
    }
}
