using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>, IFrameworkSystem,IMonoUpdate
{
    /// <summary>
    /// 一个事件只能绑定一个输入信息，一个输入信息包括两个热键或单个鼠标事件
    /// </summary>
    public Dictionary<string, InputInfo> dic_Input = new Dictionary<string, InputInfo>();
    public DynamicEventManager<DE_InputKey> eventManager;
    public void InitializedSystem()
    {
        Debug.Log("已经初始化输入系统");
        //添加输入监听
        UpdateSystem.Instance.AddUpdateListener(E_UpdateLayer.FrameworkSystem,this);
    }
    //是否开启了输入系统检测
    private bool isOpenInputCheck = true;
    public InputManager()
    {
        eventManager = new DynamicEventManager<DE_InputKey>();
    }
    /// <summary>
    /// 开启或者关闭我们的输入管理模块的检测
    /// </summary>
    /// <param name="isStart"></param>
    public void StartOrCloseInputMgr(bool isStart)
    {
        this.isOpenInputCheck = isStart;
    }
    //输入方式在输入时候定好，后续想要修改输入方式，必须重新注册
    /// <summary>
    /// 注册带有方向的快捷键
    /// </summary>
    /// <param name="key"></param>
    /// <param name="inputType"></param>
    public void RegisterDirectionKeyInfo(DE_InputKey Event, InputKey PositiveKey, InputKey NegativeKey, bool isFaded = true)
    {
        string key =  Event.Name;
        //存在的话就改键 改键成功直接OK
        if (dic_Input.ContainsKey(key)) RemoveInputInfo(Event);

        //第一次初始化
        DirectionKeyInputInfo inputInfo = new DirectionKeyInputInfo(Event, PositiveKey, NegativeKey, isFaded);
        dic_Input.Add(key, inputInfo);

    }
    /// <summary>
    /// 注册鼠标输入事件
    /// </summary>
    public void RegisterMouseKeyInputInfo(DE_InputKey Event, E_MouseInputType e_MouseInputType)
    {
        string key =  Event.Name;
        //存在的话就删了重新注册
        if (dic_Input.ContainsKey(key))
            RemoveInputInfo(Event);
        //第一次初始化
        MouseMoveOrScrollInfo inputInfo = new MouseMoveOrScrollInfo(Event, e_MouseInputType);
        dic_Input.Add(key, inputInfo);

    }
    /// <summary>
    /// 注册普通键盘按钮输入事件
    /// </summary>
    public void RegisterButtonKeyInputInfo(DE_InputKey Event, InputKey ButtonKey, E_KeyInputType e_KeyInputType, bool isFaded = true)
    {
        string key = Event.Name;
        //存在的话就改键 改键成功直接OK
        if (dic_Input.ContainsKey(key)) RemoveInputInfo(Event);

        //第一次操作
        ButtonKeyInputInfo inputInfo = new ButtonKeyInputInfo(Event, ButtonKey, e_KeyInputType);
        dic_Input.Add(key, inputInfo);
    }
    //后续改键逻辑
    /// <summary>
    /// 改方向键，只能同时改一个，成功返回true
    /// </summary>
    /// <param name="Event"></param>
    /// <param name="Key"></param>
    /// <returns></returns>
    public bool ChangeDirectionKeyInfo(DE_InputKey Event, bool isPositive, InputKey Key)
    {
        string key = Event.Name;
        if (!dic_Input.ContainsKey(key))
        {
            Debug.LogError("改键失败,没有给该输入事件注册按键监听");
            return false;
        }
        DirectionKeyInputInfo inputInfo = dic_Input[key] as DirectionKeyInputInfo;
        if (inputInfo == null)
        {
            Debug.LogError("改键失败,该快捷键不是方向快捷键输入");
            return false;
        }
        //传None为不改
        if (isPositive) inputInfo.positiveKey = Key;
        else inputInfo.negativeKey = Key;
        return true;
    }
    /// <summary>
    /// 修改鼠标特殊输入 如是滚轮还是鼠标移动
    /// </summary>
    /// <param name="Event"></param>
    /// <param name="mouseID"></param>
    /// <returns></returns>
    public bool ChangeMouseKeyDownInfo(DE_InputKey Event, E_MouseInputType e_MouseInputType)
    {
        string key = Event.Name;
        if (!dic_Input.ContainsKey(key))
        {
            Debug.LogError("改键失败,没有给该输入事件注册按键监听");
            return false;
        }
        MouseMoveOrScrollInfo inputInfo = dic_Input[key] as MouseMoveOrScrollInfo;
        //那么就将其改为
        if (inputInfo == null)
        {
            Debug.LogError("改键失败,修改的不是鼠标特殊事件");
            return false;
        }
        inputInfo.inputType = e_MouseInputType;
        return true;
    }
    /// <summary>
    /// 改普通键盘快捷键
    /// </summary>
    /// <param name="Event"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ChangeButtonKeyInfo(DE_InputKey Event, InputKey keyCode)
    {
        string key = Event.Name;
        if (!dic_Input.ContainsKey(key))
        {
            Debug.LogError("改键失败,没有给该输入事件注册按键监听");
            return false;
        }
        ButtonKeyInputInfo inputInfo = dic_Input[key] as ButtonKeyInputInfo;
        if (inputInfo == null)
        {
            Debug.LogError("改键失败,该快捷键不是普通按钮输入");
            return false;
        }
        inputInfo.ButtonKey = keyCode;
        return true;
    }
    /// <summary>
    /// 移除指定输入事件行为的输入监听
    /// </summary>
    public void RemoveInputInfo(DE_InputKey Event)
    {
        string key =  Event.Name;
        if (dic_Input.ContainsKey(key))
            dic_Input.Remove(key);
    }
    /// <summary>
    /// 取得某按键信息
    /// </summary>
    /// <returns></returns>
    public InputInfo GetKeyInfo(DE_InputKey Event)
    {
        string key =  Event.Name;
        if (dic_Input.ContainsKey(key))
            return dic_Input[key];
        else return null;
    }
    #region 玩家输入改键相关
    /// <summary>
    /// 通过玩家输入来修改普通按键
    /// </summary>
    /// <param name="callBack"></param>
    public void ChangeButtonInfoFromInput(DE_InputKey Event)
    {
        UpdateSystem.Instance.StartCoroutine(StartChangeButtonFromInput(Event));
    }

    /// <summary>
    /// 通过玩家输入来修改方向热键
    /// </summary>
    /// <param name="callBack"></param>
    public void ChangeDirctionInfoFromInput(DE_InputKey Event, bool isPostive)
    {
        UpdateSystem.Instance.StartCoroutine(StartChangeDirectionFromInput(Event, isPostive));
    }

    /// <summary>
    /// 改键输入检测，它会暂时覆盖所有的输入操作，直至按下一个键位为止
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitKeyInputCheckFromInput(UnityAction<int, KeyCode> callback)
    {
        //等一帧
        yield return 0;
        //疯狂死循环检测，直至按下一个键位置

        //没有按键就等下一帧
        while (!Input.anyKeyDown) yield return null;
        Debug.Log("按了");
        //待会检测成功后传出去的键位
        KeyCode key = KeyCode.None;
        int mouseId = -1;
        //键盘和鼠标的按键输入
        //键盘
        Array keyCodes = Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode inputKey in keyCodes)
        {
            //判断到底是谁被按下了 那么就可以得到对应的输入的键盘信息
            if (Input.GetKeyDown(inputKey))
            {
                key = inputKey;
                break;
            }
        }
        //鼠标
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                mouseId = i;
                break;
            }
        }
        callback?.Invoke(mouseId, key);

    }
    //开始通过输入来进行改键 改的普通按键
    private IEnumerator StartChangeButtonFromInput(DE_InputKey Event)
    {
        yield return UpdateSystem.Instance.StartCoroutine(WaitKeyInputCheckFromInput((mouseId, keyCode) =>
        {
            InputInfo changeInfo = dic_Input[Event.Name];
            //按了鼠标
            if (mouseId != -1)
                ChangeButtonKeyInfo(Event, new InputKey(mouseId));
            //按了键盘
            else if (keyCode != KeyCode.None && mouseId == -1)
                ChangeButtonKeyInfo(Event, new InputKey(keyCode));
        }));

    }
    private IEnumerator StartChangeDirectionFromInput(DE_InputKey Event, bool isPositive)
    {
        yield return UpdateSystem.Instance.StartCoroutine(WaitKeyInputCheckFromInput((mouseId, key) =>
        {
            InputInfo changeInfo = dic_Input[Event.Name];
            //按了鼠标
            if (mouseId != -1)
                ChangeDirectionKeyInfo(Event, isPositive, new InputKey(mouseId));
            //按了键盘
            else if (key != KeyCode.None && mouseId == -1)
                ChangeDirectionKeyInfo(Event, isPositive, new InputKey(key));
        }));

    }
    #endregion
    
    //读取配置文件逻辑
    public void ReadConfig()
    {
        throw new NotImplementedException();
    }
    //保存配置文件逻辑
    public void SaveConfig()
    {
        throw new NotImplementedException("meixie");

    }
    //更新逻辑
    public void  MonoUpdate()
    {
        //如果外部没有开启检测功能 就不要检测
        if (!isOpenInputCheck)
            return;

        foreach (InputInfo info in dic_Input.Values)
        {
            //输入更新，检测按键按下
            info.KeyUpdate();
            //数值更新，数值改动
            info.ValueUpdate();
            //最后根据是否输入调用全局监听
            info.TriggerUpdate();
        }
    }
}
