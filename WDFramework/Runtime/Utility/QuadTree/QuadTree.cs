using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ĳ���������
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class QuadTree<T> where T : class, new()
{
    /// <summary>
    /// ��ʼ���ڵ�
    /// </summary>
    public QuadNode<T> rootNode;
    /// <summary>
    /// �Ĳ��������
    /// </summary>
    public float MaxSize;
    /// <summary>
    /// �Ĳ�����������
    /// </summary>
    public int MaxDepth;
    /// <summary>
    /// �����Ĳ���
    /// </summary>
    /// <param name="Depth"></param>
    public void GenerateQuadTree( float MaxSize, int MaxDepth)
    {
        this.MaxDepth = MaxDepth;
        this.MaxSize = MaxSize;
        //��������
        rootNode = new QuadNode<T>(this, new SerializableRect(0, 0,MaxSize,MaxSize));
        //��ʼ�и�
        rootNode.DivideCell(0);
    }
    //�Ĳ������뵥������
    public void EnterObj(QuadObject obj)
    {
        if (rootNode == null)
        {
            Debug.LogError("��δ�����Ĳ���");
            return;
        }
        rootNode.EnterObj(obj);
    }
    //���������Ĳ���
    public void EnterObj(List<QuadObject> objList)
    {
        if (rootNode == null)
        {
            Debug.LogError("��δ�����Ĳ���");
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
    /// �����������ڵ�����ĸ��ӽڵ�
    /// </summary>
    /// <param name="obj">Ŀ������</param>
    /// <returns>����ĸ��ӽڵ�</returns>
    public QuadNode<T> GetDeepestNodeFromObj(QuadObject obj)
    {
        return rootNode.GetDeepestNodeFromObj(obj);
    }
    /// <summary>
    /// ͨ������õ������Ĳ����ڵ�
    /// </summary>
    /// <returns></returns>
    public QuadNode<T> GetQuadNodeFromPosition(Vector2 position)
    {
        return rootNode.GetQuadNodeFromPosition(position);
    }
}
