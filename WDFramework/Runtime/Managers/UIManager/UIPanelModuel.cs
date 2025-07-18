
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WDFramework;
/// <summary>
/// UI��������ģ��
/// ���Ĵ洢������
/// </summary>
public class UIPanelModuel : UIBaseModuel
{
    /// <summary>
    /// �Ѽ��ص�UI�����Դ
    /// </summary>
    private Dictionary<string, UIBasePanel> panelDic;
    public override void InitializeModuel()
    {
        panelDic = new Dictionary<string, UIBasePanel>();
        //ע�����UI��AB���¼�
        EventCenterSystem.Instance.AddEventListener<E_FrameworkEvent, UnityAction>(E_FrameworkEvent.LoadUIABPack, LoadUIAssetBundle, 0);
    }
    /// <summary>
    /// ����UI�����AB��
    /// </summary>
    // ֻ�ü���һ�ξͺã������ظ�����
    // ����ͨ��ȫ���¼�����|UI����������
    public void LoadUIAssetBundle(UnityAction callback = null)
    {
        ResLoader.Instance.LoadABPack(E_ABPackName.ui, callback);
    }
    public T GetPanel<T>() where T : UIBasePanel
    {
        string panelName = typeof(T).Name;
        //�������
        if (panelDic.ContainsKey(panelName))
        {
            Debug.Log("���ڣ�" + panelName);
            //��������� �ȴ����ֵ䵱�� ռ��λ�� ֮���������ʾ �Ҳ��ܵõ��ֵ��е���Ϣ�����ж�
            return panelDic[panelName] as T;
        }
        Debug.Log("��Ҫ���ڣ�" + panelName);
        //�����ھ�ͬ�����ذ�
        return LoadUIPanel<T>();
    }
    /// <summary>
    /// �õ������Ѽ��ص����
    /// </summary>
    /// <returns></returns>
    public List<UIBasePanel> GetAllPanel()
    {
        List<UIBasePanel> panels = new List<UIBasePanel>();
        panels.AddRange(panelDic.Values);
        return panels;
    }
    /// <summary>
    /// ��AB���м������
    /// ��ȡ����������������һ��
    /// </summary>
    private T LoadUIPanel<T>() where T : UIBasePanel
    {
        //ֱ�Ӵ�UI��AB���л�ȡԤ����
        GameObject prefab = ResLoader.Instance.GetABPackRes<GameObject>(E_ABPackName.ui, typeof(T).Name);
        //ʵ����Ԥ����
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
    /// ������壬���Ǽ��ص���Դ�����ڴ��ֻ����ֻ�ǻ����ټ�����
    /// </summary>
    public void DestroyPanel<T>() where T : UIBasePanel
    {
        if (!panelDic.ContainsKey(typeof(T).Name))
        {
            Debug.LogWarning($"����{typeof(T).Name}���ʧ�ܣ������ڴ����");
            return;
        }
        //��ȡ
        T panelInfo = panelDic[typeof(T).Name] as T;
        //�������
        Object.Destroy(panelInfo.gameObject);
        panelDic.Remove(typeof(T).Name);
    }

}