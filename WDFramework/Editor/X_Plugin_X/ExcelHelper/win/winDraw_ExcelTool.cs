using System.IO;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    public class winDraw_ExcelTool : BaseWindowDraw<winData_ExcelTool>
    {
        private ExcelProcessor ExcelProcessor => (window as win_ExcelTool).ExcelProcessor;
        public winDraw_ExcelTool(EditorWindow window, winData_ExcelTool data) : base(window, data) { }

        public override void Draw()
        {
            GUILayout.Label("��׺�����ã�", data.TitleStyle);
            data.SuffixName = EditorGUILayout.TextField("�Զ����׺��:", data.SuffixName);

            GUILayout.Label("�������ñ�", data.TitleStyle);
            if (GUILayout.Button("���ɻ����װ�.xlxs"))
            {
                (window as win_ExcelTool).CreateBaseExcel();
            }

            GUILayout.Label("��Excel������������������ࣺ", data.TitleStyle);
            if (GUILayout.Button("������������������.."))
            {
                var excelPath = EditorUtility.OpenFilePanel("ѡ��һ��Excel�ļ�", EditorPathHelper.GetRelativeAssetPath(data.ExcelDirectory_Path), "xlsx");
                if (string.IsNullOrEmpty(excelPath)) return;
                ExcelProcessor.ProcessSingleFile(excelPath, ProcessMode.GenerateCode);
            }
            GUILayout.Label("�����ɺõ��������������ת��Ϊ�����ƣ�", data.TitleStyle);
            if (GUILayout.Button("����"))
            {

                var excelPath = EditorUtility.OpenFilePanel("ѡ��һ��Excel�ļ�", EditorPathHelper.GetRelativeAssetPath(data.ExcelDirectory_Path), "xlsx");
                if (string.IsNullOrEmpty(excelPath)) return;
                ExcelProcessor.ProcessSingleFile(excelPath, ProcessMode.GenerateBinary);
            }
          

            // ·������
            GUILayout.Label("·������", data.TitleStyle);
            DrawPathSetting("Excel�ļ�·����", ref data.ExcelDirectory_Path);
            DrawPathSetting("���·����", ref data.OutPath);

            if (GUILayout.Button("�򿪵����ļ���"))
            {
                if (Directory.Exists(data.OutPath))
                    System.Diagnostics.Process.Start("explorer.exe", data.OutPath);
                else
                    EditorUtility.DisplayDialog("����", "�����ڸ�·���ļ���", "�ð�~");
            }

            if (GUILayout.Button("������ΪĬ��"))
            {
                data.IntiFirstCreate();
            }
        }

        private void DrawPathSetting(string label, ref string path)
        {
            EditorGUILayout.BeginHorizontal();
            path = EditorGUILayout.TextField(label, path);
            if (GUILayout.Button("����·��"))
            {
                string newPath = EditorUtility.SaveFolderPanel($"����{label}", Application.dataPath, null);
                if (!string.IsNullOrEmpty(newPath))
                {
                    path = newPath;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public override void OnCreated()
        {
        }
    }
}