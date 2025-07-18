//using System;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.Compilation;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using UnityEngine.UIElements;

///*
// * 要实现的功能
// * 1.对一个Canvas下的UI父物体进行检索，检索到有需求监听的UI控件的孩子标注一下
// * 2.不需要提前标注的物体：有默认名的（比如父类物体下有默认名的子物体）
// * 3.功能：对标注好的物体绑定一个父物体，给这个父物体添加脚本用来监听子物体UI的动作
// * 对于添加脚本：用户需要自定义脚本名，还能配置这些标注物体监听的变量名

// ※服务的是开发者，不是玩家，每个脚本只能对同一个父对象绑定一次

// * 首先通过标注可视化监听脚本
// * 1.没有监听脚本就可以直接添加绑定
// * 2.有监听脚本，检查是否与这个父物体有关联，是否已经绑定过了？
// * 3.绑定过了就不能再绑定，没有绑定就可以绑定
 
//   对于可视化监听脚本DebugEventListenerGizmos
//    1.能查看绑定的父对象是谁
//    2.能查看绑定的哪个函数
//    3.能一键找到父对象并显示
// */
////完善一下
//public class EW_UIListener : EditorMain
//{
//    /// <summary>
//    /// 默认的不要生成的监听
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
//    /// 监听者父对象
//    /// </summary>
//    public bool isFoladed = false;
//    public EW_UIListener()
//    {
//        list_CheckChilds = new List<UI_ControlInfo>();
//        list_FindChilds = new List<UIBehaviour>();
//        m_CallBackAddCompoenet();
//    }




//    #region 生成代码相关
//    /// <summary>
//    /// 监听者对象
//    /// </summary>
//    public GameObject ListenerParents;
//    /// <summary>
//    /// 孩子的父亲组件
//    /// </summary>
//    public UIBehaviour UI_parent;
//    /// <summary>
//    /// Start生命周期函数开始的具体内容
//    /// </summary>
//    private string StartContent;
//    public string ClassName;
//    /// <summary>
//    /// 生成路径
//    /// </summary>
//    private string path;

//    /// <summary>
//    /// 确认生成监听脚本
//    /// </summary>
//    public void m_CreateScript(string path)
//    {
//        ScriptInfo scriptInfo = new ScriptInfo();
//        scriptInfo.MemberInfos = new List<ScriptFieldInfo>();
//        scriptInfo.MethodInfos = new List<ScriptMethodInfo>();
//        scriptInfo.ClassName = ClassName;
//        scriptInfo.InheritClassName = "MonoBehaviour";
//        //引用信息
//        scriptInfo.UsingInfo = "using UnityEngine;\r\nusing UnityEngine.EventSystems;\r\nusing UnityEngine.UI;";
//        //先加父对象
//        UI_parent.name = ClassName + "_ListenerParents";
//        scriptInfo.MemberInfos.Add(new ScriptFieldInfo() { FieldName = UI_parent.name, Modifiers = E_AccessModifiers.Public, FieldType = "Transform" });


//        //逐一对应
//        for (int i = 0; i < list_CheckChilds.Count; i++)
//        {
//            if (list_CheckChilds[i].IsListened)
//                m_getChildMappingScriptInfo(list_CheckChilds[i], scriptInfo);
//        }
//        //加上方法
//        StartContent = "\n\tvoid Start()\n{\n" + StartContent + "\n}";
//        scriptInfo.ExtraContent = StartContent;
//        //开始生成
//        ScriptCreateHelper.m_CreateScript(scriptInfo, path);
//        //设置动态加载
//        PlayerPrefs.SetString("LastAddParentName", ListenerParents.name);
//        PlayerPrefs.SetString("LastTypeName", ClassName);
//    }

//    /// <summary>
//    /// 获取对象的父对象关系，拼接 路径 用于获取控件
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
//    /// 一一映射每一个字段方法信息
//    /// </summary>
//    /// <param name="uiInfo"></param>
//    /// <param name="scriptInfo"></param>
//    private void m_getChildMappingScriptInfo(UI_ControlInfo uiInfo, ScriptInfo scriptInfo)
//    {
//        if (uiInfo == null || scriptInfo == null)
//        {
//            Debug.LogError("参数为空！返回！");
//            return;
//        }
//        //第一个即是爹
//        UI_parent = list_FindChilds[0];

