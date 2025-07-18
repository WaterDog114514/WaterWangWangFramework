using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ɫ�࣬���ڹ����������
/// </summary>
public class BaseCharacter : MonoBehaviour
{

    public virtual void Awake()
    {
       
    }
    public EventManager<E_CharacterEvent> eventManager = new EventManager<E_CharacterEvent>();
    /// <summary>
    /// �õ�λ��ɫӵ�е�ȫ�����
    /// </summary>
    protected Dictionary<Type, CharacterComponent> dic_components = new Dictionary<Type, CharacterComponent>();
    /// <summary>
    /// �״δ����˿ؼ�����
    /// </summary>
    public void IntiFirstCreate()
    {
        eventManager = new EventManager<E_CharacterEvent>();
        dic_components = new Dictionary<Type, CharacterComponent>();
    }
    /// <summary>
    /// ��ʼ�����е����        
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
            Debug.LogWarning("�����ظ���ӽ�ɫ���");
            return dic_components[GetBaseType(typeof(T))] as T;
        }
        dic_components.Add(GetBaseType(typeof(T)), component);
        return component;
    }
    /// <summary>
    /// ��ȡ��ԭʼ�ĸ��������һ��
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
        //������ʹ�ü�ǿ����������  
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
    /// ��ȡָ������Ľӿ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>�ӿڼ���</returns>
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
    /// �ж���û��ĳ��ɫ�ؼ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool ContainComponent<T>() where T : CharacterComponent => dic_components.ContainsKey(typeof(T));
 
    public virtual void Update()
    {
        //��������߼�
        foreach (CharacterComponent component in dic_components.Values)
        {
            component.UpdateComponent();
        }
    }
   

}