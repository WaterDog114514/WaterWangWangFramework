using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    //第二部分，专门绘制面板
    public partial class winDraw_NodeEditor : BaseWindowDraw<winData_NodeEditor>
    {
        private VNEditorCheck Checker => window.Checker;
        //辅助绘制定位器
        private DrawIndexHelper LeftPanelDrawHelper = new DrawIndexHelper();
        //绘制左侧面板
        public void DrawLeftPanel()
        {
            //更新绘制逻辑助手
            LeftPanelDrawHelper.Update(LeftPanelRect);
            //绘制背景图
            GUI.DrawTexture(LeftPanelRect, data.LeftPanelBG.texture);
            //绘制边界线
            Handles.DrawSolidRectangleWithOutline(LeftPanelRect, Color.blue, Color.cyan);
            //绘制背景图
            GUI.DrawTexture(LeftPanelRect, data.LeftPanelBG.texture);
            //检测是否加载了编辑器预设
            if (data.editorPreset == null)
            {
                DrawLeftPanel_NoPreset();
                return;
            }
            DrawLeftPanel_HavePreset();
        }
        private string[] EditorOptions = new string[0];
        private int EditorOptionsIndex = 0;
        //当没有加载编辑器预设时候，绘制左侧面板逻辑
        private void DrawLeftPanel_NoPreset()
        {
            //创建一个编辑页面
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "选择一个编辑器来创建新页面：");
            LeftPanelDrawHelper.BeginHorizontalLayout(2, 1.5F);
            EditorOptionsIndex = EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(), EditorOptionsIndex, EditorOptions);
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "刷新列表"))
                window.LocateAllEditorPreset();
            LeftPanelDrawHelper.EndHorizontalLayout();
            //按钮——创建新页面
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "创建新页面"))
                window.OpenEditorFromCreateNewPage(EditorOptions[EditorOptionsIndex]);
            //按钮——绘制打开编辑页面
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "打开一个已有编辑页面"))
                window.OpenEditorFromOpenFilePanel(); ;
        }
        //绘制已经加载预设时的左边面板
        private void DrawLeftPanel_HavePreset()
        {
            //绘制编辑器预设显示
            LeftPanelDrawHelper.BeginHorizontalLayout(2, 1.5F);
            GUITextScaler.DrawScaledLabel(LeftPanelDrawHelper.GetNextSingleRect(), "当前编辑器：");
            GUI.enabled = false;
            EditorGUI.TextField(LeftPanelDrawHelper.GetNextSingleRect(), data.editorPreset.EditorName);
            GUI.enabled = true;

            LeftPanelDrawHelper.EndHorizontalLayout();
            //按钮——绘制视图操作 重置视图逻辑
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "重置视图"))
                window.ResetView();

            //按钮——绘制节点保存
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "保存编辑页面"))
                window.SaveEditorPage(false);
            //间距
            LeftPanelDrawHelper.GetNextSingleRect(0.5F);
            //按钮——另存为
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "另存为编辑页面"))
                window.SaveEditorPage(true);
            //间距
            LeftPanelDrawHelper.GetNextSingleRect(0.5F);
            //序列化按钮逻辑
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "序列化为Runtime数据"))
                window.SerializeRuntimeData();

            //绘制节点信息
            DrawNodeInfo();

        }

        //当前暂时性选择节点
        private VisualNode TempSelectedNode;
        //连接节点：
        private List<VisualNode> TempEnterNodes = new List<VisualNode>();
        private List<VisualNode> TempExitNodes = new List<VisualNode>();
        private string[] PortPosOption;
        //左侧面板节点信息绘制
        public void DrawNodeInfo()
        {
            //绘制如果选中了某节点，可以直接在页面编辑
            if (TempSelectedNode != window.Checker.selectedNode)
            {
                //刷新选中节点列表
                if (window.Checker.selectedNode != null)
                {
                    //设置临时绘制的连接信息
                    TempEnterNodes = window.Checker.selectedNode.EnterNodes;
                    TempExitNodes = window.Checker.selectedNode.ExitNodes;
                }
            }
            TempSelectedNode = window.Checker.selectedNode;

            //未选中时候不会绘制
            if (TempSelectedNode == null) return;
            //先得到节点
            TempSelectedNode = window.Checker.selectedNode;
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "当前选择节点：" + TempSelectedNode.NodeName);
            //绘制坐标
            TempSelectedNode.drawInfo.Position = EditorGUI.Vector2Field(LeftPanelDrawHelper.GetNextSingleRect(1.5F), "坐标：", TempSelectedNode.drawInfo.Position);
             //间隔
            LeftPanelDrawHelper.GetNextSingleRect(1.25F);

            LeftPanelDrawHelper.BeginHorizontalLayout(2,2);
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "进口位置设置：");
            //进口设置
            TempSelectedNode.drawInfo.EnterPortPos = 
                (VNDrawInfo.E_PortPos)EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect()  , (int)TempSelectedNode.drawInfo.EnterPortPos, PortPosOption);
            LeftPanelDrawHelper.EndHorizontalLayout();

            //出口设置
            LeftPanelDrawHelper.BeginHorizontalLayout(2,2);
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "出口位置设置：");
            TempSelectedNode.drawInfo.ExitPortPos =
                (VNDrawInfo.E_PortPos)EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(),  (int)TempSelectedNode.drawInfo.ExitPortPos, PortPosOption);
            LeftPanelDrawHelper.EndHorizontalLayout();

            //间距
            LeftPanelDrawHelper.GetNextSingleRect(1.55F);
            //绘制节点连接信息
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "出口连接：");
            //绘制进口的所有节点
            for (int i = 0; i < TempEnterNodes.Count; i++)
            {
                //得到单个节点
                var node = TempEnterNodes[i];
                LeftPanelDrawHelper.BeginHorizontalLayout(2, 1.25f);
                //绘制标签
                GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"{node.NodeName}(id:{node.ID})");
                if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "断开"))
                {
                    window.Operator.UnlinkNode(TempSelectedNode, TempEnterNodes[i]);
                }
                LeftPanelDrawHelper.EndHorizontalLayout();
            }
            //绘制出节点信息
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "出口连接：");
            //绘制进口的所有节点
            for (int i = 0; i < TempExitNodes.Count; i++)
            {
                //得到单个节点
                var node = TempExitNodes[i];
                LeftPanelDrawHelper.BeginHorizontalLayout(2, 1.25f);
                //绘制标签
                GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"{node.NodeName}(id:{node.ID})");
                if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "断开"))
                {
                    window.Operator.UnlinkNode(TempSelectedNode, TempExitNodes[i]);
                }
                LeftPanelDrawHelper.EndHorizontalLayout();
            }
        }
    }


}

