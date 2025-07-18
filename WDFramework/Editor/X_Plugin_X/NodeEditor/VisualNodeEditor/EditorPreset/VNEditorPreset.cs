using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WDEditor;
/// <summary>
//节点编辑器的预设
/// </summary>
[Serializable]
public class VNEditorPreset 
{
    //编辑器名
    public string EditorName = null;
    [SerializeField]
    //节点菜单拓展列表
    public List<VNDropmenuLayer> dropmenuLayers = new List<VNDropmenuLayer>();
    //创建节点菜单列表
 //   public Dictionary<string, Object> dic_CreateNodeDropmenu;
}
