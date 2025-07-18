using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//用于绘制自定义图标专用的辅助器
[InitializeOnLoad]
public static class CustomIconShowHelper
{
    //用来开关自定义显示

    [MenuItem("水汪汪编辑器/显示设置/自定义Icon开关")]
    public static void OpenShowGUI()
    {
        IsShow = !IsShow;
        EditorPrefs.SetBool("IsOpenCustomIconShow",IsShow);
    }
    private static bool IsShow = EditorPrefs.GetBool("IsOpenCustomIconShow");
    //后缀名——图标 的字典
    private static readonly Dictionary<string, Texture2D> customIcons = new Dictionary<string, Texture2D>();
    //延迟注册专属  后缀名——图标路径 的字典 它的存在防止注册不上
    private static readonly Dictionary<string, string> delayRegisterIcon = new Dictionary<string, string>();
    static CustomIconShowHelper()
    {
        // 监听项目窗口的绘制事件
        EditorApplication.projectWindowItemOnGUI += DrawCustomIcon;
        EditorApplication.delayCall += InitializeCustomIcons;
    }
    //将所有待注册的icon，全部注册上
    [InitializeOnLoadMethod]
    private static void InitializeCustomIcons()
    {
        foreach (var entry in delayRegisterIcon)
        {
            string iconPath = entry.Value;
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            if (icon != null) customIcons.Add(entry.Key, icon);
            else
            {
                Debug.LogWarning($"找不到对应图标: {iconPath}");
            }
        }
    }
    public static void RegisterCustomIcon(string extension, string iconPath)
    {

        if (delayRegisterIcon.ContainsKey(extension))
            delayRegisterIcon[extension] = iconPath;
        else delayRegisterIcon.Add(extension, iconPath);
    }
    //绘制自定义图标
    private static void DrawCustomIcon(string guid, Rect selectionRect)
    {
        //开关选项
        if (!IsShow) return;
        string path = AssetDatabase.GUIDToAssetPath(guid);
        foreach (var entry in customIcons)
        {
            if (path.EndsWith(entry.Key, System.StringComparison.OrdinalIgnoreCase))
            {
                //参数
                Rect iconRect = new Rect(selectionRect.x + 2, selectionRect.y + 2, 32,32);
                GUI.DrawTexture(iconRect, entry.Value);
                break;
            }
        }
    }
}
