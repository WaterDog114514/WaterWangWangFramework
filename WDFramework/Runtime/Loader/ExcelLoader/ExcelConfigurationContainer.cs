using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DictionaryContainer
{

}

/// <summary>
/// Excel的数据容器基类,T1为字典的key，T2为单行配置数据结构
/// </summary>
[System.Serializable]
public abstract class ExcelConfigurationContainer<TConfigType> :DictionaryContainer where TConfigType : ExcelConfiguration
{
    //原初字典
    public Dictionary<int, TConfigType> container;
}
