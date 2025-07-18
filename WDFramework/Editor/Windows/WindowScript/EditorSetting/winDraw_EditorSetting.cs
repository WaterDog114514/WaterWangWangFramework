using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace WDEditor
{
    /// <summary>
    /// 主编辑器窗口设置管理类，用来查看并设定编辑器信息
    /// </summary>

    public class winDraw_EditorSetting : BaseWindowDraw<winData_EditorSetting>
    {
        public winDraw_EditorSetting(EditorWindow window, winData_EditorSetting data) : base(window, data)
        {
        }

    

        public override void OnCreated()
        {
        }

        public override void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("窗口背景图：", data.BackgroundImagePath);
            if (GUILayout.Button("修改..."))
            {
                string path = EditorUtility.OpenFilePanelWithFilters("选择图片", Application.dataPath, new string[] { "png", "jpg" });
                if (path != "")
                    data.BackgroundImagePath = path;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("窗口图标：", data.EditorIcon);
            if (GUILayout.Button("修改..."))
            {
                string path = EditorUtility.OpenFilePanelWithFilters("选择图片", Application.dataPath, new string[] { "png", "jpg,icon" });
                if (path != "")
                    data.EditorIcon = path;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("窗口配置文件保存路径：", data.AutoSavePath);
            if (GUILayout.Button("修改..."))
            {
                string path = EditorUtility.OpenFolderPanel("选择图片", Application.dataPath, "");
                if (path != "")
                    data.AutoSavePath = path;
            }
            EditorGUILayout.EndHorizontal();



        }

    }
}