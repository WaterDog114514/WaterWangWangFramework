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
/// Excel 处理核心类
/// 职责：协调读取、生成、序列化流程
/// </summary>
public class ExcelProcessor
{
    private ExcelReader _excelReader;
    private CSharpCodeGenerator _codeGenerator;
    private ExcelBinarySerializer _binarySerializer;
    // 用于存储读取规则
    private ExcelReadRule readRule;
    private winData_ExcelTool data;
    /// <summary>
    /// 构造函数（依赖注入）
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
    /// 处理单个Excel文件
    /// </summary>
    /// <param name="excelPath">Excel文件路径</param>
    /// <param name="mode">处理模式（代码生成/二进制生成）</param>
    public void ProcessSingleFile(string excelPath, ProcessMode mode)
    {
        var tables = _excelReader.ReadExcel(excelPath);
        if (tables == null || tables.Count == 0) return;
        List<string> configTypeNames = new List<string>();
        // 第一个Sheet作为基准
        foreach (DataTable table in tables)
        {
            //每个不同的sheet是一个rule
            LoadReadRule(table);
            switch (mode)
            {
                //生成代码
                case ProcessMode.GenerateCode:
                    //如果有相同的配置类型名，那么就不用再次生成了
                    if (!configTypeNames.Contains(readRule.ConfigTypeName))
                    {
                        configTypeNames.Add(readRule.ConfigTypeName);
                    }
                    else
                    {
                        Debug.Log("发现相同，跳过了");
                        continue;
                    }
                    _codeGenerator.GenerateDataClass(table, data.OutPath, readRule);
                    _codeGenerator.GenerateContainerClass(table, data.OutPath, readRule);
                    break;
                //生成二进制文件
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
    /// 加载表格读取规则
    /// </summary>
    /// <param name="table">数据表</param>
    private void LoadReadRule(DataTable table)
    {
        readRule.AnnotatedRowIndex = -1;
        readRule.PropertyNameRowIndex = -1;
        readRule.PropertyTypeRowIndex = -1;
        readRule.StartReadRowIndex = -1;
        readRule.ConfigTypeName = null; // 重置
        for (int i = 0; i < table.Rows.Count; i++)
        {
            string cellValue = table.Rows[i][0].ToString().Trim();
            switch (cellValue)
            {
                case "ConfigType": // 新增配置类型名行
                    readRule.ConfigTypeName = table.Rows[i][1].ToString();
                    break;
                case "PropertyName": // 字段名标记行
                    readRule.PropertyNameRowIndex = i;
                    break;
                case "PropertyType": // 字段类型标记行
                    readRule.PropertyTypeRowIndex = i;
                    break;
                case "StartRead":    // 数据起始行标记
                    readRule.StartReadRowIndex = i;
                    break;
                case "说明":
                    readRule.AnnotatedRowIndex = i;
                    break;
            }
        }
        if (string.IsNullOrEmpty(readRule.ConfigTypeName))
        {
            throw new Exception($"Sheet {table.TableName} 缺少ConfigType配置！");
        }
    }
}