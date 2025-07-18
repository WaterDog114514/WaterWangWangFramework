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
//用来存储菜单列表的节点
[System.Serializable]
public class VNDropmenuLayer
{
    //拖拽区域
    [System.NonSerialized]
    private Rect dropArea;
    //优先级
    public int priority;
    //节点名
    public string LayerName;
    //是否折叠层级
    private bool IsFolded;
    //节点预设的list们
    public List<VisualNodePreset> list_NodePresets;

    public VNDropmenuLayer()
    {

        //创建列表
        Debug.Log("场景列表");
        list_NodePresets = new List<VisualNodePreset>();
    }

    //只用于菜单存储节点，顺便存储其instanceid，方便同步化修改
    [Serializable]
    public class VisualNodePreset
    {
        //每次重启编辑器，Object会丢失，只能通过NodeFilePath地址查找
        //节点预设本体Object文件
        [NonSerialized]
        public UnityEngine.Object NodePresetFile;
        public VisualNode Node;
        /// <summary>
        /// 他喵的唯一标识啊，用instanceID才是巨坑
        /// </summary>
        public string GUID;

        public VisualNodePreset(UnityEngine.Object nodePresetFile, VisualNode node)
        {
            NodePresetFile = nodePresetFile;
            Node = node;
        }
    }
    //绘制整个菜单层级
    public void Draw(Rect windowRect)
    {
        // 添加折叠功能
        EditorGUILayout.BeginHorizontal();
        IsFolded = EditorGUILayout.Foldout(IsFolded, LayerName);
        if (GUILayout.Button("删除本层级")) DeleteLayer?.Invoke();
        EditorGUILayout.EndHorizontal();

        //书写实际内容
        if (IsFolded)
        {
            // 开始计算内容区域的总高度
            float totalHeight = EditorGUIUtility.singleLineHeight * (5 + list_NodePresets.Count);
            //取得初始绘画位置
            Rect lastRect = GUILayoutUtility.GetLastRect();
            var drawpos = lastRect.position + Vector2.up * lastRect.height;
            // 手动计算背景的矩形区域 起始位置离一个间距
            Rect backgroundRect = new Rect(drawpos, new Vector2(windowRect.width, totalHeight));
            // 绘制背景
            EditorGUI.DrawRect(backgroundRect, new Color(255, 255, 255, 0.5f));
            // 绘制预设名
            LayerName = EditorGUILayout.TextField("当前层级名：", LayerName);
            // 绘制优先级
            priority = EditorGUILayout.IntField("优先级：", priority);
            // 绘制当前层级下每个节点的名字
            for (int i = 0; i < list_NodePresets.Count; i++)
            {
                VisualNode node = list_NodePresets[i].Node;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(node.NodeName, TextStyleHelper.Custom(12, node.drawInfo.TitleColor.color));

                // 绘制源文件显示
                GUI.enabled = false;
                EditorGUILayout.ObjectField(list_NodePresets[i].NodePresetFile, typeof(UnityEngine.Object), false);
                GUI.enabled = true;
                if (GUILayout.Button("删除")) { /* 删除逻辑 */ }
                if (GUILayout.Button("上调")) { /* 上调逻辑 */ }
                if (GUILayout.Button("下调")) { /* 下调逻辑 */ }
                EditorGUILayout.EndHorizontal();
            }

            // 绘制拖拽区域
            DrawDragView();
        }
    }

    //删除整个层级
    [NonSerialized]
    public UnityAction DeleteLayer;
    //绘制每个层级的拖拽区域
    public void DrawDragView()
    {
        //取得矩形宽高位置
        dropArea = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2), GUILayout.ExpandWidth(true));
        //拓宽位置5
        EditorGUILayout.Space(5);
        //绘制矩形拓展区域 
        GUI.Box(dropArea, "拖拽对象到这里 为该层级添加节点预设");
        EditorGUILayout.Space(5);
    }
    public void HandleDragAndDrop()
    {
        Event e = Event.current;
        // 只在矩形区域内处理拖拽
        if (dropArea.Contains(e.mousePosition))
        {
            switch (e.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    break; // 这里不需要e.Use()
                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    foreach (UnityEngine.Object draggedObject in DragAndDrop.objectReferences)
                    {

                        //获取加载地址
                        string ObjectPath = AssetDatabase.GetAssetPath(draggedObject);
                        //进行加载
                        VisualNode visualNode = BinaryManager.Load<VisualNode>(EditorPathHelper.GetAbsolutePath(ObjectPath));
                        if (visualNode != null)
                        {
                            var nodePreset = new VNDropmenuLayer.VisualNodePreset(draggedObject, visualNode);
                            //先存储文件的唯一标识
                            nodePreset.GUID = AssetDatabase.AssetPathToGUID(ObjectPath);
                            list_NodePresets.Add(nodePreset);
                        }
                    }
                    e.Use(); // 只有在处理完拖拽后使用事件
                    break;
            }
        }
    }

    //刷新当前层级节点，如果更改了预设，可以直接同步，重新加载逻辑
    public void RefreshVisualNode()
    {

        for (int i = 0; i < list_NodePresets.Count; i++)
        {

            //先得到节点预设
            var NodePreset = list_NodePresets[i];
            string LoadPath = AssetDatabase.GUIDToAssetPath(NodePreset.GUID);
            var fileObj = NodePreset.NodePresetFile != null ? NodePreset.NodePresetFile : AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(LoadPath);
            if (fileObj == null)
            {
                Debug.LogError("检测到数据源文件丢失——实在是搜索不到" + NodePreset.Node.NodeName);
                break;
            }
            //进行重新加载
            VisualNode visualNode = BinaryManager.Load<VisualNode>(LoadPath);
            //防止变化后加载不到的存储地址
            NodePreset.NodePresetFile = fileObj;
            NodePreset.Node = visualNode;
        }
    }
}