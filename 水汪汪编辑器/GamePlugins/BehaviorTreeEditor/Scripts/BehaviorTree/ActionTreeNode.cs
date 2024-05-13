using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 行为树节点基类
/// </summary>
[System.Serializable]
public class ActionTreeNode : BehaviorTreeNode
{
    private UnityAction action;

    public ActionTreeNode(UnityAction action)
    {
        this.action = action;
    }
    public void AddEvent(UnityAction action)
    {
        this.action += action;
    }

    public ActionTreeNode()
    {

    }
    public override E_NodeState Execute()
    {

        action?.Invoke();
        ChildState = E_NodeState.Succeed;
        return E_NodeState.Succeed;
        //需要条件判断的行为
    }

}

