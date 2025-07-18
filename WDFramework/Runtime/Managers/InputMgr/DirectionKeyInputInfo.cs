using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 方向键输入信息
/// </summary>
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
    public DirectionKeyInputInfo(DE_InputKey Event, InputKey positive, InputKey negative, bool isFaded = true) : base(Event)
    {
        positiveKey = positive;
        negativeKey = negative;
        IsFaded = isFaded;
    }

    public override void KeyUpdate()
    {
        if (positiveKey != null)
        {
            if (positiveKey.isKeyBoradInput)
                positiveKey.isDown = Input.GetKey(positiveKey.keyCode);
            else
                positiveKey.isDown = Input.GetMouseButton(positiveKey.MouseID);
        }
        if (negativeKey != null)
        {
            if (negativeKey.isKeyBoradInput)
                negativeKey.isDown = Input.GetKey(negativeKey.keyCode);
            else
                negativeKey.isDown = Input.GetMouseButton(negativeKey.MouseID);
        }
    }

    public override void ValueUpdate()
    {
        // 计算增加或减少的值
        float changeValue = IsFaded ? 2f * Time.deltaTime : 1;

        // 检测正方向键
        if (positiveKey != null && positiveKey.isDown)
        {
            //回正修复
            if (Value < 0) Value = 0;
            Value += changeValue;
        }
        // 检测负方向键
        else if (negativeKey != null && negativeKey.isDown)
        {
            //回正修复
            if (Value > 0) Value = 0;
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
        //按不按都会触发
        InputManager.Instance.eventManager.TriggerEvent(inputEvent, Value);

        //if ((positiveKey != null && positiveKey.isDown) || (negativeKey != null && negativeKey.isDown))
        //{
        //}
    }
}