using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 水汪汪编辑窗口基类  仅展示面板 里面毫无数据可言 有数据的是EditorMain有关的
/// </summary>
public abstract class BaseWindow : EditorWindow
{

    #region 窗口核心相关
    /// <summary>
    /// 子窗口类型
    /// </summary>
    public Type WinType;
    /// <summary>
    /// 核心类型
    /// </summary>
    public Type MainType;
    [SerializeField]

    /// <summary>
    /// 每个编辑器窗口的核心 
    /// </summary>
    public EditorMain editorMain;
    /// <summary>
    /// 取得属性 需要自己as后再用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public EditorMain p_GetMainValue()
    {
        if (editorMain == null) { 
            editorMain = Activator.CreateInstance(MainType) as EditorMain;
            editorMain.SelfWindow = this;
        }
        return editorMain;
    }
    #endregion
    #region 窗口美化 显示相关

    /// <summary>
    /// 窗口名
    /// </summary>
    protected string Title;
    /// <summary>
    /// 图标
    /// </summary>
    protected Texture Icon;
    /// <summary>
    /// 背景图片
    /// </summary>
    private Texture textrue;
    /// <summary>
    /// 是否使用背景颜色
    /// </summary>
    public bool isUseBlackground;
    /// <summary>
    /// 当前窗口大小
    /// </summary>
    private Vector2 currentWindowSize;
    /// <summary>
    /// 获取窗口宽
    /// </summary>
    [SerializeField]
    public float WindowWidth => position.width;
    /// <summary>
    /// 获取窗口高
    /// </summary>
    [SerializeField]
    public float WindowHeight => position.height;
    private Vector2 _originSize = Vector2.zero;
    /// <summary>
    /// 初始窗口大小 需要在OnEnable前调用
    /// </summary>
    protected Vector2 OriginWindowSize
    {
        get
        {
            if(_originSize == Vector2.zero)
                return new Vector2( EM_WinSetting.Instance.WindowsBackground.width/2,EM_WinSetting.Instance.WindowsBackground.height / 2);
            return _originSize;
        }
        set
        {
            _originSize = value;
        }
    }
    /// <summary>
    /// 初始化编辑器窗口的设置 所有子类必须调用base的方法传参即可
    /// </summary>
    /// <param name="Title">窗口标题</param>
    /// <param name="IconPath">窗口图标</param>
    public void IntiWindowsSetting(string Title, string IconPath)
    {
        this.Title = Title;
        this.Icon =WindowUtility.LoadAssetFromPath<Texture>(EM_WinSetting.Instance.SettingData.EditorIcon);
        titleContent = new GUIContent(this.Title, this.Icon);
    }


    #endregion

    #region 窗口启动执行相关方法
    public BaseWindow()
    {

    }

    /// <summary>
    /// 启动窗口执行方法
    /// </summary>
    protected virtual void OnEnable()
    {
        //预先设置好窗口类型，方便以后好操作
        WinType = GetType();
        //预先设置好窗口核心类型，方便以后好操作
        MainType = this.getMainType();

        textrue = EM_WinSetting.Instance.WindowsBackground;
        //设置当前位置和初始大小
        position = new Rect(new Vector2(position.x,position.y),OriginWindowSize);
    }



    /// <summary>
    /// 关闭窗口执行方法
    /// </summary>
    protected virtual void OnDestroy()
    {
    }

    /// <summary>
    /// 每次绘制的基本逻辑
    /// </summary>
    protected virtual void OnGUI()
    {
        //绘制背景图
        currentWindowSize = new Vector2(this.position.width, this.position.height);
        GUI.DrawTexture(new Rect(Vector2.zero, currentWindowSize), textrue, ScaleMode.StretchToFill);
        //自己绘制窗口逻辑
        m_DrawWindows();

        //绘制保存 加载 窗口信息的按钮
    }

    /// <summary>
    /// 真正的绘制自己的窗口方法  所有窗口必须自己实现
    /// </summary>
    protected abstract void m_DrawWindows();


    #endregion



}
