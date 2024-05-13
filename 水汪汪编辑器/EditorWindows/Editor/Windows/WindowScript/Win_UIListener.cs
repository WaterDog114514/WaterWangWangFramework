using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
/// <summary>
/// 无法保存的编辑器
/// </summary>
public class Win_UIListener: SingletonBaseWindow
{
    private EW_UIListener main;
    [MenuItem("水汪汪工具/UI组件监听生成器")]
    protected static void OpenWindow()
    {
        GetWindow<Win_UIListener>();
    }
    /// <summary>
    /// 滚动条信息
    /// </summary>
    private Vector2 scrollValue;
    protected override void OnEnable()
    {
        main = new EW_UIListener();
        base.OnEnable();
        IntiWindowsSetting("UI设置", "YuSheIcon.png");
        //重载一下S
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        main = null;
    }
    protected override void m_DrawWindows()
    {
        if(main==null) main = new EW_UIListener();
        //检索物体，让其用户标注哪些需要监听
        if (GUILayout.Button("检索该父类物体下所有子控键", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight)))
        {
            main.ClickFind();
        }
        main.isFoladed = EditorGUILayout.Foldout(main.isFoladed, "展开");
        if (main.isFoladed && main.list_CheckChilds.Count > 0)
        {
            EditorGUI.DrawRect(new Rect(0, EditorGUIUtility.singleLineHeight * 2.3F, WindowWidth, EditorGUIUtility.singleLineHeight * 10), Color.grey);
            scrollValue = GUILayout.BeginScrollView(scrollValue, GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight * 10));
            for (int i = 0; i < main.list_CheckChilds.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(main.list_CheckChilds[i].ShowInfo);
                main.list_CheckChilds[i].IsListened = GUILayout.Toggle(main.list_CheckChilds[i].IsListened, "是否监听");
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
        //确认生成
        if (main.list_CheckChilds.Count > 0)
        {
            #region 监听对象关联

            EditorGUILayout.BeginHorizontal();
            //监听对象关联
            main.ListenerParents = EditorGUILayout.ObjectField(new GUIContent("监听父对象："), main.ListenerParents, typeof(GameObject), true) as GameObject;
            //关联对象
            if (GUILayout.Button("关联监听父对象"))
            {
                EditorGUIUtility.ShowObjectPicker<GameObject>(null, true, null, 0);
            }
            GUILayout.EndHorizontal();
            if (Event.current.commandName == "ObjectSelectorUpdated")
                main.ListenerParents = EditorGUIUtility.GetObjectPickerObject() as GameObject;

            #endregion
            //监听者类名
            main.ClassName = EditorGUILayout.TextField("监听者类名",main.ClassName);
            if (GUILayout.Button("开始生成监听函数"))
            {
                main.m_ClickCreateScript();
            }
        }
    }
}
