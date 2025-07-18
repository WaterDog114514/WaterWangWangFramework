using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WDEditor;
/// <summary>
/// 下拉菜单专属拓展，避免太过于臃肿而创的类
/// 继承此类就可以达到重搞下拉菜单了
/// </summary>
public class VNEditorDropmenu
{
    private win_NodeEditor window;
    private VNEditorOperator Operator => window.Operator;
    //节点的菜单预设
    private List<VNDropmenuLayer> dropmenuLayersPreset;
    //编辑器空白处展示的菜单
    private GenericMenu ViewMenu = new GenericMenu();
    //编辑节点的菜单
    private GenericMenu OperatorNodeMenu = new GenericMenu();
    /// <summary>
    /// 下拉菜单大小
    /// </summary>
    public VNEditorDropmenu(win_NodeEditor Win, VNEditorPreset Preset)
    {
        window = Win;
        //如果是初始化创建，则创建一个为空的菜单
        if (Preset == null)
        {
            dropmenuLayersPreset = new List<VNDropmenuLayer>() { };
            return;
        }
        //非初始化的加载创建菜单，根据预设来创建
        dropmenuLayersPreset = Preset.dropmenuLayers;
        Inti_ViewMenu();
        Inti_NodeOperatorMenu();
    }
    private Vector2 LastClickPosition;
    public void Inti_ViewMenu()
    {
        for (int i = 0; i < dropmenuLayersPreset.Count; i++)
        {
            //当前层级下
            VNDropmenuLayer currentLayer = dropmenuLayersPreset[i];
            for (int j = 0; j < currentLayer.list_NodePresets.Count; j++)
            {
                VisualNode CurrentNode = currentLayer.list_NodePresets[j].Node;
                ViewMenu.AddItem(
                 new GUIContent("创建节点/" + currentLayer.LayerName + "/" + CurrentNode.NodeName),
                    false,
                    () =>
                    {
                        Operator.CreateNode(CurrentNode, LastClickPosition);
                    });
            }
        }
    }
    public void Inti_NodeOperatorMenu()
    {
        OperatorNodeMenu.AddItem(new GUIContent("连接另一节点"), false,
                  () =>
                  {
                      //进入连接状态
                      window.Checker.EnterLinkNodeState(TempOperatorMenuNode);
                  });
        OperatorNodeMenu.AddItem(new GUIContent("断开某进口节点"), false,
                () =>
                {

                });
        OperatorNodeMenu.AddItem(new GUIContent("断开某出口节点"), false,
                () =>
                {

                });
        OperatorNodeMenu.AddItem(new GUIContent("复制该节点"), false,
                () =>
                {

                });
        OperatorNodeMenu.AddItem(new GUIContent("删除该节点"), false,
             () =>
             {

             });
    }
    //展现视图的菜单
    public void ShowViewDropmenu(Vector2 pos)
    {
        //计算点击位置
        LastClickPosition = pos / window.data.ScaleValue;
        ViewMenu.DropDown(new Rect(pos, Vector2.zero));
    }


    private VisualNode TempOperatorMenuNode;
    //展现节点操作的菜单
    public void ShowOperatorNodeMenu(Vector2 pos, VisualNode OperatorNode)
    {
        //复制临时要操作的节点
        TempOperatorMenuNode = OperatorNode;
        //计算点击位置
        LastClickPosition = pos / window.data.ScaleValue;
        OperatorNodeMenu.DropDown(new Rect(pos, Vector2.zero));
    }



    private DrawIndexHelper DropmenuRectDrawHelper = new DrawIndexHelper();
    //显示要断开节点的菜单
    private void ShowUnLinkNodeDialog(VisualNode OperatorNode, bool IsEnter)
    {

      
    }
}


