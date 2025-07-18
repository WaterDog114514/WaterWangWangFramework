using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    /// <summary>
    /// 绘制助手，用来美化代码，负责绘制window具体内容
    /// </summary>
    public class winDraw_VNPresetEdtior : BaseWindowDraw<winData_VNPresetEdtior>
    {
        //inspetor主类
        private new win_VNPresetEdtior window => (base.window as win_VNPresetEdtior);
        //节点数据本身
        private VisualNode Node => data.visualBaseNode;

        public winDraw_VNPresetEdtior(EditorWindow window, winData_VNPresetEdtior data) : base(window, data)
        {
        }
        public override void OnCreated()
        {
        }
        public override void Draw()
        {
            if (data.visualBaseNode == null)
            {
                GUILayout.Label("当前未选中任何节点");
                return;
            }
            //绘制节点标题设置
            DrawTitleSetting();
            //绘制节点样式设置
            DrawStyleSetting();
            DrawParameter();
            DrawButton();
        }
        //绘制标题设置
        private void DrawTitleSetting()
        {
            //标题
            GUILayout.Label("节点名称：", TextStyleHelper.Custom(18, Color.magenta));
            Node.NodeName = EditorGUILayout.TextField(Node.NodeName);

        }
        //绘制节点样式设置
        public void DrawStyleSetting()
        {

            //标题文字颜色
            Node.drawInfo.TitleColor.color = EditorGUILayout.ColorField("标题颜色：", Node.drawInfo.TitleColor.color);
            //标题区块色
            Node.drawInfo.TitleAreaColor.color = EditorGUILayout.ColorField("标题块颜色：", Node.drawInfo.TitleAreaColor.color);
            //背景色
            Node.drawInfo.BackgroundColor.color = EditorGUILayout.ColorField("背景颜色：", Node.drawInfo.BackgroundColor.color);
        }


        public void DrawParameter()
        {
            //获取绘制参数时候的初始位置
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect DrawRect = new Rect(lastRect.position + Vector2.up * lastRect.height, new Vector2(lastRect.width, 0));
            float totalHeight = 0;
            for (int i = 0; i < Node.parameters.Count; i++)
            {
                // 获取当前参数
                var parameter = Node.parameters[i];
                //  绘制参数内容
                parameter.Draw_Inspctor(DrawRect);
                //计算之前的绘制高度
                DrawRect.position += Vector2.up * parameter.CurrentInspectorHeight;
                totalHeight += parameter.CurrentInspectorHeight;
            }
            //完美和Layout衔接
            EditorGUILayout.GetControlRect(false, totalHeight);
            //空出大量位置
            GUILayout.Space(10);
        }

        //绘制创建按钮 保存按钮
        public void DrawButton()
        {
            if (GUILayout.Button("添加新的参数"))
            {
                window.DrawCreateNewPar();
            }
            if (GUILayout.Button("保存当前节点预设"))
            {
                window.SaveData();

            }
        }
    }
}