using System;
using System.IO;
using UnityEngine;
/// <summary>
/// �Ĳ������� �����ڵ����ļ���
/// </summary>
public class QuadTerrain : MonoBehaviour
{
    /// <summary>
    /// ��������Ҳ���ļ���ʶ��
    /// </summary>
    public string TerrainName;
    [NonSerialized]
    public TerrainQuadData QuadData;
    /// <summary>
    /// �Ƿ����ɵ��ι���??
    /// </summary>
    public bool IsGenerateQuadData = false;

    /// <summary>
    /// ��Selection�ĵ����Լ�����quadData��
    /// </summary>
    public void LoadSelfData(string loadPath)
    {
        if (File.Exists(loadPath))
        {
            Debug.Log("���سɹ�" + loadPath);
            QuadData = BinaryManager.Load<TerrainQuadData>(loadPath);

        }
        else
        {
            Debug.Log("�����ڵ����ļ�" + loadPath);
        }

    }

}
