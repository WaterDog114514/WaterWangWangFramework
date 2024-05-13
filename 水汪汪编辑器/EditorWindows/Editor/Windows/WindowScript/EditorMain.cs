using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 编辑器核心逻辑继承者类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EditorMain
{
    /// <summary>
    /// 核心关联的窗口
    /// </summary>
    public BaseWindow SelfWindow;
}

