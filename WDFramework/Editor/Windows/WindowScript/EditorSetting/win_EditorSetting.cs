using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace WDEditor
{
    /// <summary>
    ///编辑器开发的设置管理类，用来查看并设定编辑器信息
    /// </summary>
    public class win_EditorSetting : BaseWindow<winDraw_EditorSetting, winData_EditorSetting>
    {

        [MenuItem("水汪汪框架/面板主设定")]
        protected static void OpenWindow()
        {

            EditorWindow.GetWindow<win_EditorSetting>();
        }


    }
}
