using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色属性持有者基类
/// </summary>
public class CharacterAttributeHolder : CharacterComponent
{
    /// <summary>
    /// 角色拥有的属性列表，可灵活变阵，如血量、攻击力等
    /// </summary>
    protected Dictionary<E_CharacterAttributeType, CharacterAttribute> dic_Attributes = new Dictionary<E_CharacterAttributeType, CharacterAttribute>();

    public CharacterAttributeHolder(BaseCharacter baseCharacter) : base(baseCharacter)
    {
    }

    public override void IntializeComponent()
    {
        dic_Attributes = new Dictionary<E_CharacterAttributeType, CharacterAttribute>();
        //AI、玩家属性自己填需要加什么


    }
    /// <summary>
    /// 添加一个数值类型
    /// </summary>
    /// <param name="Name">属性名</param>
    /// <param name="type">类型</param>
    public void AddAttribute<T>(E_CharacterAttributeType Name, T OriginValue = default) 
    {
        if (dic_Attributes.ContainsKey(Name))
        {
            Debug.Log($"已经存在{Name}的属性了，无法继续添加");
            return;
        }
        //根据不同的属性类型赋初值
        CharacterAttribute attribute = null;

        switch (OriginValue)
        {
            // 单个 int
            case int intValue:
                attribute = new NumericAttribute<int>(intValue);
                eventManager.AddRequestListener<E_CharacterAttributeType, int>(E_CharacterEvent.GetIntAttribute, GetIntAttribute);
                break;

            // int 数组
            case int[] intArray:
                attribute = new ArrayAttribute<int>(intArray);
                eventManager.AddRequestListener<E_CharacterAttributeType, int[]>(E_CharacterEvent.GetIntArrayAttribute, GetIntArrayAttribute);
                break;

            // 单个 float
            case float floatValue:
                attribute = new NumericAttribute<float>(floatValue);
                eventManager.AddRequestListener<E_CharacterAttributeType, float>(E_CharacterEvent.GetFloatAttribute, GetFloatAttribute);
                break;

            // float 数组
            case float[] floatArray:
                attribute = new ArrayAttribute<float>(floatArray);
                eventManager.AddRequestListener<E_CharacterAttributeType, float[]>(E_CharacterEvent.GetFloatArrayAttribute, GetFloatArrayAttribute);
                break;

            // 字符串
            case string strValue:
                attribute = new TextAttribute(strValue);
                eventManager.AddRequestListener<E_CharacterAttributeType, string>(E_CharacterEvent.GetStringAttribute, GetStringAttribute);
                break;

            default:
                Debug.LogError($"不支持的属性类型：{typeof(T)}");
                return;
        }
        dic_Attributes.Add(Name, attribute);
    }
    public void AddTextAttribute(E_CharacterAttributeType Name, string OriginValue = null)
    {
        if (dic_Attributes.ContainsKey(Name))
        {
            Debug.Log($"已经存在{Name}的属性了，无法继续添加");
            return;
        }
        dic_Attributes.Add(Name, new TextAttribute(OriginValue));
    }
    //移除一个属性
    public void RemoveAttribute() { }
    public override void UpdateComponent()
    {

    }

    //根据属性类型取得属性值本身
    public int GetIntAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as NumericAttribute<int>).Value;
    public int[] GetIntArrayAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as ArrayAttribute<int>).Value;
    public float GetFloatAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as NumericAttribute<float>).Value;
    public float[] GetFloatArrayAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as ArrayAttribute<float>).Value;
    public string GetStringAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as TextAttribute).Value;


}

