using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 所有Ui面板的基类
/// </summary>
public abstract class UIBasePanel : MonoBehaviour
{
    /// <summary>
    /// 用于存储所有要用到的UI控件，用历史替换原则 父类装子类
    /// </summary>
    protected Dictionary<string, UIBehaviour> controlDic = new Dictionary<string, UIBehaviour>();

    //对于每种不同的控件事件，使用不同的字典进行监听
    protected Dictionary<string, UnityAction> dic_events_ClickButton = new Dictionary<string, UnityAction>();
    protected Dictionary<string, UnityAction<float>> dic_events_ChangeSlider = new Dictionary<string, UnityAction<float>>();
    protected Dictionary<string, UnityAction<bool>> dic_events_ClickToggel = new Dictionary<string, UnityAction<bool>>();

    /// <summary>
    /// 控件默认名字 如果得到的控件名字存在于这个容器 意味着我们不会通过代码去使用它 它只会是起到显示作用的控件
    /// </summary>
    private static List<string> defaultNameList = new List<string>() { "Image",
                                                                   "Text (TMP)",
                                                                   "RawImage",
                                                                   "Background",
                                                                   "Checkmark",
                                                                   "Label",
                                                                   "Text (Legacy)",
                                                                   "Arrow",
                                                                   "Placeholder",
                                                                   "Fill",
                                                                   "Handle",
                                                                   "Viewport",
                                                                   "Scrollbar Horizontal",
                                                                   "Scrollbar Vertical"};
    protected virtual void Awake()
    {
        FindChildControl();
    }
    /// <summary>
    /// 面板显示时会调用的逻辑
    /// </summary>


    public virtual void ShowMe()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 面板隐藏时会调用的逻辑
    /// </summary>
    public virtual void HideMe()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 获取指定名字以及指定类型的组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="name">组件名字</param>
    /// <returns></returns>
    public T GetControl<T>(string name) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(name))
        {
            T control = controlDic[name] as T;
            if (control == null)
                Debug.LogError($"不存在对应名字{name}类型为{typeof(T)}的组件");
            return control;
        }
        else
        {
            Debug.LogError($"不存在对应名字{name}的组件");
            return null;
        }
    }
    //通过给相对应控件名的控件添加监听即可
    public void AddListenerButtonClickEvent(string controlName, UnityAction operation)
    {
        if (!dic_events_ClickButton.ContainsKey(controlName))
        {
            Debug.LogError($"该面板不存在名为{controlName}的控件，监听订阅失败");
            return;
        }
        dic_events_ClickButton[controlName] = operation;
    }
    public void AddListenerChangeToggelEvent(string controlName, UnityAction<bool> operation)
    {
        if (!dic_events_ClickToggel.ContainsKey(controlName))
        {
            Debug.LogError($"该面板不存在名为{controlName}的控件，监听订阅失败");
            return;
        }
        dic_events_ClickToggel[controlName] = operation;
    }

    public void AddListenerChangeSliderEvent(string controlName, UnityAction<float> operation)
    {
        if (!dic_events_ChangeSlider.ContainsKey(controlName))
        {
            Debug.LogError($"该面板不存在名为{controlName}的控件，监听订阅失败");
            return;
        }
        dic_events_ChangeSlider[controlName] = operation;
    }

    //总和调用方法
    private void FindChildControl()
    {
        //我们应该优先查找重要的组件
        FindChildrenControl<Button>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<InputField>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Dropdown>();
        //即使对象上挂在了多个组件 只要优先找到了重要组件
        //之后也可以通过重要组件得到身上其他挂载的内容
        FindChildrenControl<Text>();
        FindChildrenControl<TextMeshPro>();
        FindChildrenControl<Image>();
    }
    //查找某一类组件所有的控件
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        //获取所有控件
        T[] controls = this.GetComponentsInChildren<T>(true);
        for (int i = 0; i < controls.Length; i++)
        {
            //获取当前控件的名字
            string controlName = controls[i].gameObject.name;
            //已经记录的，或者默认名的控件将不会记录下来
            if (controlDic.ContainsKey(controlName) || defaultNameList.Contains(controlName)) continue;

            controlDic.Add(controlName, controls[i]);
            //判断控件的类型 决定是否加事件监听
            if (controls[i] is Button)
            {
                dic_events_ClickButton.Add(controlName, null);
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    dic_events_ClickButton[controlName].Invoke();
                });
            }
            else if (controls[i] is Slider)
            {
                dic_events_ChangeSlider.Add(controlName, null);
                (controls[i] as Slider).onValueChanged.AddListener((value) =>
                {
                    dic_events_ChangeSlider[controlName].Invoke(value);
                });
            }
            else if (controls[i] is Toggle)
            {
                dic_events_ClickToggel.Add(controlName, null);
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    dic_events_ClickToggel[controlName].Invoke(value);
                });
            }
        }

    }
}
