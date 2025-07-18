using Excel;
using System;
using System.Data;
using System.IO;
public class ExcelReadRule
{
    public int PropertyNameRowIndex { get; set; }
    public int PropertyTypeRowIndex { get; set; }
    public int StartReadRowIndex { get; set; }
    /// <summary>
    /// ×¢ÊÍËùÔÚĞĞ
    /// </summary>
    public int AnnotatedRowIndex { get; set; }
    public string ConfigTypeName { get; set; }
}
public enum ProcessMode
{
    GenerateCode,
    GenerateBinary
    
}