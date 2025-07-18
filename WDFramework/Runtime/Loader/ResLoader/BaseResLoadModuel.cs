using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源信息类
/// </summary>
public abstract class BaseResLoadModuel
{
    public BaseResLoadModuel(Dictionary<string, Res> dic_LoadedRes)
    {
        this.dic_LoadedRes = dic_LoadedRes;
        //加载器初始化
        initializedLoader();
    }
    protected Dictionary<string, Res> dic_LoadedRes { get; private set; }
    protected abstract void initializedLoader();
}
