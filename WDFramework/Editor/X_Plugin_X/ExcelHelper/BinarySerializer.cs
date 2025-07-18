using System.Collections.Generic;
using System.Data;
using System.Reflection;

using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// ���������л�ģ�飨��ȫ���
/// </summary>
public class ExcelBinarySerializer
{
    /// <summary>
    /// ��������ʵ���������BuildContainer����ֱ��Ǩ�ƶ�����
    /// </summary>
    public object BuildContainer(DataTable table, ExcelReadRule readRule)
    {
        // ������������
        Type containerType = FindContainerType(readRule.ConfigTypeName);
        if (containerType == null)
        {
            Debug.LogError($"{table.TableName}��δ���������������࣬�޷����ɶ������ļ�");
            return null;
        }

        // ��������ʵ��
        object containerInstance = Activator.CreateInstance(containerType);

        // ��ȡ�ֵ��ֶ���Ϣ
        FieldInfo fieldInfo = containerType.BaseType.GetField("container");
        Type dictionaryType = fieldInfo.FieldType;
        Type keyType = dictionaryType.GetGenericArguments()[0];
        Type valueType = dictionaryType.GetGenericArguments()[1];

        // �����ֵ�ʵ��
        object dictionaryInstance = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));
        MethodInfo addMethod = dictionaryType.GetMethod("Add");

        // �����ֶΣ����ӳ���������⣩
        List<FieldInfo> sortedFields = SortTableFields(table, valueType, readRule.PropertyNameRowIndex);

        // ���ж�ȡ���ݲ�����ֵ�
        for (int i =readRule.StartReadRowIndex; i < table.Rows.Count; i++)
        {
            // ����ע����
            if (table.Rows[i][0].ToString().StartsWith("//")) continue;

            // ��ȡ����ID
            string idValue = table.Rows[i][1].ToString();
            if (string.IsNullOrEmpty(idValue) || idValue.StartsWith("//")) continue;

            // ��������ֵʵ��
            object key = TypeConverter.Convert(idValue, keyType);
            object value = Activator.CreateInstance(valueType);

            // ����ֶ�����
            for (int j = 0; j < sortedFields.Count; j++)
            {
                FieldInfo field = sortedFields[j];
                object fieldValue = TypeConverter.Convert(table.Rows[i][j + 2].ToString(), field.FieldType);
                field.SetValue(value, fieldValue);
            }

            addMethod.Invoke(dictionaryInstance, new[] { key, value });
        }

        fieldInfo.SetValue(containerInstance, dictionaryInstance);
        return containerInstance;
    }

    // ���·�����ԭ����Ǩ�ƶ���
    private List<FieldInfo> SortTableFields(DataTable table, Type valueType, int propertyNameRowIndex)
    {
        List<string> fieldNames = new List<string>();
        for (int i = 2; i < table.Columns.Count; i++)
        {
            fieldNames.Add(table.Rows[propertyNameRowIndex][i].ToString());
        }

        FieldInfo[] allFields = valueType.GetFields();
        return fieldNames.Select(name => allFields.FirstOrDefault(f => f.Name == name)).Where(f => f != null).ToList();
    }

    public Type FindContainerType(string tableName)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == $"{tableName}Container");
            if (type != null) return type;
        }
        return null;
    }
}