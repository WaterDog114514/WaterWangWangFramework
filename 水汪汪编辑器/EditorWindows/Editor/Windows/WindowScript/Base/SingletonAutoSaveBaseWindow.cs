using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Compilation;
using UnityEngine;

/// <summary>
/// 可以自动存储和加载窗口数据的单一窗口类型
/// </summary>
public abstract class SingletonAutoSaveBaseWindow : BaseWindow
{
    #region 窗口绘制相关
    protected override void OnDestroy()
    {
        base.OnDestroy();
        m_CloseSaveWindowsData();
    }
    protected override void OnGUI()
    {
        base.OnGUI();
        //绘制窗口保存逻辑
        this.SettingSaveLoadDraw();

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        m_LoadWindowsSaveInfo();
        CompilationPipeline.compilationStarted += m_callback_CompipeAutoSave;
    }
    #endregion


    #region 保存窗口数据自动加载数据相关

    //不可保存数据，运行不会自动加载
    /// <summary>
    /// 默认的编辑器是可以保存数据的
    /// </summary>
    protected bool b_IsCanSaveData = true;

    /// <summary>
    /// 自动保存路径
    /// </summary>
    protected string AutoSavePath;

    /// <summary>
    /// 加载窗口时初始化逻辑
    /// </summary>
    /// <param name="OnReloadWin"></param>
    public void m_LoadWindowsSaveInfo()
    {
        if (!b_IsCanSaveData) return;
        //设置自动保存路径
        AutoSavePath = EM_WinSetting.Instance.SettingData.AutoSavePath + GetType().Name + ".json";
        if (File.Exists(AutoSavePath))
        {
            //自动加载逻辑
            EditorMain tempMain = this.LoadWindowsData(AutoSavePath);
            //要是加载的文件有不自动加载，取消新实例化的自动加载吧
            if (tempMain == null)
                editorMain = Activator.CreateInstance(MainType) as EditorMain;
            else editorMain = tempMain;
        }
        else
        {
            //不存在文件就新建一个实例
            editorMain = Activator.CreateInstance(MainType) as EditorMain;
        }
    }
    /// <summary>
    /// 关闭窗口时自动保存对象
    /// </summary>
    public void m_CloseSaveWindowsData()
    {
        if (b_IsCanSaveData)
            this.SaveWindowsDataToPath(AutoSavePath);
    }
    /// <summary>
    /// 开始编译时就要自动保存了
    /// </summary>
    /// <param name="obj"></param>
    public void m_callback_CompipeAutoSave(object obj)
    {
        Debug.Log("开始保存了" + obj);
        m_CloseSaveWindowsData();
    }
    #endregion
}
