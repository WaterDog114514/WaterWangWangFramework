using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ���Գ����߻���
/// </summary>
public class CharacterAttributeHolder : CharacterComponent
{
    /// <summary>
    /// ��ɫӵ�е������б�����������Ѫ������������
    /// </summary>
    protected Dictionary<E_CharacterAttributeType, CharacterAttribute> dic_Attributes = new Dictionary<E_CharacterAttributeType, CharacterAttribute>();

    public CharacterAttributeHolder(BaseCharacter baseCharacter) : base(baseCharacter)
    {
    }

    public override void IntializeComponent()
    {
        dic_Attributes = new Dictionary<E_CharacterAttributeType, CharacterAttribute>();
        //AI����������Լ�����Ҫ��ʲô


    }
    /// <summary>
    /// ���һ����ֵ����
    /// </summary>
    /// <param name="Name">������</param>
    /// <param name="type">����</param>
    public void AddAttribute<T>(E_CharacterAttributeType Name, T OriginValue = default) 
    {
        if (dic_Attributes.ContainsKey(Name))
        {
            Debug.Log($"�Ѿ�����{Name}�������ˣ��޷��������");
            return;
        }
        //���ݲ�ͬ���������͸���ֵ
        CharacterAttribute attribute = null;

        switch (OriginValue)
        {
            // ���� int
            case int intValue:
                attribute = new NumericAttribute<int>(intValue);
                eventManager.AddRequestListener<E_CharacterAttributeType, int>(E_CharacterEvent.GetIntAttribute, GetIntAttribute);
                break;

            // int ����
            case int[] intArray:
                attribute = new ArrayAttribute<int>(intArray);
                eventManager.AddRequestListener<E_CharacterAttributeType, int[]>(E_CharacterEvent.GetIntArrayAttribute, GetIntArrayAttribute);
                break;

            // ���� float
            case float floatValue:
                attribute = new NumericAttribute<float>(floatValue);
                eventManager.AddRequestListener<E_CharacterAttributeType, float>(E_CharacterEvent.GetFloatAttribute, GetFloatAttribute);
                break;

            // float ����
            case float[] floatArray:
                attribute = new ArrayAttribute<float>(floatArray);
                eventManager.AddRequestListener<E_CharacterAttributeType, float[]>(E_CharacterEvent.GetFloatArrayAttribute, GetFloatArrayAttribute);
                break;

            // �ַ���
            case string strValue:
                attribute = new TextAttribute(strValue);
                eventManager.AddRequestListener<E_CharacterAttributeType, string>(E_CharacterEvent.GetStringAttribute, GetStringAttribute);
                break;

            default:
                Debug.LogError($"��֧�ֵ��������ͣ�{typeof(T)}");
                return;
        }
        dic_Attributes.Add(Name, attribute);
    }
    public void AddTextAttribute(E_CharacterAttributeType Name, string OriginValue = null)
    {
        if (dic_Attributes.ContainsKey(Name))
        {
            Debug.Log($"�Ѿ�����{Name}�������ˣ��޷��������");
            return;
        }
        dic_Attributes.Add(Name, new TextAttribute(OriginValue));
    }
    //�Ƴ�һ������
    public void RemoveAttribute() { }
    public override void UpdateComponent()
    {

    }

    //������������ȡ������ֵ����
    public int GetIntAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as NumericAttribute<int>).Value;
    public int[] GetIntArrayAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as ArrayAttribute<int>).Value;
    public float GetFloatAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as NumericAttribute<float>).Value;
    public float[] GetFloatArrayAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as ArrayAttribute<float>).Value;
    public string GetStringAttribute(E_CharacterAttributeType AttributeName) => (dic_Attributes[AttributeName] as TextAttribute).Value;


}

