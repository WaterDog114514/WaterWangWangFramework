using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
/// <summary>
/// ���ӻ��ڵ�֮�� �����ڴ�������ʾ�ڵ�
/// </summary>
public class VisualNode
{
    //�ڵ�ID
    public int ID;
    //�ڵ���
    public string NodeName;
    //�ڵ������
    [NonSerialized]
    public VNDraw vnDraw;

    //�ڵ������Ϣ
    public E_NodePortType EnterPortType;
    //���ڵ����нڵ�(��id)
    public List<VisualNode> EnterNodes = new List<VisualNode>();
    //�ڵ������Ϣ
    public E_NodePortType ExitPortType;
    //���ڵ����нڵ�(��id)
    public List<VisualNode> ExitNodes = new List<VisualNode>();
    //�ڵ����
    public List<VNParameter> parameters = new List<VNParameter>();
    public VisualNode()
    {
        IntiDrawHelper();
    }
    //������Ϣ
    public VNDrawInfo drawInfo = new VNDrawInfo();
    /// <summary>
    /// ��ʼ���ڵ��߼�
    /// </summary>
    public void IntiDrawHelper()
    {
        vnDraw = new VNDraw(this);
        if (EnterNodes == null) EnterNodes = new List<VisualNode>();
        if (ExitNodes == null) ExitNodes = new List<VisualNode>();
    }

    //���¼��ڵ��Type�ı䣬Ȼ������Ӧ��
    public void CheckPortTypeChange()
    {
        //���ڼ��
        if (EnterNodes.Count > 1 && EnterPortType != E_NodePortType.Mulit)
        {
            for (int i = 1; i < EnterNodes.Count; i++)
            {
                EnterNodes.RemoveAt(i);
            }
        }
        if (EnterNodes.Count > 0 && EnterPortType == E_NodePortType.None)
        {
            EnterNodes.Clear();
        }

        //���ڼ��
        if (ExitNodes.Count > 1 && ExitPortType != E_NodePortType.Mulit)
        {
            for (int i = 1; i < ExitNodes.Count; i++)
            {
                ExitNodes.RemoveAt(i);
            }
        }
        if (ExitNodes.Count > 0 && ExitPortType == E_NodePortType.None)
        {
            ExitNodes.Clear();
        }
    }
}
