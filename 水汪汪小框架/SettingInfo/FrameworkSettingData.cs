using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.AI;



/// <summary>
/// 框架设置数据 可以获取AB包的资源路径，日志文件路径等等 
/// </summary>
[Serializable]
public class FrameworkSettingData : BaseSettingData
{
    public ABLoadSettingData abLoadSetting;
    public override void IntiValue()
    {
        abLoadSetting = new ABLoadSettingData();
        abLoadSetting.ABLoadPath = "gameAssets/";
        abLoadSetting.VoicePackName = "voice";
        abLoadSetting.ObjPrefabPackName = "obj_prefab";
        abLoadSetting.UIPrefabPackName = "ui_prefab";
        abLoadSetting.ABEditorLoadPath = "Assets/Editor/ArtRes/";
    }

    public override string DirectoryPath  => Application.dataPath + @"\水汪汪小框架\SettingInfo\Resources\"; 
    public override string DataName => "FrameworkSetting";
   
}
[Serializable]
public class ABLoadSettingData
{
    /// <summary>
    /// AB包主包名
    /// </summary>
    public string ABMainName = null;
    /// <summary>
    ///声音资源包
    /// </summary>
    public string VoicePackName;
    /// <summary>
    /// 游戏对象预制体包
    /// </summary>
    public string ObjPrefabPackName;
    /// <summary>
    /// UI预制体包
    /// </summary>
    public string UIPrefabPackName;
    /// <summary>
    /// 是否开启AB包调试，开启后从Editor开始读取
    /// </summary>
    public bool IsDebugABLoad = false;
    /// <summary>
    /// 开启从Streaming加载AB包
    /// </summary>
    public bool IsStreamingABLoad = false;
    /// <summary>
    /// AB包在编辑器中加载位置
    /// </summary>
    public string ABEditorLoadPath;
    /// <summary>
    /// AB包游戏目录中读取路径
    /// </summary>
    public string ABLoadPath;


}

