using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class FrameworkSystemManager : Singleton<FrameworkSystemManager>, IKernelSystem
{
    /// <summary>
    /// 已经激活的系统
    /// </summary>
    public List<IFrameworkSystem> systems { get; private set; }
    public void InitializedKernelSystem()
    {
        systems = new List<IFrameworkSystem>();
    }
    /// <summary>
    /// 初始化所有框架系统
    /// </summary>
    public void InitializedAllFrameworkSystem()
    {
        // 1. 获取所有实现了IFrameworkSystem接口的类型
        var systemTypes = ReflectionHelper.GetTypesImplementingInterface(typeof(IFrameworkSystem));
        foreach (var systemType in systemTypes)
        {
            try
            {
                // 2. 检查是否继承自Singleton<T>
                if (!ReflectionHelper.IsSubclassOfGenericType(systemType, typeof(Singleton<>)))
                {
                    Debug.LogWarning($"系统 {systemType.Name} 实现了IFrameworkSystem但没有继承Singleton<T>，跳过初始化");
                    continue;
                }
                // 3. 获取Instance属性 - 修正后的反射方式
                var instanceProperty = systemType.GetProperty("Instance",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (instanceProperty == null)
                {
                    // 再次尝试更宽松的搜索方式
                    instanceProperty = systemType.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .FirstOrDefault(p => p.Name == "Instance");
                    if (instanceProperty == null)
                    {
                        Debug.LogError($"系统 {systemType.Name} 没有找到Instance静态属性，尽管它继承自Singleton<T>");
                        Debug.LogError($"请检查 {systemType.Name} 是否正确定义为 public class {systemType.Name} : Singleton<{systemType.Name}>");
                        continue;
                    }
                }
                // 4. 获取实例并初始化
                var instance = instanceProperty.GetValue(null) as IFrameworkSystem;
                if (instance == null)
                {
                    Debug.LogError($"系统 {systemType.Name} 的Instance不是IFrameworkSystem类型");
                    continue;
                }
                // 5. 初始化系统并添加到列表
                systems.Add(instance);
            }
            catch (Exception ex)
            {
                Debug.LogError($"初始化系统 {systemType.Name} 时出错: {ex}");
            }
        }
        //调用所有系统的初始化方法
        foreach (var system in systems)
        {
            system.InitializedSystem();
        }
     
    }
  
    /// <summary>
    /// 销毁所有框架系统，在结束进程时候调用
    /// </summary>
    public void KillAllFrameworkSystem()
    {
    }
}