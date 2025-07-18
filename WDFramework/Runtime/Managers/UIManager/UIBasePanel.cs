using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ����UI���Ļ��࣬�ṩ�ؼ����Һ��¼�������
/// </summary>
public abstract class UIBasePanel : MonoBehaviour
{
    protected UIManagementSystem uiManager=>UIManagementSystem.Instance;
    // ʹ�ø���Ч���ֵ��ʼ���Ͳ���
    protected   Dictionary<string, UIBehaviour> dic_Controls = new Dictionary<string, UIBehaviour>();

    // ʹ��һ��ͳһ��ί���ֵ������������¼��������ڴ�ռ��
    private readonly Dictionary<string, Delegate> _eventHandlers = new Dictionary<string, Delegate>();
    /// <summary>
    /// ��ʼ���������
    /// </summary>
    protected abstract void InitializedPanel();
    // ֻ����̬���ϣ�����ÿ��ʵ����ʱ����
    private static readonly HashSet<string> DefaultControlNames = new HashSet<string>
    {
        "Image", "Text (TMP)", "RawImage", "Background", "Checkmark",
        "Label", "Text (Legacy)", "Arrow", "Placeholder", "Fill",
        "Handle", "Viewport", "Scrollbar Horizontal", "Scrollbar Vertical"
    };

    protected virtual void Awake()
    {
        InitializeControls();
        InitializedPanel();
    }

    public virtual void ShowMe() => gameObject.SetActive(true);
    public virtual void HideMe() => gameObject.SetActive(false);

    /// <summary>
    /// ��ȡָ�����ƺ����͵�UI�ؼ�
    /// </summary>
    public T GetControl<T>(string name) where T : UIBehaviour
    {
        if (dic_Controls.TryGetValue(name, out var control))
        {
            if (control is T typedControl)
            {
                return typedControl;
            }

            Debug.LogError($"�ؼ� '{name}' �����Ͳ��� {typeof(T).Name}��ʵ�������� {control.GetType().Name}");
            return null;
        }

        Debug.LogError($"δ�ҵ���Ϊ '{name}' �Ŀؼ�");
        return null;
    }

    /// <summary>
    /// ��Ӱ�ť����¼�����
    /// </summary>
    public void AddButtonListener(string controlName, UnityAction callback)
    {
        if (GetControl<Button>(controlName) is { } button)
        {
            button.onClick.AddListener(callback);
        }
    }

    /// <summary>
    /// ���Toggleֵ�ı��¼�����
    /// </summary>
    public void AddToggleListener(string controlName, UnityAction<bool> callback)
    {
        if (GetControl<Toggle>(controlName) is { } toggle)
        {
            toggle.onValueChanged.AddListener(callback);
        }
    }

    /// <summary>
    /// ���Sliderֵ�ı��¼�����
    /// </summary>
    public void AddSliderListener(string controlName, UnityAction<float> callback)
    {
        if (GetControl<Slider>(controlName) is { } slider)
        {
            slider.onValueChanged.AddListener(callback);
        }
    }

    /// <summary>s
    /// ��ȡ����������UI�ӿؼ������������Ʋ���ȡ
    /// </summary>
    private void InitializeControls()
    {
        dic_Controls = new Dictionary<string, UIBehaviour>();
        // �����ȼ����ҿؼ�����
        FindAndCacheControls<Button>();
        FindAndCacheControls<Toggle>();
        FindAndCacheControls<Slider>();
        FindAndCacheControls<InputField>();
        FindAndCacheControls<ScrollRect>();
        FindAndCacheControls<Dropdown>();
        FindAndCacheControls<Text>();
        FindAndCacheControls<TextMeshProUGUI>();
        FindAndCacheControls<Image>();
    }

    /// <summary>
    /// ���Ҳ�����ָ�����͵Ŀؼ�
    /// </summary>
    private void FindAndCacheControls<T>() where T : UIBehaviour
    {
        foreach (var control in GetComponentsInChildren<T>(true))
        {
            var controlName = control.gameObject.name;

            // ����Ĭ�����ƿؼ����ѻ���Ŀؼ�
            if (DefaultControlNames.Contains(controlName) || dic_Controls.ContainsKey(controlName))
            {
                continue;
            }

            dic_Controls.Add(controlName, control);
        }
    }

    /// <summary>
    /// ��������¼�����
    /// </summary>
    protected virtual void OnDestroy()
    {
        // �����ť�¼�
        foreach (var control in dic_Controls.Values)
        {
            if (control is Button button)
            {
                button.onClick.RemoveAllListeners();
            }
            else if (control is Toggle toggle)
            {
                toggle.onValueChanged.RemoveAllListeners();
            }
            else if (control is Slider slider)
            {
                slider.onValueChanged.RemoveAllListeners();
            }
        }

        dic_Controls.Clear();
        _eventHandlers.Clear();
    }
}