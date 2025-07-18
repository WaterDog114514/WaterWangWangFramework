using UnityEngine;
/// <summary>
/// ��ɫ��λ������  ÿ�����ֻ��Ϊ��λ�����
/// </summary>
public abstract class CharacterComponent
{
    /// <summary>
    /// ������ģ����н����ĺ������⣬�۲��߱���
    /// </summary>
    protected EventManager<E_CharacterEvent> eventManager;
    protected BaseCharacter SelfCharacter;
    /// <summary>
    /// Ϊ��ɫ������ʼ��
    /// </summary>
    public abstract void IntializeComponent();
    /// <summary>
    /// �Ҽ�ÿ֡�����߼�����
    /// </summary>
    public abstract void UpdateComponent();

    public CharacterComponent(BaseCharacter baseCharacter)
    {
        eventManager = baseCharacter.eventManager;
        SelfCharacter = baseCharacter;
        IntializeComponent() ;
    }
}
