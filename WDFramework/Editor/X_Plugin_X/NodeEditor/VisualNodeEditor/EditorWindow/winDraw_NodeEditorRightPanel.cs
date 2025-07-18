using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WDEditor
{
    //专门绘制右侧面板
    public partial class winDraw_NodeEditor : BaseWindowDraw<winData_NodeEditor>
    {
        //辅助绘制定位器
        private DrawIndexHelper RightPanelDrawHelper = new DrawIndexHelper();
        //绘制右侧节点编辑面板
        public void DrawRightPanel()
        {
            //更新助手
            RightPanelDrawHelper.Update(RightPanelRect);
            //绘制背景图
            //  GUI.DrawTexture(RightPanelRect, data.RightPanelBG);
            if (data.editorPreset == null)
            {
                DrawRightPanel_NoPreset();
                return;
            }
            DrawRightPanel_HavePreset();
        }
        //当没有预设时绘画右边面板逻辑
        private void DrawRightPanel_NoPreset()
        {
            Rect center = RightPanelDrawHelper.GetCenterRect(window.position.width / 6, window.position.height / 7);
            GUI.Label(center, "选择节点编辑器后才能编辑！！！", TextStyleHelper.Custom(10, Color.red));
        }

        //当有预设时绘画右边面板逻辑
        private void DrawRightPanel_HavePreset()
        {
            //应用窗口坐标系检测逻辑
            Checker.CheckUpdate_Window();
            //绘制视图总逻辑
            DrawNodeView();
        }
        //绘制视图窗口 包括隐藏滚动条
        //绘制所有节点
        private void DrawNodeView()
        {

            Rect FixedRect = new Rect(Vector2.zero, data.NodeViewRect.rect.size * data.ScaleValue);
            //设置滚动条
            GUIStyle horizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
            GUIStyle verticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
            // 设置为空白样式，从而使滚动条不可见
            horizontalScrollbar.fixedHeight = 0;
            verticalScrollbar.fixedWidth = 0;
            //绘制节点视口视口
            data.ViewportPosition.vector2 = GUI.BeginScrollView(RightPanelRect, data.ViewportPosition.vector2, FixedRect, horizontalScrollbar, verticalScrollbar);
            //应用视口坐标系检测逻辑
            Checker.CheckUpdate_NodeView();
            GUI.DrawTexture(FixedRect, data.RightPanelBG.texture);
            //绘制节点
            DrawNodes();
            //取消视口坐标应用
            GUI.EndScrollView();
        }
        private void DrawNodes()
        {
            foreach (var vnode in dic_Nodes.Values)
            {
                //开始绘画
                vnode.vnDraw.Draw(data.ScaleValue);
            }
        }
    }
   
}
