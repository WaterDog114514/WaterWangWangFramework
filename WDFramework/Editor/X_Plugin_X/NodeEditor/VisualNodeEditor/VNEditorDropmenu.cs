using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WDEditor;
/// <summary>
/// �����˵�ר����չ������̫����ӷ�׶�������
/// �̳д���Ϳ��Դﵽ�ظ������˵���
/// </summary>
public class VNEditorDropmenu
{
    private win_NodeEditor window;
    private VNEditorOperator Operator => window.Operator;
    //�ڵ�Ĳ˵�Ԥ��
    private List<VNDropmenuLayer> dropmenuLayersPreset;
    //�༭���հ״�չʾ�Ĳ˵�
    private GenericMenu ViewMenu = new GenericMenu();
    //�༭�ڵ�Ĳ˵�
    private GenericMenu OperatorNodeMenu = new GenericMenu();
    /// <summary>
    /// �����˵���С
    /// </summary>
    public VNEditorDropmenu(win_NodeEditor Win, VNEditorPreset Preset)
    {
        window = Win;
        //����ǳ�ʼ���������򴴽�һ��Ϊ�յĲ˵�
        if (Preset == null)
        {
            dropmenuLayersPreset = new List<VNDropmenuLayer>() { };
            return;
        }
        //�ǳ�ʼ���ļ��ش����˵�������Ԥ��������
        dropmenuLayersPreset = Preset.dropmenuLayers;
        Inti_ViewMenu();
        Inti_NodeOperatorMenu();
    }
    private Vector2 LastClickPosition;
    public void Inti_ViewMenu()
    {
        for (int i = 0; i < dropmenuLayersPreset.Count; i++)
        {
            //��ǰ�㼶��
            VNDropmenuLayer currentLayer = dropmenuLayersPreset[i];
            for (int j = 0; j < currentLayer.list_NodePresets.Count; j++)
            {
                VisualNode CurrentNode = currentLayer.list_NodePresets[j].Node;
                ViewMenu.AddItem(
                 new GUIContent("�����ڵ�/" + currentLayer.LayerName + "/" + CurrentNode.NodeName),
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
        OperatorNodeMenu.AddItem(new GUIContent("������һ�ڵ�"), false,
                  () =>
                  {
                      //��������״̬
                      window.Checker.EnterLinkNodeState(TempOperatorMenuNode);
                  });
        OperatorNodeMenu.AddItem(new GUIContent("�Ͽ�ĳ���ڽڵ�"), false,
                () =>
                {

                });
        OperatorNodeMenu.AddItem(new GUIContent("�Ͽ�ĳ���ڽڵ�"), false,
                () =>
                {

                });
        OperatorNodeMenu.AddItem(new GUIContent("���Ƹýڵ�"), false,
                () =>
                {

                });
        OperatorNodeMenu.AddItem(new GUIContent("ɾ���ýڵ�"), false,
             () =>
             {

             });
    }
    //չ����ͼ�Ĳ˵�
    public void ShowViewDropmenu(Vector2 pos)
    {
        //������λ��
        LastClickPosition = pos / window.data.ScaleValue;
        ViewMenu.DropDown(new Rect(pos, Vector2.zero));
    }


    private VisualNode TempOperatorMenuNode;
    //չ�ֽڵ�����Ĳ˵�
    public void ShowOperatorNodeMenu(Vector2 pos, VisualNode OperatorNode)
    {
        //������ʱҪ�����Ľڵ�
        TempOperatorMenuNode = OperatorNode;
        //������λ��
        LastClickPosition = pos / window.data.ScaleValue;
        OperatorNodeMenu.DropDown(new Rect(pos, Vector2.zero));
    }



    private DrawIndexHelper DropmenuRectDrawHelper = new DrawIndexHelper();
    //��ʾҪ�Ͽ��ڵ�Ĳ˵�
    private void ShowUnLinkNodeDialog(VisualNode OperatorNode, bool IsEnter)
    {

      
    }
}


