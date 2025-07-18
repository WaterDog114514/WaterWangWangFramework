
using System.Text;
using UnityEngine;
/// <summary>
/// �ı������࣬�������ǽ���һЩ�ı��Ĳ�������
/// </summary>
public static class TextTool
{
    //���������ٷ���װ��������

    private static string EnglishCharConversion(string text, char c)
    {
        switch (c)
        {
            case ';':
                return text.Replace('��', ';');
            case ':':
                return text.Replace('��', ';');
            default:
                return text;
        }
    }
    /// <summary>
    /// �ָ��ַ�����ø���ȫ��������Ϊ��Ӣ�����������ָ���
    /// </summary>
    /// <param name="text"></param>
    /// <param name="c"></param>
    /// <param name="isConversionEng"></param>
    /// <returns></returns>
    public static string[] SplitString(string text, char c, bool isConversionEng = true)
    {
        //��������ת��
        if (isConversionEng) text = EnglishCharConversion(text, c);
        return text.Split(c);
    }

    /// <summary>
    /// ��ָ������������С�����nλ
    /// </summary>
    /// <param name="value">����ĸ�����</param>
    /// <param name="len">����С�����nλ</param>
    /// <returns></returns>
    public static string GetDecimalStr(float value, int len)
    {
        //������Ҫ����С�����λС��
        return value.ToString($"F{len}");
    }
    private static StringBuilder stringBuilder = new StringBuilder();
    //��תʱ����ʾ���
    public static string SecondToHMS(int TotalSecond, bool IsRetainZero = false, string HourText = "ʱ", string MinuteText = "��", string SecondText = "��")
    {
        if(TotalSecond<=0)
            return 0+SecondText;
        stringBuilder.Clear();
        int hour = TotalSecond / 3600;
        int minute = TotalSecond % 3600 / 60;
        int second = TotalSecond % 60;
        if (hour != 0 || IsRetainZero)
            stringBuilder.Append(hour + HourText);
        if (minute != 0 || IsRetainZero)
            stringBuilder.Append(minute + MinuteText);
        if (second / 10 == 0)
            stringBuilder.Append("0" + second + SecondText);
        else
            stringBuilder.Append(second + SecondText);
        //������ͨ
        return stringBuilder.ToString();
    }
    /// <summary>
    /// ͨ���ֺŷָ�ʱ��
    /// </summary>
    /// <param name="TotalSecond"></param>
    /// <param name="IsRetainZero"></param>
    /// <returns></returns>
    public static string SecondToHMS_Semicolon(int TotalSecond, bool IsRetainZero = false)
    {
        return SecondToHMS(TotalSecond, IsRetainZero, ":", ":", "");
    }
}
