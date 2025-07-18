using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using WDEditor;
[Serializable]
/// <summary>
/// 基类数据类
/// </summary>
public abstract class BaseWindowData
{
    /// <summary>
    /// 窗口名
    /// </summary>
    public abstract string Title { get;}
    /// <summary>
    /// 是否使用背景颜色
    /// </summary>
    public bool isUseBlackground;
    /// <summary>
    /// 当前窗口大小 记录下来，下次打开还是一样大
    /// </summary>
    public SerializableVector2 currentWindowSize;
    [HideInInspector]
    public bool isFirstCreated = true;
    // 第一次创建初始化方法 子类不能调
    public void IntiFirst()
    {
        
        //只能第一次创建时候运行
        if (!isFirstCreated) return;
        isFirstCreated = false;
        //执行第一次创建逻辑
        IntiWinSize();
        IntiFirstCreate();

    }
    /// <summary>
    /// 加载时候赋予初值，由window初始化调用
    /// </summary>
    public virtual void IntiLoad()
    {

    }
    /// <summary>
    /// 初始化设置data的值只能子类重写
    /// </summary>
    public virtual void IntiFirstCreate()
    {


    }
    //初始化窗口大小
    private void IntiWinSize()
    {
        //默认设置窗口大小
        currentWindowSize.vector2 = new Vector2(768,512);
    }
}
