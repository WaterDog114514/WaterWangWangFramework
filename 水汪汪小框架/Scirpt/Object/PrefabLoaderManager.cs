using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoaderManager : Singleton_UnMono<PrefabLoaderManager>
{

    public PrefabLoaderManager()
    {
        SettingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();

    }

    private FrameworkSettingData SettingData;


    public Dictionary<string, PrefabInfo> dic_PrefabsFromName = new Dictionary<string, PrefabInfo>();
    public Dictionary<int, PrefabInfo> dic_PrefabsFromID = new Dictionary<int, PrefabInfo>();
    public enum E_LoadType
    {
        UIPrefab, GameObjPrefab
    }
    public PrefabInfo GetPrefabInfoFromName(string GameObjectName)
    {
        if (dic_PrefabsFromName.ContainsKey(GameObjectName))
            return dic_PrefabsFromName[GameObjectName];
        else
        {
            Debug.LogError($"获取{GameObjectName}预设体失败！不存在此预设体对象");
            return null;
        }
    }
    public PrefabInfo GetPrefabInfoFromID(int id)
    {
        if (dic_PrefabsFromID.ContainsKey(id))
            return dic_PrefabsFromID[id];
        else
        {
            Debug.LogError($"获取id为{id}的预设体失败！不存在此预设体对象");
            return null;
        }
    }

    /// <summary>
    /// 根据Excel表加载表中的预设体
    /// </summary>
    /// <typeparam name="T">预设体所在表</typeparam>
    public void PreLoadPrefabFrmoExcel<T>() where T : DataBaseContainer
    {
        //先获取预设体表中的路径和对象池组以及唯一id
        string[] paths = GameExcelDataLoader.Instance.GetDataPropertyInfo<T>(SettingData.loadPrefabSetting.ExcelArtPathName);
        string[] groups = GameExcelDataLoader.Instance.GetDataPropertyInfo<T>(SettingData.loadPrefabSetting.ExcelPoolGroupName);
        string[] IDs = GameExcelDataLoader.Instance.GetDataPropertyInfo<T>(SettingData.loadPrefabSetting.ExcelIDName);


        GameObject[] LoadPrefab = new GameObject[paths.Length];
        ResLoader.Instance.CreatePreloadTaskFromPaths(paths, (resCollection) =>
        {
            Debug.Log("加载完毕");
            for (int i = 0; i < resCollection.Length; i++)
            {
                GameObject obj = resCollection[i].GetAsset<GameObject>();
                if (obj == null)
                {
                    Debug.LogError($"加载预时，加载到非预设体资源！{resCollection[i].Asset.name}");
                    continue;
                }
                //成功加载开始分类记录
                LoadPrefab[i] = obj;
            }


            //赋值加组
            for (int i = 0; i < LoadPrefab.Length; i++)
            {
                PrefabInfo info = null;
                //判断有没有组，断路版
                if (groups == null || groups[i] == null || groups[i] == "")
                {
                    info = new UnLimitedPrefabInfo();
                }
                else
                {
                    info = new PoolPrefabInfo() { PoolGroup = groups[i] , identity = LoadPrefab[i].name };
                }
                info.res = LoadPrefab[i];


                //放进名字的字典
                if (!dic_PrefabsFromName.ContainsKey(LoadPrefab[i].name))
                    dic_PrefabsFromName.Add(LoadPrefab[i].name, info);
                else
                    Debug.LogError($"已有重名预设体{LoadPrefab[i].name}，请检查预设体，请勿重名！");

                //放进id的字典
                if (IDs != null)
                {
                    int id = 0;
                    try
                    {
                        id = int.Parse(IDs[i]);
                    }
                    catch
                    {
                        Debug.LogError("Excel表中有非法字符，无法转换ID为数字，请检查");
                        continue;
                    }
                    //转换成功，判断放进吗
                    if (!dic_PrefabsFromID.ContainsKey(id))
                    {
                        dic_PrefabsFromID.Add(id, info);
                    }
                    else
                    {
                        Debug.LogError($"已存在{id}的预设体ID，请保证预设体id的唯一性！");
                    }

                }

            }

        });
      


    }
    public void DemoTEst()
    {

    }
}
//预设体信息
//预设体有UI预设体和普通游戏对象预设体之分
public class PrefabInfo
{
    public GameObject res;
}
/// <summary>
/// 不会被对象池限制的预设体
/// </summary>
public class UnLimitedPrefabInfo : PrefabInfo
{

}
/// <summary>
/// 被对象池束缚的预设体
/// </summary>
public class PoolPrefabInfo : PrefabInfo
{
    public string identity;
    public string PoolGroup;
}