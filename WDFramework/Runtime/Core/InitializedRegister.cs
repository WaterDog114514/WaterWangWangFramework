using UnityEngine;
/// <summary>
///初始化核心设定类
/// </summary>
public abstract class InitializedRegister
{

    public InitializedRegister()
    {
        Initialized();
    }

    protected abstract void Initialized();
}