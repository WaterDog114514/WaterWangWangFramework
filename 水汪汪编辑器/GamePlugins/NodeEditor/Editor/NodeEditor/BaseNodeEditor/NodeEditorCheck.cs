using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class NodeEditorCheck
{

    public VisualNodeEditorWindow win;
    public Rect Size => win.Size;
    public VisualDropmenu dropmenu => win.dropmenu;
    public NodeEditorCheck(VisualNodeEditorWindow win)
    {
        this.win = win;
    }

    private bool b_isEditorState;
    public void m_CheckUpdate()
    {
        //鼠标滚轮检测
        //  m_MouseScrollCheck();
        //鼠标点击检测
        m_mouseLeftClickColliderCheck();
        //视口拖拽检测
        m_DragViewCheck();
        //组件拖拽检测到
        m_DragSingleNodeCheck();
        //右键键下拉菜单检测
        m_DropMenuCheck();
    }

    /// <summary>
    /// 是否处于编辑模式下，节点编辑模式是防止右键节点 点不了
    /// </summary>
    public bool b_IsInEditing;
    /// <summary>
    /// 检测是否有拖拽，让其不按出菜单
    /// </summary>
    private bool b_mousedeltaNoOpen;
    /// <summary>
    /// 临时鼠标位置
    /// </summary>
    public Vector2 TempCurrentMousePos;
    /// <summary>
    /// 判断鼠标位置是否在窗口范围内
    /// </summary>
    public bool b_IsInWindowSize =>
            TempCurrentMousePos.x >= Size.x &&
            TempCurrentMousePos.x <= (Size.x + Size.width) &&
            TempCurrentMousePos.y >= Size.y &&
            TempCurrentMousePos.y <= (Size.y + Size.height);
    /// <summary>
    /// 鼠标在弹窗窗口范围内吗
    /// </summary>
    public bool b_IsInDropMenuSize =>
                 TempCurrentMousePos.x >= dropmenu.ShowPos.x + Size.x &&
                TempCurrentMousePos.x <= (dropmenu.ShowPos.x + Size.x + dropmenu.Size.x) &&
                TempCurrentMousePos.y >= dropmenu.ShowPos.y + Size.y &&
                TempCurrentMousePos.y <= (dropmenu.ShowPos.y + Size.y + dropmenu.Size.y);
    /// <summary>
    /// 视口拖动检测
    /// </summary>
    public bool b_IsDragChecking;
    /// <summary>
    /// 视口拖动检测
    /// </summary>
    public void m_DragViewCheck()
    {

        //水狗无敌神风代码：只有在Size内才检测拖动

        //按住不松开下
        if (b_IsDragChecking)
        {
            win.Pos_CurrentView -= Event.current.delta * 0.1F;
            if (Event.current.button != 1) b_IsDragChecking = false;
            //减少负担,直接return
            return;
        }
        TempCurrentMousePos = Event.current.mousePosition;
        //在编辑器视口内才算
        if (Event.current.button == 1 && b_IsInWindowSize) b_IsDragChecking = true;


    }

    /// <summary>
    /// 下拉菜单检测
    /// </summary>
    public void m_DropMenuCheck()
    {
        //在输入模式先取消输入
        if (b_isEditorState && Event.current.button == 1)
        {
            b_isEditorState = false;
            return;
        }
        //按任意键取消弹出
        if (Event.current.type == EventType.KeyUp || Event.current.type == EventType.MouseUp)
        {
            if (!b_IsInDropMenuSize)
            {
                dropmenu.m_HideDropmenu();
            }

        }
        //只要有拖拽，就不tm的按出菜单了
        if (Event.current.type == EventType.MouseDrag)
        {
            b_mousedeltaNoOpen = false;
        }
        //按下右键弹出菜单
        if (Event.current.type == EventType.MouseUp)
        {


            //如果有选中节点
            if (Event.current.button == 1 && Event.current.delta == Vector2.zero && b_mousedeltaNoOpen)
            {      //在输入模式先取消输入
                if (b_isEditorState)
                {
                    b_isEditorState = false;
                    return;
                }

                //进入节点编辑模式
                if (b_m_IsInControlSize())
                {
                    dropmenu.Page = VisualDropmenu.E_DropMenuPage.EditNode;
                    //节点编辑模式是防止右键节点点不了
                    b_IsInEditing = true;
                }
                dropmenu.m_ShowDropmenu();
            }
            b_mousedeltaNoOpen = true;
        }







    }

    private double lastClickTime;
    /// <summary>
    /// 鼠标左键碰撞检测
    /// </summary>
    public void m_mouseLeftClickColliderCheck()
    {

        //双击检测
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown)
        {
            //不是双击不能输入
            if (EditorApplication.timeSinceStartup - lastClickTime < 0.3)
            {
                b_isEditorState = true;
            }
            lastClickTime = EditorApplication.timeSinceStartup;
        }
        if (b_isEditorState == false&& win.SelectedNode != RootNode_VisualBehaviorTreeNode.instance)
            GUIUtility.keyboardControl = 0;


        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            //在连接模式下 连接的逻辑
            //使用逻辑是否断路的办法检测
            if (b_m_IsInControlSize())
            {
                if (win.b_IsLinkingNode)
                    win.On_EndLinkNode(win.SelectedNode);
            }
            else
            {
                //失去焦点
                b_isEditorState = false;
            }
        }

    }

    public void m_MouseScrollCheck()
    {
        //if (Event.current.isScrollWheel && Event.current.delta != Vector2.zero)
        //{
        //    win. -= Event.current.delta.y * 0.05F;
        //    Pos_CurrentView -= Vector2.one * 7;
        //    if (win.SelectedNode < 0.35f)
        //    {
        //        win.SelectedNode = 0.35f;
        //        Pos_CurrentView += Vector2.one * 7;
        //    }
        //    else if (win.SelectedNode > 1.3f)
        //    {
        //        Pos_CurrentView += Vector2.one * 7;
        //        win.SelectedNode = 1.3f;
        //    }
        //}

    }
    /// <summary>
    /// 拖拽单个节点
    /// </summary>
    public void m_DragSingleNodeCheck()
    {
        //当在输入操作时候，不能移动
        if (GUIUtility.keyboardControl != 0) return;
        //拖拽组件条件
        //1.当选中物体不为空时
        //2.按住鼠标左键
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && win.SelectedNode != null)
        {
            win.SelectedNode.Pos_Self = m_getNodeCenter(win.SelectedNode);
        }
    }
    /// <summary>
    /// 判断鼠标是否在某个节点内，在的话就返回true然后设置SelectNode
    /// </summary>
    /// <returns>在不在嘛</returns>
    public bool b_m_IsInControlSize()
    {
        Vector2 fixedMousePos = TempCurrentMousePos - Size.position;
        //失活所有被选中节点，清除所有被选中节点
        //只有不在被编辑情况下才失活
        if (!b_IsInEditing)
        {
            if (win.SelectedNode != null)
            {
                win.SelectedNode.b_IsSelected = false;
                win.SelectedNode = null;
            }
        }
        //选中被鼠标点到的节点
        foreach (VisualBaseNode childNode in win.dic_Nodes.Values)
        {
            if (
                fixedMousePos.x >= childNode.Pos_Draw.x &&
                fixedMousePos.x <= (childNode.Pos_Draw.x + childNode.Size.x) &&
                fixedMousePos.y >= childNode.Pos_Draw.y &&
                fixedMousePos.y <= (childNode.Pos_Draw.y + childNode.Size.y))
            {
                win.SelectedNode = childNode;
                win.SelectedNode.b_IsSelected = true;
                return true;
            }
        }

        return false;
        // Debug.Log("鼠标位置："+fixedMousePos+" 节点位置" + node.Pos_Draw);

    }
    public Vector2 m_getNodeCenter(VisualBaseNode node)
    {
        return Event.current.mousePosition - Size.position - win.Pos_CurrentView - Vector2.right * (node.Size.x / 2 + Size.x) - Vector2.up * node.Size.y / 2;
    }
}
