//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//namespace WDEditor
//{
//    /// <summary>
//    /// Excel加载生成工具的数据
//    /// </summary>
//    public class winDraw_ExcelTool : BaseWindowDraw<winData_ExcelTool>
//    {
//        private ExcelSerializeHelper SerializeHelper => (window as win_ExcelTool).SerializeHelper;
//        public winDraw_ExcelTool(EditorWindow window, winData_ExcelTool data) : base(window, data)
//        {

//        }

//        public override void Draw()
//        {
//            GUILayout.Label("后缀名设置：", data.TitleStyle);
//            data.SuffixName = EditorGUILayout.TextField("自定义后缀名:", data.SuffixName);

//            GUILayout.Label("生成配置表：", data.TitleStyle);
//            if (GUILayout.Button("生成基础白板.xlxs"))
//            {
//                (window as win_ExcelTool).CreateBaseExcel();
//            }


//            GUILayout.Label("从Excel表生成容器类和数据类：", data.TitleStyle);
//            if (GUILayout.Button("生成单个Excel文件的数据类容器..."))
//            {
//               SerializeHelper.GenerateExcelInfo();
//            }
//            if (GUILayout.Button("批量生成Excel文件的数据类容器"))
//            {
//               SerializeHelper.GenerateAllExcelInfo();
//            }

//            GUILayout.Label("将生成好的容器类和数据类转换为二进制：", data.TitleStyle);
//            if (GUILayout.Button("转换单个Excel文件..."))
//            {
//               SerializeHelper.GenerateExcelBinary();
//            }
//            if (GUILayout.Button("批量转换Excel文件"))
//            {
//               SerializeHelper.GenerateAllExcelBinary();
//            }

//            //加载路径相关
//            GUILayout.Label("路径设置", data.TitleStyle);
//            //EXCEL文件路径设置
//            EditorGUILayout.BeginHorizontal();
//            data.ExcelDirectory_Path = EditorGUILayout.TextField("Excel文件路径：", data.ExcelDirectory_Path);
//            if (GUILayout.Button("设置路径"))
//            {
//                string path = EditorUtility.SaveFolderPanel("设置要转换Excel的文件夹路径", Application.dataPath, null);
//                data.ExcelDirectory_Path = path;
//            }
//            EditorGUILayout.EndHorizontal();


//            EditorGUILayout.BeginHorizontal();
//            data.OutPath = EditorGUILayout.TextField("输出路径：", data.OutPath);
//            if (GUILayout.Button("设置路径"))
//            {
//                string path = EditorUtility.SaveFolderPanel("设置要转换Excel的文件夹路径", Application.dataPath, null);
//                data.OutPath = path;
//            }
//            EditorGUILayout.EndHorizontal();

//            if (GUILayout.Button("打开导出文件夹"))
//            {
//                if (Directory.Exists(data.OutPath))
//                    System.Diagnostics.Process.Start("explorer.exe", data.OutPath);
//                else
//                    EditorUtility.DisplayDialog("错误！", "不存在该路径文件夹", "好吧~");
//            }
//            if (GUILayout.Button("重设置为默认"))
//            {
//                data.IntiFirstCreate();
//            }
//        }

//        public override void OnCreated()
//        {
//        }
//    }
//}
