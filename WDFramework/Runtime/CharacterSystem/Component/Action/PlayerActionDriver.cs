using System.Collections.Generic;
using UnityEngine;
public class PlayerActionDriver : CharacterActionDriver
{
    public PlayerActionDriver(BaseCharacter baseCharacter) : base(baseCharacter)
    {
        //dic_actions.Add(E_ActionName.Attack, new AttackAction(baseCharacter));
        //dic_actions.Add(E_ActionName.Move, new PlayerMoveAction(baseCharacter));
        //dic_actions.Add(E_ActionName.Idle, new IdleAction(baseCharacter));
        //EventCenterSystem.Instance.AddEventListener<E_InputEvent,float>(E_InputEvent.MoveHorizontal, (value) =>
        //{
        //    StartNewAction( E_ActionName.Move);
        //});

        //EventCenterSystem.Instance.AddEventListener<E_InputEvent, float>(E_InputEvent.MoveVertical, (value) =>
        //{
        //    StartNewAction(E_ActionName.Move);
        //});

        //EventCenterSystem.Instance.AddEventListener(E_InputEvent.Attack, () =>
        //{
        //    StartNewAction(E_ActionName.Attack);
        //});

    }



    //注册行为
    public override void IntializeComponent()
    {
        base.IntializeComponent();
    }
    //玩家角色移动逻辑
}
