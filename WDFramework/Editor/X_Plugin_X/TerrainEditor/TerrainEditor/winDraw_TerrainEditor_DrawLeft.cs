using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WDEditor;



public partial class winDraw_TerrainEditor : BaseWindowDraw<WinData_TerrainEditor>
{
    public void DrawLeftPanel()
    {
        LeftPanelDrawHelper.Update(LeftPanelRect);
        EditorGUI.DrawRect(LeftPanelRect, new Color(1, 1, 1, 0.5f));
        //��û�м��سɹ�ʱ�� 
        if (!isLoadedTerrain)
        {
            DrawNoCreateDataArea();
            return;
        }
        //������Ϣ��
        DrawTerrianDataArea();
        DrawEditToolArea();
        DrawShowSetting();
    }
    //���ƻ�û�д�������ʱ����߼�
    private void DrawNoCreateDataArea()
    {
        window.TerrainName = EditorGUI.TextField(LeftPanelDrawHelper.GetNextSingleRect(), "�������ƣ�", window.TerrainName);
        //���������
        data.QuadTreeSize = EditorGUI.IntField(LeftPanelDrawHelper.GetNextSingleRect(), "�Ĳ�����С(����)��", data.QuadTreeSize);
        data.MaxDepth = EditorGUI.IntField(LeftPanelDrawHelper.GetNextSingleRect(), "�Ĳ������ϸ����ȣ�", data.MaxDepth);
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "�����Ĳ��������ļ�"))
        {
            window.GenerateTerrainQuadData();
        }
    
    }
    //�������ϲ�����ݿ�
    public void DrawTerrianDataArea()
    {
        //�۵���
        isFoldTerrianDataInfoArea = EditorGUI.Foldout(LeftPanelDrawHelper.GetNextSingleRect(), isFoldTerrianDataInfoArea, "���������ļ���Ϣ");
        if (!isFoldTerrianDataInfoArea) { return; }
        //������ʽ�߼�
        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"��ǰ���� {CurrentSelectionObj.name} �������ļ���");
        //�ӳ��������л�ȡ�ļ���ֱ�Ӵֱ�
        if (SelectionTerrainComponent != null) { treeData = SelectionTerrainComponent.QuadData; } else treeData = null;

        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "��ǰ�����ļ���Ϣ��", TextStyleHelper.Custom(12, Color.green));
        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"�ܳ���:{treeData.Tree.MaxSize}");
        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"�ڵ�ϸ�����:{treeData.Tree.MaxDepth}");
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "���浱ǰ�����ļ�"))
        {
            window.SaveCurrentQuadData();    
        } 
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "ж�ص�ǰ�����ļ�"))
        {
            if (EditorUtility.DisplayDialog("��ȷ����", "ɾ�������ļ��󣬽��Ƴ������Ѿ��༭������", "ȷ��", "ȡ��"))
            {
                window.UnInstallTerrainData();
            }
        }

    }
    //�Ƿ��۵��༭����
    //���Ʊ༭����
    public void DrawEditToolArea()
    {
        //�۵���
        isFoldEditToolArea = EditorGUI.Foldout(LeftPanelDrawHelper.GetNextSingleRect(), isFoldEditToolArea, "�༭�ؿ鹤����");
        if (!isFoldEditToolArea) { return; }
        DrawEditToolArea_EditButton();
        DrawEditToolArea_PresetSelect();
    }
    //���Ʊ༭���顪���༭��ť
    private void DrawEditToolArea_EditButton()
    {

        //����ÿ����ť
        LeftPanelDrawHelper.BeginHorizontalLayout(4);
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "ʲôҲ����"))
        {
            TopViewTerrainCellDrawHelper.State_CurrentDrawTool = TopViewTerrainCellDrawHelper.E_DrawToolType.None;
        }
        Rect NoneArea = LeftPanelDrawHelper.GetLastDrawRect();
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "���õؿ�����"))
        {
            TopViewTerrainCellDrawHelper.State_CurrentDrawTool = TopViewTerrainCellDrawHelper.E_DrawToolType.Terrian;
        }
        Rect TerrainArea = LeftPanelDrawHelper.GetLastDrawRect();
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "���ù��������"))
        {
            TopViewTerrainCellDrawHelper.State_CurrentDrawTool = TopViewTerrainCellDrawHelper.E_DrawToolType.MonsterSpawn;
        }
        Rect MonsterArea = LeftPanelDrawHelper.GetLastDrawRect();
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "��������"))
        {
            TopViewTerrainCellDrawHelper.State_CurrentDrawTool = TopViewTerrainCellDrawHelper.E_DrawToolType.eraser;
        }
        Rect EraserArea = LeftPanelDrawHelper.GetLastDrawRect();
        LeftPanelDrawHelper.EndHorizontalLayout();
        //���ư�ťѡ��Ч��
        switch (TopViewTerrainCellDrawHelper.State_CurrentDrawTool)
        {
            case TopViewTerrainCellDrawHelper.E_DrawToolType.None:
                Handles.DrawSolidRectangleWithOutline(NoneArea, Color.clear, Color.green);
                break;
            case TopViewTerrainCellDrawHelper.E_DrawToolType.Terrian:
                Handles.DrawSolidRectangleWithOutline(TerrainArea, Color.clear, Color.green);
                break;
            case TopViewTerrainCellDrawHelper.E_DrawToolType.MonsterSpawn:
                Handles.DrawSolidRectangleWithOutline(MonsterArea, Color.clear, Color.green);
                break;
            case TopViewTerrainCellDrawHelper.E_DrawToolType.eraser:
                Handles.DrawSolidRectangleWithOutline(EraserArea, Color.clear, Color.green);
                break;
        }
    }
    //���Ʊ༭���顪��Ԥ��ѡ��
    private void DrawEditToolArea_PresetSelect()
    {
        switch (TopViewTerrainCellDrawHelper.State_CurrentDrawTool)
        {
            case TopViewTerrainCellDrawHelper.E_DrawToolType.None:
                break;
            case TopViewTerrainCellDrawHelper.E_DrawToolType.Terrian:
                SelectTerrainTypePresetIndex = EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(), "ѡ����ο�Ԥ��", SelectTerrainTypePresetIndex, TerrainTypePresetsOptions);
                TopViewTerrainCellDrawHelper.Index_DrawToolPresets = SelectTerrainTypePresetIndex;
                break;
            case TopViewTerrainCellDrawHelper.E_DrawToolType.MonsterSpawn:
                SelectMonsterPresetIndex = EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(), "ѡ��������Ԥ��", SelectMonsterPresetIndex, MonsterPresetsOptions);
                TopViewTerrainCellDrawHelper.Index_DrawToolPresets = SelectMonsterPresetIndex;
                break;
            case TopViewTerrainCellDrawHelper.E_DrawToolType.eraser:
                break;
        }
    }
    // ������ʾѡ��
    private void DrawShowSetting()
    {
        isFoldSettingArea = EditorGUI.Foldout(LeftPanelDrawHelper.GetNextSingleRect(), isFoldSettingArea, "����ѡ����");
        if (!isFoldSettingArea) { return; }
        //���ӻ���ѡ
        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "���ӻ�ѡ�");
        TopViewTerrainCellDrawHelper.isDrawEveryCell = EditorGUI.Toggle(LeftPanelDrawHelper.GetNextSingleRect(), "�Ƿ���ƿ��", TopViewTerrainCellDrawHelper.isDrawEveryCell);
        TopViewTerrainCellDrawHelper.isDrawTerrainTypeCell = EditorGUI.Toggle(LeftPanelDrawHelper.GetNextSingleRect(), "�Ƿ���Ƶ������ͱ�ǣ�", TopViewTerrainCellDrawHelper.isDrawTerrainTypeCell);
        TopViewTerrainCellDrawHelper.isDrawMonsterCell = EditorGUI.Toggle(LeftPanelDrawHelper.GetNextSingleRect(), "�Ƿ���Ƶ������ͱ�ǣ�", TopViewTerrainCellDrawHelper.isDrawMonsterCell);
        //���ƿ����ɫ�Զ���
        SelectTotalPresetIndex = EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(), "ѡ��Ԥ��", SelectTotalPresetIndex, TotalPresetsOptions);
        //��ɫfield
        data.dic_CellColorSetting[TotalPresetsOptions[SelectTotalPresetIndex]].color = EditorGUI.ColorField(LeftPanelDrawHelper.GetNextSingleRect(), "��ɫ��", data.dic_CellColorSetting[TotalPresetsOptions[SelectTotalPresetIndex]].color);

    }


}
