using UnityEditor;
using UnityEngine;

/// <summary>
/// 穹顶摄像机，用于渲染地形俯视图到贴图上，然后再窗口中显示渲染贴图
/// </summary>
public class TerrainTopViewCamera
{
    public win_TerrainEditor window;
    public QuadTerrain terrainComponent; // 地形组件
    public Camera topCamera; // 穹顶摄像机
    public float CameraViewSize;
    private float TerrainSize => terrainComponent.QuadData.Tree.MaxSize;
    public float CameraHeight = 100;
    public int renderTextureResolution = 1024; // RenderTexture 分辨率
    public Vector3 CameraPos => terrainComponent.transform.position + new Vector3(1, 0, 1) * TerrainSize / 2;
    private RenderTexture renderTexture;
    public TerrainTopViewCamera()
    {
        window = EditorWindow.GetWindow<win_TerrainEditor>();
    }
    /// <summary>
    /// 加载地形，并设置穹顶摄像机
    /// </summary>
    /// <param name="terrainComponent">地形组件</param>
    public void LoadTerrain(QuadTerrain terrainComponent)
    {
        this.terrainComponent = terrainComponent;
        // 先寻找看看有没有摄像机，没有就自动创建一个穹顶摄像机
        Transform cameraTrans = terrainComponent.transform.Find("TopViewCamera");
        if (cameraTrans == null)
        {
            CreateTopCamera();
        }
        else
        {
            topCamera = cameraTrans.GetComponent<Camera>();
        }
        //计算视口大小
        CameraViewSize = terrainComponent.QuadData.Tree.MaxSize / 2;
        //进行渲染出图
        RenderCamera();
    }
    /// <summary>
    /// 创建穹顶摄像机
    /// </summary>
    public void CreateTopCamera()
    {
        // 创建新的摄像机对象
        GameObject cameraObj = new GameObject("TopViewCamera");
        cameraObj.transform.SetParent(terrainComponent.transform);

        // 设置摄像机的位置和角度
        topCamera = cameraObj.AddComponent<Camera>();
        topCamera.transform.position = CameraPos + Vector3.up * CameraHeight; // 高度设为 TerrainSize
        topCamera.transform.rotation = Quaternion.Euler(90, 0, 0); // 俯视方向

        // 配置摄像机参数
        topCamera.orthographic = true; // 设置为正交摄像机
        topCamera.orthographicSize = CameraViewSize; // 设置正交摄像机视图大小
        topCamera.clearFlags = CameraClearFlags.SolidColor;
        topCamera.backgroundColor = Color.black; // 设置背景颜色为黑色
    }
    /// <summary>
    /// 渲染穹顶摄像机视图到 RenderTexture
    /// </summary>
    public void RenderCamera()
    {
        //设置摄像机
        topCamera.transform.position = CameraPos + Vector3.up * CameraHeight; // 高度设为 TerrainSize
        topCamera.transform.rotation = Quaternion.Euler(90, 0, 0); // 俯视方向
        topCamera.orthographicSize = CameraViewSize; // 设置正交摄像机视图大小
        if (topCamera == null)
        {
            Debug.LogError("找不到穹顶摄像机");
            return;
        }
        // 创建 RenderTexture
        if (renderTexture == null || renderTexture.width != renderTextureResolution)
        {
            renderTexture = new RenderTexture(renderTextureResolution, renderTextureResolution, 16);
            renderTexture.format = RenderTextureFormat.ARGB32;
            renderTexture.Create();
        }
        // 将摄像机的目标设置为 RenderTexture
        topCamera.targetTexture = renderTexture;
        // 手动渲染摄像机视图
        topCamera.Render();

        // 清理目标纹理（防止不必要的引用）
        topCamera.targetTexture = null;

    }
    /// <summary>
    /// 获取渲染完成的 RenderTexture
    /// </summary>
    public RenderTexture GetRenderTexture()
    {
        if (renderTexture == null)
        {
            Debug.LogError("贴图未能正常加载");

        }
        return renderTexture;
    }
}
