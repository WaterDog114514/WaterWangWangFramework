using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// ģ��ϵͳ���õ������ļ���������ר�ż��������ļ�
/// �洢λ��ΪStreamingAssets
/// </summary>
public class SystemSettingLoader : Singleton<SystemSettingLoader>
{
    /// <summary>
    /// �洢�ļ���λ��
    /// </summary>
    private string DirectoryPath => Application.streamingAssetsPath + "\\SettingData";
    /// <summary>
    /// �Ѿ����ع��������ļ����еĻ�ֱ�Ӵ�������
    /// </summary>
    private Dictionary<string, BaseSettingData> dic_LoadedData = new Dictionary<string, BaseSettingData>();
    /// <summary>
    /// ֻ���ڱ༭��ģʽ�²��ܱ���
    /// </summary>
#if UNITY_EDITOR

    public void SaveData<T>(T data) where T : BaseSettingData
    {
        //PC�˴洢����
        //�����ļ����Ƿ����
        if (!Directory.Exists(DirectoryPath))
            Directory.CreateDirectory(DirectoryPath);
        //�ٴμ��
        BinaryManager.SaveToPath(data, Path.Combine(DirectoryPath, $"{typeof(T).Name}.settingdata"));
    }
#endif
    public T LoadData<T>() where T : BaseSettingData, new()
    {

        //���ع�ֱ������ֱ������
        if (dic_LoadedData.ContainsKey(typeof(T).Name))
            return dic_LoadedData[typeof(T).Name] as T;
        //PC�˼���
#if UNITY_STANDALONE_WIN
        //��һ�μ���
        var LoadPath = Path.Combine(DirectoryPath, $"{typeof(T).Name}.settingdata");
        //���ж���û��·��
        if (!File.Exists(LoadPath))
        {
            Debug.LogWarning($"���ü���ʧ�ܣ������ڸ�·��{LoadPath}���Ѵ���Ĭ������");
            var defaultData = Activator.CreateInstance(typeof(T)) as T;
            return defaultData;
        }
        //�����ٽ��м���
        T data = BinaryManager.Load<T>(LoadPath);
        return data;
#endif
        //��׿�˼��� �ȴ�д

    }


}
