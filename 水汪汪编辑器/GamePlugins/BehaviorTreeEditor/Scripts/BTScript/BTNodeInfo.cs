using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 这是一套编辑器可读 游戏中也能读取的万能数据类（比较消耗内存，建议读取完后手动释放）
/// </summary>
public class BTNodeInfo 
{
    /// <summary>
    /// 用来存储内存地址，才能跟编辑器统一性
    /// </summary>
    public BaseTreeNode Node;
    /// <summary>
    /// 用来找孩子
    /// </summary>
    public E_BehaviorType NodeType;
    /// <summary>
    /// 描述
    /// </summary>
    public string Description;
    public int ID; // 存储父节点的索引
    public List<int> childsID; // 存储子节点的索引数组
    /// <summary>
    /// 该节点所包含参数
    /// </summary>
    public string[] Parameters;
    public BTNodeInfo()
    {
        childsID = new List<int>();
    }
}
/// <summary>
/// 行为树节点类型
/// </summary>
public enum E_BehaviorType
{
    RootNode,
    SelectTreeNode, SequeneTreeNode, ParallelTreeNode,
    ActionTreeNode, ConditionNode,
    DelayDecoratorNode, ReverseDecoratorNode, RepeatDecoratorNode
}