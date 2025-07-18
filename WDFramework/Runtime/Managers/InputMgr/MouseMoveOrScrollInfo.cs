using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����������룬������ƶ� �����ֻ���
/// </summary>
public class MouseMoveOrScrollInfo : InputInfo
{
    /// <summary>
    /// ���ר����������
    /// </summary>
    public E_MouseInputType inputType;
    /// <summary>
    /// ����ƶ����������ֻ���ֵ
    /// </summary>
    public Vector2 mouseDelta;
    public MouseMoveOrScrollInfo(DE_InputKey Event, E_MouseInputType inputType) : base(Event)
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
          InputManager.Instance.eventManager.TriggerEvent(inputEvent,mouseDelta);
    }
}