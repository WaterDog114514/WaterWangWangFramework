using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����������Ϣ
/// </summary>
public class DirectionKeyInputInfo : InputInfo
{
    public InputKey positiveKey;
    public InputKey negativeKey;
    /// <summary>
    /// ���º��ı�ֵ
    /// </summary>
    public float Value;
    /// <summary>
    /// ��ֵ�仯�Ƿ�Ϊ���� ��Ļ�ֻ����-1��1
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
        // �������ӻ���ٵ�ֵ
        float changeValue = IsFaded ? 2f * Time.deltaTime : 1;

        // ����������
        if (positiveKey != null && positiveKey.isDown)
        {
            //�����޸�
            if (Value < 0) Value = 0;
            Value += changeValue;
        }
        // ��⸺�����
        else if (negativeKey != null && negativeKey.isDown)
        {
            //�����޸�
            if (Value > 0) Value = 0;
            Value -= changeValue;
        }
        // ���û�а������£��𽥽�Value���ٵ�0
        else
        {
            if (Value > 0)
            {
                Value -= changeValue;
                if (Value < 0) Value = 0; // ��ֹValue��Ϊ����
            }
            else if (Value < 0)
            {
                Value += changeValue;
                if (Value > 0) Value = 0; // ��ֹValue��Ϊ����
            }
        }
        // ȷ��Value��-1��1֮��
        Value = Mathf.Clamp(Value, -1, 1);
         

    }

    public override void TriggerUpdate()
    {
        //���������ᴥ��
        InputManager.Instance.eventManager.TriggerEvent(inputEvent, Value);

        //if ((positiveKey != null && positiveKey.isDown) || (negativeKey != null && negativeKey.isDown))
        //{
        //}
    }
}