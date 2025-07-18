using UnityEditor;
using UnityEngine;
using WDEditor;
/// <summary>
/// 开发节点编辑器的辅助类 用于帮忙注册图标和双击预设文件事件
/// </summary>
public static class EditorWindowPresetHelper
{
    /// <summary>
    /// 为预设文件注册双击打开编辑器事件和自定义图标
    /// </summary>
    /// <param name="extension"></param>
    /// <param name="iconPath"></param>
    public static void RegisterPresetInProject<WindowType>(string extension, string iconPath) where WindowType : EditorWindow
    {
        //添加点击监听
        SelectionFileWatcher.AddSelectedListener(extension, (path) =>
        {
            var win = EditorWindow.GetWindow<WindowType>();
            (win as IPresetWindowEditor).LoadData(path);
            // 在这里处理您的文件
        });

        //添加自定义图标
        CustomIconShowHelper.RegisterCustomIcon(extension, iconPath);
    }
    /// <summary>
    /// 创建预设文件
    /// </summary>
    /// <typeparam name="T">预设类型</typeparam>
    /// <param name="extension">拓展名</param>
    /// <param name="defaultName"></param>
    public static void CreatePresetFile<T>(string extension, string defaultName) where T : class, new()
    {
        //设置创建路径
        string path = EditorUtility.SaveFilePanel("保存" + typeof(T).Name + "预设", "Assets", $"{defaultName}.{extension}", extension);
        if (string.IsNullOrEmpty(path)) return;

        // 确保路径是以 "Assets" 开头的
        if (!path.StartsWith(Application.dataPath))
        {
            Debug.LogError("请在Assets文件夹中创建文件！");
            return;
        }

        // 创建文件并写入初始内容
        T preset = new T();
        BinaryManager.SaveToPath(preset, path);

        // 刷新 AssetDatabase
        AssetDatabase.Refresh();
    }
}
