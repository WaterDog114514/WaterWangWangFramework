using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 所有编辑器的主设置窗口，主窗口
/// </summary>
class Win_WinSetting : SingletonBaseWindow
{
    private EM_WinSetting main;
    protected override void OnEnable()
    {
        base.OnEnable();
        IntiWindowsSetting("面板主设置", "YuSheIcon.png");
    }
    [MenuItem("水汪汪框架/面板主设定")]
    protected static void OpenWindow()
    {
        EditorWindow.GetWindow<Win_WinSetting>();
    }
    protected override void m_DrawWindows()
    {
        if (main == null) main = EM_WinSetting.Instance;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField("窗口背景图：", main.SettingData.BackgroundImagePath);
        if(GUILayout.Button("修改..."))
        {
            string path = EditorUtility.OpenFilePanelWithFilters("选择图片", Application.dataPath,new string[]{"png","jpg" });
            if (path!="") 
            main.SettingData.BackgroundImagePath =  path;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField("窗口图标：", main.SettingData.EditorIcon);
        if (GUILayout.Button("修改..."))
        {
            string path = EditorUtility.OpenFilePanelWithFilters("选择图片", Application.dataPath, new string[] { "png", "jpg,icon" });
            if (path!="") 
            main.SettingData.EditorIcon = path;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField("窗口配置文件保存路径：", main.SettingData.AutoSavePath);
        if (GUILayout.Button("修改..."))
        {
            string path = EditorUtility.OpenFolderPanel("选择图片", Application.dataPath, "");
            if (path!="") 
            main.SettingData.AutoSavePath = path;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("保存所有修改"))
        {
            main.m_SaveData();
        }

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
 
    }

    public void m_SaveSetting()
    {

    }

}
