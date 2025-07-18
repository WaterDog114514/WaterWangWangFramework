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

        // ҳǩö��
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
            // ����ҳǩ������
            DrawTabs();

            // ���ݵ�ǰѡ�е�ҳǩ���ƶ�Ӧ����
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

            // ���Ƶײ���ť��ʼ����ʾ��
            DrawButton();
        }

        private void DrawTabs()
        {
            GUILayout.Space(5);

            // ʹ�ù�������ʽ����ҳǩ
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
            GUILayout.Label("Excel�����ļ���ȡ����", TitleStyle);

            // ����·�����
            EditorGUILayout.BeginHorizontal();
            settingData.loadContainerSetting.DataPath = EditorGUILayout.TextField(
                "Excel�������ļ���·����",
                settingData.loadContainerSetting.DataPath);

            settingData.loadContainerSetting.IsDebugStreamingAssetLoad = EditorGUILayout.Toggle(
                "����Stream���Լ���",
                settingData.abLoadSetting.IsStreamingABLoad);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawABSetting()
        {
            GUILayout.Label("AB���������", TitleStyle);

            // �������
            settingData.abLoadSetting.ABMainName = EditorGUILayout.TextField(
                "������",
                settingData.abLoadSetting.ABMainName);

            // ����·�����
           // EditorGUILayout.BeginHorizontal();
            settingData.abLoadSetting.IsStreamingABLoad = EditorGUILayout.Toggle(
                "��StreamimgAsset�м���",
                settingData.abLoadSetting.IsStreamingABLoad);

            if (!settingData.abLoadSetting.IsStreamingABLoad)
            {

                settingData.abLoadSetting.ABRuntimeLoadPath = EditorGUILayout.TextField(
                    "����·��(�����ϷĿ¼)��",
                    settingData.abLoadSetting.ABRuntimeLoadPath);
            }
            else
            {
                //���StreamingAssets·��
                data.StreamingAssetDirectionName = EditorGUILayout.TextField("���StreamingĿ¼��", data.StreamingAssetDirectionName);
                if (!string.IsNullOrEmpty(data.StreamingAssetDirectionName))
                {
                    settingData.abLoadSetting.ABRuntimeLoadPath = Path.Combine(Application.streamingAssetsPath, data.StreamingAssetDirectionName);
                }
                GUI.enabled = false;
                EditorGUILayout.TextField(
                   "����·����",
                   settingData.abLoadSetting.ABRuntimeLoadPath);
                GUI.enabled = true;
            }
           // EditorGUILayout.EndHorizontal();

            GUILayout.Label($"��ǰ����·��Ϊ��{settingData.abLoadSetting.ABRuntimeLoadPath}");

            // ���ñ༭������
            EditorGUILayout.BeginHorizontal();
            settingData.abLoadSetting.IsDebugABLoad = EditorGUILayout.Toggle(
                "�����༭�����Լ���",
                settingData.abLoadSetting.IsDebugABLoad);
            GUILayout.Label("(�����󣬽�ͨ��Editorͬ������AB��)");
            EditorGUILayout.EndHorizontal();

            if (settingData.abLoadSetting.IsDebugABLoad)
            {
                settingData.abLoadSetting.ABEditorLoadPath = EditorGUILayout.TextField(
                    "�༭��AB������·����",
                    settingData.abLoadSetting.ABEditorLoadPath);
            }
        }

        private void DrawUISetting()
        {
            var uiSetting = settingData.uiSetting;

            // ���òο��ֱ���
            GUILayout.Label("UI �ο��ֱ�������", TitleStyle);
            uiSetting.ReferenceResolutionX = EditorGUILayout.IntField("width��", uiSetting.ReferenceResolutionX);
            uiSetting.ReferenceResolutionY = EditorGUILayout.IntField("height��", uiSetting.ReferenceResolutionY);

            // ����Match
            GUILayout.Label("UI Match����", TitleStyle);
            uiSetting.Match = EditorGUILayout.Slider(uiSetting.Match, 0f, 1f);
        }

        private string[] SettingOptions;

        private void DrawPoolDefaultSetting()
        {
            if (SettingOptions == null)
            {
                SettingOptions = Enum.GetNames(typeof(Pool.E_PoolType));
            }

            GUILayout.Label("�����Ĭ������", TitleStyle);
            settingData.defaultPoolSetting.PoolType = (Pool.E_PoolType)EditorGUILayout.Popup(
                "�����Ĭ�����ͣ�",
                (int)settingData.defaultPoolSetting.PoolType,
                SettingOptions);

            settingData.defaultPoolSetting.MaxCount = EditorGUILayout.IntField(
                "�����Ĭ�����ޣ�",
                settingData.defaultPoolSetting.MaxCount);
        }

        public void DrawButton()
        {
            EditorGUILayout.Space(20);
            if (GUILayout.Button("���������޸�", GUILayout.Height(30)))
            {
                (window as win_GameProjectSetting).SaveData();
            }
        }

        public override void OnCreated()
        {
        }
    }
}