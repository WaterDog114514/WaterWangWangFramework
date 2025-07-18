using UnityEngine;

public static class GUITextScaler
{
    /// <summary>
    /// ��������Ӧ���ŵ��ı���
    /// </summary>
    /// <param name="rect">�ı���ľ�������</param>
    /// <param name="text">Ҫ��ʾ���ı���</param>
    /// <param name="maxFontSize">��������С��</param>
    /// <param name="minFontSize">��С�����С��</param>
    public static void DrawScaledLabel(Rect rect, string text, int maxFontSize = 30, int minFontSize = 1)
    {
        // ��ʼ�����С
        int fontSize = maxFontSize;

        // �����ı��ߴ�
        GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = fontSize };
        Vector2 textSize = style.CalcSize(new GUIContent(text));

        // ���� Rect �ߴ���������С
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
        // �����ı����뷽ʽ����ѡ��
        style.alignment = TextAnchor.MiddleCenter;
        // �����ı�
        GUI.Label(rect, text, style);
    }
}
