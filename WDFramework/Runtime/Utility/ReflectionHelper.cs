using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


/// <summary>
/// ���乤���� ���Ի�ȡĳ�����������
/// �������Կ��Ƿ�һ����C-Sharp�������Ȼ����򼯷�����ȡ����
/// </summary>
public static class ReflectionHelper
{
    /// <summary>
    /// ��ȡĳ�Ƿ��͵����͵��������ࣨ���������Ѽ��صĳ��򼯣�
    /// </summary>
    /// <param name="baseType">��������</param>
    /// <returns>��������������б�</returns>
    public static List<Type> GetSubclasses(Type baseType)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly =>
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null);
                }
            })
            .Where(t => t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType))
            .ToList();
    }
    /// <summary>
    /// ֱ�ӷ���ĳ���͵ĸ�������
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetBaseType(Type type)
    {
        return type.BaseType;
    }
    /// <summary>
    /// ��ȡָ�����͵ĵ�N�����Ͳ�������
    /// </summary>
    /// <param name="type">Ҫ��ȡ���Ͳ���������</param>
    /// <param name="n">������������0��ʼ��</param>
    /// <returns>����ָ���ķ��Ͳ������ͣ����������򷵻� null</returns>
    public static Type GetGenericArgumentType(Type type, int n)
    {

        var genericArgs = type.GetGenericArguments();
        if (genericArgs.Length > n)
        {
            return genericArgs[n]; // ���ص�N�����Ͳ�������
        }
        return null; // ���û���ҵ������� null
    }
    /// <summary>
    /// ��ȡĳ���ͻ����͵��������ࣨ���������Ѽ��صĳ��򼯣�
    /// </summary>
    /// <param name="genericBaseType">���ͻ�����</param>
    /// <returns>��������������б�</returns>
    public static List<Type> GetSubclassesOfGenericType(Type genericBaseType)
    {
        // ��鴫��������Ƿ��Ƿ�������
        if (genericBaseType == null || !genericBaseType.IsGenericType)
        {
            throw new ArgumentException("��������ͱ����Ƿ�������", nameof(genericBaseType));
        }

        // ��ȡ�����Ѽ��صĳ���
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly =>
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // �����������ʱ����������Ч������
                    return ex.Types.Where(t => t != null);
                }
            })
            .Where(t => t != null && t.IsClass && !t.IsAbstract && IsSubclassOfGenericType(t, genericBaseType))
            .ToList();
    }
    /// <summary>
    /// ��������Ƿ��Ƿ��ͻ��������
    /// </summary>
    /// <param name="type">Ҫ��������</param>
    /// <param name="genericBaseType">���ͻ���</param>
    /// <returns>��������෵�� true�����򷵻� false</returns>
    public static bool IsSubclassOfGenericType(Type type, Type genericBaseType)
    {
        // ������ͻ����Ϊ�գ����� false
        if (type == null || genericBaseType == null)
            return false;

        // �������͵ļ̳���
        while (type != null && type != typeof(object))
        {
            // �����ǰ�����Ƿ������ͣ�������Ŀ�귺�ͻ���Ķ���ƥ��
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
                return true;

            // �������ϲ��һ�����
            type = type.BaseType;
        }

        return false;
    }
    //����string�������г��򼯣��ҵ���ӦType
    public static Type FindTypeInAssemblies(string typeName)
    {
        // ���������Ѽ��صĳ���
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // ���Ҹ�����
            Type type = assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
            if (type != null)
            {
                return type; // �ҵ�����������
            }
        }
        return null; // δ�ҵ�����
    }
    /// <summary>
    /// ��ȡ����ʵ��ָ���ӿڵ����ͣ����������Ѽ��صĳ��򼯣�
    /// </summary>
    /// <param name="interfaceType">Ҫ���ҵĽӿ�����</param>
    /// <returns>����ʵ�ָýӿڵ������б�</returns>
    public static List<Type> GetTypesImplementingInterface(Type interfaceType)
    {
        // ��֤�����Ƿ�Ϊ�ӿ�����
        if (interfaceType == null || !interfaceType.IsInterface)
        {
            throw new ArgumentException("��������ͱ����ǽӿ�����", nameof(interfaceType));
        }

        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly =>
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null);
                }
            })
            .Where(t => t != null && t.IsClass && !t.IsAbstract &&
                   interfaceType.IsAssignableFrom(t))
            .ToList();
    }
    /// <summary>
    /// ��ȡ����ʵ��ָ�����ͽӿڵ����ͣ����������Ѽ��صĳ��򼯣�
    /// </summary>
    /// <param name="genericInterfaceType">Ҫ���ҵķ��ͽӿ����Ͷ���</param>
    /// <returns>����ʵ�ָ÷��ͽӿڵ������б�</returns>
    public static List<Type> GetTypesImplementingGenericInterface(Type genericInterfaceType)
    {
        // ��֤�����Ƿ�Ϊ���ͽӿ����Ͷ���
        if (genericInterfaceType == null || !genericInterfaceType.IsInterface || !genericInterfaceType.IsGenericTypeDefinition)
        {
            throw new ArgumentException("��������ͱ����Ƿ��ͽӿ����Ͷ���", nameof(genericInterfaceType));
        }

        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly =>
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null);
                }
            })
            .Where(t => t != null && t.IsClass && !t.IsAbstract &&
                   t.GetInterfaces().Any(i => i.IsGenericType &&
                   i.GetGenericTypeDefinition() == genericInterfaceType))
            .ToList();
    }
}
