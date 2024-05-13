using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 控制可视化节点
/// </summary>
[System.Serializable]
public class ControlNode_VisualBehaviorTreeNode : VisualBehaviorTreeNode
{
    public List<int> ChildsId;


    public ControlNode_VisualBehaviorTreeNode(string Description, E_BehaviorType type) : base(Description, type)
    {
        ChildsId = new List<int>();
    }
    public override void DrawControlData()
    {
        base.DrawControlData();
        GUI.Label(m_getLabelRect(2.0F / 2), "子节点:" + ChildsId.Count,FontStyle);
        GUI.Label(m_getControlDrawRect(1.0F / 3), "优先级:",FontStyle);
        for (int i = 0; i < ChildsId.Count; i++)
        {
            GUI.Label(m_getLabelRect(2.0F / 3), $"{i}." + dic_Nodes[ChildsId[i]].Name, FontStyle);
            if (GUI.Button(m_getControlDrawRect(1.0f / 3), "上移"))
            {
                m_ListUpMove(i);
            }
        }

    }

    private void m_ListUpMove(int index)
    {

        Debug.Log(2);
        //交换位置算法
        if (index == 0) return;
        ChildsId[index] = ChildsId[index - 1] - ChildsId[index];
        ChildsId[index - 1] = ChildsId[index - 1] - ChildsId[index];
        ChildsId[index] = ChildsId[index - 1] + ChildsId[index];
    }
    public override void m_IntiSize()
    {
        Size = new Vector2(120, 90);
    }
    public override void m_DrawBackground()
    {
        //蓝褐色
        EditorGUI.DrawRect(new Rect(Pos_Draw+Vector2.up * singleLineHeight * 0.25f ,Size ), new Color(0.282F, 0.466F, 0.961F, 0.79f));
    }
    public override void t_m_DrawBeforeUpdate(Vector2 Offset)
    {
        base.t_m_DrawBeforeUpdate(Offset);
        Size = new Vector2(100,90)+ Vector2.up * 27 * ChildsId.Count;
    }
}
