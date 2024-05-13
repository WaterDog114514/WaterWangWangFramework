using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

 /// <summary>
  /// 主要用于里式替换原则 在字典中 用父类容器装载子类对象
  /// </summary>
public abstract class UIBasePanelInfo{ }

/// <summary>
/// 用于存储面板信息 和加载完成的回调函数的
/// </summary>
/// <typeparam name="T">面板的类型</typeparam>
public class PanelInfo<T> : UIBasePanelInfo where T : UIBasePanel
{
    public T panel;
    public bool isHide;
    public Res  UIRes;
    public PanelInfo()
    {
     
    }
}

