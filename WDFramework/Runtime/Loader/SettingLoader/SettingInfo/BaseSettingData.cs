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
    /// ��һ�γ�ʼ������
    /// </summary>
    public abstract void IntiValue();
}