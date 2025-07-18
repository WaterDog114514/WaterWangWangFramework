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
//    //�����⣺
//    /*
//        �������������޸ĵ�Ӱ���ֻ�����л��ɶ�����ʱ��Ӱ�죬����������Ƕ�ȡʱ���κ�Ӱ��
//        �������л�ʱ���ǽ������л���һ�������׵׵Ķ������ֵ䣬����ֱ��ͨ��Binaray��ȡ�ģ�����ֻ�迼�����л����򼴿�

//        ���ʹ�ã�
//        1.�ȵ���������ñ�����װ壬�ڴ˻�����������ñ�
//        2.�������ݺ�������
//        3.ת���ɶ�����
//        4.ͨ��Excel�Ķ����Ƽ��صľ߼��ض������ļ�ʹ��
     
//     */


//    /// <summary>
//    /// Excel�����л�����
//    /// </summary> 
//    public class ExcelSerializeHelper
//    {
//        public winData_ExcelTool data;
//        private Assembly[] AllAssembly = null;
//        /// <summary>
//        /// excel�ļ��д�ŵ�·��
//        /// </summary>
//        private string ExcelDirectory_Path => data.ExcelDirectory_Path;
//        public ExcelSerializeHelper(winData_ExcelTool data)
//        {
//            this.data = data;
//            // ���س���
//            AllAssembly = AppDomain.CurrentDomain.GetAssemblies();
//        }
//        //�ֶ���������
//        public int propertyNameRowIndex;
//        //�ֶ�����������
//        public int propertyTypeRowIndex;
//        //��ʼ��ȡ��
//        public int StartReadRowIndex;
//        //����ǰ�������ȼ��ض�ȡ����
//        private void LoadReadRule(DataTable table)
//        {
//            // ������һ�е������У��ҵ����б�ǣ�����¼��������
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
//                        StartReadRowIndex = i; // ��ȡ�����ӱ�ǵ���һ�п�ʼ
//                        break;
//                }
//            }
//        }

//        /// <summary>
//        /// ���ö�ȡ����
//        /// </summary>
//        private void ResetReadRule()
//        {
//            propertyNameRowIndex = -1;
//            propertyTypeRowIndex = -1;
//            StartReadRowIndex = -1;
//        }

//        /// <summary>
//        /// ����ָ���ĵ����ļ�
//        /// </summary>
//        public void GenerateExcelInfo()
//        {
//            string path = EditorUtility.OpenFilePanelWithFilters("ѡ��Ҫת���ĵ���Excel�ļ�", data.ExcelDirectory_Path, new string[] { "Excel files", "xlsx,xls" });
//            if (path == null || path == "") return;

//            ReallyGenerateExcelClassAndContainer(path);
//        }
//        /// <summary>
//        /// ����Ŀ¼�����е�Excel�ļ�
//        /// </summary>
//        public void GenerateAllExcelInfo()
//        {

//            if (File.Exists(ExcelDirectory_Path))
//                EditorUtility.DisplayDialog("����ʧ�ܣ�", "���������·���ļ���", "�ð�~");

//            //����ָ��·���е�����Excel�ļ� �������ɶ�Ӧ��3���ļ�
//            DirectoryInfo dInfo = Directory.CreateDirectory(ExcelDirectory_Path);
//            //�õ�ָ��·���е������ļ���Ϣ �൱�ھ��ǵõ����е�Excel��
//            FileInfo[] files = dInfo.GetFiles();
//            //���ݱ�����

//            for (int i = 0; i < files.Length; i++)
//            {
//                if (files[i].Extension != ".xlsx" &&
//                    files[i].Extension != ".xls")
//                    continue;
//                ReallyGenerateExcelClassAndContainer(files[i].FullName);

//            }
//        }
//        /// <summary>
//        /// �����������������������
//        /// </summary>
//        /// <param name="ExcelPath"></param>
//        private void ReallyGenerateExcelClassAndContainer(string ExcelPath)
//        {

//            if (!File.Exists(ExcelPath))
//            {
//                Debug.LogError("����������ʧ�ܣ�������·����" + ExcelPath);
//                return;
//            }
//            //����һ����ʱ�ĸ����࣬�Ա��ڱ߱༭���߶�ȡ
//            // ������ʱ�ļ�·��
//            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(ExcelPath));
//            // ����Excel�ĸ����ļ�����ʱ·��
//            File.Copy(ExcelPath, tempFilePath, true);
//            //���ݱ�����
//            DataTableCollection tableConllection = null;
//            using (FileStream fs = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
//            {
//                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
//                tableConllection = excelReader.AsDataSet().Tables;
//                fs.Close();
//            }
//            // ɾ����ʱ�ļ�
//            File.Delete(tempFilePath);
//            //�����ļ��е����б����Ϣ
//            foreach (DataTable table in tableConllection)
//            {
//                //�õ���������
//                LoadReadRule(table);
//                //�������ݽṹ��
//                GenerateExcelDataClass(table);
//                //����������
//                GenerateExcelContainer(table);
//            }
//        }
//        //��ʱ�洢λ��
//        private string TempGenerateDirectoryPath;
//        /// <summary>
//        /// ����Excel���Ӧ�����ݽṹ��
//        /// </summary>
//        /// <param name="table"></param>
//        private void GenerateExcelDataClass(DataTable table)
//        {

