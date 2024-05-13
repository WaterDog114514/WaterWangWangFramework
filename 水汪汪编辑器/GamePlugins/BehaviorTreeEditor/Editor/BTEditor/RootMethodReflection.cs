using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 单例获取所有方法，避免内存浪费
/// </summary>
public class RootMethodReflection
{
    public static RootMethodReflection Instance = new RootMethodReflection();
    public RootMethodReflection()
    {

    }
    /// <summary>
    /// 所有组件名称
    /// </summary>
    public string[] list_ComponentName;
    /// <summary>
    /// 每个组件对应的所有公开方法名，得通过组件名来获取数组
    /// </summary>
    public Dictionary<string, string[]> dic_void_CompoentMethods = new Dictionary<string, string[]>();
    //再设置一个用来存储bool类型的用作条件节点
    public Dictionary<string, string[]> dic_bool_CompoentMethods = new Dictionary<string, string[]>();
    public void RefreshRootNode()
    {
        //先清空一波
        dic_void_CompoentMethods.Clear();
        dic_bool_CompoentMethods.Clear();
        //初始化所有组件名
        Component[] components = RootNode_VisualBehaviorTreeNode.instance.BehaviorObj.GetComponents(typeof(MonoBehaviour));
        list_ComponentName = new string[components.Length];
        for (int i = 0; i < components.Length; i++)
        {
            Type componentType = components[i].GetType();
            list_ComponentName[i] = componentType.Name;
            MethodInfo[] void_infos = null;
            MethodInfo[] bool_infos = null;
            //筛选所有无参无返回值的方法  GPT教的，水狗也不会ε=(´ο｀*)))唉
            void_infos = componentType.GetMethods().Where(method => method.ReturnType == typeof(void) && method.GetParameters().Length == 0).ToArray();
            //筛选返回类型是bool类型，且无参的的所有方法信息    
            bool_infos = componentType.GetMethods().Where(method => method.ReturnType == typeof(bool) && method.GetParameters().Length == 0).ToArray();
            //获取所有组件名，并且给每个组件名分配很多方法
            string[] bool_CompoentMethods = new string[bool_infos.Length];
            string[] void_CompoentMethods = new string[void_infos.Length];

            //给空方法们上票
            for (int j = 0; j < void_infos.Length; j++)
            {
                void_CompoentMethods[j] = void_infos[j].Name;
            }
            dic_void_CompoentMethods.Add(componentType.Name, void_CompoentMethods);
            //给bool方法们上票
            for (int j = 0; j < bool_infos.Length; j++)
            { bool_CompoentMethods[j] = bool_infos[j].Name; }
            dic_bool_CompoentMethods.Add(componentType.Name, bool_CompoentMethods);
        }
    }
}

/// <summary>
/// 选择索引，主要给行为节点选择方法用的
/// </summary>
public class SelectedIndex
{
    public int Select1
    {
        get => _select1;
        set
        {
            if (_select1 == value) return;
            Select2 = 0;
            _select1 = value;
        }
    }
    private int _select1;
    public int Select2;
}