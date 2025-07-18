using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 四叉树管理器
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class QuadTree<T> where T : class, new()
{
    /// <summary>
    /// 起始根节点
    /// </summary>
    public QuadNode<T> rootNode;
    /// <summary>
    /// 四叉树最大宽度
    /// </summary>
    public float MaxSize;
    /// <summary>
    /// 四叉树的最大深度
    /// </summary>
    public int MaxDepth;
    /// <summary>
    /// 生成四叉树
    /// </summary>
    /// <param name="Depth"></param>
    public void GenerateQuadTree( float MaxSize, int MaxDepth)
    {
        this.MaxDepth = MaxDepth;
        this.MaxSize = MaxSize;
        //生成树根
        rootNode = new QuadNode<T>(this, new SerializableRect(0, 0,MaxSize,MaxSize));
        //开始切割
        rootNode.DivideCell(0);
    }
    //四叉树放入单个物体
    public void EnterObj(QuadObject obj)
    {
        if (rootNode == null)
        {
            Debug.LogError("还未生成四叉树");
            return;
        }
        rootNode.EnterObj(obj);
    }
    //批量放入四叉树
    public void EnterObj(List<QuadObject> objList)
    {
        if (rootNode == null)
        {
            Debug.LogError("还未生成四叉树");
            return;
        }
        foreach (var Obj in objList)
        {
            rootNode.EnterObj(Obj);
        }
    }
    public void Update()
    {

    }

    /// <summary>
    /// 查找物体所在的最深的格子节点
    /// </summary>
    /// <param name="obj">目标物体</param>
    /// <returns>最深的格子节点</returns>
    public QuadNode<T> GetDeepestNodeFromObj(QuadObject obj)
    {
        return rootNode.GetDeepestNodeFromObj(obj);
    }
    /// <summary>
    /// 通过坐标得到最深四叉树节点
    /// </summary>
    /// <returns></returns>
    public QuadNode<T> GetQuadNodeFromPosition(Vector2 position)
    {
        return rootNode.GetQuadNodeFromPosition(position);
    }
}