//            //�����ֶ������ֶ����͵Ķ�ȡ����
//            //�ֶ�����
//            DataRow rowName = table.Rows[propertyNameRowIndex];
//            //�ֶ�������
//            DataRow rowType = table.Rows[propertyTypeRowIndex];

//            TempGenerateDirectoryPath = data.OutPath + "\\" + table.TableName + "\\";
//            //�ж�·���Ƿ���� û�еĻ� �ʹ����ļ���
//            if (!Directory.Exists(TempGenerateDirectoryPath))
//                Directory.CreateDirectory(TempGenerateDirectoryPath);

//            //�������Ҫ���ɶ�Ӧ�����ݽṹ��ű� ��ʵ����ͨ����������ַ���ƴ�� Ȼ�����ļ�������
//            string str = null;

//            //��Key֮��ʼ�洢����
//            for (int i = 2; i < table.Columns.Count; i++)
//            {
//                str += "    public " + rowType[i].ToString() + " " + rowName[i].ToString() + ";\n";
//            }
//            str = "public class " + table.TableName + " : ExcelConfiguration" + "\n{\n" + str + "\n}";
//            //����
//            str = "[System.Serializable]\n" + str;
//            //��ƴ�Ӻõ��ַ����浽ָ���ļ���ȥ
//            File.WriteAllText(TempGenerateDirectoryPath + table.TableName + ".cs", str);
//            //ˢ��Project����
//            AssetDatabase.Refresh();
//        }
//        /// <summary>
//        /// ����Excel���Ӧ������������
//        /// </summary>
//        /// <param name="table"></param>
//        private void GenerateExcelContainer(DataTable table)
//        {

//            //�õ��ֶ�������
//            DataRow rowType = table.Rows[propertyTypeRowIndex];

//            string str = "using System.Collections.Generic;\n";
//            //������
//            str += "\n\n[System.Serializable]\n";
//            str += "public class " + table.TableName + $"Container : ExcelConfigurationContainer<{table.TableName.Replace("Container", null)}>" + "\n{\n\n";
//            str += "}";

//            File.WriteAllText(TempGenerateDirectoryPath + table.TableName + "Container.cs", str);


//            //ˢ��Project����
//            AssetDatabase.Refresh();
//            // ����������ɵ��¼�

//        }

//        /// <summary>
//        /// ����excel�����ļ���2��������
//        /// </summary>
//        /// <param name="table"></param>
//        public void GenerateExcelBinary()
//        {
//            string path = EditorUtility.OpenFilePanelWithFilters("ѡ��Ҫת���ĵ���Excel�ļ�", data.ExcelDirectory_Path, new string[] { "Excel files", "xlsx,xls" });
//            if (path == null || path == "") return;

//            ReallyGenerateExcelBinary(path);

//        }
//        /// <summary>
//        /// ����EXCELĿ¼������Excel�ļ���2��������
//        /// </summary>
//        public void GenerateAllExcelBinary()
//        {
//            if (File.Exists(ExcelDirectory_Path))
//                EditorUtility.DisplayDialog("����ʧ�ܣ�", "���������·���ļ���", "�ð�~");

//            //����ָ��·���е�����Excel�ļ� �������ɶ�Ӧ��3���ļ�
//            DirectoryInfo dInfo = Directory.CreateDirectory(ExcelDirectory_Path);
//            //�õ�ָ��·���е������ļ���Ϣ �൱�ھ��ǵõ����е�Excel��
//            FileInfo[] files = dInfo.GetFiles();
//            //���ݱ�����

//            for (int i = 0; i < files.Length; i++)
//            {
//                if (files[i].Extension != ".xlsx" &&
//                    files[i].Extension != ".xls")
//                    continue;
//                ReallyGenerateExcelBinary(files[i].FullName);

