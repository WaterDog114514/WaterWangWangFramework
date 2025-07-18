using UnityEngine;

public static class GUITextScaler
{
    /// <summary>
    /// 绘制自适应缩放的文本。
    /// </summary>
    /// <param name="rect">文本框的矩形区域。</param>
    /// <param name="text">要显示的文本。</param>
    /// <param name="maxFontSize">最大字体大小。</param>
    /// <param name="minFontSize">最小字体大小。</param>
    public static void DrawScaledLabel(Rect rect, string text, int maxFontSize = 30, int minFontSize = 1)
    {
        // 初始字体大小
        int fontSize = maxFontSize;

        // 测量文本尺寸
        GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = fontSize };
        Vector2 textSize = style.CalcSize(new GUIContent(text));

        // 根据 Rect 尺寸调整字体大小
        while (textSize.x > rect.width || textSize.y > rect.height)
        {
            fontSize--;
            if (fontSize < minFontSize)
            {
                fontSize = minFontSize;
                break;

            }
            style.fontSize = fontSize;
            textSize = style.CalcSize(new GUIContent(text));
        }
        // 设置文本对齐方式（可选）
        style.alignment = TextAnchor.MiddleCenter;
        // 绘制文本
        GUI.Label(rect, text, style);
    }
}
