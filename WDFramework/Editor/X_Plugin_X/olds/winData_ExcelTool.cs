//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//namespace WDEditor
//{
//    /// <summary>
//    /// Excel�������ɹ��ߵ�����
//    /// </summary>
//    [System.Serializable]
//    public class winData_ExcelTool : BaseWindowData
//    {
//        [NonSerialized]
//        public GUIStyle TitleStyle = new GUIStyle();
//        /// <summary>
//        /// �Զ���Excel�������ļ���׺��
//        /// </summary>
//        public string SuffixName;
//        public string OutPath;
//        public string ExcelDirectory_Path;
//        public override void IntiLoad()
//        {
//            TitleStyle = new GUIStyle();
//        }
//        //��ʼ����ֵ
//        public override void IntiFirstCreate()
//        {
//            SuffixName = "waterdogdata";
//            TitleStyle = new GUIStyle() { fontSize = 16};
//            TitleStyle.normal.textColor = Color.green;
//        }
//    }
//}
