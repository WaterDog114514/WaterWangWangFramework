using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ״̬�����
/// </summary>
public class CharacterStateHolder : CharacterComponent
{
    public Vector3 Position;
    /// <summary>
    /// ��ɫӵ�е�״̬�б��縡�գ����Σ����ٵȵ�״̬
    /// </summary>
    protected HashSet<E_CharacterStateName> CurrentStates = new HashSet<E_CharacterStateName>();

    public CharacterStateHolder(BaseCharacter baseCharacter) : base(baseCharacter)
    {
    }

    public override void IntializeComponent()
    {
        //ע��״̬�¼�
        eventManager.AddEventListener<E_CharacterStateName>(E_CharacterEvent.EnterState, EnterState);
        eventManager.AddEventListener<E_CharacterStateName>(E_CharacterEvent.RemoveState, RemoveState);
        eventManager.AddRequestListener<E_CharacterStateName, bool>(E_CharacterEvent.ContainState, ContainState);
    }

    public override void UpdateComponent()
    {
    }
    private void EnterState(E_CharacterStateName state)
    {
        if (!CurrentStates.Contains(state))
        {
            CurrentStates.Add(state);
        }
    }
    private void RemoveState(E_CharacterStateName state)
    {
        if (CurrentStates.Contains(state))
        {
            CurrentStates.Remove(state);
        }
    }
    /// <summary>
    /// ����ɫ��λ�Ƿ���ĳ����״̬
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private bool ContainState(E_CharacterStateName state) => CurrentStates.Contains(state);



}

