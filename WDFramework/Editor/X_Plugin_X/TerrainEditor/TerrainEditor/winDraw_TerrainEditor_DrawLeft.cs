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
        //还没有加载成功时候 
        if (!isLoadedTerrain)
        {
            DrawNoCreateDataArea();
            return;
        }
        //地形信息快
        DrawTerrianDataArea();
        DrawEditToolArea();
        DrawShowSetting();
    }
    //绘制还没有创建地形时候的逻辑
    private void DrawNoCreateDataArea()
    {
        window.TerrainName = EditorGUI.TextField(LeftPanelDrawHelper.GetNextSingleRect(), "地形名称：", window.TerrainName);
        //设置输入框
        data.QuadTreeSize = EditorGUI.IntField(LeftPanelDrawHelper.GetNextSingleRect(), "四叉树大小(长宽)：", data.QuadTreeSize);
        data.MaxDepth = EditorGUI.IntField(LeftPanelDrawHelper.GetNextSingleRect(), "四叉树最大细分深度：", data.MaxDepth);
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "生成四叉树地形文件"))
        {
            window.GenerateTerrainQuadData();
        }
    
    }
    //绘制最上层的数据块
    public void DrawTerrianDataArea()
    {
        //折叠块
        isFoldTerrianDataInfoArea = EditorGUI.Foldout(LeftPanelDrawHelper.GetNextSingleRect(), isFoldTerrianDataInfoArea, "地形配置文件信息");
        if (!isFoldTerrianDataInfoArea) { return; }
        //绘制正式逻辑
        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"当前地形 {CurrentSelectionObj.name} 的配置文件：");
        //从场景对象中获取文件，直接粗暴
        if (SelectionTerrainComponent != null) { treeData = SelectionTerrainComponent.QuadData; } else treeData = null;

        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "当前地形文件信息：", TextStyleHelper.Custom(12, Color.green));
        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"总长宽:{treeData.Tree.MaxSize}");
        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"节点细分深度:{treeData.Tree.MaxDepth}");
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "保存当前地形文件"))
        {
            window.SaveCurrentQuadData();    
        } 
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "卸载当前地形文件"))
        {
            if (EditorUtility.DisplayDialog("你确定吗？", "删除地形文件后，将移除所有已经编辑的玩意", "确定", "取消"))
            {
                window.UnInstallTerrainData();
            }
        }

    }
    //是否折叠编辑区块
    //绘制编辑区块
    public void DrawEditToolArea()
    {
        //折叠块
        isFoldEditToolArea = EditorGUI.Foldout(LeftPanelDrawHelper.GetNextSingleRect(), isFoldEditToolArea, "编辑地块工具栏");
        if (!isFoldEditToolArea) { return; }
        DrawEditToolArea_EditButton();
        DrawEditToolArea_PresetSelect();
    }
    //绘制编辑区块——编辑按钮
    private void DrawEditToolArea_EditButton()
    {

        //绘制每个按钮
        LeftPanelDrawHelper.BeginHorizontalLayout(4);
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "什么也不做"))
        {
            TopViewTerrainCellDrawHelper.State_CurrentDrawTool = TopViewTerrainCellDrawHelper.E_DrawToolType.None;
        }
        Rect NoneArea = LeftPanelDrawHelper.GetLastDrawRect();
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "设置地块类型"))
        {
            TopViewTerrainCellDrawHelper.State_CurrentDrawTool = TopViewTerrainCellDrawHelper.E_DrawToolType.Terrian;
        }
        Rect TerrainArea = LeftPanelDrawHelper.GetLastDrawRect();
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "设置怪物出生点"))
        {
            TopViewTerrainCellDrawHelper.State_CurrentDrawTool = TopViewTerrainCellDrawHelper.E_DrawToolType.MonsterSpawn;
        }
        Rect MonsterArea = LeftPanelDrawHelper.GetLastDrawRect();
        if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "擦除工具"))
        {
            TopViewTerrainCellDrawHelper.State_CurrentDrawTool = TopViewTerrainCellDrawHelper.E_DrawToolType.eraser;
        }
        Rect EraserArea = LeftPanelDrawHelper.GetLastDrawRect();
        LeftPanelDrawHelper.EndHorizontalLayout();
        //绘制按钮选中效果
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
    //绘制编辑区块——预设选择
    private void DrawEditToolArea_PresetSelect()
    {
        switch (TopViewTerrainCellDrawHelper.State_CurrentDrawTool)
        {
            case TopViewTerrainCellDrawHelper.E_DrawToolType.None:
                break;
            case TopViewTerrainCellDrawHelper.E_DrawToolType.Terrian:
                SelectTerrainTypePresetIndex = EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(), "选择地形块预设", SelectTerrainTypePresetIndex, TerrainTypePresetsOptions);
                TopViewTerrainCellDrawHelper.Index_DrawToolPresets = SelectTerrainTypePresetIndex;
                break;
            case TopViewTerrainCellDrawHelper.E_DrawToolType.MonsterSpawn:
                SelectMonsterPresetIndex = EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(), "选择怪物出生预设", SelectMonsterPresetIndex, MonsterPresetsOptions);
                TopViewTerrainCellDrawHelper.Index_DrawToolPresets = SelectMonsterPresetIndex;
                break;
            case TopViewTerrainCellDrawHelper.E_DrawToolType.eraser:
                break;
        }
    }
    // 绘制显示选项
    private void DrawShowSetting()
    {
        isFoldSettingArea = EditorGUI.Foldout(LeftPanelDrawHelper.GetNextSingleRect(), isFoldSettingArea, "设置选项栏");
        if (!isFoldSettingArea) { return; }
        //可视化勾选
        GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "可视化选项：");
        TopViewTerrainCellDrawHelper.isDrawEveryCell = EditorGUI.Toggle(LeftPanelDrawHelper.GetNextSingleRect(), "是否绘制框格：", TopViewTerrainCellDrawHelper.isDrawEveryCell);
        TopViewTerrainCellDrawHelper.isDrawTerrainTypeCell = EditorGUI.Toggle(LeftPanelDrawHelper.GetNextSingleRect(), "是否绘制地形类型标记：", TopViewTerrainCellDrawHelper.isDrawTerrainTypeCell);
        TopViewTerrainCellDrawHelper.isDrawMonsterCell = EditorGUI.Toggle(LeftPanelDrawHelper.GetNextSingleRect(), "是否绘制地形类型标记：", TopViewTerrainCellDrawHelper.isDrawMonsterCell);
        //绘制框格颜色自定义
        SelectTotalPresetIndex = EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(), "选择预设", SelectTotalPresetIndex, TotalPresetsOptions);
        //颜色field
        data.dic_CellColorSetting[TotalPresetsOptions[SelectTotalPresetIndex]].color = EditorGUI.ColorField(LeftPanelDrawHelper.GetNextSingleRect(), "颜色：", data.dic_CellColorSetting[TotalPresetsOptions[SelectTotalPresetIndex]].color);

    }


}
