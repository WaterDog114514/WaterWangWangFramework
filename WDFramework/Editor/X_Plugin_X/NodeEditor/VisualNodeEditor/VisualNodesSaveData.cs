using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 序列化节点编辑器整页的节点数据
[Serializable]
public class VisualNodesSaveData 
{
    public Dictionary<int, VisualNode> data;
}
