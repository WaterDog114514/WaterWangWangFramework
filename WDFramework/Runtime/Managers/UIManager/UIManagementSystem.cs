using UnityEngine.EventSystems;
using UnityEngine;
using WDFramework;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
/// <summary>
/// UI����ϵͳ
/// </summary>
public class UIManagementSystem : Singleton<UIManagementSystem>, IFrameworkSystem
{
    //������֮��
    public EventManager<E_UIManagerEvent> eventManager;
    // ��ģ��
    private UILayerModuel layerManager { get; set; }
    private UIEventModuel uiEventManager { get; set; }
    private UIPanelModuel panelManager { get; set; }
    private UIInitializeModuel initializeManager { get; set; }

    public void InitializedSystem()
    {
        //��ʼ������ģ��
        InitializeAllModules();
        //��ʼ��UI�ؼ�
        initializeManager.InitializeUIComponents();
    }
    /// <summary>
    /// ��ʼ������UIģ��
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
    /// ��ʾ���
    /// </summary>
    public void ShowPanel<T>(E_UILayer layer = E_UILayer.Middle) where T : UIBasePanel
    {
        //�õ����
        var panel = panelManager.GetPanel<T>();
        //����
        panel.transform.SetParent(layerManager.GetLayer(layer), false);
        panel.ShowMe();
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    public void HidePanel<T>() where T : UIBasePanel
    {
        //�õ����
        var panel = panelManager.GetPanel<T>();
        //����
        panel.HideMe();
    }
    //�����������
    public void HideAllPanel()
    {
        var panels = panelManager.GetAllPanel();
        foreach (var panel in panels)
        {
            panel.HideMe();
        }
        
    }
    /// <summary>
    /// �������
    /// ��������Դ�ڴ��У���Ϸ�������ٶ���
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    public void DestroyPanel<T>() where T : UIBasePanel
    {
        panelManager.DestroyPanel<T>();
    }
    // ֻ�ü���һ�ξͺã������ظ�����
    // ����ͨ��ȫ���¼�����|UI����������
    public void LoadUIAssetBundle(UnityAction callback)
    {
        panelManager.LoadUIAssetBundle(callback);
    }

    /// <summary>
    /// ע���Ѿ����ڵ�panel
    /// </summary>
    /// <param name="PanelObj"></param>
    public void RegisterPanel<T>(GameObject PanelObj) where T : UIBasePanel
    {
        var panelInfo = PanelObj.GetComponent<T>();
        panelManager.RegisterPanel(panelInfo);
    }

}
/// <summary>
/// �㼶ö��
/// </summary>
public enum E_UILayer
{
    /// <summary>
    /// ��ײ�
    /// </summary>
    Bottom,
    /// <summary>
    /// �в�
    /// </summary>
    Middle,
    /// <summary>
    /// �߲�
    /// </summary>
    Top,
    /// <summary>
    /// ϵͳ�� ��߲�
    /// </summary>
    System,
}