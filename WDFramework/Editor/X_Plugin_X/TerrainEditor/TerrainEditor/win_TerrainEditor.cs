using System;
using System.IO;
using TreeEditor;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using WDEditor;
using WDFramework;
using static Codice.Client.Common.DiffMergeToolConfig;


/// <summary>
/// 地形编辑器，用来编辑规划地形预设，规划区域来生成
/// </summary>
public class win_TerrainEditor : BaseWindow<winDraw_TerrainEditor, WinData_TerrainEditor>
{
    /// <summary>
    /// 当前选中地形的地形名字
    /// </summary>
    public string TerrainName;
    private string SelectionTerrainDataPath = null;
    //选中物体的Mono组件
    public QuadTerrain SelectionTerrainComponent = null;
    //选中物体的Obj
    public UnityEngine.Object CurrentSelectionObj = null;
    public Event InputEvent => Event.current;
    /// <summary>
    /// 穹顶摄像机组件
    /// </summary>
    public TerrainTopViewCamera topViewCamera;
    /// <summary>
    /// 地形编辑器绘制助手的实例
    /// </summary>
    public TopViewTerrainCellDrawHelper TopViewDrawHelper;
    #region 初始化操作
    [MenuItem("只因终焉/地形编辑器")]
    protected static void OpenWindow()
    {
        EditorWindow.GetWindow<win_TerrainEditor>();
    }

    public override void OnOpenWindow()
    {
        //初始化窗口大小自适应设置
        IntiPanelSize();
        //创建绘制助手
        CreateWinDraw();
        //添加选择地形自动加载监听
        AddUpdateListener(SelectionUpdate);
        //初始化穹顶编辑器
        TopViewDrawHelper = new TopViewTerrainCellDrawHelper();
        //初始化摄像机
        topViewCamera = new TerrainTopViewCamera();
    }
    public override void OnCloseWindow()
    {

    }
    private void IntiPanelSize()
    {
        //每次都要调整一下矩形宽高
        AddUpdateListener(() =>
        {
            //左侧面板宽度设置为1/4 位置居于(0,0)
            data.LeftPanelRect.rect = new Rect(0, 0, position.width / 4, position.height);
            //右侧面板宽度设置为3/4 位置居于左侧面板之右
            data.RightPanelRect.rect = new Rect(data.LeftPanelRect.width, 0, position.width * 3 / 4, position.height);
        });

    }
    #endregion
    /// <summary>
    /// 删除地形文件
    /// </summary>
    public void UnInstallTerrainData()
    {
        SelectionTerrainComponent.IsGenerateQuadData = false;
        SelectionTerrainComponent.TerrainName = null;
        SelectionTerrainComponent.QuadData = null;
        SelectionTerrainComponent = null;
        TerrainName = null;
        File.Delete(SelectionTerrainDataPath);
        File.Delete(SelectionTerrainDataPath+".meta");
        AssetDatabase.Refresh();
    }
    //更新选择不同地形
    public void SelectionUpdate()
    {
        if (CurrentSelectionObj != Selection.activeObject)
        {
            CurrentSelectionObj = Selection.activeObject;
            //判断是否选的是“地形物体”
            if (CurrentSelectionObj != null && CurrentSelectionObj is GameObject && (CurrentSelectionObj as GameObject).GetComponent<QuadTerrain>() != null)
            {
                //选中时候给 整个窗口 和 穹顶编辑器 加载地形数据文件
                LoadTerrainData();
                //找到了重绘面板
                Repaint();
                return;
            }
            //取消选中
            else
            {
                Repaint();
                SelectionTerrainComponent = null;
            }
        }
    }
    //给 整个窗口 和 穹顶编辑器 加载地形数据文件
    public void LoadTerrainData()
    {

        //得到要加载的地形文件
        var loadTerrainComponent = (CurrentSelectionObj as GameObject).GetComponent<QuadTerrain>();
        //不是地形就跳过
        if (loadTerrainComponent == null) return;
        SelectionTerrainComponent = loadTerrainComponent;
        //是否生成过地形文件，没生产就不要加载了 || 加载过也跳过
        if (!loadTerrainComponent.IsGenerateQuadData && SelectionTerrainComponent == loadTerrainComponent) return;

        //真正意义加载逻辑↓↓

        //加载地形的地形名
        TerrainName = loadTerrainComponent.TerrainName;
        //加载地形的文件地址
        SelectionTerrainDataPath = Application.streamingAssetsPath + $"\\QuadTerrainData\\{TerrainName}.terrainquaddata";

        if (SelectionTerrainComponent.QuadData == null) SelectionTerrainComponent.LoadSelfData(SelectionTerrainDataPath);
        //加载不到
        if (SelectionTerrainComponent.QuadData == null)
        {
            Debug.LogError("当前选择的地形文件已缺失，加载失败！！");
            return;
        }
        //给穹顶编辑器也加载
        TopViewDrawHelper.LoadTerrainData(loadTerrainComponent, winDraw.ImageDrawRect);
        //加载穹顶摄像机
        topViewCamera.LoadTerrain(loadTerrainComponent);
    }

    /// <summary>
    /// 生成地形配置文件
    /// </summary>
    public void GenerateTerrainQuadData()
    {
        if (string.IsNullOrEmpty(TerrainName))
        {
            EditorUtility.DisplayDialog("生成错误！", "地形名不能为null", "好的");
            return;
        }
        //得到文件路径
        SelectionTerrainDataPath = Application.streamingAssetsPath + $"\\QuadTerrainData\\{TerrainName}.terrainquaddata";
        //开始生成树
        TerrainQuadData quadData = new TerrainQuadData();
        quadData.Tree = new QuadTree<TerrainCellInfo>();
        quadData.Tree.GenerateQuadTree(data.QuadTreeSize, data.MaxDepth);
        //赋值深度
        quadData.Tree.MaxDepth = data.MaxDepth;
        //赋值宽度
        quadData.Tree.MaxSize = data.QuadTreeSize;
        //暂时版赋值给场景对象
        SelectionTerrainComponent.QuadData = quadData;
        SelectionTerrainComponent.TerrainName = TerrainName;
        //标记已经加载
        SelectionTerrainComponent.IsGenerateQuadData = true;
        //给穹顶编辑器也加载
        TopViewDrawHelper.LoadTerrainData(SelectionTerrainComponent, winDraw.ImageDrawRect);
        //加载穹顶摄像机
        topViewCamera.LoadTerrain(SelectionTerrainComponent);
        //写入到本地
        SaveCurrentQuadData();
    }
    public void SaveCurrentQuadData()
    {
        BinaryManager.SaveToPath(SelectionTerrainComponent.QuadData, SelectionTerrainDataPath);
        AssetDatabase.Refresh();
    }

}
