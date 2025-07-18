using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 穹顶视图格子绘制助手
/// </summary>
public partial class TopViewTerrainCellDrawHelper
{

    //当前选中地块的Obj的地形组件
    public QuadTerrain QuadTerrainComponent;
    //当前选中地块的Obj
    public GameObject TerrianGameObject;

    // 是否绘制每个方格的框框
    public bool isDrawEveryCell = true;
    public bool isDrawTerrainTypeCell = true;
    public bool isDrawMonsterCell = true;
    // 与它息息相关的窗口本体
    public win_TerrainEditor window;
    /// <summary>
    /// 方便外部获取输入事件类
    /// </summary>
    public Event InputEvent => window.InputEvent;
    //射线检测助手
    private TopViewRaycastHandler TopViewRaycastHelper;
    //区块标记颜色预设
    public Dictionary<string, SerializableColor> dic_PresetColors => window.data.dic_CellColorSetting;
    //当前的绘制按钮选择状态
    public E_DrawToolType State_CurrentDrawTool;
    //当前画笔的预设序号，用它来转成Enum反馈给单个地块
    public int Index_DrawToolPresets;


   

    //绘制工具形态
    public enum E_DrawToolType
    {
        /// <summary>
        /// 什么也不做 指针状态
        /// </summary>
        None,
        /// <summary>
        /// 绘制地形块类型
        /// </summary>
        Terrian,
        /// <summary>
        /// 绘制怪物出生点
        /// </summary>
        MonsterSpawn,
        /// <summary>
        /// 橡皮擦
        /// </summary>
        eraser
    }
    /// <summary>
    /// 当前绘制的预设
    /// </summary>
    private Enum CurrentDrawPreset;
    public TopViewTerrainCellDrawHelper()
    {
        DEMOTTT = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        IntiEditor();
    }
    //初始化穹顶编辑器
    private void IntiEditor()
    {
        //选中时候打开地形编辑器
        window = EditorWindow.GetWindow<win_TerrainEditor>();
        //初始化射线检测者
        TopViewRaycastHelper = new TopViewRaycastHandler(this);
    }
    public void LoadTerrainData(QuadTerrain _quadTerrain, Rect drawRect)
    {
        //先赋其值
        QuadTerrainComponent = _quadTerrain;
        TerrianGameObject = _quadTerrain.gameObject;
        //加载所有绘制节点
        LoadDrawNodeDic(drawRect);
    }


    public void Draw()
    {
        if (QuadTerrainComponent == null || QuadTerrainComponent.QuadData == null) return;
        if (dic_DrawNodes.Count <= 0) return;
        //射线检测
        DrawNodeCell();
        DrawTerrainTagCell();
        TopViewRaycastHelper.Update();
        //mouseTest();
        //// 如果纹理是脏的，重新生成
        //if (isNeedRepaintTexture || gridTexture == null)
        //{
        //    RenderGridToTexture();
        //}

        //// 绘制缓存的纹理
        //if (gridTexture != null)
        //{
        //    GUI.DrawTexture(TopViewDrawRect, gridTexture);
        //    GUI.DrawTexture(TopViewDrawRect, DEMOTTT);
        //}
    }
    private Texture2D DEMOTTT;
    private void mouseTest()
    {
        // 监听鼠标点击事件，进行涂鸦
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0) // 左键点击
        {
            Vector2 mousePos = Event.current.mousePosition;
            if (TopViewDrawRect.Contains(mousePos))
            {
                // 将鼠标位置转换为纹理坐标
                Vector2 texturePos = new Vector2(mousePos.x - TopViewDrawRect.x, TopViewDrawRect.height - (mousePos.y - TopViewDrawRect.y));

                // 在纹理上进行涂鸦，不立即调用 Apply()
                DrawDoodle(texturePos);
                DEMOTTT.Apply();
            }
        }
    }

    // 在纹理上绘制涂鸦
    private void DrawDoodle(Vector2 texturePos)
    {
        int brushSize = 25; // 涂鸦的方块大小
        int startX = Mathf.FloorToInt(texturePos.x - brushSize / 2);
        int startY = Mathf.FloorToInt(texturePos.y - brushSize / 2);

        // 限制在纹理范围内
        startX = Mathf.Clamp(startX, 0, DEMOTTT.width - brushSize);
        startY = Mathf.Clamp(startY, 0, DEMOTTT.height - brushSize);

        // 创建一个黑色方块区域
        Color[] brushPixels = new Color[brushSize * brushSize];
        for (int i = 0; i < brushPixels.Length; i++)
        {
            brushPixels[i] = Color.black; // 黑色方块
        }

        // 修改纹理的像素区域，记住不调用 Apply()
        DEMOTTT.SetPixels(startX, startY, brushSize, brushSize, brushPixels);
    }

    //保存预设规范
    private enum SaveSettingName
    {
        QuadTerrianEditor_IsDrawEveryCell,
        QuadTerrianEditor_CurrentTool
    }
}



