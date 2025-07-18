using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//一套适配PC的输入信息，每个输入信息关联着每个事件

/// <summary>
/// 输入信息
/// </summary>
public abstract class InputInfo
{
    public InputInfo(DE_InputKey Event)
    {
        this.inputEvent = Event;
    }
    protected DE_InputKey inputEvent;
    /// <summary>
    /// 键位更新，检测这些按键是否按下了
    /// </summary>
    public abstract void KeyUpdate();
    /// <summary>
    /// 数值更新
    /// </summary>
    public abstract void ValueUpdate();
    /// <summary>
    /// 调用全局输入事件更新
    /// </summary>
    public abstract void TriggerUpdate();
}





/// <summary>
/// 键盘或鼠标按键 按下 抬起 按着的键位信息
/// </summary>
public class InputKey
{
    public InputKey(KeyCode key)
    {
        this.keyCode = key;
        this.MouseID = -1;
    }
    public InputKey(int MouseID)
    {
        this.MouseID = MouseID;
        this.keyCode = KeyCode.None;
    }
    //鼠标输入和键盘输入只能存在一个，另一个就为null
    public int MouseID { get; private set; }
    public KeyCode keyCode { get; private set; }
    /// <summary>
    /// 是否按下了
    /// </summary>
    public bool isDown;
    /// <summary>
    /// 是否是键盘输入
    /// </summary>
    public bool isKeyBoradInput => MouseID == -1 && keyCode != KeyCode.None;

}
public enum E_KeyInputType
{
    /// <summary>
    /// 按下
    /// </summary>
    Down,
    /// <summary>
    /// 抬起
    /// </summary>
    Up,
    /// <summary>
    /// 长按
    /// </summary>
    Always,
}
public enum E_MouseInputType
{
    MouseMove,
    MouseScroll
}
