using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.Events;

public abstract class VisualBaseNode
{
    public string Name;
    protected GUIStyle FontStyle;
    protected GUIStyle TitleStyle;
    /// <summary>
    /// 节点唯一ID
    /// </summary>
    public int ID = -1;
    public Vector2 Pos_Self;
    /// <summary>
    /// 节点绘画的位置
    /// </summary>
    public Vector2 Pos_Draw;
    /// <summary>
    /// 节点窗口大小
    /// </summary>
    public Vector2 Size
    {
        get { return _size; }
        set { _size = value; }
    }
    protected Vector2 _size = Vector2.zero;
    [NonSerialized]
    public Dictionary<int, VisualBaseNode> dic_Nodes = new Dictionary<int, VisualBaseNode>();

    /// <summary>
    /// 节点绘制自身方法
    /// </summary>
    public virtual void t_m_DrawSelf()
    {
        //绘制背景
        m_DrawBackground();
        //绘制标题
        m_DrawTitle();
        //绘制所有控件
        DrawControlData();

    }
    /// <summary>
    /// 初始化绘制样式
    /// </summary>
    public virtual void m_IntiDrawStyle()
    {
        //设置控件
        FontStyle = new GUIStyle();
        FontStyle.fontSize = 13;
        FontStyle.normal.textColor = Color.black;
        FontStyle.fontStyle = UnityEngine.FontStyle.Bold;
        //设置标题
        TitleStyle = new GUIStyle();
        TitleStyle.normal.textColor = new Color(0.93F, 0.86F, 0.501F);
        TitleStyle.alignment = TextAnchor.MiddleCenter;
        TitleStyle.fontSize = 18;
    }
    /// <summary>
    /// 绘画前设置更新逻辑
    /// </summary>
    public virtual void t_m_DrawBeforeUpdate(Vector2 Offset)
    {

        //设置绘画位置
        Pos_Draw = Offset + Pos_Self;
        //被选中画绿色框框
        if (b_IsSelected)
        {
            Handles.color = Color.green;
            Handles.DrawWireCube(Pos_Draw + Size / 2, Size + Vector2.one * 2);
        }
        //重置索引
        IndexControl = 0;
        IndexLabel = 0;
    }
    /// <summary>
    /// 绘制背景
    /// </summary>
    public virtual void m_DrawBackground()
    {
        EditorGUI.DrawRect(new Rect(Pos_Draw, Size), new Color(0.325F, 0.525F, 0.111F, 0.59f));
    }
    /// <summary>
    /// 创建节点
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="size"></param>
    public VisualBaseNode()
    {
        m_IntiDrawStyle();
        m_IntiSize();
    }
    /// <summary>
    /// 这个节点是否被选中了？
    /// </summary>
    public bool b_IsSelected;
    /// <summary>
    /// 绘制标题方法
    /// </summary>
    public virtual void m_DrawTitle()
    {
        //先画标题背景色
        EditorGUI.DrawRect(m_getTitleRect(), new Color(0.56F, 0.43F, 0.81F, 0.7f));
        Name = EditorGUI.TextField(m_getTitleRect(), Name, TitleStyle);
    }
    /// <summary>
    /// 绘制控件信息
    /// </summary>
    public virtual void DrawControlData()
    {

    }
    /// <summary>
    /// 初始化窗体大小
    /// </summary>
    public virtual void m_IntiSize()
    {
        Size = new Vector2(100, 50);
    }
    protected int IndexLabel = 0;
    protected int IndexControl = 0;
    protected float singleLineHeight = EditorGUIUtility.singleLineHeight * 1.45f;
    /// <summary>
    /// 取得控件的标签宽度
    /// </summary>
    /// <returns></returns>
    public Rect m_getLabelRect(float width = 1.0f / 3)
    {
        return new Rect(new Vector2(Pos_Draw.x, Pos_Draw.y + (++IndexLabel) * singleLineHeight), new Vector2(Size.x * width, singleLineHeight));
    }
    /// <summary>
    /// 取得控件本身的宽度
    /// </summary>
    /// <param name="b_IsLine">是否占一行</param>
    /// <returns></returns>
    public Rect m_getControlDrawRect(bool b_IsLine)
    {
        ++IndexLabel;
        return new Rect(new Vector2(Pos_Draw.x, Pos_Draw.y + (++IndexControl) * singleLineHeight), new Vector2(Size.x, singleLineHeight));
    }
    public Rect m_getControlDrawRect(float width = 2.0f / 3)
    {

        return new Rect(new Vector2(Pos_Draw.x + (Size.x - Size.x * width), Pos_Draw.y + (++IndexControl) * singleLineHeight), new Vector2(Size.x * width, singleLineHeight));
    }
    public Rect m_getTitleRect()
    {
        return new Rect(new Vector2(Pos_Draw.x , Pos_Draw.y - 10), new Vector2(Size.x, singleLineHeight * 1.5F));
    }
    public void m_getLineRect()
    {
        IndexLabel++;
        IndexControl++;

    }
}
