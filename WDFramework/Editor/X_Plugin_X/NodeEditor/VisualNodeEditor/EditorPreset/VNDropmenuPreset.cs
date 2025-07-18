using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;
using WDEditor;
using static Codice.Client.Common.DiffMergeToolConfig;
using static UnityEditor.Experimental.GraphView.GraphView;
//�����洢�˵��б�Ľڵ�
[System.Serializable]
public class VNDropmenuLayer
{
    //��ק����
    [System.NonSerialized]
    private Rect dropArea;
    //���ȼ�
    public int priority;
    //�ڵ���
    public string LayerName;
    //�Ƿ��۵��㼶
    private bool IsFolded;
    //�ڵ�Ԥ���list��
    public List<VisualNodePreset> list_NodePresets;

    public VNDropmenuLayer()
    {

        //�����б�
        Debug.Log("�����б�");
        list_NodePresets = new List<VisualNodePreset>();
    }

    //ֻ���ڲ˵��洢�ڵ㣬˳��洢��instanceid������ͬ�����޸�
    [Serializable]
    public class VisualNodePreset
    {
        //ÿ�������༭����Object�ᶪʧ��ֻ��ͨ��NodeFilePath��ַ����
        //�ڵ�Ԥ�豾��Object�ļ�
        [NonSerialized]
        public UnityEngine.Object NodePresetFile;
        public VisualNode Node;
        /// <summary>
        /// ������Ψһ��ʶ������instanceID���Ǿ޿�
        /// </summary>
        public string GUID;

        public VisualNodePreset(UnityEngine.Object nodePresetFile, VisualNode node)
        {
            NodePresetFile = nodePresetFile;
            Node = node;
        }
    }
    //���������˵��㼶
    public void Draw(Rect windowRect)
    {
        // ����۵�����
        EditorGUILayout.BeginHorizontal();
        IsFolded = EditorGUILayout.Foldout(IsFolded, LayerName);
        if (GUILayout.Button("ɾ�����㼶")) DeleteLayer?.Invoke();
        EditorGUILayout.EndHorizontal();

        //��дʵ������
        if (IsFolded)
        {
            // ��ʼ��������������ܸ߶�
            float totalHeight = EditorGUIUtility.singleLineHeight * (5 + list_NodePresets.Count);
            //ȡ�ó�ʼ�滭λ��
            Rect lastRect = GUILayoutUtility.GetLastRect();
            var drawpos = lastRect.position + Vector2.up * lastRect.height;
            // �ֶ����㱳���ľ������� ��ʼλ����һ�����
            Rect backgroundRect = new Rect(drawpos, new Vector2(windowRect.width, totalHeight));
            // ���Ʊ���
            EditorGUI.DrawRect(backgroundRect, new Color(255, 255, 255, 0.5f));
            // ����Ԥ����
            LayerName = EditorGUILayout.TextField("��ǰ�㼶����", LayerName);
            // �������ȼ�
            priority = EditorGUILayout.IntField("���ȼ���", priority);
            // ���Ƶ�ǰ�㼶��ÿ���ڵ������
            for (int i = 0; i < list_NodePresets.Count; i++)
            {
                VisualNode node = list_NodePresets[i].Node;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(node.NodeName, TextStyleHelper.Custom(12, node.drawInfo.TitleColor.color));

                // ����Դ�ļ���ʾ
                GUI.enabled = false;
                EditorGUILayout.ObjectField(list_NodePresets[i].NodePresetFile, typeof(UnityEngine.Object), false);
                GUI.enabled = true;
                if (GUILayout.Button("ɾ��")) { /* ɾ���߼� */ }
                if (GUILayout.Button("�ϵ�")) { /* �ϵ��߼� */ }
                if (GUILayout.Button("�µ�")) { /* �µ��߼� */ }
                EditorGUILayout.EndHorizontal();
            }

            // ������ק����
            DrawDragView();
        }
    }

    //ɾ�������㼶
    [NonSerialized]
    public UnityAction DeleteLayer;
    //����ÿ���㼶����ק����
    public void DrawDragView()
    {
        //ȡ�þ��ο��λ��
        dropArea = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2), GUILayout.ExpandWidth(true));
        //�ؿ�λ��5
        EditorGUILayout.Space(5);
        //���ƾ�����չ���� 
        GUI.Box(dropArea, "��ק�������� Ϊ�ò㼶��ӽڵ�Ԥ��");
        EditorGUILayout.Space(5);
    }
    public void HandleDragAndDrop()
    {
        Event e = Event.current;
        // ֻ�ھ��������ڴ�����ק
        if (dropArea.Contains(e.mousePosition))
        {
            switch (e.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    break; // ���ﲻ��Ҫe.Use()
                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    foreach (UnityEngine.Object draggedObject in DragAndDrop.objectReferences)
                    {

                        //��ȡ���ص�ַ
                        string ObjectPath = AssetDatabase.GetAssetPath(draggedObject);
                        //���м���
                        VisualNode visualNode = BinaryManager.Load<VisualNode>(EditorPathHelper.GetAbsolutePath(ObjectPath));
                        if (visualNode != null)
                        {
                            var nodePreset = new VNDropmenuLayer.VisualNodePreset(draggedObject, visualNode);
                            //�ȴ洢�ļ���Ψһ��ʶ
                            nodePreset.GUID = AssetDatabase.AssetPathToGUID(ObjectPath);
                            list_NodePresets.Add(nodePreset);
                        }
                    }
                    e.Use(); // ֻ���ڴ�������ק��ʹ���¼�
                    break;
            }
        }
    }

    //ˢ�µ�ǰ�㼶�ڵ㣬���������Ԥ�裬����ֱ��ͬ�������¼����߼�
    public void RefreshVisualNode()
    {

        for (int i = 0; i < list_NodePresets.Count; i++)
        {

            //�ȵõ��ڵ�Ԥ��
            var NodePreset = list_NodePresets[i];
            string LoadPath = AssetDatabase.GUIDToAssetPath(NodePreset.GUID);
            var fileObj = NodePreset.NodePresetFile != null ? NodePreset.NodePresetFile : AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(LoadPath);
            if (fileObj == null)
            {
                Debug.LogError("��⵽����Դ�ļ���ʧ����ʵ������������" + NodePreset.Node.NodeName);
                break;
            }
            //�������¼���
            VisualNode visualNode = BinaryManager.Load<VisualNode>(LoadPath);
            //��ֹ�仯����ز����Ĵ洢��ַ
            NodePreset.NodePresetFile = fileObj;
            NodePreset.Node = visualNode;
        }
    }
}