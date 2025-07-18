using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 所有UI面板的基类，提供控件查找和事件管理功能
/// </summary>
public abstract class UIBasePanel : MonoBehaviour
{
    protected UIManagementSystem uiManager=>UIManagementSystem.Instance;
    // 使用更高效的字典初始化和查找
    protected   Dictionary<string, UIBehaviour> dic_Controls = new Dictionary<string, UIBehaviour>();

    // 使用一个统一的委托字典来管理所有事件，减少内存占用
    private readonly Dictionary<string, Delegate> _eventHandlers = new Dictionary<string, Delegate>();
    /// <summary>
    /// 初始化面板内容
    /// </summary>
    protected abstract void InitializedPanel();
    // 只读静态集合，避免每次实例化时创建
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
    /// 获取指定名称和类型的UI控件
    /// </summary>
    public T GetControl<T>(string name) where T : UIBehaviour
    {
        if (dic_Controls.TryGetValue(name, out var control))
        {
            if (control is T typedControl)
            {
                return typedControl;
            }

            Debug.LogError($"控件 '{name}' 的类型不是 {typeof(T).Name}，实际类型是 {control.GetType().Name}");
            return null;
        }

        Debug.LogError($"未找到名为 '{name}' 的控件");
        return null;
    }

    /// <summary>
    /// 添加按钮点击事件监听
    /// </summary>
    public void AddButtonListener(string controlName, UnityAction callback)
    {
        if (GetControl<Button>(controlName) is { } button)
        {
            button.onClick.AddListener(callback);
        }
    }

    /// <summary>
    /// 添加Toggle值改变事件监听
    /// </summary>
    public void AddToggleListener(string controlName, UnityAction<bool> callback)
    {
        if (GetControl<Toggle>(controlName) is { } toggle)
        {
            toggle.onValueChanged.AddListener(callback);
        }
    }

    /// <summary>
    /// 添加Slider值改变事件监听
    /// </summary>
    public void AddSliderListener(string controlName, UnityAction<float> callback)
    {
        if (GetControl<Slider>(controlName) is { } slider)
        {
            slider.onValueChanged.AddListener(callback);
        }
    }

    /// <summary>s
    /// 获取并缓存所有UI子控件，非特殊名称不获取
    /// </summary>
    private void InitializeControls()
    {
        dic_Controls = new Dictionary<string, UIBehaviour>();
        // 按优先级查找控件类型
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
    /// 查找并缓存指定类型的控件
    /// </summary>
    private void FindAndCacheControls<T>() where T : UIBehaviour
    {
        foreach (var control in GetComponentsInChildren<T>(true))
        {
            var controlName = control.gameObject.name;

            // 跳过默认名称控件和已缓存的控件
            if (DefaultControlNames.Contains(controlName) || dic_Controls.ContainsKey(controlName))
            {
                continue;
            }

            dic_Controls.Add(controlName, control);
        }
    }

    /// <summary>
    /// 清除所有事件监听
    /// </summary>
    protected virtual void OnDestroy()
    {
        // 清除按钮事件
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