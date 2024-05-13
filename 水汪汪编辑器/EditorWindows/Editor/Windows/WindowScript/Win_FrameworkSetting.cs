using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Win_FrameworkSetting : SingletonBaseWindow
{
    //垃圾颜色字体
    private GUIStyle TitleStyle = new GUIStyle();
    private FrameworkSettingData settingData;


    private void IntiTitleStyle()
    {
        TitleStyle.normal.textColor = Color.cyan;
        TitleStyle.fontSize = 16;
    }

    protected override void OnEnable()
    {
        if (settingData == null) settingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
        IntiTitleStyle();
        base.OnEnable();
        IntiWindowsSetting("小框架设置", "YuSheIcon.png");
    }
    [MenuItem("水汪汪框架/小框架主设定")]
    protected static void OpenWindow()
    {
        EditorWindow.GetWindow<Win_FrameworkSetting>();
    }
    protected override void m_DrawWindows()
    {
        //Excel配置文件加载设置
        DrawExcelSetting();
        //AB包加载相关绘制
        DrawABSetting();

    }
    private bool IsFoldExcel = false;
    private void DrawExcelSetting()
    {
        IsFoldExcel = EditorGUILayout.Foldout(IsFoldExcel, "Excel配置文件读取设置：");
        if (!IsFoldExcel)
        {
            //加载路径相关
            GUILayout.Label("加载路径设置", TitleStyle);
            EditorGUILayout.BeginHorizontal();
            settingData.loadContainerSetting.DataPath = EditorGUILayout.TextField("Excel二进制文件总路径：", settingData.loadContainerSetting.DataPath);
            settingData.loadContainerSetting.IsDebugStreamingAssetLoad = EditorGUILayout.Toggle("开启Stream测试加载", settingData.abLoadSetting.IsStreamingABLoad);
            EditorGUILayout.EndHorizontal();
        }
    }
    private bool IsFoldAB = false;
    private void DrawABSetting()
    {
        IsFoldAB = EditorGUILayout.Foldout(IsFoldAB, "AB包打包设置：");
        if (!IsFoldAB)
        {
            //加载路径相关
            GUILayout.Label("加载路径设置", TitleStyle);

            EditorGUILayout.BeginHorizontal();
            settingData.abLoadSetting.ABLoadPath = EditorGUILayout.TextField("AB包加载路径：", settingData.abLoadSetting.ABLoadPath);
            settingData.abLoadSetting.IsStreamingABLoad = EditorGUILayout.Toggle("开启Stream测试加载", settingData.abLoadSetting.IsStreamingABLoad);
            EditorGUILayout.EndHorizontal();

            //设置编辑器加载
            EditorGUILayout.BeginHorizontal();
            settingData.abLoadSetting.IsDebugABLoad = EditorGUILayout.Toggle("开启编辑器测试加载", settingData.abLoadSetting.IsDebugABLoad);
            GUILayout.Label("(开启后，将通过Editor同步加载AB包)");
            EditorGUILayout.EndHorizontal();
            if (settingData.abLoadSetting.IsDebugABLoad)
            {
                settingData.abLoadSetting.ABEditorLoadPath = EditorGUILayout.TextField("编辑器AB包加载路径：", settingData.abLoadSetting.ABEditorLoadPath);
            }

            //包名相关
            GUILayout.Label("主包名设置", TitleStyle);
            settingData.abLoadSetting.ABMainName = EditorGUILayout.TextField("主包：", settingData.abLoadSetting.ABMainName);
            GUILayout.Label("非包名设置", TitleStyle);
            settingData.abLoadSetting.ObjPrefabPackName = EditorGUILayout.TextField("对象预制体包：", settingData.abLoadSetting.ObjPrefabPackName);
            settingData.abLoadSetting.UIPrefabPackName = EditorGUILayout.TextField("UI预制体包：", settingData.abLoadSetting.UIPrefabPackName);
            EditorGUILayout.Space(20);
            if (GUILayout.Button("保存所有修改"))
            {
                SettingDataLoader.Instance.SaveData(settingData);
                AssetDatabase.Refresh();
            }
        }
    }
}
