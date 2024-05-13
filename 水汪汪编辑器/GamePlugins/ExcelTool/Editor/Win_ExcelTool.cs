using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Win_ExcelTool : SingletonBaseWindow
{
    private ExcelToolSettingData SettingInfo;


    //垃圾颜色字体
    private GUIStyle TitleStyle = new GUIStyle();
    private void IntiTitleStyle()
    {
        TitleStyle.normal.textColor = Color.red;
        TitleStyle.fontSize = 16;
    }

    protected override void OnEnable()
    {
        if (SettingInfo == null) SettingInfo = EM_ExcelTool.Instance.SettingInfo;

        IntiTitleStyle();
        base.OnEnable();
        IntiWindowsSetting("Excel加载转二进制工具", "YuSheIcon.png");

    }
    [MenuItem("水汪汪工具/Excel加载工具")]
    protected static void OpenWindow()
    {
        EditorWindow.GetWindow<Win_ExcelTool>();
    }
    protected override void m_DrawWindows()
    {
        GUILayout.Label("读表规则设置：", TitleStyle);
        SettingInfo.propertyNameRowIndex = EditorGUILayout.IntField("字段名所在行：",SettingInfo.propertyNameRowIndex);
        SettingInfo.propertyTypeRowIndex = EditorGUILayout.IntField("字段类型名所在行：",SettingInfo.propertyTypeRowIndex);
        SettingInfo.keyRowIndex= EditorGUILayout.IntField("容器类key标记所在行：",SettingInfo.keyRowIndex);
        SettingInfo.ReallyDataStartRowIndex = EditorGUILayout.IntField("真正数据存储开始读取行：",SettingInfo.ReallyDataStartRowIndex);
        SettingInfo.SuffixName = EditorGUILayout.TextField("自定义后缀名:",SettingInfo.SuffixName);
        GUILayout.Label("从Excel表生成容器类和数据类：", TitleStyle);
        if (GUILayout.Button("生成单个Excel文件的数据类容器..."))
        {
           EM_ExcelTool.Instance.GenerateExcelInfo();
        }
        if (GUILayout.Button("批量生成Excel文件的数据类容器")) 
        { 
            EM_ExcelTool.Instance.GenerateAllExcelInfo();
        }

        GUILayout.Label("将生成好的容器类和数据类转换为二进制：", TitleStyle);
        if (GUILayout.Button("转换单个Excel文件..."))
        {
           EM_ExcelTool.Instance.GenerateExcelBinary();
        }
        if (GUILayout.Button("批量转换Excel文件"))
        {
            EM_ExcelTool.Instance.GenerateAllExcelBinary();
        }

        //加载路径相关
        GUILayout.Label("路径设置", TitleStyle);
        //EXCEL文件路径设置
        EditorGUILayout.BeginHorizontal();
        SettingInfo.ExcelDirectory_Path = EditorGUILayout.TextField("Excel文件路径：",SettingInfo.ExcelDirectory_Path);
        if (GUILayout.Button("设置路径"))
        {
            string path = EditorUtility.SaveFolderPanel("设置要转换Excel的文件夹路径",Application.dataPath,null);
            SettingInfo.ExcelDirectory_Path = path;
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        SettingInfo.OutPath = EditorGUILayout.TextField("输出路径：", SettingInfo.OutPath);
        if (GUILayout.Button("设置路径"))
        {
            string path = EditorUtility.SaveFolderPanel("设置要转换Excel的文件夹路径", Application.dataPath, null);
            SettingInfo.OutPath = path;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("打开导出文件夹"))
        {
            if(Directory.Exists(SettingInfo.OutPath)) 
            System.Diagnostics.Process.Start("explorer.exe", SettingInfo.OutPath);
            else
               EditorUtility.DisplayDialog("错误！","不存在该路径文件夹","好吧~");
        }

        //设置编辑器加载
        EditorGUILayout.Space(20);
        if (GUILayout.Button("保存所有修改"))
        {
            SettingDataLoader.Instance.SaveData(SettingInfo);
            AssetDatabase.Refresh();
        }

    }
}
