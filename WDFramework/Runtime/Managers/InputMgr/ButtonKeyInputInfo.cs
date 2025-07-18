using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按钮输入信息
/// </summary>
public class ButtonKeyInputInfo : InputInfo
{
    public E_KeyInputType inputType;
    public InputKey ButtonKey;

    public ButtonKeyInputInfo(DE_InputKey Event, InputKey Buttonkey, E_KeyInputType inputType) : base(Event)
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
            InputManager.Instance.eventManager.TriggerEvent(inputEvent);
    }


}