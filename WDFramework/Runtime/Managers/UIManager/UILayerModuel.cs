using UnityEngine;
using WDFramework;
/// <summary>
/// ui层级管理模块
/// </summary>
public class UILayerModuel : UIBaseModuel
{

    private Transform bottomLayer;
    private Transform middleLayer;
    private Transform topLayer;
    private Transform systemLayer;

    public override void InitializeModuel()
    {
    }
    public void SetLayer(Transform canvasTransform)
    {
        bottomLayer = canvasTransform.Find("Bottom");
        middleLayer = canvasTransform.Find("Middle");
        topLayer = canvasTransform.Find("Top");
        systemLayer = canvasTransform.Find("System");
    }

    public Transform GetLayer(E_UILayer layer)
    {
        return layer switch
        {
            E_UILayer.Bottom => bottomLayer,
            E_UILayer.Middle => middleLayer,
            E_UILayer.Top => topLayer,
            E_UILayer.System => systemLayer,
            _ => null
        };
    }

}