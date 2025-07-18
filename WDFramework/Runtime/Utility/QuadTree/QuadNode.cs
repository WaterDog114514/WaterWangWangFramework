using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 四叉树格子
/// </summary>
[Serializable]
public class QuadNode<T> where T : class, new()
{
    /// <summary>
    /// 所属的树
    /// </summary>
    public QuadTree<T> QuadTree;
    public SerializableRect Rect; 
    /// <summary>
    /// 已经被划分了吗
    /// </summary>
    public bool isDivided;
    /// <summary>
    /// 子节点
    /// </summary>
    public List<QuadNode<T>> childNodes;
    /// <summary>
    /// 单元格中的物体
    /// </summary>
    [NonSerialized]
    public List<QuadObject> Objects;
    //数据文件
    public T data;
    public QuadNode(QuadTree<T> tree, SerializableRect rect)
    {
        this.QuadTree = tree;
        this.Rect = rect;
        this.isDivided = false;
        this.childNodes = new List<QuadNode<T>>();
        this.Objects = new List<QuadObject>();
        this.data = new T();
    }

    /// <summary>
    /// 计入物体
    /// </summary>
    /// <param name="obj"></param>
    public void EnterObj(QuadObject obj)
    {
        //不在边界内，直接返回
        if (!ContainObjCheck(obj)) return;
        //在边界内，则记录
        //避免重复记录
        if (!Objects.Contains(obj))
            Objects.Add(obj);
        foreach (var child in childNodes)
        {
            child.EnterObj(obj);
        }
    }

    /// <summary>
    /// 划分单元格子，从而创建小格子
    /// </summary>
    /// <param name="currentDepth">当前递归深度</param>
    public void DivideCell(int currentDepth)
    {
        // 如果已经达到最大深度，或者已经划分过，直接返回
        if (currentDepth >= QuadTree.MaxDepth || isDivided) return;
        // 标记为已划分
        isDivided = true;

        // 每个子节点的大小是当前节点大小的一半
        float halfWidth = Rect.rect.width / 2f;
        float halfHeight = Rect.rect.height / 2f;

        // 创建四个子节点
        for (int i = 0; i < 4; i++)
        {
            // 子节点的中心位置
            float offsetX = (i % 2 == 0) ? 0 : halfWidth; // 左右偏移
            float offsetY = (i < 2) ? 0 : halfHeight;    // 上下偏移
            SerializableRect childRect = new SerializableRect(Rect.rect.x + offsetX, Rect.rect.y + offsetY, halfWidth, halfHeight);
            // 创建子节点
            QuadNode<T> childNode = new QuadNode<T>(QuadTree, childRect);
            // 将子节点加入列表
            childNodes.Add(childNode);
            // 递归划分子节点
            childNode.DivideCell(currentDepth + 1);
        }
    }

    /// <summary>
    /// 通过坐标得到最深四叉树节点
    /// </summary>
    /// <returns></returns>
    public QuadNode<T> GetQuadNodeFromPosition(Vector2 position)
    {
        if (!Rect.rect.Contains(position))
            return null;

        if (!isDivided)
            return this;

        foreach (var child in childNodes)
        {
            var node = child.GetQuadNodeFromPosition(position);
            if (node != null)
                return node;
        }

        return null;
    }

    /// <summary>
    /// 查找物体所在的最深的格子节点
    /// </summary>
    /// <param name="obj">目标物体</param>
    /// <returns>最深的格子节点</returns>
    public QuadNode<T> GetDeepestNodeFromObj(QuadObject obj)
    {
        // 如果当前节点不包含物体，则返回 null
        if (!ContainObjCheck(obj)) return null;

        // 如果当前节点没有子节点或未划分，则说明当前节点是最深的
        if (!isDivided)
            return this;

        // 遍历子节点，递归查找物体所在的最深子节点
        foreach (var child in childNodes)
        {
            var deepestNode = child.GetDeepestNodeFromObj(obj);
            if (deepestNode != null)
                return deepestNode;
        }

        // 如果所有子节点都没有找到，返回当前节点（防止意外情况）
        return this;
    }


    /// <summary>
    /// 检测物体是否在单元格中
    /// </summary>
    public bool ContainObjCheck(QuadObject obj)
    {
        Vector2 objXYPos = new Vector2(obj.transform.position.x, obj.transform.position.z);
        // 获取当前节点的边界范围
        return Rect.rect.Contains(objXYPos);
    }

    public void Update()
    {
        // Update logic here
    }
}
