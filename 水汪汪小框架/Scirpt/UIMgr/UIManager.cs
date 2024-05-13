using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    private Dictionary<string, UIBasePanelInfo> panelDic = new Dictionary<string, UIBasePanelInfo>();

    /// <summary>
    /// 初始化管理器，实例化出基本UI控件
    /// </summary>
    public void IntiManager()
    {
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




    public UIManager()
    {
        IntiManager();
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
    public void ShowPanel<T>(E_UILayer layer = E_UILayer.Middle, UnityAction<T> callBack = null, bool isSync = false) where T : UIBasePanel
    {
        //通过面板名获取面板 预设体名必须和面板类名一致 
        PanelInfo<T> panelInfo = GetPanel<T>(isSync);
        //根据是否异步加载完成而判断
        //if (panelInfo.PanelLoadTask.isFinish)
        //    ReallyShowPanel<T>(panelInfo, layer, callBack);
        //else
        //    panelInfo.PanelLoadTask.AddCallbackCommand(() =>
        //    {
        //        ReallyShowPanel<T>(panelInfo, layer, callBack);
        //    });
    }
    /// <summary>
    /// 真正控制面板显示隐藏方法
    /// </summary>
    private void ReallyShowPanel<T>(PanelInfo<T> panelInfo, E_UILayer layer = E_UILayer.Middle, UnityAction<T> callBack = null) where T : UIBasePanel
    {
        //将面板预设体创建到对应父对象下 并且保持原本的缩放大小
        panelInfo.panel.gameObject.transform.SetParent(GetLayerFather(layer), false);
        //所有一切就绪 面板也有了，就直接操作了
        //如果要显示面板 会执行一次面板的默认显示逻辑
        panelInfo.panel.ShowMe();
        //如果存在回调 直接返回出去即可
        callBack?.Invoke(panelInfo.panel);
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
        PanelInfo<T> panelInfo = panelDic[typeof(T).Name] as PanelInfo<T>;

        //隐藏
        panelInfo.panel.HideMe();
        panelInfo.panel.gameObject.SetActive(false);
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
        PanelInfo<T> panelInfo = panelDic[typeof(T).Name] as PanelInfo<T>;

        //销毁面板
        Object.Destroy(panelInfo.panel.gameObject);
        //从容器中移除
        panelDic.Remove(typeof(T).Name);
    }

    /// <summary>
    /// 第一次操作面板时候，面板没有被加载，则需要先加载
    /// </summary>
    //public LoadTask LoadPanel<T>(bool isSync = false) where T : UIBasePanel
    //{
    //    //二重防null认证
    //    string panelName = typeof(T).Name;
    //    //不存在面板
    //    if (!panelDic.ContainsKey(panelName))
    //        panelDic.Add(panelName, new PanelInfo<T>());

    //    //取出字典中已经占好位置的数据
    //    PanelInfo<T> panelInfo = panelDic[panelName] as PanelInfo<T>;

    //    //加载成功后回调
    //    UnityAction<GameObject> CreateCallback = (obj) =>
    //    {
    //        //实例化
    //        GameObject panelObj = Object.Instantiate(obj);
    //        Object.DontDestroyOnLoad(panelObj);
    //        T panel = panelObj.GetComponent<T>();
    //        //取出字典中已经占好位置的数据
    //        PanelInfo<T> panelInfo = panelDic[panelName] as PanelInfo<T>;
    //        //存储panel
    //        panelInfo.panel = panel;
    //    };
    //    //选择是否同步还是异步加载
    //    LoadTask task = null;
    //    if (!isSync)
    //    {

    //        //实战时候，大修一波加载，要加载整体一起，只加载单个面板很傻逼的哦
    //        //实战时候，大修一波加载，要加载整体一起，只加载单个面板很傻逼的哦
    //        //实战时候，大修一波加载，要加载整体一起，只加载单个面板很傻逼的哦

    //        //task = ResLoader.Instance.LoadAB_Async<GameObject>(FrameworkSetting.Instance.data.UIPrefabPackName, panelName, CreateCallback);
    //    }
    //    else
    //    {
    //       // task = ResLoader.Instance.LoadAB_Sync<GameObject>(FrameworkSetting.Instance.data.UIPrefabPackName, panelName);
    //        CreateCallback.Invoke((task.ResInfo as Res<GameObject>).asset);
    //    }
    //    //自动开始加载
    //    task.StartLoadTask();
    //    return task;
    //}

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <typeparam name="T">面板的类型</typeparam>
    public PanelInfo<T> GetPanel<T>(bool isSync = false) where T : UIBasePanel
    {
        string panelName = typeof(T).Name;
        //不存在面板
        if (!panelDic.ContainsKey(panelName))
        {   //不存在面板 先存入字典当中 占个位置 之后如果又显示 我才能得到字典中的信息进行判断
            panelDic.Add(panelName, new PanelInfo<T>());
        }

        //取出字典中已经占好位置的数据
        PanelInfo<T> panelInfo = panelDic[panelName] as PanelInfo<T>;
        //面板没加载，那就先加载面板
        if (panelInfo.panel == null)
        {
            //panelInfo.PanelLoadTask = LoadPanel<T>(isSync);
        }

        return panelInfo;

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
