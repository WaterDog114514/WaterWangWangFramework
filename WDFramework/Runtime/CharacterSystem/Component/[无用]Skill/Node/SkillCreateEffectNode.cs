using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ����������Ч��Ŀ��ڵ�
/// </summary>
public class SkillCreateEffectNode : SingleSkillNode
{
    /// <summary>
    /// ������Ч·��
    /// </summary>
    public GameObj EffectObj;
    //���Ӷ���
    public UnityAction<GameObj> CreateEffectAction;

    public override void execute(SkillRunningData data)
    {
        if (!IsHaveChildNode(data)) return;
        //�߼�
    }
}
