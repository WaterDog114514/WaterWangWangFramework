using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;


public abstract class BaseOperatorProcessor
{

}

/// <summary>
/// ²Ù×÷²ã»ùÀà
/// </summary>
public abstract class PlayerOperatorProcessor<T> : BaseOperatorProcessor  where T : PlayerOperatorProcessor<T>
{
    public static T Instance { get;private set;}
    public PlayerOperatorProcessor()
    {
        Instance = this as T;
    }
}

