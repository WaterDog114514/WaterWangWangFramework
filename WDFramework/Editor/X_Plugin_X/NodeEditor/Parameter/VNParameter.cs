using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 可视化节点参数，仅仅存在于节点编辑器中
/// </summary>
[Serializable]
public abstract class VNParameter
{
    public abstract string DropmenuShowName { get;} 
    //用于在编辑器中显示值，也用来后期序列化传值
    public string Value;
    // 参数名，用于显示标签
    public string Name;
    /// <summary>
    /// 区分参数类型的唯一标识，在序列化时候很有用
    /// </summary>
    public abstract E_NodeParameterType ParameterType { get; }
    /// <summary>
    /// 绘制助手，用于节点参数的绘制，包括在编辑器中和Inspector编辑中
    /// </summary>
    [NonSerialized]
    public DrawIndexHelper DrawHelper = new DrawIndexHelper();
    protected abstract string InspectorParameterName { get; }
    [NonSerialized]
    public float CurrentInspectorHeight;
    /// <summary>
    /// inspector单个参数绘制高度，初始1.5f，加一个控件就是1.25F
    /// </summary>
    protected virtual float InspectorParameterHeight { get => 1.5F; }

    /// <summary>
    /// 当前在编辑器中的缩放参数
    /// </summary>
    protected float currentScaleFactor;
    //删除方法的委托
    [NonSerialized]
    public UnityAction deleteParameter;
    //是否折叠
    private bool IsFolded;
    // 绘制在Inspector里，供设置预设节点
    public void Draw_Inspctor(Rect rect)
    {
        if (DrawHelper == null) DrawHelper = new DrawIndexHelper();

        if (IsFolded)
            CurrentInspectorHeight = 1.25f * EditorGUIUtility.singleLineHeight;
        else
            CurrentInspectorHeight = 1.25f * EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight * InspectorParameterHeight;
        //计算参数区域大小
        Rect drawRect = new Rect(rect.position, new Vector2(rect.width, CurrentInspectorHeight));
        DrawHelper.Update(drawRect);
        //绘制每个背景
        EditorGUI.DrawRect(drawRect, new Color(255, 255, 255, 0.5f));


        //绘制折叠
        DrawHelper.BeginHorizontalLayout(2);
        IsFolded = EditorGUI.Foldout(DrawHelper.GetNextSingleRect(), IsFolded, $"({ParameterType})" + Name);
        if (GUI.Button(DrawHelper.GetNextSingleRect(), "删除")) deleteParameter?.Invoke();
        DrawHelper.EndHorizontalLayout();

        //绘制折叠的内容
        if (IsFolded) return;
        //标题
        DrawHelper.BeginHorizontalLayout(2, 1.5F);
        GUI.Label(DrawHelper.GetNextSingleRect(), InspectorParameterName, TextStyleHelper.Custom(14, Color.cyan));
        Name = EditorGUI.TextField(DrawHelper.GetNextSingleRect(), Name);
        DrawHelper.EndHorizontalLayout();
        //绘制额外内容
        DrawInspectorExtra();
    }
    public virtual void DrawInspectorExtra()
    {

    }
    // 绘制在节点编辑器里
    public virtual void Draw_NodeEditor(DrawIndexHelper helper, float ScaleFactor)
    {
        currentScaleFactor = ScaleFactor;
        //开始水平布局
        helper.BeginHorizontalLayout(2, 1.5F * currentScaleFactor);
        //绘制控件名
        GUITextScaler.DrawScaledLabel(helper.GetNextSingleRect(), Name);
        //绘制真正控件
        Draw_NodeEditorControl(helper);
        //结束横向布局
        helper.EndHorizontalLayout();
    }
    /// <summary>
    /// 真正在编辑器绘制控件的办法
    /// </summary>
    /// <param name="helper"></param>
    public abstract void Draw_NodeEditorControl(DrawIndexHelper helper);

}

