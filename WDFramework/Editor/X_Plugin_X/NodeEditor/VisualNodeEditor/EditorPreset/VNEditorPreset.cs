using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WDEditor;
/// <summary>
//�ڵ�༭����Ԥ��
/// </summary>
[Serializable]
public class VNEditorPreset 
{
    //�༭����
    public string EditorName = null;
    [SerializeField]
    //�ڵ�˵���չ�б�
    public List<VNDropmenuLayer> dropmenuLayers = new List<VNDropmenuLayer>();
    //�����ڵ�˵��б�
 //   public Dictionary<string, Object> dic_CreateNodeDropmenu;
}
