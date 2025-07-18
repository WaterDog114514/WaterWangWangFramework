using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    /// <summary>
    /// 绘制助手，用来美化代码，负责绘制window具体内容
    /// </summary>
    public class winDraw_VNSavePage : BaseWindowDraw<winData_VNSavePage>
    {
        public winDraw_VNSavePage(EditorWindow window, winData_VNSavePage data) : base(window, data)
        {
        }
        public override void Draw()
        {
            if (GUILayout.Button("打开编辑器编辑此页"))
            {
                (window as win_VNSavePage).OpenEditor();
            }
            if (GUILayout.Button("序列化为Runtime文件"))
            {
                VNEditorSerializer.SerializeNodes(data.SavePage.dic_Nodes);
            }
        }
        public override void OnCreated()
        {
        }
    }
}