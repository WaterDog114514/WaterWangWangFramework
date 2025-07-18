using System.IO;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    public class win_ExcelTool : BaseWindow<winDraw_ExcelTool, winData_ExcelTool>
    {
        // �滻Ϊ�µ�Э����
        private ExcelProcessor _excelProcessor;

        // ��ʼ��Э����
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

        [MenuItem("ˮ��������/Excel��������������")]
        protected static void OpenWindow()
        {
            var window = GetWindow<win_ExcelTool>();
            window.titleContent = new GUIContent("Excel����");
            window.Show();
        }

        /// <summary>
        /// ���������İװ�Excel
        /// </summary>
        public void CreateBaseExcel()
        {
            // ��ȡ·��
            var copyPath = EditorPathHelper.FindFileInAssets("����ͳһ�װ�",".xlsx");
                //Path.Combine(EditorPathHelper.DirectoryPath, "Tools\\ExcelHelper\\.xlsx");
            var createPath = EditorUtility.SaveFilePanel("������׼���ñ�ģ��", null, "δ���������ñ�", "xlsx");
            if (string.IsNullOrEmpty(createPath)) return;

            File.Copy(copyPath, createPath, overwrite: true);
            AssetDatabase.Refresh();
        }
    }
}