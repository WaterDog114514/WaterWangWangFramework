using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[Serializable]
public abstract class BaseSettingData
{
    public BaseSettingData()
    {
        IntiValue();
    }
    /// <summary>
    /// 第一次初始化方法
    /// </summary>
    public abstract void IntiValue();
}