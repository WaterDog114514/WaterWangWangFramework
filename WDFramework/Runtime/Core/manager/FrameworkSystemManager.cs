using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class FrameworkSystemManager : Singleton<FrameworkSystemManager>, IKernelSystem
{
    /// <summary>
    /// �Ѿ������ϵͳ
    /// </summary>
    public List<IFrameworkSystem> systems { get; private set; }
    public void InitializedKernelSystem()
    {
        systems = new List<IFrameworkSystem>();
    }
    /// <summary>
    /// ��ʼ�����п��ϵͳ
    /// </summary>
    public void InitializedAllFrameworkSystem()
    {
        // 1. ��ȡ����ʵ����IFrameworkSystem�ӿڵ�����
        var systemTypes = ReflectionHelper.GetTypesImplementingInterface(typeof(IFrameworkSystem));
        foreach (var systemType in systemTypes)
        {
            try
            {
                // 2. ����Ƿ�̳���Singleton<T>
                if (!ReflectionHelper.IsSubclassOfGenericType(systemType, typeof(Singleton<>)))
                {
                    Debug.LogWarning($"ϵͳ {systemType.Name} ʵ����IFrameworkSystem��û�м̳�Singleton<T>��������ʼ��");
                    continue;
                }
                // 3. ��ȡInstance���� - ������ķ��䷽ʽ
                var instanceProperty = systemType.GetProperty("Instance",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (instanceProperty == null)
                {
                    // �ٴγ��Ը����ɵ�������ʽ
                    instanceProperty = systemType.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .FirstOrDefault(p => p.Name == "Instance");
                    if (instanceProperty == null)
                    {
                        Debug.LogError($"ϵͳ {systemType.Name} û���ҵ�Instance��̬���ԣ��������̳���Singleton<T>");
                        Debug.LogError($"���� {systemType.Name} �Ƿ���ȷ����Ϊ public class {systemType.Name} : Singleton<{systemType.Name}>");
                        continue;
                    }
                }
                // 4. ��ȡʵ������ʼ��
                var instance = instanceProperty.GetValue(null) as IFrameworkSystem;
                if (instance == null)
                {
                    Debug.LogError($"ϵͳ {systemType.Name} ��Instance����IFrameworkSystem����");
                    continue;
                }
                // 5. ��ʼ��ϵͳ����ӵ��б�
                systems.Add(instance);
            }
            catch (Exception ex)
            {
                Debug.LogError($"��ʼ��ϵͳ {systemType.Name} ʱ����: {ex}");
            }
        }
        //��������ϵͳ�ĳ�ʼ������
        foreach (var system in systems)
        {
            system.InitializedSystem();
        }
     
    }
  
    /// <summary>
    /// �������п��ϵͳ���ڽ�������ʱ�����
    /// </summary>
    public void KillAllFrameworkSystem()
    {
    }
}