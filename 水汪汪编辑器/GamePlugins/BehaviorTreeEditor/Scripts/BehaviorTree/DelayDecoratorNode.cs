using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 不会卡死AI逻辑，使用协程来执行
/// </summary>
[System.Serializable]
public class DelayDecoratorNode : DecoratorNode
{

    public DelayDecoratorNode(float timeCD)
    {
        waitTime = new WaitForSeconds(timeCD);
    }
    /// <summary>
    /// 等待中，避免重复执行
    /// </summary>
    public bool IsWaiting;
    public DelayDecoratorNode()
    {

    }
    public override E_NodeState Execute()
    {
        //不在延迟中才执行
        if (!IsWaiting)
            MonoManager.Instance.StartCoroutine(ExecuteChildNode());
        ChildState = E_NodeState.Succeed;
        return E_NodeState.Succeed;
    }
    public WaitForSeconds waitTime;
    public IEnumerator ExecuteChildNode()
    {
        IsWaiting = true;
        //先执行一次
        childNode.Execute();
        yield return waitTime;
        while (true)
        {
            //分帧执行
            switch (childNode.Execute())
            {
                case E_NodeState.Succeed:
                case E_NodeState.Faild:
                    IsWaiting = false;
                    yield break;
                default:
                    break;
            }
            yield return null;
        }

    }
}
