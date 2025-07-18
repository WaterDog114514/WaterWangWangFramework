using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Excel游戏数据配置文件加载工具，配合Excel加载工具使用
/// </summary>
public class ExcelLoaderSystem : Singleton<ExcelLoaderSystem>,IFrameworkSystem
{
    private Dictionary<string, IDictionary> dic_LoadedContainer;

    public void InitializedSystem()
    {
        dic_LoadedContainer = new Dictionary<string, IDictionary>();
    }
    /// <summary>
    /// 后面要做，通过框架设置配置文件存储位置
    /// </summary>
    private string ConfigurationDiretoryPath;
    private const string FileSuffix = ".waterdogdata";
    //KeyType：字典Key，ConfigType：Configuration存储类型
    public IDictionary GetDataContainer(Type ConfigType,string FileName)
    {
        //根据是否有加载名进行选择加载
        string loadFileName = string.IsNullOrEmpty(FileName) ? ConfigType.Name + "Container" : FileName;
        if (dic_LoadedContainer.ContainsKey(loadFileName)) return dic_LoadedContainer[loadFileName];
        else
        {
            return LoadExcelContainer(ConfigType, loadFileName);
        }
    }
    /// <summary>
    /// 取得Excel配置表，通过FileName去加载StreamingAsset，此法可以加载多个相同类型的Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Dictionary<int, TConfigType> GetDataContainer<TConfigType>(string FileName = null) where TConfigType : ExcelConfiguration
    {
        //根据是否有加载名进行选择加载
        string loadFileName = string.IsNullOrEmpty(FileName) ? typeof(TConfigType).Name  : FileName;
        if (!dic_LoadedContainer.ContainsKey(loadFileName))
        {
            var dictionary = LoadExcelContainer(typeof(TConfigType), loadFileName);
            return dictionary as Dictionary<int, TConfigType>;
        }
        return dic_LoadedContainer[loadFileName] as Dictionary<int, TConfigType>;
    }

    /// <summary>
    /// 将通过StreamingAssets中加载文件  
    /// </summary>
    /// <param name="FileName">文件名(包括后缀名)</param>
    private IDictionary LoadExcelContainer(Type ConfigType, string FileName)
    {
        var path = Path.Combine(Application.streamingAssetsPath, FileName + FileSuffix);
        if (!File.Exists(path))
        {
            Debug.Log($"加载失败，不存在此{FileName}文件");
            return null;
        }
        //先看有没有。有了就不要重复添加
        var genericTypeDefinition = typeof(ExcelConfigurationContainer<>);
        var typeArguments = new Type[] { ConfigType };
        var container = BinaryManager.LoadOpenGeneric(genericTypeDefinition, typeArguments, path);
        if (container == null)
        {
            Debug.LogError("加载容器失败");
            return null;
        }
        // 获取 container 属性
        var containerProperty = container.GetType().BaseType.GetField("container");
        if (containerProperty == null)
        {
            Debug.LogError("未找到 container 属性");
            return null;
        }
        // 获取 container 属性的值
        var containerValue = containerProperty.GetValue(container) as IDictionary;
        if (containerValue == null)
        {
            Debug.LogError("container 属性值为空");
            return null;
        }

        // 更新或添加到字典
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
    /// 获得表中某一列属性所有数值信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name=""></param>
    /// <returns></returns>
    //public string[] GetDataPropertyInfo<TKey,TConfigType>(string PropertyName, string FileName) where  TConfigType : ExcelConfiguration
    //{
    //    var container = GetDataContainer<TKey,TConfigType>()  as Dictionary<TKey,TConfigType>;
    //    // 反射获取dataDic字段
    //    var dataDicField = typeof(T).GetField("dataDic");
    //    if (dataDicField == null)
    //    {
    //        Debug.LogError($"读取错误，请不要在{typeof(T).Name}中改dataDic属性名");
    //        return null;
    //    }
    //    // 获取dataDic的值
    //    var dataDicValue = dataDicField.GetValue(container) as IDictionary;

    //    // 获取字典的值类型（类型2）
    //    var valueType = dataDicField.FieldType.GetGenericArguments()[1];

    //    // 通过类型2得到名为name的字段
    //    var nameField = valueType.GetField(PropertyName);
    //    if (nameField == null)
    //    {
    //        Debug.LogWarning($"Excel表加载提示：{valueType.Name}不存在{PropertyName}的字段，已返回null");
    //        return null;
    //    }
    //    int index = 0;
    //    string[] propertyNames = new string[dataDicValue.Count];
    //    // 遍历字典，获取名为name的字段的值
    //    foreach (DictionaryEntry pair in dataDicValue)
    //    {
    //        object valueObject = pair.Value;
    //        propertyNames[index] = nameField.GetValue(valueObject).ToString();
    //        index++;
    //    }
    //    return propertyNames;
    //}
}
