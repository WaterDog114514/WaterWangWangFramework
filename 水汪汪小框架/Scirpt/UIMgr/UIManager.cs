using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

/// <summary>
/// 管理所有UI面板的管理器
/// 注意：面板预设体名要和面板类名一致！！！！！
/// </summary>
public class UIManager : Singleton_UnMono<UIManager>
{

    //ui基本控件
    private Camera uiCamera;
    private Canvas uiCanvas;
    private EventSystem uiEventSystem;

    //层级父对象
    private Transform bottomLayer;
    private Transform middleLayer;
    private Transform topLayer;
    private Transform systemLayer;

    /// <summary>
    /// 用于存储所有的面板对象
    /// </summary>
    private Dictionary<string, UIBasePanel> panelDic = new Dictionary<string, UIBasePanel>();
    private FrameworkSettingData SettingData;
    public UIManager()
    {
        IntiManager();
    }

    /// <summary>
    /// 初始化管理器，实例化出基本UI控件
    /// </summary>
    public void IntiManager()
    {
        //加载配置文件
        SettingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
        //动态创建唯一的Canvas和EventSystem（摄像机）
        uiCamera = GameObject.Instantiate(ResLoader.Instance.LoadRes_Sync<GameObject>("UI/UICamera")).GetComponent<Camera>();

        //动态创建Canvas
        uiCanvas = GameObject.Instantiate(ResLoader.Instance.LoadRes_Sync<GameObject>("UI/UICanvas")).GetComponent<Canvas>();
        //设置使用的UI摄像机
        uiCanvas.worldCamera = uiCamera;
        //找到层级父对象
        bottomLayer = uiCanvas.transform.Find("Bottom");
        middleLayer = uiCanvas.transform.Find("Middle");
        topLayer = uiCanvas.transform.Find("Top");
        systemLayer = uiCanvas.transform.Find("System");

        //动态创建EventSystem
        uiEventSystem = GameObject.Instantiate(ResLoader.Instance.LoadRes_Sync<GameObject>("UI/UIEventSystem")).GetComponent<EventSystem>();

        //过场景不移除基本控件
        Object.DontDestroyOnLoad(uiEventSystem.gameObject);
        Object.DontDestroyOnLoad(uiCanvas.gameObject);
        Object.DontDestroyOnLoad(uiCamera.gameObject);
    }
    /// <summary>
    /// 预加载面板
    /// </summary>
    /// <param name="abname"></param>
    /// <param name="panels"></param>
    public void PreLoadUIPanel(string abname, params string[] panelNames)
    {
        string[] paths = new string[panelNames.Length];
        for (int i = 0; i < panelNames.Length; i++)
        {
            if (!panelDic.ContainsKey(panelNames[i]))
                panelDic.Add(panelNames[i], null);
            paths[i] = abname + "/" + panelNames[i];

        }
        ResLoader.Instance.CreatePreloadTaskFromPaths(paths, (Panels) =>
        {
            for (int i = 0; i < Panels.Length; i++)
            {
                GameObject panelObj = Object.Instantiate(Panels[i].Asset as GameObject);
                UIBasePanel panelInfo = (panelObj).GetComponent<UIBasePanel>();
                panelDic[panelInfo.GetType().Name] = panelInfo;
                panelObj.transform.SetParent(middleLayer,false);
                panelInfo.HideMe();
            }
        });

    }

    /// <summary>
    /// 获取对应层级的父对象
    /// </summary>
    /// <param name="layer">层级枚举值</param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UILayer layer)
    {
        switch (layer)
        {
            case E_UILayer.Bottom:
                return bottomLayer;
            case E_UILayer.Middle:
                return middleLayer;
            case E_UILayer.Top:
                return topLayer;
            case E_UILayer.System:
                return systemLayer;
            default:
                return null;
        }
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板的类型</typeparam>
    /// <param name="layer">面板显示的层级</param>
    /// <param name="callBack">由于可能是异步加载 因此通过委托回调的形式 将加载完成的面板传递出去进行使用</param>
    /// <param name="isSync">是否采用同步加载 默认为false</param>
    public void ShowPanel<T>(E_UILayer layer = E_UILayer.Middle) where T : UIBasePanel
    {
        //通过面板名获取面板 预设体名必须和面板类名一致 
        T panelInfo = GetPanel<T>();

        //将面板预设体创建到对应父对象下 并且保持原本的缩放大小
        panelInfo.gameObject.transform.SetParent(GetLayerFather(layer), false);
        //所有一切就绪 面板也有了，就直接操作了
        //如果要显示面板 会执行一次面板的默认显示逻辑
        panelInfo.ShowMe();
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    public void HidePanel<T>() where T : UIBasePanel
    {
        if (!panelDic.ContainsKey(typeof(T).Name))
        {
            Debug.LogWarning($"隐藏{typeof(T).Name}面板失败，不存在此面板");
            return;
        }
        //获取
        T panelInfo = panelDic[typeof(T).Name] as T;

        //隐藏
        panelInfo.HideMe();
    }
    /// <summary>
    /// 销毁面板，但是加载的资源还在内存里，只不过只是毁灭踪迹罢了
    /// </summary>
    public void DestroyPanel<T>() where T : UIBasePanel
    {
        if (!panelDic.ContainsKey(typeof(T).Name))
        {
            Debug.LogWarning($"销毁{typeof(T).Name}面板失败，不存在此面板");
            return;
        }
        //获取
        T panelInfo = panelDic[typeof(T).Name] as T;

        //销毁面板
        Object.Destroy(panelInfo.gameObject);

        //从容器中移除
        panelDic.Remove(typeof(T).Name);
    }

    //同步加载面板。异步请用预加载！！
    public T LoadPanelSync<T>(string abName = null) where T : UIBasePanel
    {

        if (abName == null)
            abName = SettingData.abLoadSetting.UIPrefabPackName;
        //二重防null认证
        if (!panelDic.ContainsKey(typeof(T).Name))
        {
            panelDic.Add(typeof(T).Name, null);
        }
        GameObject uiObj = Object.Instantiate(ResLoader.Instance.LoadAB_Sync(abName, typeof(T).Name) as GameObject);
        T panelInfo = uiObj.GetComponent<T>();
        panelDic[typeof(T).Name] = panelInfo;
        return panelInfo;
    }

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <typeparam name="T">面板的类型</typeparam>
    public T GetPanel<T>() where T : UIBasePanel
    {
        string panelName = typeof(T).Name;
        //存在面板
        if (panelDic.ContainsKey(panelName) && panelDic[panelName] != null)
        {   //不存在面板 先存入字典当中 占个位置 之后如果又显示 我才能得到字典中的信息进行判断
            return panelDic[panelName] as T;
        }
        //不存在就同步加载把
        return LoadPanelSync<T>();
    }


    /// <summary>
    /// 为控件添加自定义事件
    /// </summary>
    /// <param name="control">对应的控件</param>
    /// <param name="type">事件的类型</param>
    /// <param name="callBack">响应的函数</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        //这种逻辑主要是用于保证 控件上只会挂载一个EventTrigger
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}
