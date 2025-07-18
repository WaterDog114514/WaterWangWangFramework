using System;
using System.Collections.Generic;
using UnityEngine;



//ڤ˼���������뷨��
/*
 * �ҵ���������ڵ�༭��ֻ��Ϊ��ʵ�ִ洢�ڵ���Ϣ�ͱ༭�ڵ���Ϣ����Ӧ���ڱ༭������ʱ��Ҫʵ�ֽڵ�֮���߼�����
 * ����ʵ���߼�Ӧ���� ĳִ���� ���ر༭�õĽڵ�������Ϣ����ִ���Լ��߼���
 * ����ʹ�ð취��дһ���ܹ���Ӧ���нڵ�������࣬���洢��Runtime�еĲ��������ǻ��Լ����õ�
 * �����ڵ�֮����߼���ϵ��������֮�⣬�Ƚ�ʲô�Ķ���Ҫ�У�
 * ������Ϊ��Ҫ��������ڵ���Ϣ�����һ��ִ�����Ŷ�
*/


/// <summary>
/// �ڵ�����  ����ʱר�ã�һ�����ݾ���һ���ڵ�
/// </summary>
[Serializable]
public class RuntimeNode
{
    //����id ���ڽ�����ϵ
    public int id;
    //��Ӧ ��������������
    [SerializeField]
    public Dictionary<string, RuntimeNodeParameter> parameters = new Dictionary<string, RuntimeNodeParameter>();

    //�����ڽڵ��¼
    public E_NodePortType EnterPortType;
    public RuntimeNode[] EnterNodes;
    public E_NodePortType ExitPortType;
    public RuntimeNode[] ExitNodes;
    //�õ����ڵ������ӵĽڵ�
    public RuntimeNode GetExitNode(int index = 0)
    {
        switch (ExitPortType)
        {
            case E_NodePortType.Mulit:
                return ExitNodes[index];
            case E_NodePortType.Single:
                return ExitNodes[0];
            case E_NodePortType.None:
                return null;
        }
        return null;
    }
    public RuntimeNode GetEnterNode(int index = 0)
    {
        switch (ExitPortType)
        {
            case E_NodePortType.Mulit:
                return EnterNodes[index];
            case E_NodePortType.Single:
                return EnterNodes[0];
            case E_NodePortType.None:
                return null;
        }
        return null;
    }
}
