//using Excel;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using UnityEditor;
//using UnityEngine;

//namespace WDEditor
//{
//    //插件详解：
//    /*
//        我们在这里所修改的影响的只是序列化成二进制时的影响，并不会对我们读取时有任何影响
//        我们序列化时候是将它序列化成一个彻彻底底的二进制字典，可以直接通过Binaray读取的，我们只需考虑序列化规则即可

//        插件使用：
//        1.先点击创建配置表基础白板，在此基础上完成配置表
//        2.生成数据和容器类
//        3.转化成二进制
//        4.通过Excel的二进制加载的具加载二进制文件使用
     
//     */


//    /// <summary>
//    /// Excel表序列化助手
//    /// </summary> 
//    public class ExcelSerializeHelper
//    {
//        public winData_ExcelTool data;
//        private Assembly[] AllAssembly = null;
//        /// <summary>
//        /// excel文件夹存放的路径
//        /// </summary>
//        private string ExcelDirectory_Path => data.ExcelDirectory_Path;
//        public ExcelSerializeHelper(winData_ExcelTool data)
//        {
//            this.data = data;
//            // 加载程序集
//            AllAssembly = AppDomain.CurrentDomain.GetAssemblies();
//        }
//        //字段名所在行
//        public int propertyNameRowIndex;
//        //字段类型所在行
//        public int propertyTypeRowIndex;
//        //开始读取行
//        public int StartReadRowIndex;
//        //加载前，必须先加载读取规则
//        private void LoadReadRule(DataTable table)
//        {
//            // 遍历第一列的所有行，找到所有标记，并记录所在行数
//            for (int i = 0; i < table.Rows.Count; i++)
//            {
//                string cellValue = table.Rows[i][0].ToString().Trim();

//                switch (cellValue)
//                {
//                    case "PropertyName":
//                        propertyNameRowIndex = i;
//                        break;
//                    case "PropertyType":
//                        propertyTypeRowIndex = i;
//                        break;
//                    case "StartRead":
//                        StartReadRowIndex = i; // 读取行数从标记的下一行开始
//                        break;
//                }
//            }
//        }

//        /// <summary>
//        /// 重置读取规则
//        /// </summary>
//        private void ResetReadRule()
//        {
//            propertyNameRowIndex = -1;
//            propertyTypeRowIndex = -1;
//            StartReadRowIndex = -1;
//        }

//        /// <summary>
//        /// 生成指定的单个文件
//        /// </summary>
//        public void GenerateExcelInfo()
//        {
//            string path = EditorUtility.OpenFilePanelWithFilters("选择要转换的单个Excel文件", data.ExcelDirectory_Path, new string[] { "Excel files", "xlsx,xls" });
//            if (path == null || path == "") return;

//            ReallyGenerateExcelClassAndContainer(path);
//        }
//        /// <summary>
//        /// 生成目录下所有的Excel文件
//        /// </summary>
//        public void GenerateAllExcelInfo()
//        {

//            if (File.Exists(ExcelDirectory_Path))
//                EditorUtility.DisplayDialog("生成失败！", "不存在这个路径文件夹", "好吧~");

//            //记在指定路径中的所有Excel文件 用于生成对应的3个文件
//            DirectoryInfo dInfo = Directory.CreateDirectory(ExcelDirectory_Path);
//            //得到指定路径中的所有文件信息 相当于就是得到所有的Excel表
//            FileInfo[] files = dInfo.GetFiles();
//            //数据表容器

//            for (int i = 0; i < files.Length; i++)
//            {
//                if (files[i].Extension != ".xlsx" &&
//                    files[i].Extension != ".xls")
//                    continue;
//                ReallyGenerateExcelClassAndContainer(files[i].FullName);

//            }
//        }
//        /// <summary>
//        /// 真正生成容器和数据类操作
//        /// </summary>
//        /// <param name="ExcelPath"></param>
//        private void ReallyGenerateExcelClassAndContainer(string ExcelPath)
//        {

//            if (!File.Exists(ExcelPath))
//            {
//                Debug.LogError("数据类生成失败，不存在路径：" + ExcelPath);
//                return;
//            }
//            //创建一个临时的复制类，以便于边编辑，边读取
//            // 创建临时文件路径
//            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(ExcelPath));
//            // 创建Excel的复制文件到临时路径
//            File.Copy(ExcelPath, tempFilePath, true);
//            //数据表容器
//            DataTableCollection tableConllection = null;
//            using (FileStream fs = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
//            {
//                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
//                tableConllection = excelReader.AsDataSet().Tables;
//                fs.Close();
//            }
//            // 删除临时文件
//            File.Delete(tempFilePath);
//            //遍历文件中的所有表的信息
//            foreach (DataTable table in tableConllection)
//            {
//                //得到主键索引
//                LoadReadRule(table);
//                //生成数据结构类
//                GenerateExcelDataClass(table);
//                //生成容器类
//                GenerateExcelContainer(table);
//            }
//        }
//        //暂时存储位置
//        private string TempGenerateDirectoryPath;
//        /// <summary>
//        /// 生成Excel表对应的数据结构类
//        /// </summary>
//        /// <param name="table"></param>
//        private void GenerateExcelDataClass(DataTable table)
//        {

