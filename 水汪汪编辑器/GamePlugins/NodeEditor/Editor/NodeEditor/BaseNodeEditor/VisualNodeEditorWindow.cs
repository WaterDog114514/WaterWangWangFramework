using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 可视化节点编辑窗口，通常嵌入在别的子窗口使用
/// </summary>
public abstract class VisualNodeEditorWindow
{
    /// <summary>
    /// 本节点编辑器的下拉菜单
    /// </summary>
    public VisualDropmenu dropmenu;
    /// <summary>
    /// 所嵌套的窗口
    /// </summary>
    public BaseWindow window;
    //public List<VisualBaseNode> list_nodes = new List<VisualBaseNode>();
    /// <summary>
    /// 节点们
    /// </summary>
    public Dictionary<int, VisualBaseNode> dic_Nodes;
    /// <summary>
    /// 避免太过臃肿，写一个检测类
    /// </summary>
    public NodeEditorCheck check;

    public int CurrentIndex = 0;
    /// <summary>
    /// 添加子节点
    /// </summary>
    /// <param name="node"></param>
    public void m_AddNode(VisualBaseNode node, int id = -1)
    {
        //是指定添加就跳过吧
        if (id != -1)
        {
            dic_Nodes.Add(id, node);
            return;
        }
        //ID必须独一无二 有了就加
        if (dic_Nodes.Count != 0)
        {
            while (dic_Nodes.ContainsKey(CurrentIndex))
            {
                CurrentIndex++;
            }
        }
        //索引设置
        node.ID = CurrentIndex;
        //添加
        dic_Nodes.Add(CurrentIndex, node);
    }
    /// <summary>
    /// 已经选中的节点
    /// </summary>
    public VisualBaseNode SelectedNode;
    public Texture BackgroundImage;

    /// <summary>
    /// 缩放系数
    /// </summary>
    public float ScaleValue = 0.8F;
    /// <summary>
    /// 当前视口坐标
    /// </summary>
    public Vector2 Pos_CurrentView;
    /// <summary>
    /// 真正的坐标和大小
    /// </summary>
    public Rect Size
    {
        get
        {
            return new Rect(
                _size.x * window.WindowWidth,
                _size.y * window.WindowHeight,
                _size.width * window.WindowWidth,
                _size.height * window.WindowHeight
                );
        }
    }
    /// <summary>
    /// 达到适应性，所以百分比大小
    /// </summary>
    public Rect _size;

    /// <summary>
    /// 初始化一个节点编辑器窗口构造
    /// </summary>
    /// <param name="window">要嵌入的窗口</param>
    /// <param name="size">所占比分</param>
    public VisualNodeEditorWindow(BaseWindow window, Rect size)
    {
        //设置被嵌入的window是谁
        this.window = window;
        //设置检测类
        check = new NodeEditorCheck(this);
        //设置窗口大小
        _size = size;
        //初始化本窗口的下拉列表
        dropmenu = new BehaviorDropMenu(this);
        //设置背景图
        BackgroundImage = WindowUtility.LoadAssetFromPath<Texture>(EM_WinSetting.Instance.SettingData.BehaviorTreeBGImage);
        //初始化节点存储器
        dic_Nodes = new Dictionary<int, VisualBaseNode>();
    }
    /// <summary>
    /// 清空所有节点
    /// </summary>
    public virtual void  m_ClearAllNodes()
    {
        dic_Nodes.Clear();
    }

    #region 绘制相关

    /// <summary>
    /// 绘制节点编辑器窗口
    /// </summary>
    public virtual void Draw()
    {
        //完成前置操作
        t_m_DrawBeginOperate();
        //完成总绘制操作
        t_m_DrawWin();
    }
    /// <summary>
    /// 绘制之前前置性更新工作
    /// </summary>
    public void t_m_DrawBeginOperate()
    {
        //绘制背景图
        m_DrawBackground();
        //检测相关
        check.m_CheckUpdate();
    }
    /// <summary>
    /// 总绘制操作
    /// </summary>
    public void t_m_DrawWin()
    {

        //绘制窗口
        GUI.BeginGroup(Size);
        //绘制下拉列表操作
        m_DrawNodes();
        dropmenu.m_drawDropMenu();
        GUI.EndGroup();
    }

    /// <summary>
    /// 绘制所有子节点的方法
    /// </summary>
    public void m_DrawNodes()
    {
        foreach (VisualBaseNode childNode in dic_Nodes.Values)
        {
            //前置更新
            childNode.t_m_DrawBeforeUpdate(m_GetTransformPos);
            //绘制逻辑
            childNode.t_m_DrawSelf();
        }
    }
    /// <summary>
    /// 获取到转换后的坐标
    /// </summary>
    public Vector2 m_GetTransformPos => Size.position + Pos_CurrentView;
    /// <summary>
    /// 绘制背景图
    /// </summary>
    public void m_DrawBackground()
    {
        GUI.DrawTexture(Size, BackgroundImage);
    }

    #endregion

    #region 连接节点相关
    /// <summary>
    /// 结束连接节点
    /// </summary>
    /// <param name="LinkedNode"></param>
    public abstract void On_EndLinkNode(VisualBaseNode LinkedNode);

    /// <summary>
    /// 开始连接节点
    /// </summary>
    /// <param name="LinkedNode"></param>
    public abstract void On_StartLinkedNode();
    /// <summary>
    /// 是否在连接节点状态中
    /// </summary>
    public bool b_IsLinkingNode;

    #endregion



}
