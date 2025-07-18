using System.IO;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    /// <summary>
    /// 全局编辑器路径助手 获取编辑器相应文件夹路径
    /// </summary>
    public static class EditorPathHelper
    {
        // 常量定义
        private const string EDITOR_FOLDER_NAME = "WDFramework";
        private const string EDITOR_SUBFOLDER = "Editor";
        private const string RESOURCE_ASSET_FOLDER = "resourceAsset";
        private const string WIN_DATA_FOLDER = "WinData";
        private const string TEXTURE_FOLDER = "Texture";
        private const string ASSETS_FOLDER = "Assets";

        /// <summary>
        /// 编辑器窗口数据存储位置
        /// </summary>
        public static string EditorWinDataPath =>
            GetPathOrCreateDirectory(Path.Combine(EditorAssetPath, WIN_DATA_FOLDER));

        /// <summary>
        /// 编辑器资源resourceAsset文件夹位置
        /// </summary>
        public static string EditorAssetPath =>
            GetPathOrCreateDirectory(Path.Combine(DirectoryPath, RESOURCE_ASSET_FOLDER));

        /// <summary>
        /// 贴图资源文件夹
        /// </summary>
        public static string EditorTexturePath => GetPathOrCreateDirectory(Path.Combine(EditorAssetPath, TEXTURE_FOLDER));

        private static string _directoryPath;

        /// <summary>
        /// 编辑器文件夹绝对路径
        /// </summary>
        public static string DirectoryPath
        {
            get
            {
                return _directoryPath;
            }
        }

        // 定位编辑器目录
        [InitializeOnLoadMethod]
        public static void LocatedEditorDirection()
        {
            if (string.IsNullOrEmpty(_directoryPath))
            {
                // 直接递归搜索
                _directoryPath = LocalDirectory(EDITOR_FOLDER_NAME) + "\\" + EDITOR_SUBFOLDER;
                EditorPrefs.SetString("MainEditorPath", _directoryPath);
                if (string.IsNullOrEmpty(_directoryPath)) Debug.LogError("无法找到水汪汪编辑器，建议埋了吧");
            }
        }

        /// <summary>
        /// 获取到编辑器文件夹路径 如果不存在文件夹就创建一个
        /// </summary>
        /// <param name="targetPath">目标路径</param>
        /// <param name="IsAbsolute">是否为绝对路径</param>
        /// <returns></returns>
        public static string GetPathOrCreateDirectory(string targetPath, bool IsAbsolute = true)
        {
            if (!IsAbsolute)
            {
                // 确保传入的相对路径不以 "Assets/" 开头
                if (targetPath.StartsWith(ASSETS_FOLDER + "/"))
                {
                    targetPath = targetPath.Substring((ASSETS_FOLDER + "/").Length);
                }
                // 将相对路径组合到 Application.dataPath
                targetPath = Path.Combine(Application.dataPath, targetPath);
            }
            // 检查目录是否存在
            if (!Directory.Exists(targetPath))
            {
                // 如果目录不存在，则创建它
                Directory.CreateDirectory(targetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            // 返回目标路径
            return targetPath;
        }

        // 定位搜索工程文件夹下某个资源文件夹的路径 （会使用下面的递归搜索）
        public static string LocalDirectory(string folderName)
        {
            // 获取项目的 Assets 目录路径
            string assetsPath = Application.dataPath; // 返回 Assets 目录的绝对路径
            // 调用递归搜索函数
            return FindFolderRecursive(assetsPath, folderName);
        }

        // 递归搜索文件夹
        private static string FindFolderRecursive(string currentPath, string folderName)
        {
            // 获取当前路径下的所有子目录
            string[] directories = Directory.GetDirectories(currentPath);

            // 遍历每个子目录
            foreach (string directory in directories)
            {
                // 检查当前目录的名称是否匹配
                if (Path.GetFileName(directory).Equals(folderName, System.StringComparison.OrdinalIgnoreCase))
                {
                    // 找到匹配的文件夹，返回路径
                    return directory;
                }

                // 如果当前目录不匹配，则递归搜索子目录
                string foundPath = FindFolderRecursive(directory, folderName);
                if (!string.IsNullOrEmpty(foundPath))
                {
                    return foundPath; // 找到的路径
                }
            }

            // 如果未找到文件夹，返回 null
            return null;
        }

        /// <summary>
        /// 去掉绝对路径 保留Asset开头的，方便资源加载
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static string GetRelativeAssetPath(string absolutePath)
        {
            // 获取 Application.dataPath
            string dataPath = Application.dataPath;
            // 标准化路径格式，确保斜杠一致
            absolutePath = Path.GetFullPath(absolutePath).Replace("\\", "/");
            dataPath = Path.GetFullPath(dataPath).Replace("\\", "/");

            // 确保路径格式的统一性（在某些操作系统中路径可能会使用反斜杠\）
            if (absolutePath.StartsWith(dataPath))
            {
                // 去掉前缀 Application.dataPath，并在相对路径前加上 "Assets/"
                return ASSETS_FOLDER + absolutePath.Substring(dataPath.Length);
            }
            else
            {
                Debug.LogError("路径不在项目 Assets 文件夹内: " + absolutePath);
                return null;
            }
        }

        #region 搜索 Assets 文件夹中的某个文件，并通过后缀名进行筛选
        /// <summary>
        /// 递归搜索 Assets 文件夹中的某个文件，并通过后缀名进行筛选。后缀名，需加点！！
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="extensions">后缀名，需加点！！</param>
        /// <returns></returns>
        public static string FindFileInAssets(string fileName, params string[] extensions)
        {
            // 获取项目的 Assets 目录路径
            string assetsPath = Application.dataPath;
            // 返回 Assets 目录的绝对路径
            // 调用递归搜索文件的函数
            string GetPath = FindFileRecursive(assetsPath, fileName, extensions);
            if (string.IsNullOrEmpty(GetPath))
                Debug.LogWarning("未找到文件：" + fileName);
            return GetPath;
        }

        // 递归搜索文件，并通过后缀名进行筛选
        private static string FindFileRecursive(string currentPath, string fileName, string[] extensions)
        {
            // 获取当前路径下的所有文件
            string[] files = Directory.GetFiles(currentPath);
            // 遍历每个文件
            foreach (string file in files)
            {
                // 检查当前文件的名称是否匹配（仅仅名字 不包括后缀名）
                if (Path.GetFileNameWithoutExtension(file).Equals(fileName, System.StringComparison.OrdinalIgnoreCase))
                {
                    //如果传了不使用后缀约束直接返回
                    if (extensions.Length == 0)
                    {
                        // 找到匹配的文件，返回完整的文件路径
                        return file;
                    }
                    // 使用了后缀约束就进行对比
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
            // 获取当前路径下的所有子目录
            string[] directories = Directory.GetDirectories(currentPath);
            // 遍历每个子目录并递归搜索文件
            foreach (string directory in directories)
            {
                string foundFilePath = FindFileRecursive(directory, fileName, extensions);
                if (!string.IsNullOrEmpty(foundFilePath))
                {
                    return foundFilePath; // 找到的文件路径
                }
            }
            // 如果未找到文件，返回 null
            return null;
        }
        #endregion

        /// <summary>
        /// 相对Assets路径或绝对路径判断  非常牛逼，很全能
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Exist(string path)
        {
            string newPath = GetAbsolutePath(path);
            // 调用绝对路径的检查方法
            return File.Exists(newPath);
        }

        // 取到绝对路径 无论传的绝对还是相对，都会出绝对路径
        public static string GetAbsolutePath(string path)
        {
            // 判断是否是绝对路径
            if (Path.IsPathRooted(path))
            {
                // 是绝对路径，直接调用绝对路径的检查方法
                return path;
            }
            else
            {
                // 检查相对路径是否以 "Assets/" 开头，避免重复拼接
                if (!path.StartsWith(ASSETS_FOLDER))
                {
                    // 将相对路径与 Application.dataPath 拼接
                    path = Path.Combine(Application.dataPath, path);
                }
                else
                {
                    // 去掉 "Assets/"，拼接到 Application.dataPath 后面
                    path = Path.Combine(Application.dataPath, path.Substring((ASSETS_FOLDER + "/").Length));
                }
                return path;
            }
        }


    }
}