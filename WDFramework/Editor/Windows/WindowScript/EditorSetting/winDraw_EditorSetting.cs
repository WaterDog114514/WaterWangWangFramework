using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace WDEditor
{
    /// <summary>
    /// ���༭���������ù����࣬�����鿴���趨�༭����Ϣ
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
            EditorGUILayout.TextField("���ڱ���ͼ��", data.BackgroundImagePath);
            if (GUILayout.Button("�޸�..."))
            {
                string path = EditorUtility.OpenFilePanelWithFilters("ѡ��ͼƬ", Application.dataPath, new string[] { "png", "jpg" });
                if (path != "")
                    data.BackgroundImagePath = path;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("����ͼ�꣺", data.EditorIcon);
            if (GUILayout.Button("�޸�..."))
            {
                string path = EditorUtility.OpenFilePanelWithFilters("ѡ��ͼƬ", Application.dataPath, new string[] { "png", "jpg,icon" });
                if (path != "")
                    data.EditorIcon = path;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("���������ļ�����·����", data.AutoSavePath);
            if (GUILayout.Button("�޸�..."))
            {
                string path = EditorUtility.OpenFolderPanel("ѡ��ͼƬ", Application.dataPath, "");
                if (path != "")
                    data.AutoSavePath = path;
            }
            EditorGUILayout.EndHorizontal();



        }

    }
}