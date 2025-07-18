using System;
using System.IO;
using UnityEngine;
/// <summary>
/// 四叉树地形 挂载在地形文件上
/// </summary>
public class QuadTerrain : MonoBehaviour
{
    /// <summary>
    /// 地形名，也是文件标识符
    /// </summary>
    public string TerrainName;
    [NonSerialized]
    public TerrainQuadData QuadData;
    /// <summary>
    /// 是否生成地形过了??
    /// </summary>
    public bool IsGenerateQuadData = false;

    /// <summary>
    /// 给Selection的地形自己加载quadData，
    /// </summary>
    public void LoadSelfData(string loadPath)
    {
        if (File.Exists(loadPath))
        {
            Debug.Log("加载成功" + loadPath);
            QuadData = BinaryManager.Load<TerrainQuadData>(loadPath);

        }
        else
        {
            Debug.Log("不存在地形文件" + loadPath);
        }

    }

}
