using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//必须得从分帧执行的角度去了解才行

/// <summary>
/// 序列节点 
/// 特点：
/// 1.会依次执行自己的子节点
/// 2.如果某一个子节点执行失败了 就会停下来 然后返回失败
/// 3.如果没有一个节点失败，那么会执行完所有子节点的逻辑 并且返回成功
/// </summary>
[System.Serializable]
public class SequeneTreeNode : ControlTreeNode
{
    /// <summary>
    /// 按顺序执行子节点
    /// </summary>
    /// <returns></returns>
    public override E_NodeState Execute()
    {
        if (childNodes.Count == 0)
        {
            Debug.LogError("没有设置任何的子节点");
            ChildState = E_NodeState.Succeed;
            return E_NodeState.Succeed;
        }
        switch (childNodes[nowIndex].Execute())
        {
            case E_NodeState.Succeed:
                //如果当前节点执行成功了，那么继续执行下一个节点的逻辑
                nowIndex++;
                if (nowIndex >= childNodes.Count)
                {
                    //执行完了，下次从0开始
                    nowIndex = 0;
                    ChildState = E_NodeState.Succeed;
                    return E_NodeState.Succeed;
                }
                break;
            case E_NodeState.Faild:
                //如果失败了，下一次也应该从头开始执行
                nowIndex = 0;
                ChildState = E_NodeState.Faild;
                return E_NodeState.Faild;
            case E_NodeState.Running:
                ChildState = E_NodeState.Running;
                return E_NodeState.Running;
        }
        //只有一种情况会从这返回 
        //成功 并且节点之后还需要继续执行
        return E_NodeState.Succeed;
    }
}