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
            GUILayout.Label("后缀名设置：", data.TitleStyle);
            data.SuffixName = EditorGUILayout.TextField("自定义后缀名:", data.SuffixName);

            GUILayout.Label("生成配置表：", data.TitleStyle);
            if (GUILayout.Button("生成基础白板.xlxs"))
            {
                (window as win_ExcelTool).CreateBaseExcel();
            }

            GUILayout.Label("从Excel表生成容器类和数据类：", data.TitleStyle);
            if (GUILayout.Button("生成数据类与容器类.."))
            {
                var excelPath = EditorUtility.OpenFilePanel("选择一个Excel文件", EditorPathHelper.GetRelativeAssetPath(data.ExcelDirectory_Path), "xlsx");
                if (string.IsNullOrEmpty(excelPath)) return;
                ExcelProcessor.ProcessSingleFile(excelPath, ProcessMode.GenerateCode);
            }
            GUILayout.Label("将生成好的容器类和数据类转换为二进制：", data.TitleStyle);
            if (GUILayout.Button("生成"))
            {

                var excelPath = EditorUtility.OpenFilePanel("选择一个Excel文件", EditorPathHelper.GetRelativeAssetPath(data.ExcelDirectory_Path), "xlsx");
                if (string.IsNullOrEmpty(excelPath)) return;
                ExcelProcessor.ProcessSingleFile(excelPath, ProcessMode.GenerateBinary);
            }
          

            // 路径设置
            GUILayout.Label("路径设置", data.TitleStyle);
            DrawPathSetting("Excel文件路径：", ref data.ExcelDirectory_Path);
            DrawPathSetting("输出路径：", ref data.OutPath);

            if (GUILayout.Button("打开导出文件夹"))
            {
                if (Directory.Exists(data.OutPath))
                    System.Diagnostics.Process.Start("explorer.exe", data.OutPath);
                else
                    EditorUtility.DisplayDialog("错误！", "不存在该路径文件夹", "好吧~");
            }

            if (GUILayout.Button("重设置为默认"))
            {
                data.IntiFirstCreate();
            }
        }

        private void DrawPathSetting(string label, ref string path)
        {
            EditorGUILayout.BeginHorizontal();
            path = EditorGUILayout.TextField(label, path);
            if (GUILayout.Button("设置路径"))
            {
                string newPath = EditorUtility.SaveFolderPanel($"设置{label}", Application.dataPath, null);
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