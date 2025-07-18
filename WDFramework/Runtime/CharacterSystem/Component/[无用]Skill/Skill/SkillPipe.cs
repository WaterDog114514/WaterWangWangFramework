using UnityEngine;

public class SkillPipe 
{
    public SkillNode rootNode;
    /// <summary>
    /// 正式的使用技能
    /// </summary>
    public void StartFirstSkillNode(SkillRunningData state)
    {
        rootNode.execute(state);
    }
}
