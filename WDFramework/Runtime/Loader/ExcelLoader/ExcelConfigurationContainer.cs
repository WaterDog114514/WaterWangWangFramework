using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DictionaryContainer
{

}

/// <summary>
/// Excel��������������,T1Ϊ�ֵ��key��T2Ϊ�����������ݽṹ
/// </summary>
[System.Serializable]
public abstract class ExcelConfigurationContainer<TConfigType> :DictionaryContainer where TConfigType : ExcelConfiguration
{
    //ԭ���ֵ�
    public Dictionary<int, TConfigType> container;
}
