using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using WDEditor;

/// <summary>
/// ѡ���ļ�����ߣ��ܰ��������ѡ����ʲô�ļ���Ȼ��ִ��ʲô�߼�
/// </summary>
[InitializeOnLoad]
public static class SelectionFileWatcher
{
    static SelectionFileWatcher()

    {
        EditorApplication.update += Update;
    }

    private static UnityEngine.Object selectedObj; // ��ǰѡ�еĶ���
    private static double lastClickTime; // �ϴε����ʱ��
    private static double currentClickTime; // ���ε����ʱ��
    private const double doubleClickInterval = 0.3; // ˫���ж���ʱ�������룩

    /// <summary>
    /// �ֵ䣬�洢�ļ���׺�����Ӧ��ί�з���
    /// </summary>
    private static Dictionary<string, UnityAction<string>> dic_Listener = new Dictionary<string, UnityAction<string>>();

    // ��ӵ������
    public static void AddSelectedListener(string suffixName, UnityAction<string> action)
    {
        if (dic_Listener.ContainsKey(suffixName))
            dic_Listener[suffixName] += action; // �����׺���Ѵ��ڣ�����µļ�������
        else
            dic_Listener.Add(suffixName, action); // �����׺�������ڣ�������׺������������
    }

    private static void Update()
    {
        // �����ѡ�еĶ���
        if (Selection.activeObject != null)
        {
            // ���ѡ�еĶ������仯
            if (selectedObj != Selection.activeObject)
            {
                selectedObj = Selection.activeObject; // ����ѡ�еĶ���
                lastClickTime = EditorApplication.timeSinceStartup; // ��¼���ʱ��
            }
            else
            {
                currentClickTime = EditorApplication.timeSinceStartup;
                try
                {
                    // �ж��Ƿ���˫��ʱ������
                    if (currentClickTime - lastClickTime <= doubleClickInterval)
                    {
                        string path = EditorPathHelper.GetAbsolutePath(AssetDatabase.GetAssetPath(selectedObj)); // ��ȡѡ�еĶ���ľ���·��
                        HandleDoubleClick(path); // ����˫���¼�
                        lastClickTime = 0; // ���ü�ʱ
                    }
                }catch
                {
                }
            }
        }
    }

    /// <summary>
    /// ����˫���¼�
    /// </summary>
    /// <param name="path">ѡ�еĶ����·��</param>
    private static void HandleDoubleClick(string path)
    {
        string suffixName = Path.GetExtension(path).Replace(".", ""); // ��ȡ�ļ���׺��
        if (dic_Listener.ContainsKey(suffixName))
            dic_Listener[suffixName]?.Invoke(path); // ִ�ж�Ӧ�ļ�������
    }
}
