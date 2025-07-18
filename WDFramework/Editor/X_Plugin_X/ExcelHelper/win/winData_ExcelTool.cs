using System;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    [System.Serializable]
    public class winData_ExcelTool : BaseWindowData
    {
        [NonSerialized]
        public GUIStyle TitleStyle = new GUIStyle();

        /// <summary>
        /// 自定义Excel二进制文件后缀名
        /// </summary>
        public string SuffixName;

        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutPath;

        /// <summary>
        /// Excel文件路径
        /// </summary>
        public string ExcelDirectory_Path;

        public override string Title => "Excel转换助手";

        public override void IntiLoad()
        {
            TitleStyle = new GUIStyle();
        }

        // 初始化数值
        public override void IntiFirstCreate()
        {

            SuffixName = "waterdogdata";
            TitleStyle = new GUIStyle() { fontSize = 16 };
            TitleStyle.normal.textColor = Color.green;
            OutPath = Application.dataPath; // 默认输出路径为Assets目录
            ExcelDirectory_Path = Application.dataPath; // 默认Excel路径为Assets目录
        }
    }
}