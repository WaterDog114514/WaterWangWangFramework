using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DecoratorNode_VisualBehaviorTreeNode : VisualBehaviorTreeNode
{
    public int ChildID = -1;

    public DecoratorNode_VisualBehaviorTreeNode(string Description, E_BehaviorType type) : base(Description, type)
    {
        switch (NodeType)
        {
            case E_BehaviorType.DelayDecoratorNode:
            case E_BehaviorType.RepeatDecoratorNode:
                Parameter = new string[1];
                break;
        }

    }
    public override void m_IntiSize()
    {
        Size = new Vector2(120,90);
    }
    public override void DrawControlData()
    {
        base.DrawControlData();
        switch (NodeType)
        {
            case E_BehaviorType.DelayDecoratorNode:
                GUI.Label(m_getLabelRect(2.0f/3), "延迟时间:",FontStyle);
                Parameter[0] = EditorGUI.TextField(m_getControlDrawRect(1.0f/3), Parameter[0]);
                break;
            case E_BehaviorType.RepeatDecoratorNode:
                GUI.Label(m_getLabelRect(2.0f/3), "重复次数:", FontStyle);
                Parameter[0] = EditorGUI.TextField(m_getControlDrawRect(1.0f/3), Parameter[0]);
                break;
        }
    }
    public override void m_DrawBackground()
    {
        //粉色
        EditorGUI.DrawRect(new Rect(Pos_Draw, Size), new Color(0.901F, 0.533F, 0.896F, 0.76f));
    }
}