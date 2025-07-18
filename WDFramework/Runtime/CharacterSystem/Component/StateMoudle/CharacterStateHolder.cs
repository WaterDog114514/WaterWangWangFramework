using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色状态监测者
/// </summary>
public class CharacterStateHolder : CharacterComponent
{
    public Vector3 Position;
    /// <summary>
    /// 角色拥有的状态列表，如浮空，隐形，加速等等状态
    /// </summary>
    protected HashSet<E_CharacterStateName> CurrentStates = new HashSet<E_CharacterStateName>();

    public CharacterStateHolder(BaseCharacter baseCharacter) : base(baseCharacter)
    {
    }

    public override void IntializeComponent()
    {
        //注册状态事件
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
    /// 检测角色单位是否处于某动作状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private bool ContainState(E_CharacterStateName state) => CurrentStates.Contains(state);



}

