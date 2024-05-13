using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 条件节点，用来判断条件，如果成功就返回Succee，通常搭配顺序节点来使用
/// </summary>
[System.Serializable]
public class ConditionNode : BehaviorTreeNode
{
    /// <summary>
    /// 条件方法
    /// </summary>
    public Func<bool> ConditionAction;
    public ConditionNode(Func<bool> conditionAction)
    {
        ConditionAction = conditionAction;
    }
    public ConditionNode()
    {

    }
    public override E_NodeState Execute()
    {
        if (ConditionAction == null)
        {
            Debug.LogError("没有设置节点委托");
            return E_NodeState.Succeed;
        }
        //需要条件判断的行为
        if(ConditionAction.Invoke())
        {
            ChildState =  E_NodeState.Succeed;
            return E_NodeState.Succeed;
        }
        else
        {
            ChildState = E_NodeState.Faild;
            return E_NodeState.Faild;
        }


    }
    public void AddEvent(Func<bool> conditionAction)
    {
        this.ConditionAction += conditionAction;
    }
}

