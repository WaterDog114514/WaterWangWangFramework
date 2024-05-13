using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 行为树编辑器窗口
/// </summary>
public class Win_BehaviorTree : SingletonBaseWindow
{
    public BehaviorTreeNodeEditorWindow editorWindow;

    
    #region 初始化逻辑
    [MenuItem("水汪汪插件/行为树可视化编辑器")]
    public static void Open()
    {
        GetWindow<Win_BehaviorTree>();
    }

    protected override void OnEnable()
    {
        m_IntiTreeBehavior();
        IntiWindowsSetting("行为树编辑器", "SaveIcon.png");
        base.OnEnable();
    }

    private void m_IntiTreeBehavior()
    {
        editorWindow = new BehaviorTreeNodeEditorWindow(this, new Rect(0.15F, 0, 0.85F, 1F));
    }


    #endregion
    protected override void m_DrawWindows()
    {
        editorWindow.Draw();
        DrawLeftWindows();
        //保证刷新率
        Repaint();
    }
    /// <summary>
    /// 绘制左边窗口
    /// </summary>
    private void DrawLeftWindows()
    {
        //画下左边列表
        GUILayout.Label("当前选中节点：", GUILayout.Width(LeftWindowWeight));
        EditorGUILayout.TextField(editorWindow.SelectedNode != null ? editorWindow.SelectedNode.Name : "无", GUILayout.Width(LeftWindowWeight));
        if(GUILayout.Button("保存所有节点", GUILayout.Width(LeftWindowWeight)))
        {
            editorWindow.Loader.m_SaveAllData();
           // main.m_SaveAllData();
        }
    }
    /// <summary>
    /// 获得左边窗口的宽度
    /// </summary>
    private float LeftWindowWeight => editorWindow.Size.x;
}
