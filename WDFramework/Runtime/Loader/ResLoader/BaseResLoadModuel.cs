using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Դ��Ϣ��
/// </summary>
public abstract class BaseResLoadModuel
{
    public BaseResLoadModuel(Dictionary<string, Res> dic_LoadedRes)
    {
        this.dic_LoadedRes = dic_LoadedRes;
        //��������ʼ��
        initializedLoader();
    }
    protected Dictionary<string, Res> dic_LoadedRes { get; private set; }
    protected abstract void initializedLoader();
}
