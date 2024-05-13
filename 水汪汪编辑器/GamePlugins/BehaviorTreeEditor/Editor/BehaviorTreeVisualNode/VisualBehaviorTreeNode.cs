using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 一切可视化行为树节点的老爹
/// </summary>
[System.Serializable]
public class VisualBehaviorTreeNode : VisualBaseNode
{

    /// <summary>
    /// 节点描述
    /// </summary>
    public string Description;
    public BaseTreeNode BehaviorNode;
    public E_NodeState TempState = E_NodeState.Faild;
    public E_BehaviorType NodeType;
    public int FatherID = -1;
    public string[] Parameter;
    /// <summary>
    /// 正在处于监察模式吗
    /// </summary>
    public bool b_IsCheckingMode;
    /// <summary>
    /// 行为树节点最基本的初始化
    /// </summary>
    /// <param name="Description"></param>
    /// <param name="type"></param>
    public VisualBehaviorTreeNode(string Description, E_BehaviorType type)
    {
        this.Description = Description;
        NodeType = type;

    }
    public override void DrawControlData()
    {
        base.DrawControlData();
        //绘制描述
        Description = EditorGUI.TextField(m_getControlDrawRect(true), Description == null ? "节点描述...." : Description);

    }
    public override void t_m_DrawBeforeUpdate(Vector2 Offset)
    {
        base.t_m_DrawBeforeUpdate(Offset);
        //先绘制连线先嘛
        m_DrawLink();
    }
    /// <summary>
    /// 绘制连接关系 由自己到爹
    /// </summary>
    public void m_DrawLink()
    {
        //有老爸才能画啊
        if (FatherID == -1) return;
        if (dic_Nodes[FatherID] is RootNode_VisualBehaviorTreeNode)
            Handles.color = Color.red;
        else
            Handles.color = Color.cyan;
        //中点坐标1 起点上方
        Vector2 center1 = Pos_Draw + Vector2.right * Size / 2 + Vector2.up *
            (dic_Nodes[FatherID].Pos_Draw.y + dic_Nodes[FatherID].Size.y - Pos_Draw.y) / 2;
        //中点坐标2 终点下方
        Vector2 center2 = dic_Nodes[FatherID].Pos_Draw + Vector2.up * dic_Nodes[FatherID].Size.y + Vector2.right * dic_Nodes[FatherID].Size / 2
            - Vector2.up * (dic_Nodes[FatherID].Pos_Draw.y + dic_Nodes[FatherID].Size.y - Pos_Draw.y) / 2;
        //分别对应自身中点  爹到自己的中点  爹的中点  
        Handles.DrawLine(Pos_Draw + Vector2.right * Size / 2, center1);
        Handles.DrawLine(
              center1
            , center2);
        Handles.DrawLine(center2,
              dic_Nodes[FatherID].Pos_Draw + Vector2.up * dic_Nodes[FatherID].Size.y + Vector2.right * dic_Nodes[FatherID].Size.x / 2);

        Handles.DrawWireCube(dic_Nodes[FatherID].Pos_Draw + Vector2.up * dic_Nodes[FatherID].Size.y + Vector2.right * dic_Nodes[FatherID].Size.x / 2, Vector2.one * 20);
    }
    public override void m_IntiSize()
    {
        Size = new Vector2(100, 60);
    }

    public override void t_m_DrawSelf()
    {

        //监测模式下绘制
        if (b_IsCheckingMode)
        {
            //更新状态
            TempState = BehaviorNode.ChildState;
            m_DrawDynimicCheckState();
        }
        //绘制背景
        m_DrawBackground();
        //绘制标题
        m_DrawTitle();
        //根据监察模式是否变灰色
        EditorGUI.BeginDisabledGroup(b_IsCheckingMode);
        //绘制所有控件
        DrawControlData();
        EditorGUI.EndDisabledGroup();
    }
    public override void m_IntiDrawStyle()
    {
        base.m_IntiDrawStyle();
        CheckStyle = new GUIStyle();
        CheckStyle.normal.textColor = Color.red;
        CheckStyle.alignment = TextAnchor.MiddleCenter;
        CheckStyle.fontStyle = UnityEngine.FontStyle.Bold;
        CheckStyle.fontSize = 19;
    }
    private GUIStyle CheckStyle;
    public void m_DrawDynimicCheckState()
    {
        Vector2 size = new Vector2(Size.x, singleLineHeight * 1.55F);
        switch (TempState)
        {
            case E_NodeState.Succeed:
                if(NodeType == E_BehaviorType.DelayDecoratorNode)
                {
                    //延迟节点会显示剩余时间
                    CheckStyle.normal.textColor = Color.cyan;
                    GUI.Label(new Rect(Pos_Draw - Vector2.up * singleLineHeight * 1.65f, size), "延迟等待中", CheckStyle);
                    return;
                }
                CheckStyle.normal.textColor = Color.green;
                GUI.Label(new Rect(Pos_Draw - Vector2.up * singleLineHeight * 1.65f, size), "执行成功", CheckStyle);
                break;
            case E_NodeState.Faild:
                CheckStyle.normal.textColor = Color.red;
                GUI.Label(new Rect(Pos_Draw - Vector2.up * singleLineHeight * 1.65f, size), "未执行/失败", CheckStyle);
                break;
            case E_NodeState.Running:
                CheckStyle.normal.textColor = Color.yellow;
                GUI.Label(new Rect(Pos_Draw - Vector2.up * singleLineHeight * 1.65f, size), "执行中...", CheckStyle);
                break;
        }
    }