//        //先判断有没有重复的
//        if (scriptInfo.MemberInfos.Count > 0)
//        {
//            foreach (var member in scriptInfo.MemberInfos)
//            {
//                if (member.FieldName == uiInfo.behaviour.name)
//                {
//                    EditorUtility.DisplayDialog("重复控件名", $"有两个控件类型相同的对象重名了{member.FieldName}", "确定");
//                    return;
//                }
//            }
//        }

//        //检查组件名字的不法字符
//        char[] CharName = uiInfo.behaviour.name.ToCharArray();
//        foreach (char c in CharName)
//        {
//            if (c == '(' || c == ')')
//            {
//                EditorUtility.DisplayDialog("重复控件名", $"存在不法字符{uiInfo.behaviour.name}", "确定");
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

//        //声明相关的拼接
//        StartContent += $"\t{uiInfo.behaviour.name} = {UI_parent.name}.transform.Find(\"{GetPath(uiInfo.behaviour.transform, UI_parent.transform)}\").GetComponent<{type.Name}>();\n\t\t";
//        //一一映射每一个监听的方法体和Start监听函数起手式
//        switch (type.Name)
//        {
//            case "Button":
//                StartContent += "//\t按钮监听\n\t\t";
//                StartContent += $"\t{uiInfo.behaviour.name}.onClick.AddListener(On{uiInfo.behaviour.name}Click);\n\t\t";
//                methodInfo.FieldName = $"On{uiInfo.behaviour.name}Click";
//                break;
//            case "Toggle":
//                StartContent += "//\t选框监听\n\t\t";
//                StartContent += $"\t{uiInfo.behaviour.name}.onValueChanged.AddListener(On{uiInfo.behaviour.name}ValueChanged);\n\t\t";
//                methodInfo.FieldName = $"On{uiInfo.behaviour.name}ValueChanged";
//                methodInfo.ParameterInfo = "bool value";
//                break;
//            case "Slider":
//                StartContent += "//\t滑动条\n\t\t";
//                StartContent += $"\t{uiInfo.behaviour.name}.onValueChanged.AddListener(On{uiInfo.behaviour.name}ValueChanged);\n\t\t";
//                methodInfo.FieldName += $"On{uiInfo.behaviour.name}ValueChanged";
//                methodInfo.ParameterInfo = "float value";
//                break;
//            default:
//                break;
//        }
//        //把字段信息和方法信息写入Scriptinfo中
//        scriptInfo.MemberInfos.Add(fieldInfo);
//        scriptInfo.MethodInfos.Add(methodInfo);
//    }

//    /// <summary>
//    /// 只能靠着刷新重置内存方法来加载了
//    /// </summary>
//    public void m_CallBackAddCompoenet()
//    {
//        //目前没有一点办法，只能持久化来搞
//        //因为每次都会刷新数据，编译后把内存清空
//        string name = PlayerPrefs.GetString("LastAddParentName");
//        string TypeName = PlayerPrefs.GetString("LastTypeName");
//        if (name == "" || TypeName == "")
//        {
//            return;
//        }
//        //加载程序集
//        System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets")) + "Library\\ScriptAssemblies\\Assembly-CSharp.dll");

//        GameObject.Find(name).AddComponent(assembly.GetType(TypeName));
//        //直接清空 防止下一次发癫
//        PlayerPrefs.SetString("LastAddParentName", null);
//        PlayerPrefs.SetString("LastTypeName", null);


//    }
//    #endregion


//    #region 检索查找相关

