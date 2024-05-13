using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 主编辑器窗口设置管理类，用来存储并设定编辑器风格信息，也可以用来生成编辑器类
/// </summary>
class EM_FrameworkSetting : EditorMain
{
    public static EM_FrameworkSetting Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EM_FrameworkSetting();
            return _instance;
        }
    }

    

    /// <summary>
    /// 唯一单利
    /// </summary>
    private static EM_FrameworkSetting _instance = new EM_FrameworkSetting();
    public Texture WindowsBackground;
    public EM_FrameworkSetting()
    {

    }

    public void m_SaveData()
    {

    }
}