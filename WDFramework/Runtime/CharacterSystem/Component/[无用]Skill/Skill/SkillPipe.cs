using UnityEngine;

public class SkillPipe 
{
    public SkillNode rootNode;
    /// <summary>
    /// ��ʽ��ʹ�ü���
    /// </summary>
    public void StartFirstSkillNode(SkillRunningData state)
    {
        rootNode.execute(state);
    }
}
