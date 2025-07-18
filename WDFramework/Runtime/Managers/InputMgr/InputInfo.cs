using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//һ������PC��������Ϣ��ÿ��������Ϣ������ÿ���¼�

/// <summary>
/// ������Ϣ
/// </summary>
public abstract class InputInfo
{
    public InputInfo(DE_InputKey Event)
    {
        this.inputEvent = Event;
    }
    protected DE_InputKey inputEvent;
    /// <summary>
    /// ��λ���£������Щ�����Ƿ�����
    /// </summary>
    public abstract void KeyUpdate();
    /// <summary>
    /// ��ֵ����
    /// </summary>
    public abstract void ValueUpdate();
    /// <summary>
    /// ����ȫ�������¼�����
    /// </summary>
    public abstract void TriggerUpdate();
}





/// <summary>
/// ���̻���갴�� ���� ̧�� ���ŵļ�λ��Ϣ
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
    //�������ͼ�������ֻ�ܴ���һ������һ����Ϊnull
    public int MouseID { get; private set; }
    public KeyCode keyCode { get; private set; }
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool isDown;
    /// <summary>
    /// �Ƿ��Ǽ�������
    /// </summary>
    public bool isKeyBoradInput => MouseID == -1 && keyCode != KeyCode.None;

}
public enum E_KeyInputType
{
    /// <summary>
    /// ����
    /// </summary>
    Down,
    /// <summary>
    /// ̧��
    /// </summary>
    Up,
    /// <summary>
    /// ����
    /// </summary>
    Always,
}
public enum E_MouseInputType
{
    MouseMove,
    MouseScroll
}
