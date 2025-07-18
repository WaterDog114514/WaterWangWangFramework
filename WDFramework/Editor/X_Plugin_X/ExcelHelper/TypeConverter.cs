using Excel;
using System;
using System.Data;
using System.IO;
using System.Linq;
/// <summary>
/// ����ת��������
/// ���ܣ����ַ���ת��Ϊָ����������
/// ֧�����ͣ�int/float/double/bool/string ����������ʽ
/// </summary>
public static class TypeConverter
{
    /// <summary>
    /// �ַ�����ָ�����͵�ת��
    /// </summary>
    /// <param name="value">ԭʼ�ַ���</param>
    /// <param name="targetType">Ŀ������</param>
    /// <returns>ת����Ķ���</returns>
    /// <exception cref="NotSupportedException">������֧�ֵ�����</exception>
    public static object Convert(string value, Type targetType)
    {
        if (string.IsNullOrEmpty(value)) return null;

        try
        {
            // �������ʹ���
            if (targetType == typeof(int)) return int.Parse(value);
            else if (targetType == typeof(float)) return float.Parse(value);
            else if (targetType == typeof(double)) return double.Parse(value);
            else if (targetType == typeof(bool)) return bool.Parse(value);
            else if (targetType == typeof(string)) return value;

            // �������ʹ�����ʽ��value1|value2|value3��
            else if (targetType.IsArray)
            {
                Type elementType = targetType.GetElementType();
                var values = value.Split('|')
                    .Select(v => Convert(v, elementType))
                    .ToArray();

                // ������̬����
                Array array = Array.CreateInstance(elementType, values.Length);
                Array.Copy(values, array, values.Length);
                return array;
            }
            else
            {
                throw new NotSupportedException($"��֧�ֵ�����: {targetType.Name}");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"����ת��ʧ��: {value} -> {targetType.Name}", ex);
        }
    }
}