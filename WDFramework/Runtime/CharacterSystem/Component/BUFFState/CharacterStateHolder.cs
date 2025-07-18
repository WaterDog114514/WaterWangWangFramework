using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// BUFF×´Ì¬³ÖÓÐÕß
/// </summary>
public class BUFFStateHolder : CharacterComponent
{
    protected Dictionary<string,BUFFState> dic_BuffState;

    public BUFFStateHolder(BaseCharacter baseCharacter) : base(baseCharacter)
    {
    }

    public override void IntializeComponent()
    {
    }

    public override void UpdateComponent()
    {
    }

    public void AddBuffState(E_CharacterAttributeType type, BUFFState state)
    {
       
    }
    public void UpdateBUFFState()
    {

    }
    public void RemoveBuffState()
    {

    }
}
