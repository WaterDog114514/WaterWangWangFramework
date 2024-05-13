using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 水狗窗口实用工具类 拓展类 保存方法等
/// </summary>
public static class WindowUtility
{
    #region 设置数据 已经不用，使用Json加载即可
    /// <summary>
    /// 记录当前面板数据，用来保存
    /// </summary>
    // public static object[] RecordWindowData()
    // {
    //     //反射一波 获取所有数据
    //     Type t = this.GetType();
    //     FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
    //     object[] recordValues = new object[fields.Length];
    //     for (int i = 0; i < fields.Length; i++)
    //     {
    //         //大筛查一波
    //         if (fields[i].Name == "Title" || fields[i].Name == "Icon" ||
    //             fields[i].Name == "m_TitleContent" ||
    //             fields[i].Name == "m_Parent" || fields[i].Name == "m_Pos" ||
    //             fields[i].Name == "m_Notification" || fields[i].Name == "m_FadeoutTime" || fields[i].Name == "originWindowData") //continue;
    //         recordValues[i] = fields[i].GetValue(this);
    //     }
    //     return recordValues;
    // }


    // protected void SetWindowsData(object[] datas)
    // {
    //     //反射一波 获取所有数据
    //     Type t = this.GetType();
    //     FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
    //     for (int i = 0; i < fields.Length; i++)
    //     {
    //         //大筛查一波
    //         if (fields[i].Name == "Title" || fields[i].Name == "Icon" ||
    //             fields[i].Name == "m_TitleContent" ||
    //             fields[i].Name == "m_Parent" || fields[i].Name == "m_Pos" ||
    //             fields[i].Name == "m_Notification" || fields[i].Name == "m_FadeoutTime" || fields[i].Name == "originWindowData") continue;
    //         fields[i].SetValue(this, originWindowData[i]);
    //     }
    // }
    #endregion

    /// <summary>
    /// 让父类的main和父类窗口一一对应的赋值
    /// </summary>
    public static Type getMainType(this BaseWindow win)
    {
        Type type = win.GetType();
        if (type == typeof(Win_WinSetting))
            return typeof(EM_WinSetting);
        else if (type == typeof(Win_UIListener))
            return typeof(EW_UIListener);
        return null;
    }

    /// <summary>
    /// 保存文档，让用户自定义选择保存路径
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SaveWindowsDataCustom(this BaseWindow win)
    {
        string path = EditorUtility.SaveFilePanelInProject("选择保存路径", "", "json", "");
        SaveWindowsDataToPath(win, path);
    }
    /// <summary>
    /// 保存文档到指定路径
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="win"></param>
    /// <param name="path"></param>
    public static void SaveWindowsDataToPath(this BaseWindow win, string path)
    {
        object data = win.editorMain;
        JsonManager.Instance.SaveDataToPath(data, path, JsonType.JsonUtlity);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 加载窗口数据使用面板加载
    /// </summary>
    /// <param name="win"></param>
    public static void LoadWindowsData(this BaseWindow win)
    {
        //非常巧妙 历史转换来交替使用
        string path = EditorUtility.OpenFilePanel("选择打开的路径", "", "json");
        LoadWindowsData(win, path);
    }
    /// <summary>
    /// 加载窗口数据
    /// </summary>
    /// <param name="win"></param>
    public static EditorMain LoadWindowsData(this BaseWindow win, string path)
    {
        //非常巧妙 历史转换来交替使用
        object data = JsonManager.Instance.LoadDataFromPath(win.MainType, path);
        return data as EditorMain;
    }

    /// <summary>
    /// 绘制保存 重载入等之类的
    /// </summary>
    /// <typeparam name="T">EditorMain的类型</typeparam>
    /// <param name="win">要绘制的窗口</param>
    public static void SettingSaveLoadDraw(this BaseWindow win)
    {
        EditorGUILayout.Space(40);
        if (GUILayout.Button(new GUIContent("保存数据到某文件夹")))
        {
            SaveWindowsDataCustom(win);
        }
        //载入数据相关
        if (GUILayout.Button(new GUIContent("从...载入面板数据")))
        {
            LoadWindowsData(win);
        }
        if (GUILayout.Button(new GUIContent("重置所有数据")))
        {
            win.editorMain = Activator.CreateInstance(win.MainType) as EditorMain;
        }

    }

    /// <summary>
    /// 设置路径的同时，创建文件夹，仅用于编辑器的保存路径string用
    /// </summary>
    /// <param name="ParamterPath"></param>
    /// <param name="targetPath"></param>
    public static void SettingPathAndCreateDirectory(ref string ParamterPath, string targetPath)
    {
        ParamterPath = targetPath;
        string DirectoryPath = null;
        if (targetPath.Contains('/'))
            DirectoryPath = targetPath.Substring(0, targetPath.LastIndexOf('/'));
        if (!Directory.Exists(DirectoryPath))
            Directory.CreateDirectory(DirectoryPath);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 牛逼的加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ast"></param>
    /// <param name="loadPath"></param>
    /// <returns></returns>
    public static T LoadAssetFromPath<T>(string loadPath) where T : UnityEngine.Object
    {
        if(loadPath==null) return null;
        string path = "Assets/" + loadPath.Replace(Application.dataPath + "/", null);
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }
}
