
using System.Text;
using UnityEngine;
/// <summary>
/// 文本工具类，方便我们进行一些文本的操作处理
/// </summary>
public static class TextTool
{
    //有需求了再疯狂封装方法即可

    private static string EnglishCharConversion(string text, char c)
    {
        switch (c)
        {
            case ';':
                return text.Replace('；', ';');
            case ':':
                return text.Replace('；', ';');
            default:
                return text;
        }
    }
    /// <summary>
    /// 分割字符，变得更安全，不会因为中英文问题而报错分割了
    /// </summary>
    /// <param name="text"></param>
    /// <param name="c"></param>
    /// <param name="isConversionEng"></param>
    /// <returns></returns>
    public static string[] SplitString(string text, char c, bool isConversionEng = true)
    {
        //进行中文转义
        if (isConversionEng) text = EnglishCharConversion(text, c);
        return text.Split(c);
    }

    /// <summary>
    /// 让指定浮点数保留小数点后n位
    /// </summary>
    /// <param name="value">具体的浮点数</param>
    /// <param name="len">保留小数点后n位</param>
    /// <returns></returns>
    public static string GetDecimalStr(float value, int len)
    {
        //代表想要保留小数点后几位小数
        return value.ToString($"F{len}");
    }
    private static StringBuilder stringBuilder = new StringBuilder();
    //秒转时间显示相关
    public static string SecondToHMS(int TotalSecond, bool IsRetainZero = false, string HourText = "时", string MinuteText = "分", string SecondText = "秒")
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
        //各显神通
        return stringBuilder.ToString();
    }
    /// <summary>
    /// 通过分号分割时间
    /// </summary>
    /// <param name="TotalSecond"></param>
    /// <param name="IsRetainZero"></param>
    /// <returns></returns>
    public static string SecondToHMS_Semicolon(int TotalSecond, bool IsRetainZero = false)
    {
        return SecondToHMS(TotalSecond, IsRetainZero, ":", ":", "");
    }
}
