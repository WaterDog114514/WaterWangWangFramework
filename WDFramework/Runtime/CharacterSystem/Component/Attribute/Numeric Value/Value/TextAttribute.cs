using UnityEngine;

/// <summary>
/// 字符串属性 应用角色的名字 概述等
/// </summary>
public class TextAttribute : CharacterAttribute
{
    public string Value;

    public TextAttribute(string value)
    {
        Value = value;
    }
}
