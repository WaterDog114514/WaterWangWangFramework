using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 并发节点  所有子节点返回失败或成功才算整体完成
/// </summary>
[System.Serializable]
public sealed class ParallelTreeNode : ControlTreeNode
{
    /// <summary>
    /// 用来记录每个节点是否执行完毕
    /// </summary>
    public bool[] FinishIndex;
    private E_NodeState tempState;
    public override E_NodeState Execute()
    {
        //第一次执行，就新做一个
        if (FinishIndex == null)
        {
            FinishIndex = new bool[childNodes.Count];
        }
        for (int i = 0; i < FinishIndex.Length; i++)
        {
            //没有完成就执行 然后检索
            if (FinishIndex[i] == false)
            {
                tempState = childNodes[i].Execute();
                //检索下
                switch (tempState)
                {
                    //完成了记录索引
                    case E_NodeState.Succeed:
                    case E_NodeState.Faild:
                        FinishIndex[i] = true;
                        break;
                    //不做完继续做
                    case E_NodeState.Running:
                        break;
                }
            }
        }
        //检查所有的完成了么
        for (int i = 0; i < FinishIndex.Length; i++)
        {
            //只要有一个还没有完成 就退出返回运行中状态
            if (FinishIndex[i] == false)
            {
                ChildState = E_NodeState.Running;
                return E_NodeState.Running;
            }
        }
        //全部执行完毕，让他们变成false重置状态
        for (int i = 0; i < FinishIndex.Length; i++)
        {
            FinishIndex[i] = false;
        }
        //所有都做完了，返回成功
        ChildState = E_NodeState.Succeed;
        return E_NodeState.Succeed;

    }

}
