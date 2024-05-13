using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BehaviorDropMenu : VisualDropmenu
{

    public BehaviorDropMenu(VisualNodeEditorWindow Win) : base(Win)
    {
        Size = new Vector2(100, EditorGUIUtility.singleLineHeight * 8);
    }

    public override void m_DrawMainPage()
    {
        if (GUI.Button(m_GetButtonRect(), "创建行为树节点"))
        {
            Page = E_DropMenuPage.CreateNode;
        }
        //清空
        if (GUI.Button(m_GetButtonRect(), "清空节点"))
        {
           win.m_ClearAllNodes();
            m_HideDropmenu();
        }
        //3.导出
        if (GUI.Button(m_GetButtonRect(), "导出页面"))
        {
            m_HideDropmenu();
        }
    }
    public override void m_DrawEditNodePage()
    {
        if (GUI.Button(m_GetButtonRect(), "连接到父节点"))
        {
            //开始连接
            (win as BehaviorTreeNodeEditorWindow).b_IsSonToFather = true;
            win.On_StartLinkedNode();
            m_HideDropmenu();
        }
        if (GUI.Button(m_GetButtonRect(), "添加子节点"))
        {
            //开始连接
            (win as BehaviorTreeNodeEditorWindow).b_IsSonToFather = false;
            win.On_StartLinkedNode();
            m_HideDropmenu();
        }
        if (GUI.Button(m_GetButtonRect(), "设置为起始节点"))
        {
            (win as BehaviorTreeNodeEditorWindow).m_SettingAsStartNode(win.SelectedNode as VisualBehaviorTreeNode);
            m_HideDropmenu();
        }
        if (GUI.Button(m_GetButtonRect(), "删除该节点"))
        {
            m_RemoveNode();
            m_HideDropmenu();
        }
    }
    /// <summary>
    /// 移除选中节点方法
    /// </summary>
    public void m_RemoveNode()
    {
        VisualBehaviorTreeNode SelectedNode = win.SelectedNode as VisualBehaviorTreeNode;

        //切断联系后删除
        SelectedNode.m_DisConnectedFather();
        SelectedNode.m_DisConnectedSons();
        //彻底移除
        win.dic_Nodes.Remove(SelectedNode.ID);
    }

    public override void m_DrawCreatePage()
    {
        if (GUI.Button(m_GetButtonRect(), "选择节点"))
        {
            //先关窗口
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.SelectTreeNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "顺序节点"))
        {
            //先关窗口
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.SequeneTreeNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "并行节点"))
        {
            //先关窗口
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.ParallelTreeNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "动作节点"))
        {
            //先关窗口
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.ActionTreeNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "条件节点"))
        {
            //先关窗口
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.ConditionNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "反转装饰节点"))
        {
            //先关窗口
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.ReverseDecoratorNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "延迟执行节点"))
        {
            //先关窗口
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.DelayDecoratorNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "重复节点"))
        {
            //先关窗口
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.RepeatDecoratorNode);
        }
    }





}