//            //设置字段名和字段类型的读取行列
//            //字段名行
//            DataRow rowName = table.Rows[propertyNameRowIndex];
//            //字段类型行
//            DataRow rowType = table.Rows[propertyTypeRowIndex];

//            TempGenerateDirectoryPath = data.OutPath + "\\" + table.TableName + "\\";
//            //判断路径是否存在 没有的话 就创建文件夹
//            if (!Directory.Exists(TempGenerateDirectoryPath))
//                Directory.CreateDirectory(TempGenerateDirectoryPath);

//            //如果我们要生成对应的数据结构类脚本 其实就是通过代码进行字符串拼接 然后存进文件就行了
//            string str = null;

//            //从Key之后开始存储数据
//            for (int i = 2; i < table.Columns.Count; i++)
//            {
//                str += "    public " + rowType[i].ToString() + " " + rowName[i].ToString() + ";\n";
//            }
//            str = "public class " + table.TableName + " : ExcelConfiguration" + "\n{\n" + str + "\n}";
//            //特性
//            str = "[System.Serializable]\n" + str;
//            //把拼接好的字符串存到指定文件中去
//            File.WriteAllText(TempGenerateDirectoryPath + table.TableName + ".cs", str);
//            //刷新Project窗口
//            AssetDatabase.Refresh();
//        }
//        /// <summary>
//        /// 生成Excel表对应的数据容器类
//        /// </summary>
//        /// <param name="table"></param>
//        private void GenerateExcelContainer(DataTable table)
//        {

//            //得到字段类型行
//            DataRow rowType = table.Rows[propertyTypeRowIndex];

//            string str = "using System.Collections.Generic;\n";
//            //加特性
//            str += "\n\n[System.Serializable]\n";
//            str += "public class " + table.TableName + $"Container : ExcelConfigurationContainer<{table.TableName.Replace("Container", null)}>" + "\n{\n\n";
//            str += "}";

//            File.WriteAllText(TempGenerateDirectoryPath + table.TableName + "Container.cs", str);


//            //刷新Project窗口
//            AssetDatabase.Refresh();
//            // 监听编译完成的事件

//        }

//        /// <summary>
//        /// 生成excel单个文件的2进制数据
//        /// </summary>
//        /// <param name="table"></param>
//        public void GenerateExcelBinary()
//        {
//            string path = EditorUtility.OpenFilePanelWithFilters("选择要转换的单个Excel文件", data.ExcelDirectory_Path, new string[] { "Excel files", "xlsx,xls" });
//            if (path == null || path == "") return;

//            ReallyGenerateExcelBinary(path);

//        }
//        /// <summary>
//        /// 生成EXCEL目录下所有Excel文件的2进制数据
//        /// </summary>
//        public void GenerateAllExcelBinary()
//        {
//            if (File.Exists(ExcelDirectory_Path))
//                EditorUtility.DisplayDialog("生成失败！", "不存在这个路径文件夹", "好吧~");

//            //记在指定路径中的所有Excel文件 用于生成对应的3个文件
//            DirectoryInfo dInfo = Directory.CreateDirectory(ExcelDirectory_Path);
//            //得到指定路径中的所有文件信息 相当于就是得到所有的Excel表
//            FileInfo[] files = dInfo.GetFiles();
//            //数据表容器

//            for (int i = 0; i < files.Length; i++)
//            {
//                if (files[i].Extension != ".xlsx" &&
//                    files[i].Extension != ".xls")
//                    continue;
//                ReallyGenerateExcelBinary(files[i].FullName);

//            }
//        }
//        //真正生成二进制数据操作
//        private void ReallyGenerateExcelBinary(string ExcelPath)
//        {
//            if (!File.Exists(ExcelPath))
//            {
//                Debug.LogError("二进制文件转换生成失败，不存在Excel路径：" + ExcelPath);
//                return;
//            }
//            // 创建临时文件路径
//            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(ExcelPath));
//            // 创建Excel的复制文件到临时路径
//            File.Copy(ExcelPath, tempFilePath, true);
//            //数据表容器
//            DataTableCollection tableConllection = null;
//            using (FileStream fs = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
//            {
//                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
//                tableConllection = excelReader.AsDataSet().Tables;
//                fs.Close();
//            }
//            // 删除临时文件
//            File.Delete(tempFilePath);
//            //正式读取表然后生成二进制文件
//            foreach (DataTable table in tableConllection)
//            {
//                //先加载读取规则
//                LoadReadRule(table);
//                Type ContainerType = null;
//                //遍历全部程序集
//                foreach (var assembly in AllAssembly)
//                {
//                    foreach (var type in assembly.GetTypes())
//                    {
//                        if (table.TableName + "Container" == type.Name)
//                            ContainerType = type;
//                    }
//                }
//                //判断有没有打入程序集
//                if (ContainerType == null)
//                {
//                    Debug.Log($"{table.TableName}暂未生成容器和数据类，无法生成二进制文件");
//                    continue;
//                }
//                // 创建容器的实例
//                object ContainerInstance = Activator.CreateInstance(ContainerType);

