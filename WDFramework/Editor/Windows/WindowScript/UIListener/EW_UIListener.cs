//using System;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.Compilation;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using UnityEngine.UIElements;

///*
// * Ҫʵ�ֵĹ���
// * 1.��һ��Canvas�µ�UI��������м����������������������UI�ؼ��ĺ��ӱ�עһ��
// * 2.����Ҫ��ǰ��ע�����壺��Ĭ�����ģ����縸����������Ĭ�����������壩
// * 3.���ܣ��Ա�ע�õ������һ�������壬�������������ӽű���������������UI�Ķ���
// * ������ӽű����û���Ҫ�Զ���ű���������������Щ��ע��������ı�����

// ��������ǿ����ߣ�������ң�ÿ���ű�ֻ�ܶ�ͬһ���������һ��

// * ����ͨ����ע���ӻ������ű�
// * 1.û�м����ű��Ϳ���ֱ����Ӱ�
// * 2.�м����ű�������Ƿ�������������й������Ƿ��Ѿ��󶨹��ˣ�
// * 3.�󶨹��˾Ͳ����ٰ󶨣�û�а󶨾Ϳ��԰�
 
//   ���ڿ��ӻ������ű�DebugEventListenerGizmos
//    1.�ܲ鿴�󶨵ĸ�������˭
//    2.�ܲ鿴�󶨵��ĸ�����
//    3.��һ���ҵ���������ʾ
// */
////����һ��
//public class EW_UIListener : EditorMain
//{
//    /// <summary>
//    /// Ĭ�ϵĲ�Ҫ���ɵļ���
//    /// </summary>
//    private List<string> list_defaultName = new List<string>() { "Image",
//                                                            "Text (TMP)",
//                                                            "RawImage",
//                                                            "Background",
//                                                            "Checkmark",
//                                                            "Label",
//                                                            "Text (Legacy)",
//                                                            "Arrow",
//                                                            "Placeholder",
//                                                            "Fill",
//                                                            "Slider",
//                                                            "Canvas",
//                                                            "Handle",
//                                                            "Viewport",
//                                                            "Scrollbar Horizontal",
//                                                            "Scrollbar Vertical"};
//    /// <summary>
//    /// �����߸�����
//    /// </summary>
//    public bool isFoladed = false;
//    public EW_UIListener()
//    {
//        list_CheckChilds = new List<UI_ControlInfo>();
//        list_FindChilds = new List<UIBehaviour>();
//        m_CallBackAddCompoenet();
//    }




//    #region ���ɴ������
//    /// <summary>
//    /// �����߶���
//    /// </summary>
//    public GameObject ListenerParents;
//    /// <summary>
//    /// ���ӵĸ������
//    /// </summary>
//    public UIBehaviour UI_parent;
//    /// <summary>
//    /// Start�������ں�����ʼ�ľ�������
//    /// </summary>
//    private string StartContent;
//    public string ClassName;
//    /// <summary>
//    /// ����·��
//    /// </summary>
//    private string path;

//    /// <summary>
//    /// ȷ�����ɼ����ű�
//    /// </summary>
//    public void m_CreateScript(string path)
//    {
//        ScriptInfo scriptInfo = new ScriptInfo();
//        scriptInfo.MemberInfos = new List<ScriptFieldInfo>();
//        scriptInfo.MethodInfos = new List<ScriptMethodInfo>();
//        scriptInfo.ClassName = ClassName;
//        scriptInfo.InheritClassName = "MonoBehaviour";
//        //������Ϣ
//        scriptInfo.UsingInfo = "using UnityEngine;\r\nusing UnityEngine.EventSystems;\r\nusing UnityEngine.UI;";
//        //�ȼӸ�����
//        UI_parent.name = ClassName + "_ListenerParents";
//        scriptInfo.MemberInfos.Add(new ScriptFieldInfo() { FieldName = UI_parent.name, Modifiers = E_AccessModifiers.Public, FieldType = "Transform" });


//        //��һ��Ӧ
//        for (int i = 0; i < list_CheckChilds.Count; i++)
//        {
//            if (list_CheckChilds[i].IsListened)
//                m_getChildMappingScriptInfo(list_CheckChilds[i], scriptInfo);
//        }
//        //���Ϸ���
//        StartContent = "\n\tvoid Start()\n{\n" + StartContent + "\n}";
//        scriptInfo.ExtraContent = StartContent;
//        //��ʼ����
//        ScriptCreateHelper.m_CreateScript(scriptInfo, path);
//        //���ö�̬����
//        PlayerPrefs.SetString("LastAddParentName", ListenerParents.name);
//        PlayerPrefs.SetString("LastTypeName", ClassName);
//    }

//    /// <summary>
//    /// ��ȡ����ĸ������ϵ��ƴ�� ·�� ���ڻ�ȡ�ؼ�
//    /// </summary>
//    /// <param name="obj"></param>
//    /// <returns></returns>
//    private string GetPath(Transform obj, Transform panelTrans)
//    {
//        string path = obj.name;
//        while (obj.parent != panelTrans)
//        {
//            path = obj.parent.name + "/" + path;
//            obj = obj.parent;
//        }
//        return path;
//    }

