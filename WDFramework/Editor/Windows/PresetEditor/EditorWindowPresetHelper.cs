using UnityEditor;
using UnityEngine;
using WDEditor;
/// <summary>
/// �����ڵ�༭���ĸ����� ���ڰ�æע��ͼ���˫��Ԥ���ļ��¼�
/// </summary>
public static class EditorWindowPresetHelper
{
    /// <summary>
    /// ΪԤ���ļ�ע��˫���򿪱༭���¼����Զ���ͼ��
    /// </summary>
    /// <param name="extension"></param>
    /// <param name="iconPath"></param>
    public static void RegisterPresetInProject<WindowType>(string extension, string iconPath) where WindowType : EditorWindow
    {
        //��ӵ������
        SelectionFileWatcher.AddSelectedListener(extension, (path) =>
        {
            var win = EditorWindow.GetWindow<WindowType>();
            (win as IPresetWindowEditor).LoadData(path);
            // �����ﴦ�������ļ�
        });

        //����Զ���ͼ��
        CustomIconShowHelper.RegisterCustomIcon(extension, iconPath);
    }
    /// <summary>
    /// ����Ԥ���ļ�
    /// </summary>
    /// <typeparam name="T">Ԥ������</typeparam>
    /// <param name="extension">��չ��</param>
    /// <param name="defaultName"></param>
    public static void CreatePresetFile<T>(string extension, string defaultName) where T : class, new()
    {
        //���ô���·��
        string path = EditorUtility.SaveFilePanel("����" + typeof(T).Name + "Ԥ��", "Assets", $"{defaultName}.{extension}", extension);
        if (string.IsNullOrEmpty(path)) return;

        // ȷ��·������ "Assets" ��ͷ��
        if (!path.StartsWith(Application.dataPath))
        {
            Debug.LogError("����Assets�ļ����д����ļ���");
            return;
        }

        // �����ļ���д���ʼ����
        T preset = new T();
        BinaryManager.SaveToPath(preset, path);

        // ˢ�� AssetDatabase
        AssetDatabase.Refresh();
    }
}
