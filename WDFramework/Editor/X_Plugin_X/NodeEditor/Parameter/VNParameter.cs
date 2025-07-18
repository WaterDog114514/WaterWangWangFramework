using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ���ӻ��ڵ���������������ڽڵ�༭����
/// </summary>
[Serializable]
public abstract class VNParameter
{
    public abstract string DropmenuShowName { get;} 
    //�����ڱ༭������ʾֵ��Ҳ�����������л���ֵ
    public string Value;
    // ��������������ʾ��ǩ
    public string Name;
    /// <summary>
    /// ���ֲ������͵�Ψһ��ʶ�������л�ʱ�������
    /// </summary>
    public abstract E_NodeParameterType ParameterType { get; }
    /// <summary>
    /// �������֣����ڽڵ�����Ļ��ƣ������ڱ༭���к�Inspector�༭��
    /// </summary>
    [NonSerialized]
    public DrawIndexHelper DrawHelper = new DrawIndexHelper();
    protected abstract string InspectorParameterName { get; }
    [NonSerialized]
    public float CurrentInspectorHeight;
    /// <summary>
    /// inspector�����������Ƹ߶ȣ���ʼ1.5f����һ���ؼ�����1.25F
    /// </summary>
    protected virtual float InspectorParameterHeight { get => 1.5F; }

    /// <summary>
    /// ��ǰ�ڱ༭���е����Ų���
    /// </summary>
    protected float currentScaleFactor;
    //ɾ��������ί��
    [NonSerialized]
    public UnityAction deleteParameter;
    //�Ƿ��۵�
    private bool IsFolded;
    // ������Inspector�������Ԥ��ڵ�
    public void Draw_Inspctor(Rect rect)
    {
        if (DrawHelper == null) DrawHelper = new DrawIndexHelper();

        if (IsFolded)
            CurrentInspectorHeight = 1.25f * EditorGUIUtility.singleLineHeight;
        else
            CurrentInspectorHeight = 1.25f * EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight * InspectorParameterHeight;
        //������������С
        Rect drawRect = new Rect(rect.position, new Vector2(rect.width, CurrentInspectorHeight));
        DrawHelper.Update(drawRect);
        //����ÿ������
        EditorGUI.DrawRect(drawRect, new Color(255, 255, 255, 0.5f));


        //�����۵�
        DrawHelper.BeginHorizontalLayout(2);
        IsFolded = EditorGUI.Foldout(DrawHelper.GetNextSingleRect(), IsFolded, $"({ParameterType})" + Name);
        if (GUI.Button(DrawHelper.GetNextSingleRect(), "ɾ��")) deleteParameter?.Invoke();
        DrawHelper.EndHorizontalLayout();

        //�����۵�������
        if (IsFolded) return;
        //����
        DrawHelper.BeginHorizontalLayout(2, 1.5F);
        GUI.Label(DrawHelper.GetNextSingleRect(), InspectorParameterName, TextStyleHelper.Custom(14, Color.cyan));
        Name = EditorGUI.TextField(DrawHelper.GetNextSingleRect(), Name);
        DrawHelper.EndHorizontalLayout();
        //���ƶ�������
        DrawInspectorExtra();
    }
    public virtual void DrawInspectorExtra()
    {

    }
    // �����ڽڵ�༭����
    public virtual void Draw_NodeEditor(DrawIndexHelper helper, float ScaleFactor)
    {
        currentScaleFactor = ScaleFactor;
        //��ʼˮƽ����
        helper.BeginHorizontalLayout(2, 1.5F * currentScaleFactor);
        //���ƿؼ���
        GUITextScaler.DrawScaledLabel(helper.GetNextSingleRect(), Name);
        //���������ؼ�
        Draw_NodeEditorControl(helper);
        //�������򲼾�
        helper.EndHorizontalLayout();
    }
    /// <summary>
    /// �����ڱ༭�����ƿؼ��İ취
    /// </summary>
    /// <param name="helper"></param>
    public abstract void Draw_NodeEditorControl(DrawIndexHelper helper);

}

