using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 数据对象。不继承mono，不直接在场景中展现
/// </summary>
public class DataObj : Obj
{
    public override string PoolIdentity => GetType().Name;
}
