using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ɫ��Ϊ������ ��Ϊ������
/// </summary>
public class CharacterActionDriver : CharacterComponent
{
    protected Dictionary<E_ActionName, BaseAction> dic_actions = new Dictionary<E_ActionName, BaseAction>();

    public CharacterActionDriver(BaseCharacter baseCharacter) : base(baseCharacter)
    {

    }

    public override void IntializeComponent()
    {
        //�������

    }
    protected BaseAction CurrentAction;
    public override void UpdateComponent()
    {
        if (CurrentAction != null)
            CurrentAction.ActionUpdate();
    }
    public void StartNewAction(E_ActionName actionName)
    {
        BaseAction action = dic_actions[actionName];
        //ֻ�в����ظ���ǰ��Ϊ��������������������л���Ϊ
        if (CurrentAction != action && action.EvaluateEnterCondition())
        {
            Debug.Log($"�л�״̬{actionName}");
            CurrentAction?.ActionEnd();
            CurrentAction = action;
            action.ActionStart();
        }
    }

    protected void AddAction(E_ActionName name, BaseAction action)
    {
        if (!dic_actions.ContainsKey(name))
        {
            dic_actions.Add(name, action);
        }
    }
    /// <summary>
    /// ������ǰ״̬��תΪIdle״̬
    /// </summary>
    public void EndCurrentStateToIdle()
    {
        StartNewAction(E_ActionName.Idle);
    }
}
