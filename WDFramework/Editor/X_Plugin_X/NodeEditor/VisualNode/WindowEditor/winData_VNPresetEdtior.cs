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
    /// VNPreset设置窗口的数据
    /// </summary>
    [Serializable]
    public class winData_VNPresetEdtior : BaseWindowData
    {
        [NonSerialized]
        /// <summary>
        /// 读取到的节点数据预设本身。
        /// </summary>
        public VisualNode visualBaseNode;
        /// <summary>
        /// 节点序列化文件路径
        /// </summary>
        public string FilePath;

        public override string Title => "节点预设编辑";
    }
}