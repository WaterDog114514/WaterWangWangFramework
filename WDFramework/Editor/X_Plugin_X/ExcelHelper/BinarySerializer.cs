using System.Collections.Generic;
using System.Data;
using System.Reflection;

using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// 二进制序列化模块（完全解耦）
/// </summary>
public class ExcelBinarySerializer
{
    /// <summary>
    /// 构建容器实例（从你的BuildContainer方法直接迁移而来）
    /// </summary>
    public object BuildContainer(DataTable table, ExcelReadRule readRule)
    {
        // 查找容器类型
        Type containerType = FindContainerType(readRule.ConfigTypeName);
        if (containerType == null)
        {
            Debug.LogError($"{table.TableName}暂未生成容器和数据类，无法生成二进制文件");
            return null;
        }

        // 创建容器实例
        object containerInstance = Activator.CreateInstance(containerType);

        // 获取字典字段信息
        FieldInfo fieldInfo = containerType.BaseType.GetField("container");
        Type dictionaryType = fieldInfo.FieldType;
        Type keyType = dictionaryType.GetGenericArguments()[0];
        Type valueType = dictionaryType.GetGenericArguments()[1];

        // 创建字典实例
        object dictionaryInstance = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));
        MethodInfo addMethod = dictionaryType.GetMethod("Add");

        // 排序字段（解决映射排序问题）
        List<FieldInfo> sortedFields = SortTableFields(table, valueType, readRule.PropertyNameRowIndex);

        // 逐行读取数据并填充字典
        for (int i =readRule.StartReadRowIndex; i < table.Rows.Count; i++)
        {
            // 跳过注释行
            if (table.Rows[i][0].ToString().StartsWith("//")) continue;

            // 获取主键ID
            string idValue = table.Rows[i][1].ToString();
            if (string.IsNullOrEmpty(idValue) || idValue.StartsWith("//")) continue;

            // 创建键和值实例
            object key = TypeConverter.Convert(idValue, keyType);
            object value = Activator.CreateInstance(valueType);

            // 填充字段数据
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

    // 以下方法从原类中迁移而来
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