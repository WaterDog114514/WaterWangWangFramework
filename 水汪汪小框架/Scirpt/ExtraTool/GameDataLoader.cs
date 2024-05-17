using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Excel游戏数据配置文件加载工具，配合Excel加载工具使用
/// </summary>
public class GameExcelDataLoader : Singleton_UnMono<GameExcelDataLoader>
{
    private Dictionary<string, DataBaseContainer> dic_LoadedContainer = new Dictionary<string, DataBaseContainer>();
    private FrameworkSettingData FKsettingData = null;
    private ExcelToolSettingData ExcelsettingData = null;
    public GameExcelDataLoader()
    {
        FKsettingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
        ExcelsettingData= SettingDataLoader.Instance.LoadData<ExcelToolSettingData>();
        
    }
    /// <summary>
    /// 取得Excel配置表，如果不存在则会加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetDataContainer<T>() where T : DataBaseContainer
    {
        string key =typeof(T).Name.Replace("Container",null);
        if (!dic_LoadedContainer.ContainsKey(key))
        {
            string path = FKsettingData.loadContainerSetting.IsDebugStreamingAssetLoad? Application.streamingAssetsPath+"/settinginfo": FKsettingData.loadContainerSetting.DataPath;
            //先加载
           T Container = BinaryManager.Instance.Load<T>($"{path}/{key}.{ExcelsettingData.SuffixName}");
            dic_LoadedContainer.Add(key, Container);
        }
        return dic_LoadedContainer[key] as T;
    }

    /// <summary>
    /// 获得表中某一列属性所有数值信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name=""></param>
    /// <returns></returns>
    public string[] GetDataPropertyInfo<T>(string PropertyName) where T : DataBaseContainer
    {
        T container =  GetDataContainer<T>();
        // 反射获取dataDic字段
        var dataDicField = typeof(T).GetField("dataDic");
        if (dataDicField == null)
        {
            Debug.LogError($"读取错误，请不要在{typeof(T).Name}中改dataDic属性名");
            return null;
        }
        // 获取dataDic的值
        var dataDicValue = dataDicField.GetValue(container) as IDictionary;

        // 获取字典的值类型（类型2）
        var valueType = dataDicField.FieldType.GetGenericArguments()[1];

        // 通过类型2得到名为name的字段
        var nameField = valueType.GetField(PropertyName);
        if (nameField == null)
        {
            Debug.LogWarning($"Excel表加载提示：{valueType.Name}不存在{PropertyName}的字段，已返回null");
            return null;
        }
        int index =0;
        string[] propertyNames =  new string[dataDicValue.Count];
        // 遍历字典，获取名为name的字段的值
        foreach (DictionaryEntry pair in dataDicValue)
        {
            object valueObject = pair.Value;
            propertyNames[index] = nameField.GetValue(valueObject).ToString();
            index++;
        }
        return propertyNames;
    }
}
