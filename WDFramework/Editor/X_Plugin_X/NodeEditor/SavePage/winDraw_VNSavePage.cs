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
    /// �������֣������������룬�������window��������
    /// </summary>
    public class winDraw_VNSavePage : BaseWindowDraw<winData_VNSavePage>
    {
        public winDraw_VNSavePage(EditorWindow window, winData_VNSavePage data) : base(window, data)
        {
        }
        public override void Draw()
        {
            if (GUILayout.Button("�򿪱༭���༭��ҳ"))
            {
                (window as win_VNSavePage).OpenEditor();
            }
            if (GUILayout.Button("���л�ΪRuntime�ļ�"))
            {
                VNEditorSerializer.SerializeNodes(data.SavePage.dic_Nodes);
            }
        }
        public override void OnCreated()
        {
        }
    }
}