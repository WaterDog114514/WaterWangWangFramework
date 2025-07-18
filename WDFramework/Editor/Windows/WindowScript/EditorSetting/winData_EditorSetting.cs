using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace WDEditor
{
    /// <summary>
    /// ���༭���������ù����࣬�����鿴���趨�༭����Ϣ
    /// </summary>
    [Serializable]
    public class winData_EditorSetting : BaseWindowData
    {
        /// <summary>
        /// �༭��������Դ�洢·��
        /// </summary>
        public string BackgroundImagePath = null;
        /// <summary>
        /// ��Ϊ���༭���ı���ͼ
        /// </summary>
        public string BehaviorTreeBGImage;
        public string AutoSavePath = null;
        /// <summary>
        /// �༭��ͼ������
        /// </summary>
        public string EditorIcon;

        public override string Title =>"�༭������";
    }
}