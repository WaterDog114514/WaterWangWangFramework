using System;
using UnityEngine;
using WDFramework;



/// <summary>
/// 游戏项目设置
/// 可以获取AB包的资源路径，日志文件路径等等 
/// </summary>
[Serializable]
public class GameProjectSettingData : BaseSettingData
{
    public ABLoadSettingData abLoadSetting;
    public LoadContainerSettingData loadContainerSetting;
    public UISettingData uiSetting;
    public override void IntiValue()
    {
        //初始化所有设定子类
        abLoadSetting = new ABLoadSettingData();
        loadContainerSetting = new LoadContainerSettingData();
        uiSetting = new UISettingData();
        defaultPoolSetting = new PoolSetting() { MaxCount = 20, PoolType = Pool.E_PoolType.Expansion };
    }
    /// <summary>
    /// AB包加载设置
    /// </summary>
    [Serializable]
    public class ABLoadSettingData
    {
        /// <summary>
        /// AB包主包名
        /// </summary>
        public string ABMainName = null;
        /// <summary>
        /// 是否开启AB包调试，开启后从Editor开始读取
        /// </summary>
        public bool IsDebugABLoad = false;
        /// <summary>
        /// 开启从Streaming加载AB包，勾选后，ab包加载路径设置为StreamingAsset
        /// </summary>
        public bool IsStreamingABLoad = false;
        /// <summary>
        /// AB包在编辑器中加载位置
        /// </summary>
        public string ABEditorLoadPath;
        /// <summary>
        /// AB包游戏目录相对路径中读取路径
        /// </summary>
        public string ABRuntimeLoadPath;
        public ABLoadSettingData()
        {
            
        }
    }

    /// <summary>
    /// 游戏配置文件加载设置
    /// </summary>
    [Serializable]
    public class LoadContainerSettingData
    {

        public string DataPath;
        public string SuffixName;
        public bool IsDebugStreamingAssetLoad = false;

    }
    /// <summary>
    /// 默认没有设置组的对象池预设
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