using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//必须得从分帧执行的角度去了解才行

/// <summary>
/// 选择节点
/// 特点：
/// 1.会依次执行自己的子节点
/// 2.如果当前节点执行成功了 就不会继续执行后续节点
/// 3.如果当前节点执行失败了 就会继续往后执行 直到成功
/// 4.若一个都没有成功则返回失败
/// </summary>
[System.Serializable]
public class SelectTreeNode : ControlTreeNode
{
    //这里不采用while循环，因为while循环会一帧把所有逻辑跑完
    //我们的需求是多帧才完成一次轮回，或者使用携程去等待时机去调用执行下去
    public override E_NodeState Execute()
    {
        //如果选择节点中 有某个节点执行成功 就不必继续往后执行了
        //直接返回成功即可
        if (childNodes.Count == 0)
        {
            Debug.LogError("此控制节点没有任何的子节点");
            ChildState = E_NodeState.Succeed;
            return E_NodeState.Succeed;

        }
        switch (childNodes[nowIndex].Execute())
        {
            //成功了 
            case E_NodeState.Succeed:
                //重新开始
                    nowIndex = 0;
                ChildState = E_NodeState.Succeed;
                return E_NodeState.Succeed;

            case E_NodeState.Faild:
                //选择节点，失败跳到下一个
                nowIndex++;
                //已经没有更多的节点可以执行了
                //那证明前面的都失败了
                if (nowIndex == childNodes.Count)
                {
                    nowIndex = 0;
                    ChildState = E_NodeState.Faild;
                    return E_NodeState.Faild;
                }
                break;

            case E_NodeState.Running:
                ChildState = E_NodeState.Running;
                return E_NodeState.Running;
        }
        //只有当选择节点没有执行完时 并且当前节点失败时 才会来到这
        //证明还希望再下一帧继续往后执行 所以这里返回成功
        return E_NodeState.Succeed;
    }
}
