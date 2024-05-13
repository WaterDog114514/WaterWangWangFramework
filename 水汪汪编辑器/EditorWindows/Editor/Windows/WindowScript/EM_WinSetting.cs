using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
/// <summary>
/// 主编辑器窗口设置管理类，用来存储并设定编辑器风格信息，也可以用来生成编辑器类
/// </summary>
class EM_WinSetting : EditorMain, ISaveLoadWindowMain
{
    public static EM_WinSetting Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EM_WinSetting();
            return _instance;
        }
    }

    public Data_WinSetting SettingData;
    public string DirectoryPath => Application.dataPath + @"\水汪汪编辑器\Editor\Windows\EditorAsset\EditorData\";

    public string DataName => "WinEditorSetting.json";

    /// <summary>
    /// 唯一单利
    /// </summary>
    private static EM_WinSetting _instance = new EM_WinSetting();
    public Texture WindowsBackground;
    public EM_WinSetting()
    {
        m_LoadData();
        WindowsBackground = WindowUtility.LoadAssetFromPath<Texture>(SettingData.BackgroundImagePath);
        //存档逻辑 啊

    }

    public void m_SaveData()
    {
        //再次检查
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }
        JsonManager.Instance.SaveDataToPath(SettingData, DirectoryPath + DataName);

    }

    public void m_LoadData()
    {
        if (File.Exists(DirectoryPath + DataName))
        {
            SettingData = JsonManager.Instance.LoadDataFromPath<Data_WinSetting>(DirectoryPath + DataName);
            return;
        }
        //第一次创建，设置为默认
        SettingData = new Data_WinSetting();

        //默认背景
        WindowUtility.SettingPathAndCreateDirectory(ref SettingData.BackgroundImagePath, Application.dataPath + "/水汪汪编辑器/EditorAsset/ArtEditorAsset/bg.png");
        //默认存档位置
        WindowUtility.SettingPathAndCreateDirectory(ref SettingData.AutoSavePath, Application.dataPath + "/水汪汪编辑器/EditorAsset/EditorData/");
        //默认的行为树图片
        WindowUtility.SettingPathAndCreateDirectory(ref SettingData.BehaviorTreeBGImage, Application.dataPath + "/水汪汪编辑器/EditorAsset/ArtEditorAsset/TreeRight.jpg");
        //编辑器图标
        WindowUtility.SettingPathAndCreateDirectory(ref SettingData.EditorIcon, Application.dataPath + "/水汪汪编辑器/EditorAsset/ArtEditorAsset/Icon.jpg");

    }
}
[Serializable]
class Data_WinSetting
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
}