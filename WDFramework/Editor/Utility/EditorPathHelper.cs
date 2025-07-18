using System.IO;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    /// <summary>
    /// ȫ�ֱ༭��·������ ��ȡ�༭����Ӧ�ļ���·��
    /// </summary>
    public static class EditorPathHelper
    {
        // ��������
        private const string EDITOR_FOLDER_NAME = "WDFramework";
        private const string EDITOR_SUBFOLDER = "Editor";
        private const string RESOURCE_ASSET_FOLDER = "resourceAsset";
        private const string WIN_DATA_FOLDER = "WinData";
        private const string TEXTURE_FOLDER = "Texture";
        private const string ASSETS_FOLDER = "Assets";

        /// <summary>
        /// �༭���������ݴ洢λ��
        /// </summary>
        public static string EditorWinDataPath =>
            GetPathOrCreateDirectory(Path.Combine(EditorAssetPath, WIN_DATA_FOLDER));

        /// <summary>
        /// �༭����ԴresourceAsset�ļ���λ��
        /// </summary>
        public static string EditorAssetPath =>
            GetPathOrCreateDirectory(Path.Combine(DirectoryPath, RESOURCE_ASSET_FOLDER));

        /// <summary>
        /// ��ͼ��Դ�ļ���
        /// </summary>
        public static string EditorTexturePath => GetPathOrCreateDirectory(Path.Combine(EditorAssetPath, TEXTURE_FOLDER));

        private static string _directoryPath;

        /// <summary>
        /// �༭���ļ��о���·��
        /// </summary>
        public static string DirectoryPath
        {
            get
            {
                return _directoryPath;
            }
        }

        // ��λ�༭��Ŀ¼
        [InitializeOnLoadMethod]
        public static void LocatedEditorDirection()
        {
            if (string.IsNullOrEmpty(_directoryPath))
            {
                // ֱ�ӵݹ�����
                _directoryPath = LocalDirectory(EDITOR_FOLDER_NAME) + "\\" + EDITOR_SUBFOLDER;
                EditorPrefs.SetString("MainEditorPath", _directoryPath);
                if (string.IsNullOrEmpty(_directoryPath)) Debug.LogError("�޷��ҵ�ˮ�����༭�����������˰�");
            }
        }

        /// <summary>
        /// ��ȡ���༭���ļ���·�� ����������ļ��оʹ���һ��
        /// </summary>
        /// <param name="targetPath">Ŀ��·��</param>
        /// <param name="IsAbsolute">�Ƿ�Ϊ����·��</param>
        /// <returns></returns>
        public static string GetPathOrCreateDirectory(string targetPath, bool IsAbsolute = true)
        {
            if (!IsAbsolute)
            {
                // ȷ����������·������ "Assets/" ��ͷ
                if (targetPath.StartsWith(ASSETS_FOLDER + "/"))
                {
                    targetPath = targetPath.Substring((ASSETS_FOLDER + "/").Length);
                }
                // �����·����ϵ� Application.dataPath
                targetPath = Path.Combine(Application.dataPath, targetPath);
            }
            // ���Ŀ¼�Ƿ����
            if (!Directory.Exists(targetPath))
            {
                // ���Ŀ¼�����ڣ��򴴽���
                Directory.CreateDirectory(targetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            // ����Ŀ��·��
            return targetPath;
        }

        // ��λ���������ļ�����ĳ����Դ�ļ��е�·�� ����ʹ������ĵݹ�������
        public static string LocalDirectory(string folderName)
        {
            // ��ȡ��Ŀ�� Assets Ŀ¼·��
            string assetsPath = Application.dataPath; // ���� Assets Ŀ¼�ľ���·��
            // ���õݹ���������
            return FindFolderRecursive(assetsPath, folderName);
        }

        // �ݹ������ļ���
        private static string FindFolderRecursive(string currentPath, string folderName)
        {
            // ��ȡ��ǰ·���µ�������Ŀ¼
            string[] directories = Directory.GetDirectories(currentPath);

            // ����ÿ����Ŀ¼
            foreach (string directory in directories)
            {
                // ��鵱ǰĿ¼�������Ƿ�ƥ��
                if (Path.GetFileName(directory).Equals(folderName, System.StringComparison.OrdinalIgnoreCase))
                {
                    // �ҵ�ƥ����ļ��У�����·��
                    return directory;
                }

                // �����ǰĿ¼��ƥ�䣬��ݹ�������Ŀ¼
                string foundPath = FindFolderRecursive(directory, folderName);
                if (!string.IsNullOrEmpty(foundPath))
                {
                    return foundPath; // �ҵ���·��
                }
            }

            // ���δ�ҵ��ļ��У����� null
            return null;
        }

        /// <summary>
        /// ȥ������·�� ����Asset��ͷ�ģ�������Դ����
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static string GetRelativeAssetPath(string absolutePath)
        {
            // ��ȡ Application.dataPath
            string dataPath = Application.dataPath;
            // ��׼��·����ʽ��ȷ��б��һ��
            absolutePath = Path.GetFullPath(absolutePath).Replace("\\", "/");
            dataPath = Path.GetFullPath(dataPath).Replace("\\", "/");

            // ȷ��·����ʽ��ͳһ�ԣ���ĳЩ����ϵͳ��·�����ܻ�ʹ�÷�б��\��
            if (absolutePath.StartsWith(dataPath))
            {
                // ȥ��ǰ׺ Application.dataPath���������·��ǰ���� "Assets/"
                return ASSETS_FOLDER + absolutePath.Substring(dataPath.Length);
            }
            else
            {
                Debug.LogError("·��������Ŀ Assets �ļ�����: " + absolutePath);
                return null;
            }
        }

        #region ���� Assets �ļ����е�ĳ���ļ�����ͨ����׺������ɸѡ
        /// <summary>
        /// �ݹ����� Assets �ļ����е�ĳ���ļ�����ͨ����׺������ɸѡ����׺������ӵ㣡��
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="extensions">��׺������ӵ㣡��</param>
        /// <returns></returns>
        public static string FindFileInAssets(string fileName, params string[] extensions)
        {
            // ��ȡ��Ŀ�� Assets Ŀ¼·��
            string assetsPath = Application.dataPath;
            // ���� Assets Ŀ¼�ľ���·��
            // ���õݹ������ļ��ĺ���
            string GetPath = FindFileRecursive(assetsPath, fileName, extensions);
            if (string.IsNullOrEmpty(GetPath))
                Debug.LogWarning("δ�ҵ��ļ���" + fileName);
            return GetPath;
        }

        // �ݹ������ļ�����ͨ����׺������ɸѡ
        private static string FindFileRecursive(string currentPath, string fileName, string[] extensions)
        {
            // ��ȡ��ǰ·���µ������ļ�
            string[] files = Directory.GetFiles(currentPath);
            // ����ÿ���ļ�
            foreach (string file in files)
            {
                // ��鵱ǰ�ļ��������Ƿ�ƥ�䣨�������� ��������׺����
                if (Path.GetFileNameWithoutExtension(file).Equals(fileName, System.StringComparison.OrdinalIgnoreCase))
                {
                    //������˲�ʹ�ú�׺Լ��ֱ�ӷ���
                    if (extensions.Length == 0)
                    {
                        // �ҵ�ƥ����ļ��������������ļ�·��
                        return file;
                    }
                    // ʹ���˺�׺Լ���ͽ��жԱ�
                    else
                    {
                        string suffixName = Path.GetExtension(file);
                        foreach (var name in extensions)
                        {
                            if (suffixName.Equals(name))
                                return file;
                        }
                    }
                }
            }
            // ��ȡ��ǰ·���µ�������Ŀ¼
            string[] directories = Directory.GetDirectories(currentPath);
            // ����ÿ����Ŀ¼���ݹ������ļ�
            foreach (string directory in directories)
            {
                string foundFilePath = FindFileRecursive(directory, fileName, extensions);
                if (!string.IsNullOrEmpty(foundFilePath))
                {
                    return foundFilePath; // �ҵ����ļ�·��
                }
            }
            // ���δ�ҵ��ļ������� null
            return null;
        }
        #endregion

        /// <summary>
        /// ���Assets·�������·���ж�  �ǳ�ţ�ƣ���ȫ��
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Exist(string path)
        {
            string newPath = GetAbsolutePath(path);
            // ���þ���·���ļ�鷽��
            return File.Exists(newPath);
        }

        // ȡ������·�� ���۴��ľ��Ի�����ԣ����������·��
        public static string GetAbsolutePath(string path)
        {
            // �ж��Ƿ��Ǿ���·��
            if (Path.IsPathRooted(path))
            {
                // �Ǿ���·����ֱ�ӵ��þ���·���ļ�鷽��
                return path;
            }
            else
            {
                // ������·���Ƿ��� "Assets/" ��ͷ�������ظ�ƴ��
                if (!path.StartsWith(ASSETS_FOLDER))
                {
                    // �����·���� Application.dataPath ƴ��
                    path = Path.Combine(Application.dataPath, path);
                }
                else
                {
                    // ȥ�� "Assets/"��ƴ�ӵ� Application.dataPath ����
                    path = Path.Combine(Application.dataPath, path.Substring((ASSETS_FOLDER + "/").Length));
                }
                return path;
            }
        }


    }
}