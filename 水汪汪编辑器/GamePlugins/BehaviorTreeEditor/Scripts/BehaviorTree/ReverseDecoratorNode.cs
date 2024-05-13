using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 反转装饰节点 能够反转子状态结果
/// </summary>
[System.Serializable]
public class ReverseDecoratorNode : DecoratorNode
{

    public override E_NodeState Execute()
    {
        //翻转结果，不多BB
        switch (childNode.Execute())
        {
            case E_NodeState.Succeed:
                ChildState = E_NodeState.Faild;
                return E_NodeState.Faild;
            case E_NodeState.Faild:
                ChildState = E_NodeState.Succeed;
                return E_NodeState.Succeed;
            default:
                ChildState =  E_NodeState.Running;
                return E_NodeState.Running;
        }

    }
}

