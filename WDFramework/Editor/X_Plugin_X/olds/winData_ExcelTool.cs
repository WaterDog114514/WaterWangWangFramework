//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//namespace WDEditor
//{
//    /// <summary>
//    /// Excel加载生成工具的数据
//    /// </summary>
//    [System.Serializable]
//    public class winData_ExcelTool : BaseWindowData
//    {
//        [NonSerialized]
//        public GUIStyle TitleStyle = new GUIStyle();
//        /// <summary>
//        /// 自定义Excel二进制文件后缀名
//        /// </summary>
//        public string SuffixName;
//        public string OutPath;
//        public string ExcelDirectory_Path;
//        public override void IntiLoad()
//        {
//            TitleStyle = new GUIStyle();
//        }
//        //初始化数值
//        public override void IntiFirstCreate()
//        {
//            SuffixName = "waterdogdata";
//            TitleStyle = new GUIStyle() { fontSize = 16};
//            TitleStyle.normal.textColor = Color.green;
//        }
//    }
//}
