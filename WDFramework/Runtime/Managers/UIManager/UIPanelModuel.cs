
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WDFramework;
/// <summary>
/// UI面板管理器模块
/// 面板的存储、加载
/// </summary>
public class UIPanelModuel : UIBaseModuel
{
    /// <summary>
    /// 已加载的UI面板资源
    /// </summary>
    private Dictionary<string, UIBasePanel> panelDic;
    public override void InitializeModuel()
    {
        panelDic = new Dictionary<string, UIBasePanel>();
        //注册加载UI的AB包事件
        EventCenterSystem.Instance.AddEventListener<E_FrameworkEvent, UnityAction>(E_FrameworkEvent.LoadUIABPack, LoadUIAssetBundle, 0);
    }
    /// <summary>
    /// 加载UI界面的AB包
    /// </summary>
    // 只用加载一次就好，无需重复加载
    // 可以通过全局事件调用|UI管理器调用
    public void LoadUIAssetBundle(UnityAction callback = null)
    {
        ResLoader.Instance.LoadABPack(E_ABPackName.ui, callback);
    }
    public T GetPanel<T>() where T : UIBasePanel
    {
        string panelName = typeof(T).Name;
        //存在面板
        if (panelDic.ContainsKey(panelName))
        {
            Debug.Log("存在：" + panelName);
            //不存在面板 先存入字典当中 占个位置 之后如果又显示 我才能得到字典中的信息进行判断
            return panelDic[panelName] as T;
        }
        Debug.Log("不要存在：" + panelName);
        //不存在就同步加载把
        return LoadUIPanel<T>();
    }
    /// <summary>
    /// 得到所有已加载的面板
    /// </summary>
    /// <returns></returns>
    public List<UIBasePanel> GetAllPanel()
    {
        List<UIBasePanel> panels = new List<UIBasePanel>();
        panels.AddRange(panelDic.Values);
        return panels;
    }
    /// <summary>
    /// 从AB包中加载面板
    /// 读取规则：面板名和组件名一致
    /// </summary>
    private T LoadUIPanel<T>() where T : UIBasePanel
    {
        //直接从UI的AB包中获取预制体
        GameObject prefab = ResLoader.Instance.GetABPackRes<GameObject>(E_ABPackName.ui, typeof(T).Name);
        //实例化预制体
        GameObject uiObj = Object.Instantiate(prefab);
        T panelInfo = uiObj.GetComponent<T>();
        panelDic[typeof(T).Name] = panelInfo;
        //Object.DontDestroyOnLoad(uiObj);
        return panelInfo;
    }
    public void RegisterPanel<T>(T panel) where T : UIBasePanel
    {
        if (panel == null) return;
        panelDic[typeof(T).Name] = panel;
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
        panelDic.Remove(typeof(T).Name);
    }

}