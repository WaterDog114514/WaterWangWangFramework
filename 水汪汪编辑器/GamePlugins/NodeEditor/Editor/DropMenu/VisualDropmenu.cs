using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 下拉菜单专属拓展，避免太过于臃肿而创的类
/// 继承此类就可以达到重搞下拉菜单了
/// </summary>
public abstract class VisualDropmenu
{
    public E_DropMenuPage Page = E_DropMenuPage.Main;


    public VisualNodeEditorWindow win;
    /// <summary>
    /// 下拉菜单大小
    /// </summary>
    public Vector2 Size;
    /// <summary>
    /// 是否展示页面
    /// </summary>
    private bool b_IsShowDropMenu;
    public Vector2 ShowPos;
    public VisualDropmenu(VisualNodeEditorWindow Win)
    {
        win = Win;
        Size = new Vector2(100, 50);
    }
    /// <summary>
    /// 打开下拉菜单
    /// </summary>
    public void m_drawDropMenu()
    {
        if (!b_IsShowDropMenu) return;
        //下拉列表背景
        EditorGUI.DrawRect(new Rect(ShowPos, Size), new Color(1, 1, 1, 0.34f));
        OneButtonIndex = -1;
        //根据页面来绘画不同菜单展示
        switch (Page)
        {
            case E_DropMenuPage.Main:
                m_DrawMainPage();
                break;
            case E_DropMenuPage.EditNode:
                m_DrawEditNodePage();
                break;
            case E_DropMenuPage.CreateNode:
                m_DrawCreatePage();
                break;
        }



    }
    /// <summary>
    /// 绘画按钮叠加设置位置用
    /// </summary>
    protected int OneButtonIndex;
    /// <summary>
    /// 取到按钮的Rect
    /// </summary>
    /// <returns></returns>
    public Rect m_GetButtonRect()
    {
        OneButtonIndex++;
        return new Rect(ShowPos + Vector2.up * EditorGUIUtility.singleLineHeight * (OneButtonIndex), new Vector2(Size.x, EditorGUIUtility.singleLineHeight));
    }

    /// <summary>
    /// 创建新节点
    /// </summary>
    public virtual void m_DrawMainPage()
    {

    }
    /// <summary>
    /// 绘画编辑某单个节点的菜单
    /// </summary>
    public virtual void m_DrawEditNodePage()
    {
        ////删除节点
        //if (GUI.Button(m_GetButtonRect(), "删除该节点"))
        //{
        //    win.list_nodes.Remove(win.SelectedNode);
        //    m_HideDropmenu();
        //}
    }
    /// <summary>
    /// 绘制创建节点页面
    /// </summary>
    public abstract void m_DrawCreatePage();
    /// <summary>
    /// 当前要绘制的页面
    /// </summary>
    public enum E_DropMenuPage
    {
        Main, EditNode, CreateNode
    }
    /// <summary>
    /// 展示下拉菜单方法
    /// </summary>
    public void m_ShowDropmenu()
    {
        b_IsShowDropMenu = true;
        win.b_IsLinkingNode = false;
        //矫正
        ShowPos = Event.current.mousePosition - win.Size.position;

    }
    /// <summary>
    /// 隐藏菜单方法
    /// </summary>
    public void m_HideDropmenu()
    {
        //退出节点编辑模式 节点编辑模式是防止右键节点点不了
        b_IsShowDropMenu = false;
        //重置
        win.check.b_IsInEditing = false;
        //重置页面
        Page = E_DropMenuPage.Main;
    }

}