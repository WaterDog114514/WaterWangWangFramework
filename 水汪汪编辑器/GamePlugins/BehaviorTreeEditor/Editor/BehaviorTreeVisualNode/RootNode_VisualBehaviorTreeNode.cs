using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 开始根节点
/// </summary>
[System.Serializable]
public class RootNode_VisualBehaviorTreeNode : VisualBehaviorTreeNode
{

    public int ChildID = -1;
    /// <summary>
    /// 根节点只能有一个，不如设置为静态
    /// </summary>
    public static RootNode_VisualBehaviorTreeNode instance = null;
    public GameObject BehaviorObj
    {
        //刷新数据
        set
        {
            //上一次就润
            if (_obj == value) return;
            _obj = value;
            //空就不操作了哦
            if (_obj == null) return;
            Parameter[0] = BehaviorObj.GetInstanceID().ToString();
            RootMethodReflection.Instance.RefreshRootNode();
        }
        get => _obj;
    }
    private GameObject _obj;
    public override void m_IntiSize()
    {
        Size = new Vector2(100, 90);
    }
    public RootNode_VisualBehaviorTreeNode(string Description, E_BehaviorType type) : base(Description, type)
    {
        instance = this;
        Parameter = new string[1] { "-1" };
    }
    public override void m_DrawTitle()
    {
        GUI.Label(m_getTitleRect(), Name, TitleStyle);
    }
    public override void m_IntiDrawStyle()
    {
        base.m_IntiDrawStyle();
        TitleStyle.normal.textColor = Color.red;
    }
    /// <summary>
    /// 第一个开始的子节点
    /// </summary>
    public override void DrawControlData()
    {
        //绘制预制体选择框
        GUI.Label(m_getControlDrawRect(true), "AI预制体：", FontStyle);
        BehaviorObj = (GameObject)EditorGUI.ObjectField(m_getControlDrawRect(true), BehaviorObj, typeof(GameObject), true);
    }

    public override void m_DrawBackground()
    {
        //肉色
        EditorGUI.DrawRect(new Rect(Pos_Draw , Size), new Color(1F, 0.85F, 0.72F, 0.69f));
    }
}
