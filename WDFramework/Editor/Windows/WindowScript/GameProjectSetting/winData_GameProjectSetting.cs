using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 主编辑器窗口设置管理类，用来存储并设定编辑器风格信息，也可以用来生成编辑器类
/// </summary>
namespace WDEditor
{
    [Serializable]
    public class winData_GameProjectSetting : BaseWindowData
    {
        /// <summary>
        /// 相对AB包在Streaming中路径
        /// </summary>
        public string StreamingAssetDirectionName;
        public winDraw_GameProjectSetting.SettingsTab CurrentTab;
        //垃圾颜色字体
        [NonSerialized]
        public GUIStyle TitleStyle = new GUIStyle();
        [NonSerialized]
        public GameProjectSettingData settingData;

        public override string Title => "项目设置";

        public override void IntiLoad()
        {
            TitleStyle = new GUIStyle();
        }

    }
}
