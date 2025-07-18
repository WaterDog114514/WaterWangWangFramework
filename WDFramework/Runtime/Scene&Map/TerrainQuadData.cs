using System;
using UnityEngine;
/// <summary>
/// 一个地形的四叉树数据，用来二进制序列化存储，以及在加载地图时候使用
/// </summary>
[Serializable]
public class TerrainQuadData 
{
    /// <summary>
    /// 整个地形的根节点，只要存储根节点，也相当于存储了所有子节点
    /// </summary>
    public QuadTree<TerrainCellInfo> Tree;
    /// <summary>
    /// 地形预制体路径
    /// </summary>
    public string TerrainPrefabPath;
    /// <summary>
    /// 地形预制体文件，必要时候用它来换算坐标
    /// </summary>
   
 
}
