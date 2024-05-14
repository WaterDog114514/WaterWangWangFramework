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

}
