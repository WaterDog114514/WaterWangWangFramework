using System;
using UnityEngine;


[Serializable]
public class winData_TerrainCreator : BaseWindowData
{
    //�µ����ļ���
    public string newFileName;
    //�ϴα����ļ��е�ַ
    public string LastSaveDirectoryPath;
    [NonSerialized]
    public GameObject prefab;
    /// <summary>
    /// ���ɼ�X����
    /// </summary>
    public int CreateNumber;
    public float Size;
    [NonSerialized]
    public string newCopyDirectoryPath;
    public string[] newDatasPath;
    [NonSerialized]
    public TerrainData[] terrainDatas;

    public override string Title => "����������";

    public override void IntiFirstCreate()
    {

    }


}
