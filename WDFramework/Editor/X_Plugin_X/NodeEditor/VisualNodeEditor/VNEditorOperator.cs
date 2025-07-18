using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WDEditor;
/// <summary>
/// 节点逻辑操作类——由检测类、节点编辑器类驱动
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
    /// 清空所有节点
    /// </summary>
    public virtual void m_ClearAllNodes()
    {
        dic_Nodes.Clear();
    }
    //开始连接某某节点，前者id是主动方，后者是被连接的
    public void LinkNode(VisualNode BeginNode, VisualNode EndNode)
    {
        //先确认是不是和自己连接
        if(BeginNode == EndNode) return;
        //再次确认自己是不是已经连接过终端节点了
        if(BeginNode.ExitNodes.Contains(EndNode)|| EndNode.EnterNodes.Contains(BeginNode)) return;
        switch (EndNode.EnterPortType)
        {
            case E_NodePortType.Mulit:
                // 如果被连接的节点允许多个连接
                EndNode.EnterNodes.Add(BeginNode);
                BeginNode.ExitNodes.Add(EndNode);
                break;
            case E_NodePortType.Single:
                // 如果被连接的节点只允许单个连接
                if (EndNode.EnterNodes.Count == 0)
                {
                    EndNode.EnterNodes.Add(BeginNode);
                    BeginNode.ExitNodes.Add(EndNode);
                }
                else
                {
                    Debug.LogWarning("被连接的节点已经有一个连接。");
                }
                break;
            case E_NodePortType.None:
                // 如果被连接的节点不允许连接
                Debug.LogWarning("被连接的节点不接受连接。");
                break;
        }
    }
    //取消连接某某节点的逻辑
    public void UnlinkNode(VisualNode BeginNode, VisualNode EndNode)
    {

        if (EndNode.EnterNodes.Contains(BeginNode))
        {
            EndNode.EnterNodes.Remove(BeginNode);
        }
        else
        {
            Debug.LogWarning("被连接节点的进口节点列表中不包含该主动方节点的ID。");
        }

        if (BeginNode.ExitNodes.Contains(EndNode))
        {
            BeginNode.ExitNodes.Remove(EndNode);
        }
        else
        {
            Debug.LogWarning("主动方节点的出口节点列表中不包含该被连接节点的ID。");
        }
    }
    //创建节点
    public void CreateNode(VisualNode VNodePreset,Vector2 CreatePosition)
    {
        //深度克隆
        VisualNode newNode = BinaryManager.DeepClone(VNodePreset);
        //帮他初始化
        newNode.drawInfo.Position = CreatePosition;
        newNode.IntiDrawHelper();
        //   Debug.Log(Event.current.mousePosition);
        AddNode(newNode);
    }
    public void RemoveNode(VisualNode VNode)
    {
        dic_Nodes.Remove(VNode.ID);
    }
    //序列化然后保存节点页面
    /// <summary>
    /// 添加子节点
    /// </summary>
    /// <param name="node"></param>
    public void AddNode(VisualNode node, int id = -1)
    {
        //用于反序列化，指定id添加
        if (id != -1)
        {
            dic_Nodes.Add(id, node);
            return;
        }
        //用于菜单创建添加节点，会自动设置id，会根据迭代来加设置
        int CurrentIndex = 0;
        if (dic_Nodes.Count != 0)
        {
            while (dic_Nodes.ContainsKey(CurrentIndex))
            {
                CurrentIndex++;
            }
        }
        //索引设置
        node.ID = CurrentIndex;
        //添加
        dic_Nodes.Add(CurrentIndex, node);
    }
}