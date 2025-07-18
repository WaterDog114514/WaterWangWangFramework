using UnityEngine;
/// <summary>
/// 用于编辑器开发中，自定义字体样式类
/// </summary>
public class TextStyleHelper
{
    public static GUIStyle Custom(int FontSize, Color FontColor)
    {
        // 创建一个新的GUIStyle
        GUIStyle CustomStyle = new GUIStyle(GUI.skin.label);
        // 设置字体大小
        CustomStyle.fontSize = FontSize;
        // 设置字体颜色
        CustomStyle.normal.textColor = FontColor;
        return CustomStyle;
    }
}
