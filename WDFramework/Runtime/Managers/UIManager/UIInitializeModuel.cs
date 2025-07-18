using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIInitializeModuel : UIBaseModuel
{
    // �������
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
    /// ��ʼ��������UI���Ŀؼ�����ȫ���봴���汾��
    /// </summary>
    public void InitializeUIComponents()
    {
        // ����UI�����
        GameObject cameraObj = new GameObject("UICamera");
        uiCamera = cameraObj.AddComponent<Camera>();
        // �������������
        uiCamera.clearFlags = CameraClearFlags.Depth;
        //uiCamera.cullingMask = LayerMask.GetMask("Everything");
        uiCamera.orthographic = true;
        uiCamera.orthographicSize = 5;
        uiCamera.nearClipPlane = 0.3f;
        uiCamera.farClipPlane = 1000f;
        uiCamera.depth = 10; // ȷ��UI����������������֮��

        // ����Canvas
        GameObject canvasObj = new GameObject("UICanvas");
        uiCanvas = canvasObj.AddComponent<Canvas>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        uiCanvas.worldCamera = uiCamera;
        // ��������ľ���
        uiCanvas.planeDistance = 100;
        // ���CanvasScaler����UI����
        var uiCanvasScaler = canvasObj.AddComponent<CanvasScaler>();
        uiCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        uiCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        uiCanvasScaler.referenceResolution = new Vector2(settingData.ReferenceResolutionX, settingData.ReferenceResolutionY);
        uiCanvasScaler.matchWidthOrHeight = settingData.Match;
        // ���GraphicRaycaster����UI����
        canvasObj.AddComponent<GraphicRaycaster>();

        // �����ĸ��㼶
        CreateUILayer("Bottom", canvasObj.transform);
        CreateUILayer("Middle", canvasObj.transform);
        CreateUILayer("Top", canvasObj.transform);
        CreateUILayer("System", canvasObj.transform);
        // ���ò㼶������
        layerManager.SetLayer(uiCanvas.transform);
        // ����EventSystem
        GameObject eventSystemObj = new GameObject("UIEventSystem");
        eventSystemObj.AddComponent<EventSystem>();
        eventSystemObj.AddComponent<StandaloneInputModule>(); // �������ģ��
        //�������ۺ�
        var UIObj = new GameObject("UI");
        cameraObj.transform.SetParent(UIObj.transform, false);
        canvasObj.transform.SetParent(UIObj.transform, false);
        eventSystemObj.transform.SetParent(UIObj.transform, false);
        // ���������Ƴ������ؼ�
        Object.DontDestroyOnLoad(UIObj);
       // Object.DontDestroyOnLoad(eventSystemObj);
       // Object.DontDestroyOnLoad(canvasObj);
       // Object.DontDestroyOnLoad(cameraObj);
    }

    /// <summary>
    /// ����UI�㼶����
    /// </summary>
    /// <param name="name">�㼶����</param>
    /// <param name="parent">���ڵ�</param>
    /// <returns>�����Ĳ㼶Transform</returns>
    private Transform CreateUILayer(string name, Transform parent)
    {
        GameObject layer = new GameObject(name);
        layer.transform.SetParent(parent);
        layer.transform.localPosition = Vector3.zero;
        layer.transform.localScale = Vector3.one;

        // ���RectTransform������ȫ������
        RectTransform rt = layer.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        return layer.transform;
    }

}