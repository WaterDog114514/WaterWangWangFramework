using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 自定义显示行为树保存数据
/// </summary>
[CustomEditor(typeof(BTNodeData))]
public class CustomBehaviorTreeDataShow : Editor
{
    public void OnEnable()
    {
        //报空自动关联
        if ((target as BTNodeData).BehaviorTreeData == null)
        {
            (target as BTNodeData).BehaviorTreeData = AssetDatabase.LoadAssetAtPath<TextAsset>((target as BTNodeData).TreeNodeDataPath);
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("打开编辑器修改"))
        {
            if ((target as BTNodeData).BehaviorTreeData != null)
            {
                Win_BehaviorTree win = EditorWindow.GetWindow<Win_BehaviorTree>();
                win.editorWindow.Loader.m_LoadStaticData(target as BTNodeData);
            }
        }
    }
}

[CustomEditor(typeof(BTNodeObjcetDriver))]
public class CustomBehaviorTreeDriverShow : Editor
{
    public GUIStyle CheckStyle;
    public GUIStyle TitleStyle;

    public void SettingStyle()
    {
        CheckStyle = new GUIStyle(GUI.skin.button);
        CheckStyle.normal.textColor = Color.green;
        CheckStyle.fontStyle = FontStyle.Bold;
        CheckStyle.onNormal.textColor = Color.red;
        CheckStyle.onHover.textColor = Color.yellow;
    }

    private void OnEnable()
    {
        TitleStyle = new GUIStyle();
        TitleStyle.normal.textColor = Color.cyan;
        TitleStyle.fontStyle = FontStyle.Bold;
        TitleStyle.alignment = TextAnchor.MiddleCenter;

    }
    public override void OnInspectorGUI()
    {
        SettingStyle();
        EditorGUILayout.LabelField("行为树AI物体驱动器", TitleStyle);
        (target as BTNodeObjcetDriver).data = EditorGUILayout.ObjectField("行为树数据文件：", (target as BTNodeObjcetDriver).data, typeof(BTNodeData), false) as BTNodeData;
        if (GUILayout.Button("打开动态监测", CheckStyle))
        {
            if (!Application.isPlaying)
            {

                Debug.LogWarning("需要在运行模式下才能开启动态监测");
                return;
            }
            (target as BTNodeObjcetDriver).b_DynamicCheck = true;
            (target as BTNodeObjcetDriver).RootNode = BTNodeLoader.Instance.LoadAndCheck((target as BTNodeObjcetDriver), ref (target as BTNodeObjcetDriver).dynamicDic);
            Dictionary<string, BTNodeInfo> dic = (target as BTNodeObjcetDriver).dynamicDic;
            if (dic != null)
            {
                Win_BehaviorTree win = EditorWindow.GetWindow<Win_BehaviorTree>();
                win.editorWindow.CheckingDirver = target as BTNodeObjcetDriver;
                win.editorWindow.Loader.m_LoadDynamicData(dic);
            }
            else
            {
                Debug.LogWarning("需要在运行模式之前开启打钩动态监测才能点击！！！");
            }
        }

    }
}
