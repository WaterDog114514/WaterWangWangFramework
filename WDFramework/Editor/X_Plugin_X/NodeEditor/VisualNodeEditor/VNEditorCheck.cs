using System;
using UnityEditor;
using UnityEngine;
using WDEditor;

public class VNEditorCheck
{
    //操作状态，根据当前状态会使用某些逻辑
    private enum E_OperatorState
    {
        //空闲啥都不干
        Idle,
        //正在编辑节点参数
        EditParameter,
        //正在连接节点
        LinkingNode,
        //正在取消连接节点
        UnLinkingNode,
    }
    private E_OperatorState OperatorState = E_OperatorState.Idle;
    private win_NodeEditor window;
    private winData_NodeEditor data => window.data;
    private VNEditorDropmenu dropmenu => window.Dropmenu;
    public VNEditorCheck(win_NodeEditor win)
    {
        this.window = win;
    }
    public VisualNode selectedNode;
    //正在执行连接的开始节点 
    private VisualNode LinkingBeginNode;
    private bool IsEnterEditNodeState;
    /// <summary>
    /// 鼠标真实位置
    /// </summary>
    private Vector2 MouseActualPosition => Event.current.mousePosition;
    public void CheckUpdate_Window()
    {
        // Debug.Log("鼠标位置：" + MouseActualPosition);
        //检测是否在视口中，不在就不能进行检测后续节点坐标的检测了
        IsEnterViewCheck = true;
        if (!CheckMouseInRightPanel)
        {
            IsEnterViewCheck = false;
            return;
        }
    }
    //是否执行节点视图的检测逻辑
    private bool IsEnterViewCheck = false;
    // 应用节点坐标系的检测
    public void CheckUpdate_NodeView()
    {
        if (!IsEnterViewCheck) return;
        // Debug.Log("鼠标位置：" + MouseActualPosition);
        //检测是否在视口中，不在就不能进行检测
        Check_MouseScroll();
        Check_SingleRightClick();
        Check_DragSingleNode();
        Check_SingleLeftClick();
    }
    /// <summary>
    /// 是否处于编辑模式下，节点编辑模式是防止右键节点 点不了
    /// </summary>
    public bool IsInEditing;
    /// <summary>
    /// 正在拖拽视口？
    /// </summary>
    public bool IsDragingView;
    /// <summary>
    /// 监测鼠标是否位于视口中
    /// </summary>
    private bool CheckMouseInRightPanel
    {
        get => data.RightPanelRect.rect.Contains(MouseActualPosition);

    }
    /// <summary>
    /// 下拉菜单检测，右键打开下拉菜单
    /// </summary>
    public void Check_SingleRightClick()
    {
        if (Event.current.button == 1 && Event.current.delta == Vector2.zero)
        {
            var ClickNode = VNColliderCheck();
            //先检查是否有其他状态，有的话先取消状态
            if(OperatorState != E_OperatorState.Idle)
            {
                ExitLinkNodeState();
                EnterIdleState();
            }

            //进行菜单逻辑检验
            if (ClickNode == null)
                //显示视图操作菜单
                window.Dropmenu.ShowViewDropmenu(MouseActualPosition);
            else
                //显示节点操作编辑菜单
                window.Dropmenu.ShowOperatorNodeMenu(MouseActualPosition, ClickNode);
        }
    }
    private double lastClickTime;
    // 鼠标单机左键碰撞检测
    public void Check_SingleLeftClick()
    {
        //选取检测
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            var ClickNode = VNColliderCheck();
            //根据不同状态执行逻辑
            //无所事事就选择
            if (OperatorState == E_OperatorState.Idle)
            {
                //如果点击不到东西
                if (ClickNode == null) 
                    selectedNode = null;
                else
                    selectedNode = ClickNode;
            }
            //正在连接的话进行连接逻辑
            else if (OperatorState == E_OperatorState.LinkingNode && LinkingBeginNode != null)
            {
                //连接到空位置就不做事情
                if(ClickNode== null)
                    return;

                //连接完成后退出状态
                window.Operator.LinkNode(LinkingBeginNode, ClickNode);
                ExitLinkNodeState();
            }
        }

    }
    //鼠标双击左键检测
    public void Check_MouseTwoLeftClick()
    {
        //双击检测
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown)
        {
            //不是双击不能输入
            if (EditorApplication.timeSinceStartup - lastClickTime < 0.3)
            {
                IsEnterEditNodeState = true;
            }
            lastClickTime = EditorApplication.timeSinceStartup;
        }
    }
    //鼠标滚轮检测——拖拽和缩放视图
    public void Check_MouseScroll()
    {
        //禁用鼠标滚轮的上下移动
        if (Event.current.type == EventType.ScrollWheel)
        {
            //当滚动时候进行拦截两个滚动条滚动
            Event.current.Use();

            // 缩放逻辑
            var zoomPivot = Event.current.mousePosition;
            float oldScale = data.ScaleValue;
            data.ScaleValue -= Event.current.delta.y * 0.05f;
            data.ScaleValue = Mathf.Clamp(data.ScaleValue, 0.5f, 1.5f);
            // 计算缩放中心的偏移
            Vector2 delta = (data.ViewportPosition.vector2 + zoomPivot) * (data.ScaleValue / oldScale) - (data.ViewportPosition.vector2 + zoomPivot);
            data.ViewportPosition.vector2 += delta;
        }

        //拖拽视图检测
        if (!Event.current.control && Event.current.button == 2 && Event.current.type == EventType.MouseDrag && Event.current.delta != Vector2.zero)
        {
            data.ViewportPosition.vector2 += Event.current.delta;
            IsDragingView = true;
        }
        else IsDragingView = false;
    }
    /// <summary>
    /// 拖拽单个节点
    /// </summary>
    public void Check_DragSingleNode()
    {
        //当在输入操作时候，不能移动
        //  if (GUIUtility.keyboardControl != 0) return;
        //拖拽组件条件
        //1.当选中物体不为空时
        //2.按住鼠标左键
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && selectedNode != null && !IsEnterEditNodeState)
        {
            selectedNode.drawInfo.Position += Event.current.delta / data.ScaleValue;
        }

    }
    //检测鼠标是否在某节点区域里，返回所在节点
    public VisualNode VNColliderCheck()
    {
        // Debug.Log("鼠标位置："+MouseActualPosition);
        foreach (var visualNode in data.dic_Nodes.Values)
        {
            //   Debug.Log("节点区块位置："+visualNode.vnDraw.DrawRect);
            if (visualNode.vnDraw.DrawRect.Contains(MouseActualPosition))
            {
                return visualNode;
            }
        }
        return null;
    }


    //进入连接节点状态
    public void EnterLinkNodeState(VisualNode BeginNode)
    {
        LinkingBeginNode = BeginNode;
        OperatorState = E_OperatorState.LinkingNode;
    }
    //退出连接节点状态
    public void ExitLinkNodeState()
    {
        LinkingBeginNode = null;
        EnterIdleState();
    }
    public void EnterIdleState()
    {
        OperatorState = E_OperatorState.Idle;
    }

}
