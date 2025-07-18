using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    //第一部分，申请引用。以及绘制主面板逻辑
    public partial class winDraw_NodeEditor : BaseWindowDraw<winData_NodeEditor>
    {
        public new win_NodeEditor window => base.window as win_NodeEditor;
        //下拉菜单
        public VNEditorDropmenu Dropmenu => window.Dropmenu;
        //检测类
        public VNEditorCheck Check => window.Checker;
        // 节点们
        public Dictionary<int, VisualNode> dic_Nodes => window.data.dic_Nodes;
        public winDraw_NodeEditor(EditorWindow window, winData_NodeEditor data) : base(window, data)
        {

        }
        //左侧面板的位置和大小
        private Rect LeftPanelRect
        {
            set => data.LeftPanelRect.rect = value;
            get => data.LeftPanelRect.rect;
        }
        //右侧面板的位置和大小
        private Rect RightPanelRect
        {
            set => data.RightPanelRect.rect = value;
            get => data.RightPanelRect.rect;
        }
        /// <summary>
        /// 绘制节点编辑器窗口
        /// </summary>
        public override void Draw()
        {
            DrawRightPanel();
            DrawLeftPanel();
            //重绘刷新
            window.Repaint();
        }
        /// <summary>
        /// 避免太过臃肿，写一个检测类
        /// </summary>
        public int CurrentIndex = 0;
        /// <summary>
        /// 已经选中的节点
        /// </summary>
        public VisualNode SelectedNode;
        public override void OnCreated()
        {
            IntiPanelSize();
            IntiData();
        }
        private void IntiData()
        {
            if (data.ExistEditorPreset.Count == 0)
            {
                window.LocateAllEditorPreset();
            }
            //给选择编辑器的option赋值
            if (data.ExistEditorPreset.Count > 0)
                EditorOptions = data.ExistEditorPreset.Keys.ToArray();
            //节点口位置枚举选项初始化
            PortPosOption = Enum.GetNames(typeof(VNDrawInfo.E_PortPos));
        }
        //初始化方法
        //初始化面板大小
        private void IntiPanelSize()
        {
            //每次都要调整一下矩形宽高
            window.AddUpdateListener(() =>
            {
                //左侧面板宽度设置为1/4 位置居于(0,0)
                LeftPanelRect = new Rect(0, 0, WindowRect.width / 4, WindowRect.height);
                //右侧面板宽度设置为3/4 位置居于左侧面板之右
                RightPanelRect = new Rect(LeftPanelRect.width, 0, WindowRect.width * 3 / 4, WindowRect.height);
            });

        }
    }
}