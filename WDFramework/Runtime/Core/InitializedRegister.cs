using UnityEngine;
/// <summary>
///��ʼ�������趨��
/// </summary>
public abstract class InitializedRegister
{

    public InitializedRegister()
    {
        Initialized();
    }

    protected abstract void Initialized();
}