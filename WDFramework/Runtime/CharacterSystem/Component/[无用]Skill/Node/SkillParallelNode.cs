using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������νڵ� ���нڵ�ͬʱ����
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