    public override void m_DrawTitle()
    {
        if (!b_IsCheckingMode)
            base.m_DrawTitle();
        else
        {
            EditorGUI.DrawRect(m_getTitleRect(), new Color(0.56F, 0.43F, 0.81F, 0.96f));
            EditorGUI.LabelField(m_getTitleRect(), Name, TitleStyle);
        }
    }
    /// <summary>
    /// 清除所有与孩子们的联系
    /// </summary>
    public void m_DisConnectedSons()
    {

        if (this is RootNode_VisualBehaviorTreeNode)
        {
            RootNode_VisualBehaviorTreeNode Node = this as RootNode_VisualBehaviorTreeNode;
            if (Node.ChildID != -1)
                (dic_Nodes[Node.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
            Node.ChildID = -1;
        }
        else if (this is DecoratorNode_VisualBehaviorTreeNode)
        {
            DecoratorNode_VisualBehaviorTreeNode Node = this as DecoratorNode_VisualBehaviorTreeNode;
            if (Node.ChildID != -1)
                (dic_Nodes[Node.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
            Node.ChildID = -1;
        }
        else if (this is ControlNode_VisualBehaviorTreeNode)
        {
            ControlNode_VisualBehaviorTreeNode Node = this as ControlNode_VisualBehaviorTreeNode;
            //先把崽子关系搞清
            if (Node.ChildsId.Count == 0) return;
            for (int i = 0; i < Node.ChildsId.Count; i++)
            {
                (dic_Nodes[Node.ChildsId[i]] as VisualBehaviorTreeNode).FatherID = -1;
            }
            Node.ChildsId.Clear();
        }
        //动作节点是不可能是父节点的啦

    }
    /// <summary>
    /// 清除自己与老爹的联系
    /// </summary>
    public void m_DisConnectedFather()
    {
        if (FatherID == -1) return;
        if (dic_Nodes[FatherID] is RootNode_VisualBehaviorTreeNode)
        {
            (dic_Nodes[FatherID] as RootNode_VisualBehaviorTreeNode).ChildID = -1;
        }
        else if (dic_Nodes[FatherID] is DecoratorNode_VisualBehaviorTreeNode)
        {
            (dic_Nodes[FatherID] as DecoratorNode_VisualBehaviorTreeNode).ChildID = -1;
        }
        else if (dic_Nodes[FatherID] is ControlNode_VisualBehaviorTreeNode)
        {
            (dic_Nodes[FatherID] as ControlNode_VisualBehaviorTreeNode).ChildsId.Remove(ID);
        }
        //动作节点是不可能是父节点的啦
        FatherID = -1;
    }
}


//public void DemoDrawSon()
//{
//    if(this is ControlNode_VisualBehaviorTreeNode)
//    {
//        for (int i = 0; i < (this as ControlNode_VisualBehaviorTreeNode).ChildsId.Count; i++)
//        {
//            EditorGUI.LabelField(m_getLabelRect(), $"孩子{i}:",FontStyle);
//            EditorGUI.LabelField(m_getControlDrawRect(), (this as ControlNode_VisualBehaviorTreeNode).ChildsId[i].ToString());
//        }
//    }
//    else if (this is DecoratorNode_VisualBehaviorTreeNode)
//    {
//        EditorGUI.LabelField(m_getLabelRect(), $"孩子:",FontStyle);
//        EditorGUI.LabelField(m_getControlDrawRect(), (this as DecoratorNode_VisualBehaviorTreeNode).ChildID.ToString());
//    }

//}