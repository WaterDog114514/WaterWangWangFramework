using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ĳ�������
/// </summary>
[Serializable]
public class QuadNode<T> where T : class, new()
{
    /// <summary>
    /// ��������
    /// </summary>
    public QuadTree<T> QuadTree;
    public SerializableRect Rect; 
    /// <summary>
    /// �Ѿ�����������
    /// </summary>
    public bool isDivided;
    /// <summary>
    /// �ӽڵ�
    /// </summary>
    public List<QuadNode<T>> childNodes;
    /// <summary>
    /// ��Ԫ���е�����
    /// </summary>
    [NonSerialized]
    public List<QuadObject> Objects;
    //�����ļ�
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
    /// ��������
    /// </summary>
    /// <param name="obj"></param>
    public void EnterObj(QuadObject obj)
    {
        //���ڱ߽��ڣ�ֱ�ӷ���
        if (!ContainObjCheck(obj)) return;
        //�ڱ߽��ڣ����¼
        //�����ظ���¼
        if (!Objects.Contains(obj))
            Objects.Add(obj);
        foreach (var child in childNodes)
        {
            child.EnterObj(obj);
        }
    }

    /// <summary>
    /// ���ֵ�Ԫ���ӣ��Ӷ�����С����
    /// </summary>
    /// <param name="currentDepth">��ǰ�ݹ����</param>
    public void DivideCell(int currentDepth)
    {
        // ����Ѿ��ﵽ�����ȣ������Ѿ����ֹ���ֱ�ӷ���
        if (currentDepth >= QuadTree.MaxDepth || isDivided) return;
        // ���Ϊ�ѻ���
        isDivided = true;

        // ÿ���ӽڵ�Ĵ�С�ǵ�ǰ�ڵ��С��һ��
        float halfWidth = Rect.rect.width / 2f;
        float halfHeight = Rect.rect.height / 2f;

        // �����ĸ��ӽڵ�
        for (int i = 0; i < 4; i++)
        {
            // �ӽڵ������λ��
            float offsetX = (i % 2 == 0) ? 0 : halfWidth; // ����ƫ��
            float offsetY = (i < 2) ? 0 : halfHeight;    // ����ƫ��
            SerializableRect childRect = new SerializableRect(Rect.rect.x + offsetX, Rect.rect.y + offsetY, halfWidth, halfHeight);
            // �����ӽڵ�
            QuadNode<T> childNode = new QuadNode<T>(QuadTree, childRect);
            // ���ӽڵ�����б�
            childNodes.Add(childNode);
            // �ݹ黮���ӽڵ�
            childNode.DivideCell(currentDepth + 1);
        }
    }

    /// <summary>
    /// ͨ������õ������Ĳ����ڵ�
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
    /// �����������ڵ�����ĸ��ӽڵ�
    /// </summary>
    /// <param name="obj">Ŀ������</param>
    /// <returns>����ĸ��ӽڵ�</returns>
    public QuadNode<T> GetDeepestNodeFromObj(QuadObject obj)
    {
        // �����ǰ�ڵ㲻�������壬�򷵻� null
        if (!ContainObjCheck(obj)) return null;

        // �����ǰ�ڵ�û���ӽڵ��δ���֣���˵����ǰ�ڵ��������
        if (!isDivided)
            return this;

        // �����ӽڵ㣬�ݹ�����������ڵ������ӽڵ�
        foreach (var child in childNodes)
        {
            var deepestNode = child.GetDeepestNodeFromObj(obj);
            if (deepestNode != null)
                return deepestNode;
        }

        // ��������ӽڵ㶼û���ҵ������ص�ǰ�ڵ㣨��ֹ���������
        return this;
    }


    /// <summary>
    /// ��������Ƿ��ڵ�Ԫ����
    /// </summary>
    public bool ContainObjCheck(QuadObject obj)
    {
        Vector2 objXYPos = new Vector2(obj.transform.position.x, obj.transform.position.z);
        // ��ȡ��ǰ�ڵ�ı߽緶Χ
        return Rect.rect.Contains(objXYPos);
    }

    public void Update()
    {
        // Update logic here
    }
}
