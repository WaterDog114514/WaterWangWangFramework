using System;
using System.Collections.Generic;
using UnityEngine;
//待改进——引入水狗新版对象池


//节点数据加载，待重写
public static class RuntimeNodeLoader
{
    //存储形式 路径 —— 反序列化助手
    private static Dictionary<string, DeSerializeNodeHelper> dic_Helper = new Dictionary<string, DeSerializeNodeHelper>();
    //返回一个页面的反序列助手，负责该页的反序列化操作
    public static DeSerializeNodeHelper LoadNode(string dataPath)
    {
        //已经有相同路径的直接返回
        if (dic_Helper.ContainsKey(dataPath))
            return dic_Helper[dataPath];

        //没有则先加载，然后新建
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
            Debug.LogError("节点并未包含id：" + NodeID);
            return null;
        }
        DeSerializeNodeHelper.Node node = new DeSerializeNodeHelper.Node();
        node.id = NodeID;
        node.parameters = NodesContainer.Nodes[NodeID].parameters;
        return node;
    }
    /// <summary>
    /// 正在反序列化操作的节点
    /// </summary>
    public class Node
    {
        public int id;
        //包含参数
        public Dictionary<string, RuntimeNodeParameter> parameters;
        public T DeSerializeParameter<T>(string parameterName) => DeSerializeParameter<T>(parameters[parameterName]);
        public T DeSerializeParameter<T>(RuntimeNodeParameter parameter)
        {
            string value = parameter.Value;
            //进行转
            try
            {
                if (typeof(T) == typeof(int))
                {
                    return (T)(object)int.Parse(value); // 可能抛出异常
                }
                else if (typeof(T) == typeof(float))
                {
                    return (T)(object)float.Parse(value); // 可能抛出异常
                }
                else if (typeof(T) == typeof(string))
                {
                    return (T)(object)value; // 直接返回字符串
                }
            }
            //转换异常提示
            catch (Exception ex)
            {
                throw new InvalidCastException($"无法转换为 {typeof(T)}: 值 \"{value}\" 不能被解析。错误信息: {ex.Message}");
            }
            throw new InvalidCastException($"无法转换为 {typeof(T)}: 值 \"{value}\" 不符合支持的类型。");
        }
    }

}