//    /// <summary>
//    /// һһӳ��ÿһ���ֶη�����Ϣ
//    /// </summary>
//    /// <param name="uiInfo"></param>
//    /// <param name="scriptInfo"></param>
//    private void m_getChildMappingScriptInfo(UI_ControlInfo uiInfo, ScriptInfo scriptInfo)
//    {
//        if (uiInfo == null || scriptInfo == null)
//        {
//            Debug.LogError("����Ϊ�գ����أ�");
//            return;
//        }
//        //��һ�����ǵ�
//        UI_parent = list_FindChilds[0];

//        //���ж���û���ظ���
//        if (scriptInfo.MemberInfos.Count > 0)
//        {
//            foreach (var member in scriptInfo.MemberInfos)
//            {
//                if (member.FieldName == uiInfo.behaviour.name)
//                {
//                    EditorUtility.DisplayDialog("�ظ��ؼ���", $"�������ؼ�������ͬ�Ķ���������{member.FieldName}", "ȷ��");
//                    return;
//                }
//            }
//        }

//        //���������ֵĲ����ַ�
//        char[] CharName = uiInfo.behaviour.name.ToCharArray();
//        foreach (char c in CharName)
//        {
//            if (c == '(' || c == ')')
//            {
//                EditorUtility.DisplayDialog("�ظ��ؼ���", $"���ڲ����ַ�{uiInfo.behaviour.name}", "ȷ��");
//                return;
//            }
//        }

//        ScriptFieldInfo fieldInfo = new ScriptFieldInfo();
//        Type type = uiInfo.behaviour.GetType();
//        fieldInfo.Modifiers = E_AccessModifiers.Public;
//        fieldInfo.FieldName = uiInfo.behaviour.name;
//        fieldInfo.FieldType = type.Name;

//        ScriptMethodInfo methodInfo = new ScriptMethodInfo();
//        methodInfo.Modifiers = E_AccessModifiers.Public;
//        methodInfo.FieldType = "void";

//        //������ص�ƴ��
//        StartContent += $"\t{uiInfo.behaviour.name} = {UI_parent.name}.transform.Find(\"{GetPath(uiInfo.behaviour.transform, UI_parent.transform)}\").GetComponent<{type.Name}>();\n\t\t";
//        //һһӳ��ÿһ�������ķ������Start������������ʽ
//        switch (type.Name)
//        {
//            case "Button":
//                StartContent += "//\t��ť����\n\t\t";
//                StartContent += $"\t{uiInfo.behaviour.name}.onClick.AddListener(On{uiInfo.behaviour.name}Click);\n\t\t";
//                methodInfo.FieldName = $"On{uiInfo.behaviour.name}Click";
//                break;
//            case "Toggle":
//                StartContent += "//\tѡ�����\n\t\t";
//                StartContent += $"\t{uiInfo.behaviour.name}.onValueChanged.AddListener(On{uiInfo.behaviour.name}ValueChanged);\n\t\t";
//                methodInfo.FieldName = $"On{uiInfo.behaviour.name}ValueChanged";
//                methodInfo.ParameterInfo = "bool value";
//                break;
//            case "Slider":
//                StartContent += "//\t������\n\t\t";
//                StartContent += $"\t{uiInfo.behaviour.name}.onValueChanged.AddListener(On{uiInfo.behaviour.name}ValueChanged);\n\t\t";
//                methodInfo.FieldName += $"On{uiInfo.behaviour.name}ValueChanged";
//                methodInfo.ParameterInfo = "float value";
//                break;
//            default:
//                break;
//        }
//        //���ֶ���Ϣ�ͷ�����Ϣд��Scriptinfo��
//        scriptInfo.MemberInfos.Add(fieldInfo);
//        scriptInfo.MethodInfos.Add(methodInfo);
//    }

//    /// <summary>
//    /// ֻ�ܿ���ˢ�������ڴ淽����������
//    /// </summary>
//    public void m_CallBackAddCompoenet()
//    {
//        //Ŀǰû��һ��취��ֻ�ܳ־û�����
//        //��Ϊÿ�ζ���ˢ�����ݣ��������ڴ����
//        string name = PlayerPrefs.GetString("LastAddParentName");
//        string TypeName = PlayerPrefs.GetString("LastTypeName");
//        if (name == "" || TypeName == "")
//        {
//            return;
//        }
//        //���س���
//        System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets")) + "Library\\ScriptAssemblies\\Assembly-CSharp.dll");

//        GameObject.Find(name).AddComponent(assembly.GetType(TypeName));
//        //ֱ����� ��ֹ��һ�η��
//        PlayerPrefs.SetString("LastAddParentName", null);
//        PlayerPrefs.SetString("LastTypeName", null);


//    }
//    #endregion


