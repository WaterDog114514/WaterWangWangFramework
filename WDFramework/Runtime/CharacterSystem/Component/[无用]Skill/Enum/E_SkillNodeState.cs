using UnityEngine;
/// <summary>
/// ���ܽڵ��������ݣ����ڽڵ�֮�䴫��
/// </summary>
public class SkillRunningData : DataObj
{
    /// <summary>
    /// ����ʩ����
    /// </summary>
    public GameObj SkillCaster;
    /// <summary>
    /// ���弼������ִ��״̬
    /// </summary>
    public E_SkillState SkillState;
    /// <summary>
    /// �Ƿ����ڼ�ʱ
    /// </summary>
    public bool isTiming = false;
}

public enum E_SkillState
{
    Finished,
    Running
}