using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WDFramework;
using static GameProjectSettingData;

namespace WDEditor
{
    public class winDraw_GameProjectSetting : BaseWindowDraw<winData_GameProjectSetting>
    {
        private GameProjectSettingData settingData => data.settingData;
        private GUIStyle TitleStyle { get => data.TitleStyle; }

        // 页签枚举
        public enum SettingsTab
        {
            Excel,
            AB,
            UI,
            Pool

        }

        private SettingsTab currentTab { get => data.CurrentTab; set => data.CurrentTab = value; }

        public winDraw_GameProjectSetting(EditorWindow window, winData_GameProjectSetting data) : base(window, data)
        {
        }

        public override void Draw()
        {
            // 绘制页签工具栏
            DrawTabs();

            // 根据当前选中的页签绘制对应内容
            switch (currentTab)
            {
                case SettingsTab.Excel:
                    DrawExcelSetting();
                    break;
                case SettingsTab.AB:
                    DrawABSetting();
                    break;
                case SettingsTab.UI:
                    DrawUISetting();
                    break;
                case SettingsTab.Pool:
                    DrawPoolDefaultSetting();
                    break;
            }

            // 绘制底部按钮（始终显示）
            DrawButton();
        }

        private void DrawTabs()
        {
            GUILayout.Space(5);

            // 使用工具栏样式绘制页签
            GUIStyle toolbarStyle = new GUIStyle(EditorStyles.toolbarButton)
            {
                fixedHeight = 25,
                fontSize = 12,
                fontStyle = FontStyle.Bold
            };

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                currentTab = (SettingsTab)GUILayout.Toolbar(
                    (int)currentTab,
                    Enum.GetNames(typeof(SettingsTab)),
                    toolbarStyle,
                    GUI.ToolbarButtonSize.FitToContents);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
        }

        private void DrawExcelSetting()
        {
            GUILayout.Label("Excel配置文件读取设置", TitleStyle);

            // 加载路径相关
            EditorGUILayout.BeginHorizontal();
            settingData.loadContainerSetting.DataPath = EditorGUILayout.TextField(
                "Excel二进制文件总路径：",
                settingData.loadContainerSetting.DataPath);

            settingData.loadContainerSetting.IsDebugStreamingAssetLoad = EditorGUILayout.Toggle(
                "开启Stream测试加载",
                settingData.abLoadSetting.IsStreamingABLoad);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawABSetting()
        {
            GUILayout.Label("AB包打包设置", TitleStyle);

            // 包名相关
            settingData.abLoadSetting.ABMainName = EditorGUILayout.TextField(
                "主包：",
                settingData.abLoadSetting.ABMainName);

            // 加载路径相关
           // EditorGUILayout.BeginHorizontal();
            settingData.abLoadSetting.IsStreamingABLoad = EditorGUILayout.Toggle(
                "从StreamimgAsset中加载",
                settingData.abLoadSetting.IsStreamingABLoad);

            if (!settingData.abLoadSetting.IsStreamingABLoad)
            {

                settingData.abLoadSetting.ABRuntimeLoadPath = EditorGUILayout.TextField(
                    "加载路径(相对游戏目录)：",
                    settingData.abLoadSetting.ABRuntimeLoadPath);
            }
            else
            {
                //相对StreamingAssets路径
                data.StreamingAssetDirectionName = EditorGUILayout.TextField("相对Streaming目录：", data.StreamingAssetDirectionName);
                if (!string.IsNullOrEmpty(data.StreamingAssetDirectionName))
                {
                    settingData.abLoadSetting.ABRuntimeLoadPath = Path.Combine(Application.streamingAssetsPath, data.StreamingAssetDirectionName);
                }
                GUI.enabled = false;
                EditorGUILayout.TextField(
                   "加载路径：",
                   settingData.abLoadSetting.ABRuntimeLoadPath);
                GUI.enabled = true;
            }
           // EditorGUILayout.EndHorizontal();

            GUILayout.Label($"当前加载路径为：{settingData.abLoadSetting.ABRuntimeLoadPath}");

            // 设置编辑器加载
            EditorGUILayout.BeginHorizontal();
            settingData.abLoadSetting.IsDebugABLoad = EditorGUILayout.Toggle(
                "开启编辑器测试加载",
                settingData.abLoadSetting.IsDebugABLoad);
            GUILayout.Label("(开启后，将通过Editor同步加载AB包)");
            EditorGUILayout.EndHorizontal();

            if (settingData.abLoadSetting.IsDebugABLoad)
            {
                settingData.abLoadSetting.ABEditorLoadPath = EditorGUILayout.TextField(
                    "编辑器AB包加载路径：",
                    settingData.abLoadSetting.ABEditorLoadPath);
            }
        }

        private void DrawUISetting()
        {
            var uiSetting = settingData.uiSetting;

            // 设置参考分辨率
            GUILayout.Label("UI 参考分辨率设置", TitleStyle);
            uiSetting.ReferenceResolutionX = EditorGUILayout.IntField("width：", uiSetting.ReferenceResolutionX);
            uiSetting.ReferenceResolutionY = EditorGUILayout.IntField("height：", uiSetting.ReferenceResolutionY);

            // 设置Match
            GUILayout.Label("UI Match设置", TitleStyle);
            uiSetting.Match = EditorGUILayout.Slider(uiSetting.Match, 0f, 1f);
        }

        private string[] SettingOptions;

        private void DrawPoolDefaultSetting()
        {
            if (SettingOptions == null)
            {
                SettingOptions = Enum.GetNames(typeof(Pool.E_PoolType));
            }

            GUILayout.Label("对象池默认设置", TitleStyle);
            settingData.defaultPoolSetting.PoolType = (Pool.E_PoolType)EditorGUILayout.Popup(
                "对象池默认类型：",
                (int)settingData.defaultPoolSetting.PoolType,
                SettingOptions);

            settingData.defaultPoolSetting.MaxCount = EditorGUILayout.IntField(
                "对象池默认上限：",
                settingData.defaultPoolSetting.MaxCount);
        }

        public void DrawButton()
        {
            EditorGUILayout.Space(20);
            if (GUILayout.Button("保存所有修改", GUILayout.Height(30)))
            {
                (window as win_GameProjectSetting).SaveData();
            }
        }

        public override void OnCreated()
        {
        }
    }
}