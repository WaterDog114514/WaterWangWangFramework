using System;
using System.Collections.Generic;
using UnityEngine;
//���Ľ���������ˮ���°�����


//�ڵ����ݼ��أ�����д
public static class RuntimeNodeLoader
{
    //�洢��ʽ ·�� ���� �����л�����
    private static Dictionary<string, DeSerializeNodeHelper> dic_Helper = new Dictionary<string, DeSerializeNodeHelper>();
    //����һ��ҳ��ķ��������֣������ҳ�ķ����л�����
    public static DeSerializeNodeHelper LoadNode(string dataPath)
    {
        //�Ѿ�����ͬ·����ֱ�ӷ���
        if (dic_Helper.ContainsKey(dataPath))
            return dic_Helper[dataPath];

        //û�����ȼ��أ�Ȼ���½�
        var container = BinaryManager.Load<RuntimeNodeContainer>(dataPath);
        var helper = new DeSerializeNodeHelper(container);
        dic_Helper.Add(dataPath, helper);
        return helper;
    }

}
public class DeSerializeNodeHelper
{
    public RuntimeNodeContainer NodesContainer;

    public DeSerializeNodeHelper(RuntimeNodeContainer nodesContainer)
    {
        NodesContainer = nodesContainer;
    }

    public DeSerializeNodeHelper.Node GetNode(int NodeID)
    {
        if (!NodesContainer.Nodes.ContainsKey(NodeID))
        {
            Debug.LogError("�ڵ㲢δ����id��" + NodeID);
            return null;
        }
        DeSerializeNodeHelper.Node node = new DeSerializeNodeHelper.Node();
        node.id = NodeID;
        node.parameters = NodesContainer.Nodes[NodeID].parameters;
        return node;
    }
    /// <summary>
    /// ���ڷ����л������Ľڵ�
    /// </summary>
    public class Node
    {
        public int id;
        //��������
        public Dictionary<string, RuntimeNodeParameter> parameters;
        public T DeSerializeParameter<T>(string parameterName) => DeSerializeParameter<T>(parameters[parameterName]);
        public T DeSerializeParameter<T>(RuntimeNodeParameter parameter)
        {
            string value = parameter.Value;
            //����ת
            try
            {
                if (typeof(T) == typeof(int))
                {
                    return (T)(object)int.Parse(value); // �����׳��쳣
                }
                else if (typeof(T) == typeof(float))
                {
                    return (T)(object)float.Parse(value); // �����׳��쳣
                }
                else if (typeof(T) == typeof(string))
                {
                    return (T)(object)value; // ֱ�ӷ����ַ���
                }
            }
            //ת���쳣��ʾ
            catch (Exception ex)
            {
                throw new InvalidCastException($"�޷�ת��Ϊ {typeof(T)}: ֵ \"{value}\" ���ܱ�������������Ϣ: {ex.Message}");
            }
            throw new InvalidCastException($"�޷�ת��Ϊ {typeof(T)}: ֵ \"{value}\" ������֧�ֵ����͡�");
        }
    }

}