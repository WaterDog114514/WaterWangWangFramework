using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
/// <summary>
/// ���༭���������ù����࣬�����洢���趨�༭�������Ϣ��Ҳ�����������ɱ༭����
/// </summary>
namespace WDEditor
{
    [Serializable]
    public class winData_GameProjectSetting : BaseWindowData
    {
        /// <summary>
        /// ���AB����Streaming��·��
        /// </summary>
        public string StreamingAssetDirectionName;
        public winDraw_GameProjectSetting.SettingsTab CurrentTab;
        //������ɫ����
        [NonSerialized]
        public GUIStyle TitleStyle = new GUIStyle();
        [NonSerialized]
        public GameProjectSettingData settingData;

        public override string Title => "��Ŀ����";

        public override void IntiLoad()
        {
            TitleStyle = new GUIStyle();
        }

    }
}
