using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    /// <summary>
    /// VNPreset���ô��ڵ�����
    /// </summary>
    [Serializable]
    public class winData_VNPresetEdtior : BaseWindowData
    {
        [NonSerialized]
        /// <summary>
        /// ��ȡ���Ľڵ�����Ԥ�豾��
        /// </summary>
        public VisualNode visualBaseNode;
        /// <summary>
        /// �ڵ����л��ļ�·��
        /// </summary>
        public string FilePath;

        public override string Title => "�ڵ�Ԥ��༭";
    }
}