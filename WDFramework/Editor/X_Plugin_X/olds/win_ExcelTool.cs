//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//namespace WDEditor
//{
//    public class win_ExcelTool : BaseWindow<winDraw_ExcelTool, winData_ExcelTool>
//    {
//        //�������л��ĺ���
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
//        [MenuItem("ˮ��������/Excel��������������")]
//        protected static void OpenWindow()
//        {
//            EditorWindow.GetWindow<win_ExcelTool>();
//        }
//        /// <summary>
//        /// ���������İװ�Excel
//        /// </summary>
//        public void CreateBaseExcel()
//        {
//            //��ȡ·��
//            var CopyPath = Path.Combine(EditorPathHelper.DirectoryPath, "Tools\\ExcelHelper\\����ͳһ�װ�.xlsx");
//            var CreatePath = EditorUtility.SaveFilePanel("������׼���ñ�ģ��", null, "δ���������ñ�", "xlsx");
//            if (string.IsNullOrEmpty(CreatePath)) return;
//            File.Copy(CopyPath, CreatePath, overwrite: true);
//            AssetDatabase.Refresh();
//        }
//    }
//}