using System;
using Excel;
using System.IO;
using System.Text;
using System.Data;
using UnityEngine;

/// <summary>
/// C# ����������ʵ��
/// ���ܣ�
/// 1. ������Excel����Ӧ��������
/// 2. ���ɰ����ֵ�ṹ��������
/// </summary>
public class CSharpCodeGenerator
{


    public void GenerateDataClass(DataTable table, string outputPath, ExcelReadRule readRule)
    {
        // ����У��
        if (table == null || table.Rows.Count == 0) throw new ArgumentException("�������Ϊ��");
        if (readRule.PropertyNameRowIndex < 0 || readRule.PropertyTypeRowIndex < 0) throw new ArgumentOutOfRangeException("��������Ч");

        // ��ȡ�ֶζ�����
        DataRow rowName = table.Rows[readRule.PropertyNameRowIndex];
        DataRow rowType = table.Rows[readRule.PropertyTypeRowIndex];
        DataRow rowAnnotated = table.Rows[readRule.AnnotatedRowIndex];

        // �������Ŀ¼
        Directory.CreateDirectory(outputPath);

        // ���������
        StringBuilder classCode = new StringBuilder();
        classCode.AppendLine("[System.Serializable]"); // ������л����
        classCode.AppendLine($"public class {readRule.ConfigTypeName} : ExcelConfiguration"); // �̳л���
        classCode.AppendLine("{");

        //�ǿ��ֶθ���
        int RealCount = 0;
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (string.IsNullOrEmpty(rowType[i].ToString())) continue;
            RealCount++;
        }

        // �ӵ�3�п�ʼ�����ֶΣ�����ǰ���У�ע���к�ID�У�
        for (int i = 2; i < RealCount; i++)
        {
            string annotated = rowAnnotated[i].ToString().Replace("\n",null);

            //д��ע��
            if (!string.IsNullOrEmpty(annotated))
            classCode.AppendLine($"\t/// <summary>\n\t///{annotated}\n\t/// </summary>");
            //д���ֶ�
            classCode.AppendLine($"\tpublic {rowType[i]} {rowName[i]};");
        }
        classCode.AppendLine("}");

        // д���ļ�
        string filePath = Path.Combine(outputPath, $"{readRule.ConfigTypeName}.cs");
        File.WriteAllText(filePath, classCode.ToString());
    }

    public void GenerateContainerClass(DataTable table, string outputPath, ExcelReadRule readRule)
    {
        // ����У��
        if (table == null) throw new ArgumentException("�������Ϊ��");

        // �������������
        StringBuilder containerCode = new StringBuilder();
        containerCode.AppendLine("using System.Collections.Generic;"); // ���������ռ�
        containerCode.AppendLine("[System.Serializable]"); // ���л����
        containerCode.AppendLine($"public class {readRule.ConfigTypeName}Container : ExcelConfigurationContainer<{readRule.ConfigTypeName}>"); // ������������
        containerCode.AppendLine("{");
        containerCode.AppendLine("}");

        // д���ļ�
        string filePath = Path.Combine(outputPath, $"{readRule.ConfigTypeName}Container.cs");
        File.WriteAllText(filePath, containerCode.ToString());
    }
}