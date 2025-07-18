using System;
using UnityEngine;
using WDFramework;



/// <summary>
/// ��Ϸ��Ŀ����
/// ���Ի�ȡAB������Դ·������־�ļ�·���ȵ� 
/// </summary>
[Serializable]
public class GameProjectSettingData : BaseSettingData
{
    public ABLoadSettingData abLoadSetting;
    public LoadContainerSettingData loadContainerSetting;
    public UISettingData uiSetting;
    public override void IntiValue()
    {
        //��ʼ�������趨����
        abLoadSetting = new ABLoadSettingData();
        loadContainerSetting = new LoadContainerSettingData();
        uiSetting = new UISettingData();
        defaultPoolSetting = new PoolSetting() { MaxCount = 20, PoolType = Pool.E_PoolType.Expansion };
    }
    /// <summary>
    /// AB����������
    /// </summary>
    [Serializable]
    public class ABLoadSettingData
    {
        /// <summary>
        /// AB��������
        /// </summary>
        public string ABMainName = null;
        /// <summary>
        /// �Ƿ���AB�����ԣ��������Editor��ʼ��ȡ
        /// </summary>
        public bool IsDebugABLoad = false;
        /// <summary>
        /// ������Streaming����AB������ѡ��ab������·������ΪStreamingAsset
        /// </summary>
        public bool IsStreamingABLoad = false;
        /// <summary>
        /// AB���ڱ༭���м���λ��
        /// </summary>
        public string ABEditorLoadPath;
        /// <summary>
        /// AB����ϷĿ¼���·���ж�ȡ·��
        /// </summary>
        public string ABRuntimeLoadPath;
        public ABLoadSettingData()
        {
            
        }
    }

    /// <summary>
    /// ��Ϸ�����ļ���������
    /// </summary>
    [Serializable]
    public class LoadContainerSettingData
    {

        public string DataPath;
        public string SuffixName;
        public bool IsDebugStreamingAssetLoad = false;

    }
    /// <summary>
    /// Ĭ��û��������Ķ����Ԥ��
    /// </summary>
    public PoolSetting defaultPoolSetting;

    [System.Serializable]
    public class UISettingData
    {
        public int ReferenceResolutionX;
        public int ReferenceResolutionY;
        public float Match;
        public UISettingData()
        {
            Match = 0;
            ReferenceResolutionX = 1920;
            ReferenceResolutionY = 1080;
        }
    }
}