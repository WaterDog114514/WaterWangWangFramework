using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WDEditor;


/// <summary>
//序列化器 用于给特定对应节点编辑器进行序列化操作
/// </summary>
public static class VNEditorSerializer
{
    public static void SerializeNodes(Dictionary<int, VisualNode> dic_nodes)
    {
        //所有节点的集合
        var container = new RuntimeNodeContainer();
        //对该页所有节点进行序列化为运行数据
        foreach (var visualNode in dic_nodes.Values)
        {
            RuntimeNode runtimeNode = new RuntimeNode();
            //赋值id
            runtimeNode.id = visualNode.ID;
            runtimeNode.parameters = new Dictionary<string, RuntimeNodeParameter>();
            //赋值所有参数
            foreach (var par in visualNode.parameters)
            {
                RuntimeNodeParameter parameter = new RuntimeNodeParameter()
                {
                    type = par.ParameterType,
                    Value = par.Value
                };
                runtimeNode.parameters.Add(par.Name, parameter);
            }
            //添加所有节点到集合
            container.Nodes.Add(runtimeNode.id, runtimeNode);
        }
        //输出为文件
        string SavePath = EditorUtility.SaveFilePanel("序列化为运行数据", "Assets", "未命名序列化", "runtimenodes");
        BinaryManager.SaveToPath(container, SavePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}