//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//namespace WDEditor
//{
//    /// <summary>
//    /// Excel�������ɹ��ߵ�����
//    /// </summary>
//    public class winDraw_ExcelTool : BaseWindowDraw<winData_ExcelTool>
//    {
//        private ExcelSerializeHelper SerializeHelper => (window as win_ExcelTool).SerializeHelper;
//        public winDraw_ExcelTool(EditorWindow window, winData_ExcelTool data) : base(window, data)
//        {

//        }

//        public override void Draw()
//        {
//            GUILayout.Label("��׺�����ã�", data.TitleStyle);
//            data.SuffixName = EditorGUILayout.TextField("�Զ����׺��:", data.SuffixName);

//            GUILayout.Label("�������ñ�", data.TitleStyle);
//            if (GUILayout.Button("���ɻ����װ�.xlxs"))
//            {
//                (window as win_ExcelTool).CreateBaseExcel();
//            }


//            GUILayout.Label("��Excel������������������ࣺ", data.TitleStyle);
//            if (GUILayout.Button("���ɵ���Excel�ļ�������������..."))
//            {
//               SerializeHelper.GenerateExcelInfo();
//            }
//            if (GUILayout.Button("��������Excel�ļ�������������"))
//            {
//               SerializeHelper.GenerateAllExcelInfo();
//            }

//            GUILayout.Label("�����ɺõ��������������ת��Ϊ�����ƣ�", data.TitleStyle);
//            if (GUILayout.Button("ת������Excel�ļ�..."))
//            {
//               SerializeHelper.GenerateExcelBinary();
//            }
//            if (GUILayout.Button("����ת��Excel�ļ�"))
//            {
//               SerializeHelper.GenerateAllExcelBinary();
//            }

//            //����·�����
//            GUILayout.Label("·������", data.TitleStyle);
//            //EXCEL�ļ�·������
//            EditorGUILayout.BeginHorizontal();
//            data.ExcelDirectory_Path = EditorGUILayout.TextField("Excel�ļ�·����", data.ExcelDirectory_Path);
//            if (GUILayout.Button("����·��"))
//            {
//                string path = EditorUtility.SaveFolderPanel("����Ҫת��Excel���ļ���·��", Application.dataPath, null);
//                data.ExcelDirectory_Path = path;
//            }
//            EditorGUILayout.EndHorizontal();


//            EditorGUILayout.BeginHorizontal();
//            data.OutPath = EditorGUILayout.TextField("���·����", data.OutPath);
//            if (GUILayout.Button("����·��"))
//            {
//                string path = EditorUtility.SaveFolderPanel("����Ҫת��Excel���ļ���·��", Application.dataPath, null);
//                data.OutPath = path;
//            }
//            EditorGUILayout.EndHorizontal();

//            if (GUILayout.Button("�򿪵����ļ���"))
//            {
//                if (Directory.Exists(data.OutPath))
//                    System.Diagnostics.Process.Start("explorer.exe", data.OutPath);
//                else
//                    EditorUtility.DisplayDialog("����", "�����ڸ�·���ļ���", "�ð�~");
//            }
//            if (GUILayout.Button("������ΪĬ��"))
//            {
//                data.IntiFirstCreate();
//            }
//        }

//        public override void OnCreated()
//        {
//        }
//    }
//}
