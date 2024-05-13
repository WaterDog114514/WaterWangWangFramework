using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制节点：主要用来控制子节点的逻辑运转
/// </summary>
[System.Serializable]
public abstract class ControlTreeNode: BaseTreeNode
{
    /// <summary>
    /// 当前执行逻辑的子节点序号
    /// </summary>
    public int nowIndex;

    /// <summary>
    /// 用于存储子节点的容器 该节点的所有子节点都会存储在该List中
    /// </summary>
    protected List<BaseTreeNode> childNodes = new List<BaseTreeNode>();

    /// <summary>
    /// 添加子节点的方法 使用变长参数 因为一个节点可能有n个子节点 通过变长参数 更加的方便
    /// </summary>
    public void AddNode(BaseTreeNode node)
    {
        childNodes.Add(node);
    }
    public override void ResetShowState()
    {
        base.ResetShowState();
        for (int i = 0; i < childNodes.Count; i++)
        {
            childNodes[i].ResetShowState();
        }
    }

}