//            }
//        }
//        //�������ɶ��������ݲ���
//        private void ReallyGenerateExcelBinary(string ExcelPath)
//        {
//            if (!File.Exists(ExcelPath))
//            {
//                Debug.LogError("�������ļ�ת������ʧ�ܣ�������Excel·����" + ExcelPath);
//                return;
//            }
//            // ������ʱ�ļ�·��
//            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(ExcelPath));
//            // ����Excel�ĸ����ļ�����ʱ·��
//            File.Copy(ExcelPath, tempFilePath, true);
//            //���ݱ�����
//            DataTableCollection tableConllection = null;
//            using (FileStream fs = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
//            {
//                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
//                tableConllection = excelReader.AsDataSet().Tables;
//                fs.Close();
//            }
//            // ɾ����ʱ�ļ�
//            File.Delete(tempFilePath);
//            //��ʽ��ȡ��Ȼ�����ɶ������ļ�
//            foreach (DataTable table in tableConllection)
//            {
//                //�ȼ��ض�ȡ����
//                LoadReadRule(table);
//                Type ContainerType = null;
//                //����ȫ������
//                foreach (var assembly in AllAssembly)
//                {
//                    foreach (var type in assembly.GetTypes())
//                    {
//                        if (table.TableName + "Container" == type.Name)
//                            ContainerType = type;
//                    }
//                }
//                //�ж���û�д������
//                if (ContainerType == null)
//                {
//                    Debug.Log($"{table.TableName}��δ���������������࣬�޷����ɶ������ļ�");
//                    continue;
//                }
//                // ����������ʵ��
//                object ContainerInstance = Activator.CreateInstance(ContainerType);

//                //Debug.Log(ContainerInstance);
//                // ��ȡDictionary�ֶ�
//                FieldInfo fieldInfo = ContainerType.BaseType.GetField("container");
//                // ��ȡDictionary������
//                Type dictionaryType = fieldInfo.FieldType;

//                // ��ȡ����ֵ������
//                Type[] typeArguments = dictionaryType.GetGenericArguments();
//                Type keyType = typeArguments[0];
//                //ֵ���ͣ������ݶ�������
//                Type valueType = typeArguments[1];
//                // ����Dictionaryʵ��
//                Type specificDictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
//                object dictionaryInstance = Activator.CreateInstance(specificDictionaryType);
//                // ��ȡAdd����
//                MethodInfo addMethod = specificDictionaryType.GetMethod("Add");

//                //!!ţ�ƽ�������ӳ�������������**
//                //�ȵõ������ֶΣ��������������ģ���ʱ�������ǰ���������
//                FieldInfo[] FieldsInfo = valueType.GetFields();
//                List<string> FieldsName = new List<string>();
//                for (int i = 2; i < table.Columns.Count; i++)
//                {
//                    FieldsName.Add(table.Rows[propertyNameRowIndex][i].ToString());
//                }
//                //������������
//                List<FieldInfo> sortFiledInfos = new List<FieldInfo>();
//                //��������
//                for (int i = 0; i < FieldsName.Count; i++)
//                {
//                    for (int j = 0; j < FieldsInfo.Length; j++)
//                    {
//                        if (FieldsName[i] == FieldsInfo[j].Name)
//                            sortFiledInfos.Add(FieldsInfo[j]);
//                    }
//                }

//                //����д������
//                for (int i = StartReadRowIndex; i < table.Rows.Count; i++)
//                {
//                    //�õ���һע���У�����ע���У�ע���в���ȡ
//                    if (table.Rows[i][0].ToString().StartsWith("//")) { continue; }
//                    //�õ�id��
//                    string idValue = table.Rows[i][1].ToString();
//                    if (idValue.StartsWith("//") || string.IsNullOrEmpty(idValue)) { continue; }

//                    // ��������ֵ��ʵ������ӵ�Dictionary
//                    object keyValue = ConvertFromString(idValue, keyType);
//                    object ValueInstance = Activator.CreateInstance(valueType);


//                    //���������ĳ�Ա�ֶ���
//                    for (int j = 0; j < sortFiledInfos.Count; j++)
//                    {
//                        FieldInfo field = sortFiledInfos[j];
//                        //�ӵڶ��п�ʼ�洢��Ҳ����key֮����ֶ�
//                        field.SetValue(ValueInstance, ConvertFromString(table.Rows[i][j + 2].ToString(), field.FieldType));
//                    }
//                    // ����add�����������������
//                    addMethod.Invoke(dictionaryInstance, new object[] { keyValue, ValueInstance });
//                }
//                // ��Dictionaryʵ����ֵ���ֶ�
//                fieldInfo.SetValue(ContainerInstance, dictionaryInstance);
//                //ֱ��tm���л�
//                if (!Directory.Exists(data.OutPath + "\\"))
//                {
//                    Directory.CreateDirectory(data.OutPath + "\\" + table.TableName + "\\");
//                }
//                //Debug.Log(data.OutPath + "\\" + table.TableName + "." + data.SuffixName);
//                //����Ϊ ����Container.�Զ����׺��
//                BinaryManager.SaveToPath(ContainerInstance
//                    , data.OutPath + "\\" + table.TableName + "Container" + "." + data.SuffixName);
//                //����Ĭ�ϲ���
//                ResetReadRule();
//            }
//            AssetDatabase.Refresh();
//        }

//        /// <summary>
//        /// װ��ת������
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
//                    //����ת��
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