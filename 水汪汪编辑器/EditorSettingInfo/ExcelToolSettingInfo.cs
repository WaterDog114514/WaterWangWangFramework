using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Excel保存二进制路径设置文件，编辑器模式专用
public class ExcelToolSettingInfo : BaseSettingData
{

    [JsonIgnore]
    public override string DirectoryPath => Application.dataPath + "\\水汪汪编辑器\\EditorAsset\\PluginsData\\Resources\\";
    [JsonIgnore]
    public override string DataName => "ExcelToolSettingInfo";

    /// <summary>
    /// 属性名所在行
    /// </summary>
    public int propertyNameRowIndex =0;
    /// <summary>
    /// 属性类型名所在行
    /// </summary>
    public int propertyTypeRowIndex =1;
    /// <summary>
    /// key标签所在行
    /// </summary>
    public int keyRowIndex = 2 ;
    /// <summary>
    /// 真正数据开始记录所在行
    /// </summary>
    public int ReallyDataStartRowIndex = 4;
    public string OutPath;
    public string ExcelDirectory_Path;
    public override void IntiValue()
    {
        //初始化
        ExcelDirectory_Path = Application.dataPath + "\\水汪汪编辑器\\GamePlugins\\ExcelTool\\Excel\\";
        OutPath = Application.dataPath + "\\水汪汪编辑器\\GamePlugins\\ExcelTool\\out\\";
        propertyNameRowIndex = 0;
        propertyTypeRowIndex = 1;   
        keyRowIndex = 2;
        ReallyDataStartRowIndex =4;
    }
}
