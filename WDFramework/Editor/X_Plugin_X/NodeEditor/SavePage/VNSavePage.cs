using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
/// <summary>
/// 序列化保存一页的信息
/// </summary>
public class VNSavePage
{
    public string OpenFilePath;
    //所有节点数据
    public Dictionary<int, VisualNode> dic_Nodes = new Dictionary<int, VisualNode>();
    //编辑器预设
    public VNEditorPreset editorPreset;
    //编辑器视口位置
    public SerializableVector2 ViewportPosition ;
    public float ScaleValue = 1F;
    public float ViewportScaleOffset = 1F;
}