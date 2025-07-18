using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// ��ɫ��Ϊ����
/// </summary>
public abstract class BaseAction
{
    public BaseCharacter BehaviorCharacter;
    protected CharacterController controller;
    protected EventManager<E_CharacterEvent> eventManager;
    protected CharacterActionDriver driver;
    public BaseAction(BaseCharacter behavior)
    {
        BehaviorCharacter = behavior;
        controller = BehaviorCharacter.GetComponent<CharacterController>();
        eventManager = BehaviorCharacter.eventManager;
    }
    //��Ϊ��������
    public abstract void ActionStart();
    public abstract void ActionUpdate();
    public abstract void ActionEnd();
    /// <summary>
    /// �ж��Ƿ�����������Ϊ������
    /// </summary>
    /// <returns></returns>
    public abstract bool EvaluateEnterCondition();
    /// <summary>
    /// ��������״̬
    /// </summary>
    protected virtual void ExitState()
    {
      BehaviorCharacter.GetCharacterComponent<CharacterActionDriver>().EndCurrentStateToIdle();
    }
}
