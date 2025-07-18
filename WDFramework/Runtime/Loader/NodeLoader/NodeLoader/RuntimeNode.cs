using System;
using System.Collections.Generic;
using UnityEngine;



//冥思苦想垃圾想法：
/*
 * 我当初做这个节点编辑器只是为了实现存储节点信息和编辑节点信息，不应该在编辑器开发时候要实现节点之间逻辑处理
 * 真正实现逻辑应该由 某执行器 加载编辑好的节点数据信息后，再执行自己逻辑的
 * 正常使用办法：写一个能够对应所有节点的真正类，来存储这Runtime中的参数，他们会自己做好的
 * ！！节点之间的逻辑联系除了连接之外，比较什么的都不要有！
 * 比如行为树要根据这个节点信息来设计一个执行器才对
*/


/// <summary>
/// 节点数据  运行时专用，一个数据就是一个节点
/// </summary>
[Serializable]
public class RuntimeNode
{
    //自身id 用于建立联系
    public int id;
    //对应 参数名——参数
    [SerializeField]
    public Dictionary<string, RuntimeNodeParameter> parameters = new Dictionary<string, RuntimeNodeParameter>();

    //进出口节点记录
    public E_NodePortType EnterPortType;
    public RuntimeNode[] EnterNodes;
    public E_NodePortType ExitPortType;
    public RuntimeNode[] ExitNodes;
    //得到出口单个连接的节点
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
