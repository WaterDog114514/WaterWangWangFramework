using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace WDEditor
{
    /// <summary>
    ///编辑器开发的设置管理类，用来查看并设定编辑器信息
    /// </summary>
    public class win_ExtensionEditor : EditorWindow
    {

        [MenuItem("水汪汪框架/生成编辑器拓展")]
        protected static void OpenWindow()
        {
            EditorWindow.GetWindow<win_ExtensionEditor>();
        }
        public string FileName;
        public void OnGUI()
        {
            FileName = EditorGUILayout.TextField("编辑器名：", FileName);
            if (GUILayout.Button("生成三板斧..."))
            {
                string path = EditorUtility.SaveFolderPanel("选择生成路径", Application.dataPath, "");
                if (string.IsNullOrEmpty(path)) return;
                CreateWindow(FileName, path);
                CreateDraw(FileName, path);
                CreateData(FileName, path);

            }
        }
        /// <summary>
        /// 生成Data文件
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="path"></param>
        public void CreateData(string Name, string path)
        {
            ScriptBuildTask task = new ScriptBuildTask($"winData_{Name}");
            task.SetInheritance("BaseWindowData").AddMethod("public override void IntiFirstCreate()\r\n{\r\n}")
                .SetOutpath(Path.Combine(path, $"winData_{Name}.cs"));
            task.Execute();
           
        }
        /// <summary>
        /// 生成Window文件
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="path"></param>
        public void CreateWindow(string Name, string path)
        {
            ScriptBuildTask task = new ScriptBuildTask($"win_{Name}");
            task.SetInheritance($"BaseWindow<winDraw_{Name}, winData_{Name}>").AddMethod("[MenuItem(\"水汪汪框架/生成编辑器拓展\")]\r\n    protected static void OpenWindow()\r\n{}")
                .SetOutpath(Path.Combine(path, $"win_{Name}.cs"));
            task.Execute();
        }
        public void CreateDraw(string Name, string path)
        {
            ScriptBuildTask task = new ScriptBuildTask($"winDraw_{Name}");
            task.SetInheritance($"BaseWindowDraw<winData_{Name}>")
                .AddUsing("using UnityEditor;")
                .AddMethod("[MenuItem(\"水汪汪框架/生成编辑器拓展\")]")
                .AddMethod("protected static void OpenWindow()\\r\\n{}\"")
                .SetOutpath(Path.Combine(path, $"winDraw_{Name}.cs"));
            task.Execute();
        }
    }
}
