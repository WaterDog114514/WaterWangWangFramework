using UnityEngine;
/// <summary>
/// ���ڱ༭�������У��Զ���������ʽ��
/// </summary>
public class TextStyleHelper
{
    public static GUIStyle Custom(int FontSize, Color FontColor)
    {
        // ����һ���µ�GUIStyle
        GUIStyle CustomStyle = new GUIStyle(GUI.skin.label);
        // ���������С
        CustomStyle.fontSize = FontSize;
        // ����������ɫ
        CustomStyle.normal.textColor = FontColor;
        return CustomStyle;
    }
}