//                //Debug.Log(ContainerInstance);
//                // 获取Dictionary字段
//                FieldInfo fieldInfo = ContainerType.BaseType.GetField("container");
//                // 获取Dictionary的类型
//                Type dictionaryType = fieldInfo.FieldType;

//                // 获取键和值的类型
//                Type[] typeArguments = dictionaryType.GetGenericArguments();
//                Type keyType = typeArguments[0];
//                //值类型，即数据对象类型
//                Type valueType = typeArguments[1];
//                // 创建Dictionary实例
//                Type specificDictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
//                object dictionaryInstance = Activator.CreateInstance(specificDictionaryType);
//                // 获取Add方法
//                MethodInfo addMethod = specificDictionaryType.GetMethod("Add");

//                //!!牛逼解决：解决映射排序错误问题**
//                //先得到所有字段，包括父类和子类的，这时候子类最靠前，父类最靠后
//                FieldInfo[] FieldsInfo = valueType.GetFields();
//                List<string> FieldsName = new List<string>();
//                for (int i = 2; i < table.Columns.Count; i++)
//                {
//                    FieldsName.Add(table.Rows[propertyNameRowIndex][i].ToString());
//                }
//                //创建排序数组
//                List<FieldInfo> sortFiledInfos = new List<FieldInfo>();
//                //进行排序
//                for (int i = 0; i < FieldsName.Count; i++)
//                {
//                    for (int j = 0; j < FieldsInfo.Length; j++)
//                    {
//                        if (FieldsName[i] == FieldsInfo[j].Name)
//                            sortFiledInfos.Add(FieldsInfo[j]);
//                    }
//                }

//                //逐行写入数据
//                for (int i = StartReadRowIndex; i < table.Rows.Count; i++)
//                {
//                    //得到第一注释列，跳过注释行，注释行不读取
//                    if (table.Rows[i][0].ToString().StartsWith("//")) { continue; }
//                    //得到id列
//                    string idValue = table.Rows[i][1].ToString();
//                    if (idValue.StartsWith("//") || string.IsNullOrEmpty(idValue)) { continue; }

//                    // 创建键和值的实例并添加到Dictionary
//                    object keyValue = ConvertFromString(idValue, keyType);
//                    object ValueInstance = Activator.CreateInstance(valueType);


//                    //遍历容器的成员字段们
//                    for (int j = 0; j < sortFiledInfos.Count; j++)
//                    {
//                        FieldInfo field = sortFiledInfos[j];
//                        //从第二列开始存储，也就是key之后的字段
//                        field.SetValue(ValueInstance, ConvertFromString(table.Rows[i][j + 2].ToString(), field.FieldType));
//                    }
//                    // 调用add方法，添加整条数据
//                    addMethod.Invoke(dictionaryInstance, new object[] { keyValue, ValueInstance });
//                }
//                // 将Dictionary实例赋值给字段
//                fieldInfo.SetValue(ContainerInstance, dictionaryInstance);
//                //直接tm序列化
//                if (!Directory.Exists(data.OutPath + "\\"))
//                {
//                    Directory.CreateDirectory(data.OutPath + "\\" + table.TableName + "\\");
//                }
//                //Debug.Log(data.OutPath + "\\" + table.TableName + "." + data.SuffixName);
//                //命名为 表名Container.自定义后缀名
//                BinaryManager.SaveToPath(ContainerInstance
//                    , data.OutPath + "\\" + table.TableName + "Container" + "." + data.SuffixName);
//                //重置默认参数
//                ResetReadRule();
//            }
//            AssetDatabase.Refresh();
//        }

//        /// <summary>
//        /// 装箱转换工厂
//        /// </summary>
//        /// <param name="value"></param>
//        /// <param name="column"></param>
//        /// <returns></returns>
//        private object ConvertFromString(string value, Type type)
//        {

//            object obj = null;
//            try
//            {
//                switch (type.Name)
//                {
//                    case "Int32":
//                        obj = Convert.ToInt32(value);
//                        break;
//                    case "String":
//                        obj = Convert.ToString(value);
//                        break;
//                    case "Single":
//                        obj = Convert.ToSingle(value);
//                        break;
//                    case "Double":
//                        obj = Convert.ToDouble(value);
//                        break;
//                    case "Boolean":
//                        obj = Convert.ToBoolean(value);
//                        break;
//                    //数组转换
//                    case "Int32[]":
//                        obj = value.Split('|').Select(int.Parse).ToArray();
//                        break;
//                    case "Single[]":
//                        obj = value.Split('|').Select(float.Parse).ToArray();
//                        break;
//                    case "Double[]":
//                        obj = value.Split('|').Select(double.Parse).ToArray();
//                        break;
//                }
//            }
//            catch
//            {
//                obj = null;
//            }
//            return obj;
//        }

//    }
//}