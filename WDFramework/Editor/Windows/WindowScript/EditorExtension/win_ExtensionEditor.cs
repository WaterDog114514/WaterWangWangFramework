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
    ///�༭�����������ù����࣬�����鿴���趨�༭����Ϣ
    /// </summary>
    public class win_ExtensionEditor : EditorWindow
    {

        [MenuItem("ˮ�������/���ɱ༭����չ")]
        protected static void OpenWindow()
        {
            EditorWindow.GetWindow<win_ExtensionEditor>();
        }
        public string FileName;
        public void OnGUI()
        {
            FileName = EditorGUILayout.TextField("�༭������", FileName);
            if (GUILayout.Button("�������師..."))
            {
                string path = EditorUtility.SaveFolderPanel("ѡ������·��", Application.dataPath, "");
                if (string.IsNullOrEmpty(path)) return;
                CreateWindow(FileName, path);
                CreateDraw(FileName, path);
                CreateData(FileName, path);

            }
        }
        /// <summary>
        /// ����Data�ļ�
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
        /// ����Window�ļ�
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="path"></param>
        public void CreateWindow(string Name, string path)
        {
            ScriptBuildTask task = new ScriptBuildTask($"win_{Name}");
            task.SetInheritance($"BaseWindow<winDraw_{Name}, winData_{Name}>").AddMethod("[MenuItem(\"ˮ�������/���ɱ༭����չ\")]\r\n    protected static void OpenWindow()\r\n{}")
                .SetOutpath(Path.Combine(path, $"win_{Name}.cs"));
            task.Execute();
        }
        public void CreateDraw(string Name, string path)
        {
            ScriptBuildTask task = new ScriptBuildTask($"winDraw_{Name}");
            task.SetInheritance($"BaseWindowDraw<winData_{Name}>")
                .AddUsing("using UnityEditor;")
                .AddMethod("[MenuItem(\"ˮ�������/���ɱ༭����չ\")]")
                .AddMethod("protected static void OpenWindow()\\r\\n{}\"")
                .SetOutpath(Path.Combine(path, $"winDraw_{Name}.cs"));
            task.Execute();
        }
    }
}
