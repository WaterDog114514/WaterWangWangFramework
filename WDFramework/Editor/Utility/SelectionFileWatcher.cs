using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using WDEditor;

/// <summary>
/// 选择文件监管者，能帮你监听你选择了什么文件，然后执行什么逻辑
/// </summary>
[InitializeOnLoad]
public static class SelectionFileWatcher
{
    static SelectionFileWatcher()

    {
        EditorApplication.update += Update;
    }

    private static UnityEngine.Object selectedObj; // 当前选中的对象
    private static double lastClickTime; // 上次点击的时间
    private static double currentClickTime; // 本次点击的时间
    private const double doubleClickInterval = 0.3; // 双击判定的时间间隔（秒）

    /// <summary>
    /// 字典，存储文件后缀名与对应的委托方法
    /// </summary>
    private static Dictionary<string, UnityAction<string>> dic_Listener = new Dictionary<string, UnityAction<string>>();

    // 添加点击监听
    public static void AddSelectedListener(string suffixName, UnityAction<string> action)
    {
        if (dic_Listener.ContainsKey(suffixName))
            dic_Listener[suffixName] += action; // 如果后缀名已存在，添加新的监听方法
        else
            dic_Listener.Add(suffixName, action); // 如果后缀名不存在，新增后缀名及监听方法
    }

    private static void Update()
    {
        // 如果有选中的对象
        if (Selection.activeObject != null)
        {
            // 如果选中的对象发生变化
            if (selectedObj != Selection.activeObject)
            {
                selectedObj = Selection.activeObject; // 更新选中的对象
                lastClickTime = EditorApplication.timeSinceStartup; // 记录点击时间
            }
            else
            {
                currentClickTime = EditorApplication.timeSinceStartup;
                try
                {
                    // 判断是否在双击时间间隔内
                    if (currentClickTime - lastClickTime <= doubleClickInterval)
                    {
                        string path = EditorPathHelper.GetAbsolutePath(AssetDatabase.GetAssetPath(selectedObj)); // 获取选中的对象的绝对路径
                        HandleDoubleClick(path); // 处理双击事件
                        lastClickTime = 0; // 重置计时
                    }
                }catch
                {
                }
            }
        }
    }

    /// <summary>
    /// 处理双击事件
    /// </summary>
    /// <param name="path">选中的对象的路径</param>
    private static void HandleDoubleClick(string path)
    {
        string suffixName = Path.GetExtension(path).Replace(".", ""); // 获取文件后缀名
        if (dic_Listener.ContainsKey(suffixName))
            dic_Listener[suffixName]?.Invoke(path); // 执行对应的监听方法
    }
}
