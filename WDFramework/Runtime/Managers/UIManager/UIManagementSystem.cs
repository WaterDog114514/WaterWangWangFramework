using UnityEngine.EventSystems;
using UnityEngine;
using WDFramework;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
/// <summary>
/// UI管理系统
/// </summary>
public class UIManagementSystem : Singleton<UIManagementSystem>, IFrameworkSystem
{
    //解耦者之王
    public EventManager<E_UIManagerEvent> eventManager;
    // 子模块
    private UILayerModuel layerManager { get; set; }
    private UIEventModuel uiEventManager { get; set; }
    private UIPanelModuel panelManager { get; set; }
    private UIInitializeModuel initializeManager { get; set; }

    public void InitializedSystem()
    {
        //初始化所有模块
        InitializeAllModules();
        //初始化UI控件
        initializeManager.InitializeUIComponents();
    }
    /// <summary>
    /// 初始化所有UI模块
    /// </summary>
    private void InitializeAllModules()
    {
        eventManager = new EventManager<E_UIManagerEvent>();
        layerManager = new UILayerModuel();
        uiEventManager = new UIEventModuel();
        panelManager = new UIPanelModuel();
        initializeManager = new UIInitializeModuel(layerManager);
    }
    /// <summary>
    /// 显示面板
    /// </summary>
    public void ShowPanel<T>(E_UILayer layer = E_UILayer.Middle) where T : UIBasePanel
    {
        //得到面板
        var panel = panelManager.GetPanel<T>();
        //设置
        panel.transform.SetParent(layerManager.GetLayer(layer), false);
        panel.ShowMe();
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    public void HidePanel<T>() where T : UIBasePanel
    {
        //得到面板
        var panel = panelManager.GetPanel<T>();
        //隐藏
        panel.HideMe();
    }
    //隐藏所有面板
    public void HideAllPanel()
    {
        var panels = panelManager.GetAllPanel();
        foreach (var panel in panels)
        {
            panel.HideMe();
        }
        
    }
    /// <summary>
    /// 销毁面板
    /// 但还在资源内存中，游戏对象销毁而已
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    public void DestroyPanel<T>() where T : UIBasePanel
    {
        panelManager.DestroyPanel<T>();
    }
    // 只用加载一次就好，无需重复加载
    // 可以通过全局事件调用|UI管理器调用
    public void LoadUIAssetBundle(UnityAction callback)
    {
        panelManager.LoadUIAssetBundle(callback);
    }

    /// <summary>
    /// 注册已经存在的panel
    /// </summary>
    /// <param name="PanelObj"></param>
    public void RegisterPanel<T>(GameObject PanelObj) where T : UIBasePanel
    {
        var panelInfo = PanelObj.GetComponent<T>();
        panelManager.RegisterPanel(panelInfo);
    }

}
/// <summary>
/// 层级枚举
/// </summary>
public enum E_UILayer
{
    /// <summary>
    /// 最底层
    /// </summary>
    Bottom,
    /// <summary>
    /// 中层
    /// </summary>
    Middle,
    /// <summary>
    /// 高层
    /// </summary>
    Top,
    /// <summary>
    /// 系统层 最高层
    /// </summary>
    System,
}