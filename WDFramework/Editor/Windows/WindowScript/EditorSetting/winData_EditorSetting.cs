using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace WDEditor
{
    /// <summary>
    /// 主编辑器窗口设置管理类，用来查看并设定编辑器信息
    /// </summary>
    [Serializable]
    public class winData_EditorSetting : BaseWindowData
    {
        /// <summary>
        /// 编辑器美术资源存储路径
        /// </summary>
        public string BackgroundImagePath = null;
        /// <summary>
        /// 行为树编辑器的背景图
        /// </summary>
        public string BehaviorTreeBGImage;
        public string AutoSavePath = null;
        /// <summary>
        /// 编辑器图标设置
        /// </summary>
        public string EditorIcon;

        public override string Title =>"编辑器设置";
    }
}