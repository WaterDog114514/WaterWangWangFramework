using System;
using UnityEngine;
/// <summary>
/// һ�����ε��Ĳ������ݣ��������������л��洢���Լ��ڼ��ص�ͼʱ��ʹ��
/// </summary>
[Serializable]
public class TerrainQuadData 
{
    /// <summary>
    /// �������εĸ��ڵ㣬ֻҪ�洢���ڵ㣬Ҳ�൱�ڴ洢�������ӽڵ�
    /// </summary>
    public QuadTree<TerrainCellInfo> Tree;
    /// <summary>
    /// ����Ԥ����·��
    /// </summary>
    public string TerrainPrefabPath;
    /// <summary>
    /// ����Ԥ�����ļ�����Ҫʱ����������������
    /// </summary>
   
 
}
