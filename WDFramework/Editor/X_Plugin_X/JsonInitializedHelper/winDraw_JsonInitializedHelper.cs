using System;
using UnityEditor;
using UnityEngine;


namespace WDEditor
{
    public class winDraw_JsonInitializedHelper : BaseWindowDraw<winData_JsonInitializedHelper>
    {
        public winDraw_JsonInitializedHelper(EditorWindow window, winData_JsonInitializedHelper data) : base(window, data)
        {
        }
        public override void Draw()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Json������");
            data.ClassName = EditorGUILayout.TextField(data.ClassName);
            GUILayout.EndHorizontal();
            data.jsonType =(JsonType)EditorGUILayout.EnumPopup(data.jsonType);
            if (GUILayout.Button("��ʼ����"))
            {
                (window as win_JsonInitializedHelper).GenerateJsonModuel(data.ClassName,data.jsonType);
            }
        }
        public override void OnCreated()
        {
        }


    }
}