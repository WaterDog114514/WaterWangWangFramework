using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WDEditor;


//绘制主窗口
public partial class winDraw_TerrainEditor : BaseWindowDraw<WinData_TerrainEditor>
{
    //窗口引用
    public new win_TerrainEditor window => (base.window as win_TerrainEditor);
    /// <summary>
    /// 是否成功加载了地形
    /// </summary>
    private bool isLoadedTerrain => SelectionTerrainComponent != null && SelectionTerrainComponent.IsGenerateQuadData && SelectionTerrainComponent.QuadData != null && TopViewTerrainCellDrawHelper != null;
    //当前选择的地形Obj的地形组件
    private QuadTerrain SelectionTerrainComponent => window.SelectionTerrainComponent;
    //当前选择的地形Obj
    private UnityEngine.Object CurrentSelectionObj => window.CurrentSelectionObj;
    /// <summary>
    /// 当前选中地形的树文件
    /// </summary>
    private TerrainQuadData treeData = null;
    public winDraw_TerrainEditor(EditorWindow window, WinData_TerrainEditor data) : base(window, data)
    {

    }
    //是否折叠data区块
    private bool isFoldEditToolArea = true;
    private bool isFoldTerrianDataInfoArea = false;
    private bool isFoldSettingArea;
    //怪物的下拉菜单选择
    private string[] MonsterPresetsOptions;
    //地块类型的下拉菜单选择
    private string[] TerrainTypePresetsOptions;
    //Mon+Type的下拉菜单绘制参数
    private string[] TotalPresetsOptions;
    //当前选择索引
    private int SelectTotalPresetIndex;
    private int SelectMonsterPresetIndex;
    private int SelectTerrainTypePresetIndex;
    //初始化方法
    //辅助绘制定位器
    private DrawIndexHelper LeftPanelDrawHelper = new DrawIndexHelper();
    private DrawIndexHelper RightPanelDrawHelper = new DrawIndexHelper();
    //左侧面板的位置和大小
    private Rect LeftPanelRect
    {
        set => data.LeftPanelRect.rect = value;
        get => data.LeftPanelRect.rect;
    }
    //右侧面板的位置和大小
    private Rect RightPanelRect
    {
        set => data.RightPanelRect.rect = value;
        get => data.RightPanelRect.rect;
    }
    public override void OnCreated()
    {
        LoadSetting();
    }
    //加载地形块预设
    public void LoadSetting()
    {
        var dic = data.dic_CellColorSetting;
        string[] cellTypePresets = Enum.GetNames(typeof(TerrainCellInfo.E_TerrainCellType));
        string[] MonsterPresets = Enum.GetNames(typeof(TerrainCellInfo.E_MonsterCellType));
        //选项赋值
        TerrainTypePresetsOptions = cellTypePresets;
        MonsterPresetsOptions = MonsterPresets;
        //判断是否不合逻辑需要更新
        if (dic == null || dic.Count != MonsterPresets.Length + cellTypePresets.Length)
        {
            Debug.LogError("dic又未null了");
            data.dic_CellColorSetting = new Dictionary<string, SerializableColor>();
            //添加
            foreach (string item in cellTypePresets)
            {
                data.dic_CellColorSetting.Add(item, new SerializableColor(1, 1, 1, 1));
            }
            foreach (string item in MonsterPresets)
            {
                data.dic_CellColorSetting.Add(item, new SerializableColor(1, 1, 1, 1));
            }
            var tempPresetOptions = new List<string>();
            tempPresetOptions.AddRange(cellTypePresets);
            tempPresetOptions.AddRange(MonsterPresets);
            TotalPresetsOptions = tempPresetOptions.ToArray();
        }
        //如果dic正常加载的话
        else
        {
            TotalPresetsOptions = dic.Keys.ToArray<string>();
        }

    }
    private void DrawNoSelectAnyTerrain()
    {
        GUILayout.Label("还未选择任何地形",TextStyleHelper.Custom(12,Color.red));
    }
    //绘制地形编辑窗口主方法
    public override void Draw()
    {
        if(window.SelectionTerrainComponent==null)
        {
            DrawNoSelectAnyTerrain();
            return; 
        }
        //绘制左区块
        DrawLeftPanel();
        //绘制右边区块
        DrawRightPanel();
    }

}