//    #region �����������

//    /// <summary>
//    /// Ҫ�����ĸ�����
//    /// </summary>
//    public GameObject CheckObj;
//    /// <summary>
//    /// ����ִ�м����ĺ�����
//    /// </summary>
//    public List<UI_ControlInfo> list_CheckChilds;
//    /// <summary>
//    /// �Ѿ��ҵ��ĺ�����
//    /// </summary>
//    public List<UIBehaviour> list_FindChilds;
//    /// <summary>
//    /// ����������
//    /// </summary>
//    public void ClickFind()
//    {
//        //������
//        list_FindChilds.Clear();
//        list_CheckChilds.Clear();
//        //������
//        CheckObj = Selection.activeGameObject;
//        if(CheckObj==null)Debug.LogWarning("��ֻ���һ�������󣬽��м���");
//        list_FindChilds.AddRange(CheckObj.GetComponentsInChildren<UIBehaviour>());
//        //��ʾһ��
//        if (list_FindChilds.Count <= 0)
//        {
//            EditorUtility.DisplayDialog("δ���ҵ�", "���ҵ�0��ӵ��UI�ؼ���������", "ȷ��");
//            return;
//        }
//        isFoladed = true;
//        //�󶨸���
//        UI_parent = list_FindChilds[0];
//        //��ʼ����ɸ��
//        m_ScreenFindChild();
//    }
//    /// <summary>
//    /// ����ɸ�飬ʹ���в��ҵĶ�������ɸ���б�
//    /// </summary>
//    public void m_ScreenFindChild()
//    {
//        //����ɸ�����ʱ����
//        UI_ControlInfo tempInfo;
//        Transform TempUp;
//        int TempIndex;
//        for (int i = 0; i < list_FindChilds.Count; i++)
//        {
//            //д�������Ϣ
//            tempInfo = new UI_ControlInfo();
//            tempInfo.ShowInfo = $"({list_FindChilds[i].GetType().Name}) " + list_FindChilds[i].name;
//            tempInfo.behaviour = list_FindChilds[i];
//            TempUp = list_FindChilds[i].transform;
//            TempIndex = 1;

//            #region ����ɸ���Ƿ����

//            //��������ɸ��Ĭ���Ƿ����
//            foreach (var defaultName in list_defaultName)
//            {
//                if (defaultName == list_FindChilds[i].name)
//                {
//                    tempInfo.IsListened = false;
//                    break;
//                }
//            }
//            //�ӿռ�����ɸѡ�Ƿ����
//            Type behaviorType = tempInfo.behaviour.GetType();
//            if (behaviorType == typeof(Text) || behaviorType == typeof(UnityEngine.UI.Image))
//            {
//                tempInfo.IsListened = false;
//            }
//            #endregion

//            #region ���������򿿺������߼�
//            if (list_FindChilds[i].transform == CheckObj.transform)
//                TempIndex = 0;
//            while (true)
//            {

//                TempUp = TempUp.parent;
//                if (TempUp == CheckObj.transform || list_FindChilds[i].transform == CheckObj.transform)
//                {
//                    break;
//                }
//                //�Ҳ�����ʼ�����
//                else if (TempUp == null)
//                {
//                    EditorUtility.DisplayDialog("����", "�Ҳ���������" + list_FindChilds[i].name, "�˸�ѽ·");
//                    return;
//                }
//                TempIndex++;
//            }
//            //������ʾ����
//            for (int j = 0; j < TempIndex; j++)
//            {
//                tempInfo.ShowInfo = "       " + tempInfo.ShowInfo;
//            }
//            #endregion
//            //��������ɸ����б�
//            list_CheckChilds.Add(tempInfo);
//        }
//    }

//    #endregion

  

//    /// <summary>
//    /// ������ɴ��밴ť��ִ�е��߼�
//    /// </summary>
//    public void m_ClickCreateScript()
//    {
//        //���жϻ�����Ϣд����
//        if (ClassName == "")
//        {
//            EditorUtility.DisplayDialog("����", "����û����д", "�˸�ѽ·");
//            return;
//        }
//        if (ListenerParents == null)
//        {
//            EditorUtility.DisplayDialog("����", "������û�й�������", "�˸�ѽ·");
//            return;
//        }

//        path = EditorUtility.SaveFolderPanel("ѡ�񱣴�·��", ScriptCreateHelper.AssetsPath, null);
//        m_CreateScript(path);
//    }


  
    
    
    
    
//    /// <summary>
//    /// �����ؼ���Ϣ
//    /// </summary>
//    public class UI_ControlInfo
//    {
//        /// <summary>
//        /// չʾ������е���Ϣ
//        /// </summary>
//        public string ShowInfo;
//        /// <summary>
//        /// ���ؼ� as��
//        /// </summary>
//        public UIBehaviour behaviour;
//        /// <summary>
//        /// �Ƿ���Ҫ����
//        /// </summary>
//        public bool IsListened = true;
//    }
//}
