using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 装饰节点  能够控制要执行多少次 延迟执行等内容
/// </summary>
[System.Serializable]
public abstract class DecoratorNode : BaseTreeNode
{
    public BaseTreeNode childNode;
    public override void ResetShowState()
    {
        base.ResetShowState();
        childNode.ResetShowState();
    }
}

