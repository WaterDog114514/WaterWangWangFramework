using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WDEditor;
using static GameStageManager;

//���ڻ����Զ���ͼ��ר�õĸ�����
public static class FrameworkCreateHelper
{

    [MenuItem("ˮ�������/�����ں�(����)")]
    public static void CreateGameKernel()
    {
        var obj =   Object.FindObjectOfType<GameKernelMono>();
        if(obj!=null)
        {
            Debuger.LogCyan("����ʧ�ܣ��Ѵ��ڸ��ںˣ������ظ�����");
            Selection.activeObject = obj.gameObject;
            return;
        }
        var newObj = new GameObject();
        obj = newObj.AddComponent<GameKernelMono>();
        newObj.name = "GameKernel";
        //�����ɹ�
        Selection.activeObject = obj.gameObject;

    }
    [MenuItem("ˮ�������/������ʼ��First�׶νű������裩")]
    public static void CreateFirstStage()
    {
        // ͨ�������������InitialStage������
        var initialStageTypes = ReflectionHelper.GetSubclasses(typeof(InitialStage));
        if(initialStageTypes.Count > 0 )
        {
            Debuger.LogCyan($"�Ѵ��ڳ�ʼ����һ�׶νű�{initialStageTypes[0].Name}�������ٴδ���");
            return;
        }
        var ScriptPath = EditorPathHelper.LocalDirectory("WDProjectScript");
        var path = EditorUtility.SaveFilePanel("������ʼ���׶νű�",ScriptPath,null,"cs");
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
