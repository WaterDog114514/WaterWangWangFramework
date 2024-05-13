using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//一套适配PC的输入信息，每个输入信息关联着每个事件

/// <summary>
/// 输入信息
/// </summary>
public abstract class InputInfo
{
    public InputInfo(E_InputEvent Event)
    {
        this.inputEvent = Event;
    }
    protected E_InputEvent inputEvent;
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

public class ButtonKeyInputInfo : InputInfo
{
    public E_KeyInputType inputType;
    public InputKey ButtonKey;

    public ButtonKeyInputInfo(E_InputEvent Event,InputKey Buttonkey , E_KeyInputType inputType) : base(Event)
    {
        this.inputType = inputType;
        this.ButtonKey = Buttonkey;
    }

    public override void KeyUpdate()
    {
        switch (inputType)
        {
            case E_KeyInputType.Down:
                if (ButtonKey.isKeyBoradInput)
                    ButtonKey.isDown = Input.GetKeyDown(ButtonKey.keyCode);
                else
                    ButtonKey.isDown = Input.GetMouseButtonDown(ButtonKey.MouseID);
                break;

            case E_KeyInputType.Up:
                if (ButtonKey.isKeyBoradInput)
                    ButtonKey.isDown = Input.GetKeyUp(ButtonKey.keyCode);
                else
                    ButtonKey.isDown = Input.GetMouseButtonUp(ButtonKey.MouseID);
                break;

            case E_KeyInputType.Always:
                if (ButtonKey.isKeyBoradInput)
                    ButtonKey.isDown = Input.GetKey(ButtonKey.keyCode);
                else
                    ButtonKey.isDown = Input.GetMouseButton(ButtonKey.MouseID);
                break;
        }
    }
    public override void ValueUpdate()
    {

    }
    public override void TriggerUpdate()
    {
        if (ButtonKey.isDown)
            EventCenterManager.Instance.TriggerInputEvent(inputEvent);
    }


}

public class DirectionKeyInputInfo : InputInfo
{
    public InputKey positiveKey;
    public InputKey negativeKey;
    /// <summary>
    /// 按下后会改变值
    /// </summary>
    public float Value;
    /// <summary>
    /// 数值变化是否为渐变 否的话只纯在-1和1
    /// </summary>
    public bool IsFaded = true;
    public DirectionKeyInputInfo(E_InputEvent Event, InputKey positive, InputKey negative, bool isFaded = true) : base(Event)
    {
        positiveKey = positive;
        negativeKey = negative;
        IsFaded = isFaded;
    }

    public override void KeyUpdate()
    {
        if (positiveKey.isKeyBoradInput)
            positiveKey.isDown = Input.GetKey(positiveKey.keyCode);
        else
            positiveKey.isDown = Input.GetMouseButton(positiveKey.MouseID);

        if (negativeKey.isKeyBoradInput)
            negativeKey.isDown = Input.GetKey(negativeKey.keyCode);
        else
            negativeKey.isDown = Input.GetMouseButton(negativeKey.MouseID);

    }

    public override void ValueUpdate()
    {
        // 计算增加或减少的值
        float changeValue = IsFaded ? 2f * Time.deltaTime : 1;

        // 检测正方向键
        if (positiveKey.isDown)
        {
            Value += changeValue;
        }
        // 检测负方向键
        else if (negativeKey.isDown)
        {
            Value -= changeValue;
        }
        // 如果没有按键按下，逐渐将Value减少到0
        else
        {
            if (Value > 0)
            {
                Value -= changeValue;
                if (Value < 0) Value = 0; // 防止Value变为负数
            }
            else if (Value < 0)
            {
                Value += changeValue;
                if (Value > 0) Value = 0; // 防止Value变为正数
            }
        }

        // 确保Value在-1和1之间
        Value = Mathf.Clamp(Value, -1, 1);
    }

    public override void TriggerUpdate()
    {
        if (positiveKey.isDown || negativeKey.isDown)
            EventCenterManager.Instance.TriggerInputEvent(inputEvent);
    }
}
/// <summary>
/// 鼠标特殊输入，如鼠标移动 鼠标滚轮滑动
/// </summary>
public class MouseMoveOrScrollInfo : InputInfo
{
    /// <summary>
    /// 鼠标专属输入类型
    /// </summary>
    public E_MouseInputType inputType;
    /// <summary>
    /// 鼠标移动或者鼠标滚轮滑动值
    /// </summary>
    public Vector2 mouseDelta;
    public MouseMoveOrScrollInfo(E_InputEvent Event, E_MouseInputType inputType) : base(Event)
    {
        this.inputType = inputType;
    }
    public bool IsChange;
    public override void KeyUpdate()
    {
        switch (inputType)
        {
            case E_MouseInputType.MouseMove:
                IsChange = Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0 ? false : true;
                break;
            case E_MouseInputType.MouseScroll:
                IsChange = Input.mouseScrollDelta == Vector2.zero ? false : true;
                break;
        }

    }

    public override void ValueUpdate()
    {
        switch (inputType)
        {
            case E_MouseInputType.MouseScroll:
                mouseDelta = Input.mouseScrollDelta;
                break;
            case E_MouseInputType.MouseMove:
                mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                break;
        }
    }

    public override void TriggerUpdate()
    {
        if (IsChange)
            EventCenterManager.Instance.TriggerInputEvent(inputEvent);
    }
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
