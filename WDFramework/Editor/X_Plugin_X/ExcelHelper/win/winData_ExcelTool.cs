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
        /// �Զ���Excel�������ļ���׺��
        /// </summary>
        public string SuffixName;

        /// <summary>
        /// ���·��
        /// </summary>
        public string OutPath;

        /// <summary>
        /// Excel�ļ�·��
        /// </summary>
        public string ExcelDirectory_Path;

        public override string Title => "Excelת������";

        public override void IntiLoad()
        {
            TitleStyle = new GUIStyle();
        }

        // ��ʼ����ֵ
        public override void IntiFirstCreate()
        {

            SuffixName = "waterdogdata";
            TitleStyle = new GUIStyle() { fontSize = 16 };
            TitleStyle.normal.textColor = Color.green;
            OutPath = Application.dataPath; // Ĭ�����·��ΪAssetsĿ¼
            ExcelDirectory_Path = Application.dataPath; // Ĭ��Excel·��ΪAssetsĿ¼
        }
    }
}