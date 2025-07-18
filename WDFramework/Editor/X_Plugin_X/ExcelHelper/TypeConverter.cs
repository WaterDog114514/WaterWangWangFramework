using Excel;
using System;
using System.Data;
using System.IO;
using System.Linq;
/// <summary>
/// 类型转换工具类
/// 功能：将字符串转换为指定类型数据
/// 支持类型：int/float/double/bool/string 及其数组形式
/// </summary>
public static class TypeConverter
{
    /// <summary>
    /// 字符串到指定类型的转换
    /// </summary>
    /// <param name="value">原始字符串</param>
    /// <param name="targetType">目标类型</param>
    /// <returns>转换后的对象</returns>
    /// <exception cref="NotSupportedException">遇到不支持的类型</exception>
    public static object Convert(string value, Type targetType)
    {
        if (string.IsNullOrEmpty(value)) return null;

        try
        {
            // 基础类型处理
            if (targetType == typeof(int)) return int.Parse(value);
            else if (targetType == typeof(float)) return float.Parse(value);
            else if (targetType == typeof(double)) return double.Parse(value);
            else if (targetType == typeof(bool)) return bool.Parse(value);
            else if (targetType == typeof(string)) return value;

            // 数组类型处理（格式：value1|value2|value3）
            else if (targetType.IsArray)
            {
                Type elementType = targetType.GetElementType();
                var values = value.Split('|')
                    .Select(v => Convert(v, elementType))
                    .ToArray();

                // 创建动态数组
                Array array = Array.CreateInstance(elementType, values.Length);
                Array.Copy(values, array, values.Length);
                return array;
            }
            else
            {
                throw new NotSupportedException($"不支持的类型: {targetType.Name}");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"类型转换失败: {value} -> {targetType.Name}", ex);
        }
    }
}