using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WDEditor;


//����������
public partial class winDraw_TerrainEditor : BaseWindowDraw<WinData_TerrainEditor>
{
    //��������
    public new win_TerrainEditor window => (base.window as win_TerrainEditor);
    /// <summary>
    /// �Ƿ�ɹ������˵���
    /// </summary>
    private bool isLoadedTerrain => SelectionTerrainComponent != null && SelectionTerrainComponent.IsGenerateQuadData && SelectionTerrainComponent.QuadData != null && TopViewTerrainCellDrawHelper != null;
    //��ǰѡ��ĵ���Obj�ĵ������
    private QuadTerrain SelectionTerrainComponent => window.SelectionTerrainComponent;
    //��ǰѡ��ĵ���Obj
    private UnityEngine.Object CurrentSelectionObj => window.CurrentSelectionObj;
    /// <summary>
    /// ��ǰѡ�е��ε����ļ�
    /// </summary>
    private TerrainQuadData treeData = null;
    public winDraw_TerrainEditor(EditorWindow window, WinData_TerrainEditor data) : base(window, data)
    {

    }
    //�Ƿ��۵�data����
    private bool isFoldEditToolArea = true;
    private bool isFoldTerrianDataInfoArea = false;
    private bool isFoldSettingArea;
    //����������˵�ѡ��
    private string[] MonsterPresetsOptions;
    //�ؿ����͵������˵�ѡ��
    private string[] TerrainTypePresetsOptions;
    //Mon+Type�������˵����Ʋ���
    private string[] TotalPresetsOptions;
    //��ǰѡ������
    private int SelectTotalPresetIndex;
    private int SelectMonsterPresetIndex;
    private int SelectTerrainTypePresetIndex;
    //��ʼ������
    //�������ƶ�λ��
    private DrawIndexHelper LeftPanelDrawHelper = new DrawIndexHelper();
    private DrawIndexHelper RightPanelDrawHelper = new DrawIndexHelper();
    //�������λ�úʹ�С
    private Rect LeftPanelRect
    {
        set => data.LeftPanelRect.rect = value;
        get => data.LeftPanelRect.rect;
    }
    //�Ҳ�����λ�úʹ�С
    private Rect RightPanelRect
    {
        set => data.RightPanelRect.rect = value;
        get => data.RightPanelRect.rect;
    }
    public override void OnCreated()
    {
        LoadSetting();
    }
    //���ص��ο�Ԥ��
    public void LoadSetting()
    {
        var dic = data.dic_CellColorSetting;
        string[] cellTypePresets = Enum.GetNames(typeof(TerrainCellInfo.E_TerrainCellType));
        string[] MonsterPresets = Enum.GetNames(typeof(TerrainCellInfo.E_MonsterCellType));
        //ѡ�ֵ
        TerrainTypePresetsOptions = cellTypePresets;
        MonsterPresetsOptions = MonsterPresets;
        //�ж��Ƿ񲻺��߼���Ҫ����
        if (dic == null || dic.Count != MonsterPresets.Length + cellTypePresets.Length)
        {
            Debug.LogError("dic��δnull��");
            data.dic_CellColorSetting = new Dictionary<string, SerializableColor>();
            //���
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
        //���dic�������صĻ�
        else
        {
            TotalPresetsOptions = dic.Keys.ToArray<string>();
        }

    }
    private void DrawNoSelectAnyTerrain()
    {
        GUILayout.Label("��δѡ���κε���",TextStyleHelper.Custom(12,Color.red));
    }
    //���Ƶ��α༭����������
    public override void Draw()
    {
        if(window.SelectionTerrainComponent==null)
        {
            DrawNoSelectAnyTerrain();
            return; 
        }
        //����������
        DrawLeftPanel();
        //�����ұ�����
        DrawRightPanel();
    }

}
