using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIInitializeModuel : UIBaseModuel
{
    // 基础组件
    private UILayerModuel layerManager;
    private Canvas uiCanvas;
    private Camera uiCamera;
    private GameProjectSettingData.UISettingData settingData;
    public override void InitializeModuel()
    {
        settingData = SystemSettingLoader.Instance.LoadData<GameProjectSettingData>().uiSetting;
    }
    public UIInitializeModuel(UILayerModuel layerModuel)
    {
        layerManager = layerModuel;
    }

    /// <summary>
    /// 初始化并创建UI核心控件（完全代码创建版本）
    /// </summary>
    public void InitializeUIComponents()
    {
        // 创建UI摄像机
        GameObject cameraObj = new GameObject("UICamera");
        uiCamera = cameraObj.AddComponent<Camera>();
        // 设置摄像机属性
        uiCamera.clearFlags = CameraClearFlags.Depth;
        //uiCamera.cullingMask = LayerMask.GetMask("Everything");
        uiCamera.orthographic = true;
        uiCamera.orthographicSize = 5;
        uiCamera.nearClipPlane = 0.3f;
        uiCamera.farClipPlane = 1000f;
        uiCamera.depth = 10; // 确保UI摄像机在其他摄像机之上

        // 创建Canvas
        GameObject canvasObj = new GameObject("UICanvas");
        uiCanvas = canvasObj.AddComponent<Canvas>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        uiCanvas.worldCamera = uiCamera;
        // 与摄像机的距离
        uiCanvas.planeDistance = 100;
        // 添加CanvasScaler用于UI适配
        var uiCanvasScaler = canvasObj.AddComponent<CanvasScaler>();
        uiCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        uiCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        uiCanvasScaler.referenceResolution = new Vector2(settingData.ReferenceResolutionX, settingData.ReferenceResolutionY);
        uiCanvasScaler.matchWidthOrHeight = settingData.Match;
        // 添加GraphicRaycaster用于UI交互
        canvasObj.AddComponent<GraphicRaycaster>();

        // 创建四个层级
        CreateUILayer("Bottom", canvasObj.transform);
        CreateUILayer("Middle", canvasObj.transform);
        CreateUILayer("Top", canvasObj.transform);
        CreateUILayer("System", canvasObj.transform);
        // 设置层级管理器
        layerManager.SetLayer(uiCanvas.transform);
        // 创建EventSystem
        GameObject eventSystemObj = new GameObject("UIEventSystem");
        eventSystemObj.AddComponent<EventSystem>();
        eventSystemObj.AddComponent<StandaloneInputModule>(); // 添加输入模块
        //设置三聚合
        var UIObj = new GameObject("UI");
        cameraObj.transform.SetParent(UIObj.transform, false);
        canvasObj.transform.SetParent(UIObj.transform, false);
        eventSystemObj.transform.SetParent(UIObj.transform, false);
        // 过场景不移除基本控件
        Object.DontDestroyOnLoad(UIObj);
       // Object.DontDestroyOnLoad(eventSystemObj);
       // Object.DontDestroyOnLoad(canvasObj);
       // Object.DontDestroyOnLoad(cameraObj);
    }

    /// <summary>
    /// 创建UI层级对象
    /// </summary>
    /// <param name="name">层级名称</param>
    /// <param name="parent">父节点</param>
    /// <returns>创建的层级Transform</returns>
    private Transform CreateUILayer(string name, Transform parent)
    {
        GameObject layer = new GameObject(name);
        layer.transform.SetParent(parent);
        layer.transform.localPosition = Vector3.zero;
        layer.transform.localScale = Vector3.one;

        // 添加RectTransform并设置全屏拉伸
        RectTransform rt = layer.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        return layer.transform;
    }

}