//    /// <summary>
//    /// 要检索的父物体
//    /// </summary>
//    public GameObject CheckObj;
//    /// <summary>
//    /// 正在执行检索的孩子们
//    /// </summary>
//    public List<UI_ControlInfo> list_CheckChilds;
//    /// <summary>
//    /// 已经找到的孩子们
//    /// </summary>
//    public List<UIBehaviour> list_FindChilds;
//    /// <summary>
//    /// 检索子物体
//    /// </summary>
//    public void ClickFind()
//    {
//        //先清理
//        list_FindChilds.Clear();
//        list_CheckChilds.Clear();
//        //再搜索
//        CheckObj = Selection.activeGameObject;
//        if(CheckObj==null)Debug.LogWarning("请只点击一个父对象，进行监听");
//        list_FindChilds.AddRange(CheckObj.GetComponentsInChildren<UIBehaviour>());
//        //提示一波
//        if (list_FindChilds.Count <= 0)
//        {
//            EditorUtility.DisplayDialog("未查找到", "查找到0个拥有UI控件的子物体", "确定");
//            return;
//        }
//        isFoladed = true;
//        //绑定父亲
//        UI_parent = list_FindChilds[0];
//        //开始进行筛查
//        m_ScreenFindChild();
//    }
//    /// <summary>
//    /// 进行筛查，使所有查找的东西加入筛查列表
//    /// </summary>
//    public void m_ScreenFindChild()
//    {
//        //后续筛查的临时变量
//        UI_ControlInfo tempInfo;
//        Transform TempUp;
//        int TempIndex;
//        for (int i = 0; i < list_FindChilds.Count; i++)
//        {
//            //写入基本信息
//            tempInfo = new UI_ControlInfo();
//            tempInfo.ShowInfo = $"({list_FindChilds[i].GetType().Name}) " + list_FindChilds[i].name;
//            tempInfo.behaviour = list_FindChilds[i];
//            TempUp = list_FindChilds[i].transform;
//            TempIndex = 1;

//            #region 初步筛查是否监听

//            //从名字中筛查默认是否监听
//            foreach (var defaultName in list_defaultName)
//            {
//                if (defaultName == list_FindChilds[i].name)
//                {
//                    tempInfo.IsListened = false;
//                    break;
//                }
//            }
//            //从空间类型筛选是否监听
//            Type behaviorType = tempInfo.behaviour.GetType();
//            if (behaviorType == typeof(Text) || behaviorType == typeof(UnityEngine.UI.Image))
//            {
//                tempInfo.IsListened = false;
//            }
//            #endregion

//            #region 子物体排序靠后排列逻辑
//            if (list_FindChilds[i].transform == CheckObj.transform)
//                TempIndex = 0;
//            while (true)
//            {

//                TempUp = TempUp.parent;
//                if (TempUp == CheckObj.transform || list_FindChilds[i].transform == CheckObj.transform)
//                {
//                    break;
//                }
//                //找不到开始报错吧
//                else if (TempUp == null)
//                {
//                    EditorUtility.DisplayDialog("错误！", "找不到父对象" + list_FindChilds[i].name, "八嘎呀路");
//                    return;
//                }
//                TempIndex++;
//            }
//            //排序显示靠后
//            for (int j = 0; j < TempIndex; j++)
//            {
//                tempInfo.ShowInfo = "       " + tempInfo.ShowInfo;
//            }
//            #endregion
//            //加入正在筛查的列表
//            list_CheckChilds.Add(tempInfo);
//        }
//    }

//    #endregion

  

//    /// <summary>
//    /// 点击生成代码按钮后执行的逻辑
//    /// </summary>
//    public void m_ClickCreateScript()
//    {
//        //先判断基本信息写了吗
//        if (ClassName == "")
//        {
//            EditorUtility.DisplayDialog("错误！", "类名没有填写", "八嘎呀路");
//            return;
//        }
//        if (ListenerParents == null)
//        {
//            EditorUtility.DisplayDialog("错误！", "监听者没有关联！！", "八嘎呀路");
//            return;
//        }

//        path = EditorUtility.SaveFolderPanel("选择保存路径", ScriptCreateHelper.AssetsPath, null);
//        m_CreateScript(path);
//    }


  
    
    
    
    
//    /// <summary>
//    /// 单个控件信息
//    /// </summary>
//    public class UI_ControlInfo
//    {
//        /// <summary>
//        /// 展示在面板中的信息
//        /// </summary>
//        public string ShowInfo;
//        /// <summary>
//        /// 主控件 as用
//        /// </summary>
//        public UIBehaviour behaviour;
//        /// <summary>
//        /// 是否需要监听
//        /// </summary>
//        public bool IsListened = true;
//    }
//}
