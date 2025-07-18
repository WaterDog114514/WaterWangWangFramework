using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
/// <summary>
/// ���л�����һҳ����Ϣ
/// </summary>
public class VNSavePage
{
    public string OpenFilePath;
    //���нڵ�����
    public Dictionary<int, VisualNode> dic_Nodes = new Dictionary<int, VisualNode>();
    //�༭��Ԥ��
    public VNEditorPreset editorPreset;
    //�༭���ӿ�λ��
    public SerializableVector2 ViewportPosition ;
    public float ScaleValue = 1F;
    public float ViewportScaleOffset = 1F;
}