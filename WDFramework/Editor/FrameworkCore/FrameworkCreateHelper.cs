using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WDEditor;
using static GameStageManager;

//用于绘制自定义图标专用的辅助器
public static class FrameworkCreateHelper
{

    [MenuItem("水汪汪框架/创建内核(必需)")]
    public static void CreateGameKernel()
    {
        var obj =   Object.FindObjectOfType<GameKernelMono>();
        if(obj!=null)
        {
            Debuger.LogCyan("创建失败：已存在该内核，无需重复创建");
            Selection.activeObject = obj.gameObject;
            return;
        }
        var newObj = new GameObject();
        obj = newObj.AddComponent<GameKernelMono>();
        newObj.name = "GameKernel";
        //创建成功
        Selection.activeObject = obj.gameObject;

    }
    [MenuItem("水汪汪框架/创建初始化First阶段脚本（必需）")]
    public static void CreateFirstStage()
    {
        // 通过反射查找所有InitialStage的子类
        var initialStageTypes = ReflectionHelper.GetSubclasses(typeof(InitialStage));
        if(initialStageTypes.Count > 0 )
        {
            Debuger.LogCyan($"已存在初始化第一阶段脚本{initialStageTypes[0].Name}，无须再次创建");
            return;
        }
        var ScriptPath = EditorPathHelper.LocalDirectory("WDProjectScript");
        var path = EditorUtility.SaveFilePanel("创建初始化阶段脚本",ScriptPath,null,"cs");
        if(string.IsNullOrEmpty(path) ) { return;}
        var ClassName = Path.GetFileNameWithoutExtension(path);
        ScriptBuildTask task = new ScriptBuildTask(ClassName);
        task.SetInheritance("InitialStage")
            .AddUsing("using static GameStageManager;\r\nusing System.Collections;")
            .AddMember("public override GameStage nextAutoChangeStage => null;")
            .AddMethod("public override IEnumerator StageOperator()\r\n        {\r\n           \r\n        }")
            .SetOutpath(path);
        task.Execute();
            
            
    }
}
