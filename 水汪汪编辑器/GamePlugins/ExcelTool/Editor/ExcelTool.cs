using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class EM_ExcelTool : Singleton_UnMono<EM_ExcelTool>
{

    private Assembly assembly = null;
    public ExcelToolSettingData SettingInfo;
    /// <summary>
    /// excel文件夹存放的路径
    /// </summary>
    private string ExcelDirectory_Path => SettingInfo.ExcelDirectory_Path;
    /// <summary>
    /// 二进制导出文件夹路径
    /// </summary>

    public EM_ExcelTool()
    {
        SettingInfo = SettingDataLoader.Instance.LoadData<ExcelToolSettingData>();
        // 获取程序集的路径
        string assemblyPath = Path.Combine(Application.dataPath, "../Library/ScriptAssemblies/Assembly-CSharp.dll");
        // 加载程序集
        assembly = Assembly.LoadFile(assemblyPath);
    }


    /// <summary>
    /// 生成指定的单个文件
    /// </summary>
    public void GenerateExcelInfo()
    {
        string path = EditorUtility.OpenFilePanelWithFilters("选择要转换的单个Excel文件", SettingInfo.ExcelDirectory_Path, new string[] { "Excel files", "xlsx,xls" });
        if (path == null || path == "") return;

        ReallyGenerateExcelInfo(path);
    }


    /// <summary>
    /// 生成目录下所有的Excel文件
    /// </summary>
    public void GenerateAllExcelInfo()
    {

        if (File.Exists(ExcelDirectory_Path))
            EditorUtility.DisplayDialog("生成失败！", "不存在这个路径文件夹", "好吧~");

        //记在指定路径中的所有Excel文件 用于生成对应的3个文件
        DirectoryInfo dInfo = Directory.CreateDirectory(ExcelDirectory_Path);
        //得到指定路径中的所有文件信息 相当于就是得到所有的Excel表
        FileInfo[] files = dInfo.GetFiles();
        //数据表容器

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".xlsx" &&
                files[i].Extension != ".xls")
                continue;
            ReallyGenerateExcelInfo(files[i].FullName);

        }
    }


    //暂时存储位置
    private string TempGenerateDirectoryPath;
    /// <summary>
    /// 生成Excel表对应的数据结构类
    /// </summary>
    /// <param name="table"></param>
    private void GenerateExcelDataClass(DataTable table)
    {
        //字段名行
        DataRow rowName = table.Rows[SettingInfo.propertyNameRowIndex];
        //字段类型行
        DataRow rowType = table.Rows[SettingInfo.propertyTypeRowIndex];

        TempGenerateDirectoryPath = SettingInfo.OutPath + "\\" + table.TableName + "\\";
        //判断路径是否存在 没有的话 就创建文件夹
        if (!Directory.Exists(TempGenerateDirectoryPath))
            Directory.CreateDirectory(TempGenerateDirectoryPath);

        //如果我们要生成对应的数据结构类脚本 其实就是通过代码进行字符串拼接 然后存进文件就行了
        string str = null;
        //变量进行字符串拼接
        for (int i = 0; i < table.Columns.Count; i++)
        {
            str += "    public " + rowType[i].ToString() + " " + rowName[i].ToString() + ";\n";
        }
        str = "public class " + table.TableName + "\n{\n" + str + "\n}";
        //特性
        str = "[System.Serializable]\n" + str;
        //把拼接好的字符串存到指定文件中去
        File.WriteAllText(TempGenerateDirectoryPath + table.TableName + ".cs", str);
        //刷新Project窗口
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成Excel表对应的数据容器类
    /// </summary>
    /// <param name="table"></param>
    private void GenerateExcelContainer(DataTable table)
    {
        //得到主键索引
        int keyIndex = GetKeyIndex(table);
        //得到字段类型行
        DataRow rowType = table.Rows[SettingInfo.propertyTypeRowIndex];

        string str = "using System.Collections.Generic;\n";
        //加特性
        str += "\n\n[System.Serializable]\n";
        str += "public class " + table.TableName + "Container : DataBaseContainer" + "\n{\n";
        str += "\tpublic Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">";
        str += " dataDic = new " + "Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">();\n";
        str += "}";

        File.WriteAllText(TempGenerateDirectoryPath + table.TableName + "Container.cs", str);


        //刷新Project窗口
        AssetDatabase.Refresh();
        // 监听编译完成的事件

    }

    //真正生成容器和数据类操作
    private void ReallyGenerateExcelInfo(string ExcelPath)
    {
        if (!File.Exists(ExcelPath))
        {
            Debug.LogError("数据类生成失败，不存在路径：" + ExcelPath);
            return;
        }
        //数据表容器
        DataTableCollection tableConllection = null;
        using (FileStream fs = new FileStream(ExcelPath, FileMode.Open, FileAccess.Read))
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            tableConllection = excelReader.AsDataSet().Tables;
            fs.Close();
        }
        //遍历文件中的所有表的信息
        foreach (DataTable table in tableConllection)
        {
            //生成数据结构类
            GenerateExcelDataClass(table);
            //生成容器类
            GenerateExcelContainer(table);
        }
    }



    /// <summary>
    /// 生成excel单个文件的2进制数据
    /// </summary>
    /// <param name="table"></param>
    public void GenerateExcelBinary()
    {
        string path = EditorUtility.OpenFilePanelWithFilters("选择要转换的单个Excel文件", SettingInfo.ExcelDirectory_Path, new string[] { "Excel files", "xlsx,xls" });
        if (path == null || path == "") return;
       
        ReallyGenerateExcelBinary(path);

    }
    /// <summary>
    /// 生成EXCEL目录下所有Excel文件的2进制数据
    /// </summary>
    public void GenerateAllExcelBinary()
    {
        if (File.Exists(ExcelDirectory_Path))
            EditorUtility.DisplayDialog("生成失败！", "不存在这个路径文件夹", "好吧~");

        //记在指定路径中的所有Excel文件 用于生成对应的3个文件
        DirectoryInfo dInfo = Directory.CreateDirectory(ExcelDirectory_Path);
        //得到指定路径中的所有文件信息 相当于就是得到所有的Excel表
        FileInfo[] files = dInfo.GetFiles();
        //数据表容器

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".xlsx" &&
                files[i].Extension != ".xls")
                continue;
           ReallyGenerateExcelBinary(files[i].FullName);

        }
    }

    //真正生成二进制数据操作
    private void ReallyGenerateExcelBinary(string ExcelPath)
    {
        if (!File.Exists(ExcelPath))
        {
            Debug.LogError("二进制文件转换生成失败，不存在Excel路径：" + ExcelPath);
            return;
        }
        //数据表容器
        DataTableCollection tableConllection = null;
        using (FileStream fs = new FileStream(ExcelPath, FileMode.Open, FileAccess.Read))
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            tableConllection = excelReader.AsDataSet().Tables;
            fs.Close();
        }
        foreach (DataTable table in tableConllection)
        {
            Type ContainerType = null;
            foreach (var type in assembly.GetTypes())
            {
                if (table.TableName + "Container" == type.Name)
                    ContainerType = type;
            }
            //判断有没有打入程序集
            if (ContainerType == null)
            {
                Debug.Log($"{table.TableName}暂未生成容器和数据类，无法生成二进制文件");
                continue;
            }
            // 创建容器的实例
            object ContainerInstance = Activator.CreateInstance(ContainerType);

            //Debug.Log(ContainerInstance);
            // 获取Dictionary字段
            FieldInfo fieldInfo = ContainerType.GetField("dataDic");
            // 获取Dictionary的类型
            Type dictionaryType = fieldInfo.FieldType;

            // 获取键和值的类型
            Type[] typeArguments = dictionaryType.GetGenericArguments();
            Type keyType = typeArguments[0];
            Type valueType = typeArguments[1];
            // 创建Dictionary实例
            Type specificDictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            object dictionaryInstance = Activator.CreateInstance(specificDictionaryType);
            // 获取Add方法
            MethodInfo addMethod = specificDictionaryType.GetMethod("Add");
            //逐个写入数据
            for (int i = SettingInfo.ReallyDataStartRowIndex - 1; i < table.Rows.Count; i++)
            {
                // 创建键和值的实例并添加到Dictionary
                object keyValue = ConvertFromString(table.Rows[i][GetKeyIndex(table)].ToString(), keyType);
                object ValueInstance = Activator.CreateInstance(valueType);
                //遍历容器的成员字段们
                for (int j = 0; j < valueType.GetFields().Length; j++)
                {
                    FieldInfo field = valueType.GetFields()[j];
                    field.SetValue(ValueInstance, ConvertFromString(table.Rows[i][j].ToString(), field.FieldType));
                }
                // 存储主键的变量名
                addMethod.Invoke(dictionaryInstance, new object[] { keyValue, ValueInstance });
            }
            // 将Dictionary实例赋值给字段
            fieldInfo.SetValue(ContainerInstance, dictionaryInstance);
            //直接tm序列化
            if (!Directory.Exists(SettingInfo.OutPath + "\\" + table.TableName + "\\"))
            {
                Directory.CreateDirectory(SettingInfo.OutPath + "\\" + table.TableName + "\\");
            }
            BinaryManager.Instance.Save(ContainerInstance, table.TableName + "."+SettingInfo.SuffixName
                , SettingInfo.OutPath + "\\" + table.TableName + "\\");
            //重置默认参数
            Reset();
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取容器类字典主键的列数索引
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private int GetKeyIndex(DataTable table)
    {
        if (KeyIndex != -1) return KeyIndex;
        DataRow row = table.Rows[SettingInfo.keyRowIndex];
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (row[i].ToString() == "key")
            {
                KeyIndex = i;
                return i;
            }
        }
        return 0;
    }
    private int KeyIndex = -1;
    /// <summary>
    /// 装箱转换工厂
    /// </summary>
    /// <param name="value"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    private object ConvertFromString(string value, Type type)
    {

        object obj = null;
        try
        {
            switch (type.Name)
            {
                case "Int32":
                    obj = Convert.ToInt32(value);
                    break;
                case "String":
                    obj = Convert.ToString(value);
                    break;
                case "Single":
                    obj = Convert.ToSingle(value);
                    break;
                case "Double":
                    obj = Convert.ToDouble(value);
                    break;
                case "Boolean":
                    obj = Convert.ToBoolean(value);
                    break;
            }
        }
        catch
        {
            obj = null;
        }
        return obj;
    }
    private void Reset()
    {
        KeyIndex = -1;
    }
}