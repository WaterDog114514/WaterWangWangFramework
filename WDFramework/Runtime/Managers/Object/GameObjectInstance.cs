using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ����ʵ�� ������
/// </summary>
public class GameObjectInstance : MonoBehaviour
{
    public GameObj gameObj { private set; get; }
    public void Inti(GameObj obj)
    {
        gameObj = obj;
    }
}
