using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
/// <summary>
/// 行为树节点基类
/// </summary>
public abstract class BaseTreeNode
{
    public E_NodeState ChildState = E_NodeState.Faild;
    /// <summary>
    /// 执行节点逻辑的抽象方法 子类必须去实现该方法
    /// 此地使用了递归的思维
    /// </summary>
    /// <returns>结果状态</returns>
    public abstract E_NodeState Execute();
    /// <summary>
    /// 重置所有状态
    /// </summary>
    public virtual void ResetShowState()
    {
        ChildState = E_NodeState.Faild;
    }
}

