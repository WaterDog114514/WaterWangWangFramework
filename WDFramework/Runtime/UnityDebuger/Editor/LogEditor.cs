/*------------------------------------------------------------------
*
* Title: 毕业级日志系统 
*
* Description: 支持本地文件写入、自定义颜色日志、FPS实时显示、手机日志运行时查看、日志代码编译剔除、ProtoBuff转Json
* 
* Author: 腾讯课堂 铸梦xy
*
* Date: 2023.8.13
*
* Modify: 
-------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class LogEditor
{
    [MenuItem("ZMLog/打开日志系统")]
    public static void LoadReport()
    {
        ScriptingDefineSymbols.AddScriptingDefineSymbol("OPEN_LOG");
        GameObject reportObj = GameObject.Find("Reporter");
        if (reportObj==null)
        {
            reportObj= GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/UnityDebuger/Unity-Logs-Viewer/Reporter.prefab"));
            reportObj.name = "Reporter";
            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            AssetDatabase.Refresh();
            Debug.Log("Open Log Finish!");
        }
    }
    [MenuItem("ZMLog/关闭日志系统")]
    public static void CloseReport()
    {
        ScriptingDefineSymbols.RemoveScriptingDefineSymbol("OPEN_LOG");
        GameObject reportObj = GameObject.Find("Reporter");
        if (reportObj!=null)
        {
            GameObject.DestroyImmediate(reportObj);
            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            AssetDatabase.Refresh();
            Debug.Log("Cloase Log Finish!");
        }
    }
}
