using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 并行修饰节点 多行节点同时进行
/// </summary>
public class SkillParallelNode : MultipleSkillNode
{
    public override void execute(SkillRunningData data)
    {
        if(!IsHaveChildNode(data)) return;
        for (int i = 0; i <  childrenNodes.Count; i++)
        {
            childrenNodes[i].execute(data);
        }
    }
}
