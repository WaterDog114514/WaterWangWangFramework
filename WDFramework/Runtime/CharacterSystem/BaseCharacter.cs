using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基本角色类，用于管理组件关联
/// </summary>
public class BaseCharacter : MonoBehaviour
{

    public virtual void Awake()
    {
       
    }
    public EventManager<E_CharacterEvent> eventManager = new EventManager<E_CharacterEvent>();
    /// <summary>
    /// 该单位角色拥有的全部组件
    /// </summary>
    protected Dictionary<Type, CharacterComponent> dic_components = new Dictionary<Type, CharacterComponent>();
    /// <summary>
    /// 首次创建此控件调用
    /// </summary>
    public void IntiFirstCreate()
    {
        eventManager = new EventManager<E_CharacterEvent>();
        dic_components = new Dictionary<Type, CharacterComponent>();
    }
    /// <summary>
    /// 初始化所有的组件        
    /// </summary>
    public void IntiCharacterComponent()
    {
        foreach (var component in dic_components.Values)
        {
            if (component != null)
                component.IntializeComponent();
        }
    }

    public T AddCharacterComponent<T>(T component) where T : CharacterComponent
    {
        if (dic_components.ContainsKey(typeof(T)))
        {
            Debug.LogWarning("尝试重复添加角色组件");
            return dic_components[GetBaseType(typeof(T))] as T;
        }
        dic_components.Add(GetBaseType(typeof(T)), component);
        return component;
    }
    /// <summary>
    /// 获取最原始的根父类的下一级
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Type GetBaseType(Type type)
    {

        Type baseType = type.BaseType;
        while (baseType != null && baseType.BaseType != typeof(CharacterComponent))
        {
            baseType = baseType.BaseType;
        }
        return baseType ?? type;
    }
    public T GetCharacterComponent<T>() where T : CharacterComponent
    {
        //不存在使用加强父子类搜索  
        if (!dic_components.ContainsKey(typeof(T)))
        {
            foreach (var component in dic_components.Keys)
            {
                if (typeof(T).IsAssignableFrom(component.GetType()))
                    return component as T;
            }
            return null;
        }
        return dic_components[typeof(T)] as T;
    }
    /// <summary>
    /// 获取指定组件的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>接口集合</returns>
    /// <summary>
    public List<T> GetComponentInterfaces<T>() where T : class, ICharacterOperator
    {
        List<T> list = new List<T>();
        foreach (var component in dic_components.Values)
        {
            if (component is T)
            {
                list.Add(component as T);
            }
        }

        return list;
    }
    /// <summary>
    /// 判断有没有某角色控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool ContainComponent<T>() where T : CharacterComponent => dic_components.ContainsKey(typeof(T));
 
    public virtual void Update()
    {
        //更新组件逻辑
        foreach (CharacterComponent component in dic_components.Values)
        {
            component.UpdateComponent();
        }
    }
   

}