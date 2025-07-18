using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WDEditor;
/// <summary>
/// �ڵ��߼������ࡪ���ɼ���ࡢ�ڵ�༭��������
/// </summary>
public class VNEditorOperator
{
    private win_NodeEditor window;
    private Dictionary<int, VisualNode> dic_Nodes => window.data.dic_Nodes;

    public VNEditorOperator(win_NodeEditor win)
    {
        this.window = win;
    }
    /// <summary>
    /// ������нڵ�
    /// </summary>
    public virtual void m_ClearAllNodes()
    {
        dic_Nodes.Clear();
    }
    //��ʼ����ĳĳ�ڵ㣬ǰ��id���������������Ǳ����ӵ�
    public void LinkNode(VisualNode BeginNode, VisualNode EndNode)
    {
        //��ȷ���ǲ��Ǻ��Լ�����
        if(BeginNode == EndNode) return;
        //�ٴ�ȷ���Լ��ǲ����Ѿ����ӹ��ն˽ڵ���
        if(BeginNode.ExitNodes.Contains(EndNode)|| EndNode.EnterNodes.Contains(BeginNode)) return;
        switch (EndNode.EnterPortType)
        {
            case E_NodePortType.Mulit:
                // ��������ӵĽڵ�����������
                EndNode.EnterNodes.Add(BeginNode);
                BeginNode.ExitNodes.Add(EndNode);
                break;
            case E_NodePortType.Single:
                // ��������ӵĽڵ�ֻ����������
                if (EndNode.EnterNodes.Count == 0)
                {
                    EndNode.EnterNodes.Add(BeginNode);
                    BeginNode.ExitNodes.Add(EndNode);
                }
                else
                {
                    Debug.LogWarning("�����ӵĽڵ��Ѿ���һ�����ӡ�");
                }
                break;
            case E_NodePortType.None:
                // ��������ӵĽڵ㲻��������
                Debug.LogWarning("�����ӵĽڵ㲻�������ӡ�");
                break;
        }
    }
    //ȡ������ĳĳ�ڵ���߼�
    public void UnlinkNode(VisualNode BeginNode, VisualNode EndNode)
    {

        if (EndNode.EnterNodes.Contains(BeginNode))
        {
            EndNode.EnterNodes.Remove(BeginNode);
        }
        else
        {
            Debug.LogWarning("�����ӽڵ�Ľ��ڽڵ��б��в��������������ڵ��ID��");
        }

        if (BeginNode.ExitNodes.Contains(EndNode))
        {
            BeginNode.ExitNodes.Remove(EndNode);
        }
        else
        {
            Debug.LogWarning("�������ڵ�ĳ��ڽڵ��б��в������ñ����ӽڵ��ID��");
        }
    }
    //�����ڵ�
    public void CreateNode(VisualNode VNodePreset,Vector2 CreatePosition)
    {
        //��ȿ�¡
        VisualNode newNode = BinaryManager.DeepClone(VNodePreset);
        //������ʼ��
        newNode.drawInfo.Position = CreatePosition;
        newNode.IntiDrawHelper();
        //   Debug.Log(Event.current.mousePosition);
        AddNode(newNode);
    }
    public void RemoveNode(VisualNode VNode)
    {
        dic_Nodes.Remove(VNode.ID);
    }
    //���л�Ȼ�󱣴�ڵ�ҳ��
    /// <summary>
    /// ����ӽڵ�
    /// </summary>
    /// <param name="node"></param>
    public void AddNode(VisualNode node, int id = -1)
    {
        //���ڷ����л���ָ��id���
        if (id != -1)
        {
            dic_Nodes.Add(id, node);
            return;
        }
        //���ڲ˵�������ӽڵ㣬���Զ�����id������ݵ�����������
        int CurrentIndex = 0;
        if (dic_Nodes.Count != 0)
        {
            while (dic_Nodes.ContainsKey(CurrentIndex))
            {
                CurrentIndex++;
            }
        }
        //��������
        node.ID = CurrentIndex;
        //���
        dic_Nodes.Add(CurrentIndex, node);
    }
}