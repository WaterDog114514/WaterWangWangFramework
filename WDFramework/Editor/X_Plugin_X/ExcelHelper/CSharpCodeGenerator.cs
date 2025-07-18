using System;
using Excel;
using System.IO;
using System.Text;
using System.Data;
using UnityEngine;

/// <summary>
/// C# 代码生成器实现
/// 功能：
/// 1. 生成与Excel表格对应的数据类
/// 2. 生成包含字典结构的容器类
/// </summary>
public class CSharpCodeGenerator
{


    public void GenerateDataClass(DataTable table, string outputPath, ExcelReadRule readRule)
    {
        // 参数校验
        if (table == null || table.Rows.Count == 0) throw new ArgumentException("表格数据为空");
        if (readRule.PropertyNameRowIndex < 0 || readRule.PropertyTypeRowIndex < 0) throw new ArgumentOutOfRangeException("行索引无效");

        // 获取字段定义行
        DataRow rowName = table.Rows[readRule.PropertyNameRowIndex];
        DataRow rowType = table.Rows[readRule.PropertyTypeRowIndex];
        DataRow rowAnnotated = table.Rows[readRule.AnnotatedRowIndex];

        // 创建输出目录
        Directory.CreateDirectory(outputPath);

        // 构建类代码
        StringBuilder classCode = new StringBuilder();
        classCode.AppendLine("[System.Serializable]"); // 添加序列化标记
        classCode.AppendLine($"public class {readRule.ConfigTypeName} : ExcelConfiguration"); // 继承基类
        classCode.AppendLine("{");

        //非空字段个数
        int RealCount = 0;
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (string.IsNullOrEmpty(rowType[i].ToString())) continue;
            RealCount++;
        }

        // 从第3列开始生成字段（跳过前两列：注释列和ID列）
        for (int i = 2; i < RealCount; i++)
        {
            string annotated = rowAnnotated[i].ToString().Replace("\n",null);

            //写入注释
            if (!string.IsNullOrEmpty(annotated))
            classCode.AppendLine($"\t/// <summary>\n\t///{annotated}\n\t/// </summary>");
            //写入字段
            classCode.AppendLine($"\tpublic {rowType[i]} {rowName[i]};");
        }
        classCode.AppendLine("}");

        // 写入文件
        string filePath = Path.Combine(outputPath, $"{readRule.ConfigTypeName}.cs");
        File.WriteAllText(filePath, classCode.ToString());
    }

    public void GenerateContainerClass(DataTable table, string outputPath, ExcelReadRule readRule)
    {
        // 参数校验
        if (table == null) throw new ArgumentException("表格数据为空");

        // 构建容器类代码
        StringBuilder containerCode = new StringBuilder();
        containerCode.AppendLine("using System.Collections.Generic;"); // 引入命名空间
        containerCode.AppendLine("[System.Serializable]"); // 序列化标记
        containerCode.AppendLine($"public class {readRule.ConfigTypeName}Container : ExcelConfigurationContainer<{readRule.ConfigTypeName}>"); // 泛型容器基类
        containerCode.AppendLine("{");
        containerCode.AppendLine("}");

        // 写入文件
        string filePath = Path.Combine(outputPath, $"{readRule.ConfigTypeName}Container.cs");
        File.WriteAllText(filePath, containerCode.ToString());
    }
}