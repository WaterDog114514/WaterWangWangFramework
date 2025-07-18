using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WDEditor;


/// <summary>
//���л��� ���ڸ��ض���Ӧ�ڵ�༭���������л�����
/// </summary>
public static class VNEditorSerializer
{
    public static void SerializeNodes(Dictionary<int, VisualNode> dic_nodes)
    {
        //���нڵ�ļ���
        var container = new RuntimeNodeContainer();
        //�Ը�ҳ���нڵ�������л�Ϊ��������
        foreach (var visualNode in dic_nodes.Values)
        {
            RuntimeNode runtimeNode = new RuntimeNode();
            //��ֵid
            runtimeNode.id = visualNode.ID;
            runtimeNode.parameters = new Dictionary<string, RuntimeNodeParameter>();
            //��ֵ���в���
            foreach (var par in visualNode.parameters)
            {
                RuntimeNodeParameter parameter = new RuntimeNodeParameter()
                {
                    type = par.ParameterType,
                    Value = par.Value
                };
                runtimeNode.parameters.Add(par.Name, parameter);
            }
            //������нڵ㵽����
            container.Nodes.Add(runtimeNode.id, runtimeNode);
        }
        //���Ϊ�ļ�
        string SavePath = EditorUtility.SaveFilePanel("���л�Ϊ��������", "Assets", "δ�������л�", "runtimenodes");
        BinaryManager.SaveToPath(container, SavePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}