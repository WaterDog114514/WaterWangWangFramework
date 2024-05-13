using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 起始根节点：用来开始
/// </summary>
[System.Serializable]
public class RootTreeNode : BaseTreeNode
{
    public BaseTreeNode childNode;
    public override E_NodeState Execute()
    {
        if (childNode == null) Debug.LogError("请检查行为树AI数据文件是否绑定，节点数据是否绑定在.asset里");
        ChildState = childNode.Execute();
        return ChildState;
    }
    public override void ResetShowState()
    {
        //根本就没有执行或者执行失败 运行中就不用清空了
        if (ChildState != E_NodeState.Succeed) return;
        base.ResetShowState();
        childNode.ResetShowState();
    }
}

