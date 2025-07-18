using System.IO;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    public class win_ExcelTool : BaseWindow<winDraw_ExcelTool, winData_ExcelTool>
    {
        // 替换为新的协调类
        private ExcelProcessor _excelProcessor;

        // 初始化协调类
        public ExcelProcessor ExcelProcessor
        {
            get
            {
                if (_excelProcessor == null)
                {
                    _excelProcessor = new ExcelProcessor(data);
                }
                return _excelProcessor;
            }
        }

        [MenuItem("水汪汪工具/Excel二进制生成助手")]
        protected static void OpenWindow()
        {
            var window = GetWindow<win_ExcelTool>();
            window.titleContent = new GUIContent("Excel工具");
            window.Show();
        }

        /// <summary>
        /// 创建基础的白板Excel
        /// </summary>
        public void CreateBaseExcel()
        {
            // 获取路径
            var copyPath = EditorPathHelper.FindFileInAssets("基础统一白板",".xlsx");
                //Path.Combine(EditorPathHelper.DirectoryPath, "Tools\\ExcelHelper\\.xlsx");
            var createPath = EditorUtility.SaveFilePanel("创建标准配置表模板", null, "未命名的配置表", "xlsx");
            if (string.IsNullOrEmpty(createPath)) return;

            File.Copy(copyPath, createPath, overwrite: true);
            AssetDatabase.Refresh();
        }
    }
}