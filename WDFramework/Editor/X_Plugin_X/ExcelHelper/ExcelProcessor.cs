using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using WDEditor;

/// <summary>
/// Excel ���������
/// ְ��Э����ȡ�����ɡ����л�����
/// </summary>
public class ExcelProcessor
{
    private ExcelReader _excelReader;
    private CSharpCodeGenerator _codeGenerator;
    private ExcelBinarySerializer _binarySerializer;
    // ���ڴ洢��ȡ����
    private ExcelReadRule readRule;
    private winData_ExcelTool data;
    /// <summary>
    /// ���캯��������ע�룩
    /// </summary>
    public ExcelProcessor(winData_ExcelTool winData)
    {
        _excelReader = new ExcelReader();
        _codeGenerator = new CSharpCodeGenerator();
        _binarySerializer = new ExcelBinarySerializer();
        readRule = new ExcelReadRule();
        data = winData;
    }

    /// <summary>
    /// ������Excel�ļ�
    /// </summary>
    /// <param name="excelPath">Excel�ļ�·��</param>
    /// <param name="mode">����ģʽ����������/���������ɣ�</param>
    public void ProcessSingleFile(string excelPath, ProcessMode mode)
    {
        var tables = _excelReader.ReadExcel(excelPath);
        if (tables == null || tables.Count == 0) return;
        List<string> configTypeNames = new List<string>();
        // ��һ��Sheet��Ϊ��׼
        foreach (DataTable table in tables)
        {
            //ÿ����ͬ��sheet��һ��rule
            LoadReadRule(table);
            switch (mode)
            {
                //���ɴ���
                case ProcessMode.GenerateCode:
                    //�������ͬ����������������ô�Ͳ����ٴ�������
                    if (!configTypeNames.Contains(readRule.ConfigTypeName))
                    {
                        configTypeNames.Add(readRule.ConfigTypeName);
                    }
                    else
                    {
                        Debug.Log("������ͬ��������");
                        continue;
                    }
                    _codeGenerator.GenerateDataClass(table, data.OutPath, readRule);
                    _codeGenerator.GenerateContainerClass(table, data.OutPath, readRule);
                    break;
                //���ɶ������ļ�
                case ProcessMode.GenerateBinary:
                    var container = _binarySerializer.BuildContainer(table, readRule);
                    if (container != null)
                    {
                        string path = Path.Combine(data.OutPath, $"{table.TableName}.{data.SuffixName}");
                        BinaryManager.SaveToPath(container, path);
                    }
                    break;
            }
        }
    }
    /// <summary>
    /// ���ر���ȡ����
    /// </summary>
    /// <param name="table">���ݱ�</param>
    private void LoadReadRule(DataTable table)
    {
        readRule.AnnotatedRowIndex = -1;
        readRule.PropertyNameRowIndex = -1;
        readRule.PropertyTypeRowIndex = -1;
        readRule.StartReadRowIndex = -1;
        readRule.ConfigTypeName = null; // ����
        for (int i = 0; i < table.Rows.Count; i++)
        {
            string cellValue = table.Rows[i][0].ToString().Trim();
            switch (cellValue)
            {
                case "ConfigType": // ����������������
                    readRule.ConfigTypeName = table.Rows[i][1].ToString();
                    break;
                case "PropertyName": // �ֶ��������
                    readRule.PropertyNameRowIndex = i;
                    break;
                case "PropertyType": // �ֶ����ͱ����
                    readRule.PropertyTypeRowIndex = i;
                    break;
                case "StartRead":    // ������ʼ�б��
                    readRule.StartReadRowIndex = i;
                    break;
                case "˵��":
                    readRule.AnnotatedRowIndex = i;
                    break;
            }
        }
        if (string.IsNullOrEmpty(readRule.ConfigTypeName))
        {
            throw new Exception($"Sheet {table.TableName} ȱ��ConfigType���ã�");
        }
    }
}