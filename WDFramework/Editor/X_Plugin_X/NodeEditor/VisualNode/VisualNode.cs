using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
/// <summary>
/// 可视化节点之父 用于在窗口中显示节点
/// </summary>
public class VisualNode
{
    //节点ID
    public int ID;
    //节点名
    public string NodeName;
    //节点绘制器
    [NonSerialized]
    public VNDraw vnDraw;

    //节点进口信息
    public E_NodePortType EnterPortType;
    //进口的所有节点(存id)
    public List<VisualNode> EnterNodes = new List<VisualNode>();
    //节点出口信息
    public E_NodePortType ExitPortType;
    //出口的所有节点(存id)
    public List<VisualNode> ExitNodes = new List<VisualNode>();
    //节点参数
    public List<VNParameter> parameters = new List<VNParameter>();
    public VisualNode()
    {
        IntiDrawHelper();
    }
    //绘制信息
    public VNDrawInfo drawInfo = new VNDrawInfo();
    /// <summary>
    /// 初始化节点逻辑
    /// </summary>
    public void IntiDrawHelper()
    {
        vnDraw = new VNDraw(this);
        if (EnterNodes == null) EnterNodes = new List<VisualNode>();
        if (ExitNodes == null) ExitNodes = new List<VisualNode>();
    }

    //重新检测节点口Type改变，然后重新应用
    public void CheckPortTypeChange()
    {
        //进口检测
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

        //出口检测
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
