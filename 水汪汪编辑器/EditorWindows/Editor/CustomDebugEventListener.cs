using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 仅仅用来修改可视化事件监听器
/// </summary>
[CustomEditor(typeof(DebugEventListenerGizmos))]
public class CustomDebugEventListener : Editor
{
    private SerializedProperty list;
    private Transform ListennerTransform;

    private void OnEnable()
    {
        ListennerTransform = (target as DebugEventListenerGizmos).event_Listener;
    }
    public override void OnInspectorGUI()
    {
        GUILayout.Label("可视化监听绑定查看器");
        EditorGUILayout.ObjectField(new GUIContent("监听父对象："), ListennerTransform, typeof(Transform), true);
        if (GUILayout.Button("定位到监听父对象"))
        {
            EditorGUIUtility.PingObject(ListennerTransform);
        }

    }
}
