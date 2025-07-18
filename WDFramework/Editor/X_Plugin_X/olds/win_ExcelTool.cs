//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//namespace WDEditor
//{
//    public class win_ExcelTool : BaseWindow<winDraw_ExcelTool, winData_ExcelTool>
//    {
//        //真正序列化的核心
//        public ExcelSerializeHelper SerializeHelper
//        {
//            get
//            {
//                if (_serializeHelper == null)
//                {
//                    _serializeHelper = new ExcelSerializeHelper(data);
//                }
//                return _serializeHelper;
//            }
//        }
//        private ExcelSerializeHelper _serializeHelper;
//        [MenuItem("水汪汪工具/Excel二进制生成助手")]
//        protected static void OpenWindow()
//        {
//            EditorWindow.GetWindow<win_ExcelTool>();
//        }
//        /// <summary>
//        /// 创建基础的白板Excel
//        /// </summary>
//        public void CreateBaseExcel()
//        {
//            //获取路径
//            var CopyPath = Path.Combine(EditorPathHelper.DirectoryPath, "Tools\\ExcelHelper\\基础统一白板.xlsx");
//            var CreatePath = EditorUtility.SaveFilePanel("创建标准配置表模板", null, "未命名的配置表", "xlsx");
//            if (string.IsNullOrEmpty(CreatePath)) return;
//            File.Copy(CopyPath, CreatePath, overwrite: true);
//            AssetDatabase.Refresh();
//        }
//    }
//}