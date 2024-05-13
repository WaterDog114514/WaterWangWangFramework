using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 装饰
/// 节点  能够控制要执行多少次 延迟执行等内容
/// </summary>
[System.Serializable]
public class RepeatDecoratorNode : DecoratorNode
{
    /// <summary>
    /// 执行总次数
    /// </summary>
    public int TotalExecuteCount = 1;
    /// <summary>
    /// 当前执行次数
    /// </summary>
    private int CurrentExecuteCount = 0;
    public RepeatDecoratorNode()
    {

    }
    public RepeatDecoratorNode(int totalExecuteCount)
    {
        TotalExecuteCount = totalExecuteCount;
    }
    public override E_NodeState Execute()
    {
       ChildState = childNode.Execute();
        if (ChildState == E_NodeState.Succeed || ChildState == E_NodeState.Faild)
        {
            //每次子节点执行成功或失败才算一次 防止多个循环子节点出问题
            CurrentExecuteCount++;
            //执行到指定次数 跳过了
            if (CurrentExecuteCount >= TotalExecuteCount)
            {
                CurrentExecuteCount = 0;
                //取最后执行那次的结果作为最后结果
                return ChildState;
            }
        }
        //管他成功还是失败 必须给我执行完多少次才行
        ChildState = E_NodeState.Running;
        return E_NodeState.Running;

    }
}

