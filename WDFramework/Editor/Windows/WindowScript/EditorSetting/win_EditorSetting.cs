using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace WDEditor
{
    /// <summary>
    ///�༭�����������ù����࣬�����鿴���趨�༭����Ϣ
    /// </summary>
    public class win_EditorSetting : BaseWindow<winDraw_EditorSetting, winData_EditorSetting>
    {

        [MenuItem("ˮ�������/������趨")]
        protected static void OpenWindow()
        {

            EditorWindow.GetWindow<win_EditorSetting>();
        }


    }
}
