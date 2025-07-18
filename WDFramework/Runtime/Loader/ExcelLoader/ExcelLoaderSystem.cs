using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Excel��Ϸ���������ļ����ع��ߣ����Excel���ع���ʹ��
/// </summary>
public class ExcelLoaderSystem : Singleton<ExcelLoaderSystem>,IFrameworkSystem
{
    private Dictionary<string, IDictionary> dic_LoadedContainer;

    public void InitializedSystem()
    {
        dic_LoadedContainer = new Dictionary<string, IDictionary>();
    }
    /// <summary>
    /// ����Ҫ����ͨ��������������ļ��洢λ��
    /// </summary>
    private string ConfigurationDiretoryPath;
    private const string FileSuffix = ".waterdogdata";
    //KeyType���ֵ�Key��ConfigType��Configuration�洢����
    public IDictionary GetDataContainer(Type ConfigType,string FileName)
    {
        //�����Ƿ��м���������ѡ�����
        string loadFileName = string.IsNullOrEmpty(FileName) ? ConfigType.Name + "Container" : FileName;
        if (dic_LoadedContainer.ContainsKey(loadFileName)) return dic_LoadedContainer[loadFileName];
        else
        {
            return LoadExcelContainer(ConfigType, loadFileName);
        }
    }
    /// <summary>
    /// ȡ��Excel���ñ�ͨ��FileNameȥ����StreamingAsset���˷����Լ��ض����ͬ���͵�Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Dictionary<int, TConfigType> GetDataContainer<TConfigType>(string FileName = null) where TConfigType : ExcelConfiguration
    {
        //�����Ƿ��м���������ѡ�����
        string loadFileName = string.IsNullOrEmpty(FileName) ? typeof(TConfigType).Name  : FileName;
        if (!dic_LoadedContainer.ContainsKey(loadFileName))
        {
            var dictionary = LoadExcelContainer(typeof(TConfigType), loadFileName);
            return dictionary as Dictionary<int, TConfigType>;
        }
        return dic_LoadedContainer[loadFileName] as Dictionary<int, TConfigType>;
    }

    /// <summary>
    /// ��ͨ��StreamingAssets�м����ļ�  
    /// </summary>
    /// <param name="FileName">�ļ���(������׺��)</param>
    private IDictionary LoadExcelContainer(Type ConfigType, string FileName)
    {
        var path = Path.Combine(Application.streamingAssetsPath, FileName + FileSuffix);
        if (!File.Exists(path))
        {
            Debug.Log($"����ʧ�ܣ������ڴ�{FileName}�ļ�");
            return null;
        }
        //�ȿ���û�С����˾Ͳ�Ҫ�ظ����
        var genericTypeDefinition = typeof(ExcelConfigurationContainer<>);
        var typeArguments = new Type[] { ConfigType };
        var container = BinaryManager.LoadOpenGeneric(genericTypeDefinition, typeArguments, path);
        if (container == null)
        {
            Debug.LogError("��������ʧ��");
            return null;
        }
        // ��ȡ container ����
        var containerProperty = container.GetType().BaseType.GetField("container");
        if (containerProperty == null)
        {
            Debug.LogError("δ�ҵ� container ����");
            return null;
        }
        // ��ȡ container ���Ե�ֵ
        var containerValue = containerProperty.GetValue(container) as IDictionary;
        if (containerValue == null)
        {
            Debug.LogError("container ����ֵΪ��");
            return null;
        }

        // ���»���ӵ��ֵ�
        if (dic_LoadedContainer.ContainsKey(FileName))
        {
            dic_LoadedContainer[FileName] = containerValue;
        }
        else
        {
            dic_LoadedContainer.Add(FileName, containerValue);
        }

        return containerValue;
    }


    /// <summary>
    /// ��ñ���ĳһ������������ֵ��Ϣ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name=""></param>
    /// <returns></returns>
    //public string[] GetDataPropertyInfo<TKey,TConfigType>(string PropertyName, string FileName) where  TConfigType : ExcelConfiguration
    //{
    //    var container = GetDataContainer<TKey,TConfigType>()  as Dictionary<TKey,TConfigType>;
    //    // �����ȡdataDic�ֶ�
    //    var dataDicField = typeof(T).GetField("dataDic");
    //    if (dataDicField == null)
    //    {
    //        Debug.LogError($"��ȡ�����벻Ҫ��{typeof(T).Name}�и�dataDic������");
    //        return null;
    //    }
    //    // ��ȡdataDic��ֵ
    //    var dataDicValue = dataDicField.GetValue(container) as IDictionary;

    //    // ��ȡ�ֵ��ֵ���ͣ�����2��
    //    var valueType = dataDicField.FieldType.GetGenericArguments()[1];

    //    // ͨ������2�õ���Ϊname���ֶ�
    //    var nameField = valueType.GetField(PropertyName);
    //    if (nameField == null)
    //    {
    //        Debug.LogWarning($"Excel�������ʾ��{valueType.Name}������{PropertyName}���ֶΣ��ѷ���null");
    //        return null;
    //    }
    //    int index = 0;
    //    string[] propertyNames = new string[dataDicValue.Count];
    //    // �����ֵ䣬��ȡ��Ϊname���ֶε�ֵ
    //    foreach (DictionaryEntry pair in dataDicValue)
    //    {
    //        object valueObject = pair.Value;
    //        propertyNames[index] = nameField.GetValue(valueObject).ToString();
    //        index++;
    //    }
    //    return propertyNames;
    //}
}
