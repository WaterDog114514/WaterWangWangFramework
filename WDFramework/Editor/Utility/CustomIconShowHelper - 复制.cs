using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//���ڻ����Զ���ͼ��ר�õĸ�����
[InitializeOnLoad]
public static class CustomIconShowHelper
{
    //���������Զ�����ʾ

    [MenuItem("ˮ�����༭��/��ʾ����/�Զ���Icon����")]
    public static void OpenShowGUI()
    {
        IsShow = !IsShow;
        EditorPrefs.SetBool("IsOpenCustomIconShow",IsShow);
    }
    private static bool IsShow = EditorPrefs.GetBool("IsOpenCustomIconShow");
    //��׺������ͼ�� ���ֵ�
    private static readonly Dictionary<string, Texture2D> customIcons = new Dictionary<string, Texture2D>();
    //�ӳ�ע��ר��  ��׺������ͼ��·�� ���ֵ� ���Ĵ��ڷ�ֹע�᲻��
    private static readonly Dictionary<string, string> delayRegisterIcon = new Dictionary<string, string>();
    static CustomIconShowHelper()
    {
        // ������Ŀ���ڵĻ����¼�
        EditorApplication.projectWindowItemOnGUI += DrawCustomIcon;
        EditorApplication.delayCall += InitializeCustomIcons;
    }
    //�����д�ע���icon��ȫ��ע����
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
                Debug.LogWarning($"�Ҳ�����Ӧͼ��: {iconPath}");
            }
        }
    }
    public static void RegisterCustomIcon(string extension, string iconPath)
    {

        if (delayRegisterIcon.ContainsKey(extension))
            delayRegisterIcon[extension] = iconPath;
        else delayRegisterIcon.Add(extension, iconPath);
    }
    //�����Զ���ͼ��
    private static void DrawCustomIcon(string guid, Rect selectionRect)
    {
        //����ѡ��
        if (!IsShow) return;
        string path = AssetDatabase.GUIDToAssetPath(guid);
        foreach (var entry in customIcons)
        {
            if (path.EndsWith(entry.Key, System.StringComparison.OrdinalIgnoreCase))
            {
                //����
                Rect iconRect = new Rect(selectionRect.x + 2, selectionRect.y + 2, 32,32);
                GUI.DrawTexture(iconRect, entry.Value);
                break;
            }
        }
    }
}
