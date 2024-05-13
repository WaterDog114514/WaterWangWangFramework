using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BehaviorTreeNodeEditorWindow : VisualNodeEditorWindow
{
    /// <summary>
    /// 正在处于监察模式吗
    /// </summary>
    public bool b_IsCheckingMode;
    /// <summary>
    /// 正在监视的物体
    /// </summary>
    public BTNodeObjcetDriver CheckingDirver;
    /// <summary>
    /// 起始根节点
    /// </summary>
    public RootNode_VisualBehaviorTreeNode RootNode;
    /// <summary>
    /// 加载本地存档专用
    /// </summary>
    public BTEditorSaveLoader Loader;
    /// <summary>
    /// 用来连接时过度的节点
    /// </summary>
    public VisualBehaviorTreeNode Temp_LinkNode;
    /// <summary>
    /// 是不是儿子连接到爹
    /// </summary>
    public bool b_IsSonToFather;
    public BehaviorTreeNodeEditorWindow(BaseWindow window, Rect size) : base(window, size)
    {
        // CreateRootNode();
        Loader = new BTEditorSaveLoader(this);
        CreateRootNode();
        // new RootNode_VisualBehaviorTreeNode(null, Size.position + Size.size / 2, new Vector2(120, 60));
    }

    public void CreateRootNode()
    {
        RootNode = new RootNode_VisualBehaviorTreeNode(null, E_BehaviorType.RootNode);
        RootNode.Name = "开始根节点";
        RootNode.Pos_Self = Vector2.right * Size.size.x / 2 + Vector2.up * Size.size.y / 3;
        RootNode.dic_Nodes = dic_Nodes;
        m_AddNode(RootNode);
    }
    //绘制重写
    public override void Draw()
    {

       
        base.Draw();
        if (GUI.Button(
            new Rect(Size.position + new Vector2(Size.width - 80, Size.height - 50), new Vector2(75, 45)), "自动排列节点"))
        {
            m_ClickAutoArrange();
        }
    }

    public override void On_EndLinkNode(VisualBaseNode LinkedNode)
    {
        //儿子连接到爹
        if (b_IsSonToFather)
        {
            m_LinkNode(LinkedNode as VisualBehaviorTreeNode, Temp_LinkNode);
        }
        else
        {
            m_LinkNode(Temp_LinkNode, LinkedNode as VisualBehaviorTreeNode);
        }

        Temp_LinkNode = null;
        b_IsLinkingNode = false;
    }
    //开始连接
    public override void On_StartLinkedNode()
    {
        b_IsLinkingNode = true;
        Temp_LinkNode = SelectedNode as VisualBehaviorTreeNode;
    }
    //清空所有节点
    public override void m_ClearAllNodes()
    {
        base.m_ClearAllNodes();
        CurrentIndex = 0;
        CreateRootNode();
    }

    /// <summary>
    /// 设置某节点为起始节点
    /// </summary>
    /// <param name="node"></param>
    public void m_SettingAsStartNode(VisualBehaviorTreeNode node)
    {
        if (RootNode.ChildID != -1)
        {
            (dic_Nodes[RootNode.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
        }
        node.FatherID = RootNode.ID;
        RootNode.ChildID = node.ID;
    }

    /// <summary>
    /// 连接两个节点
    /// </summary>
    /// <param name="Father"></param>
    /// <param name="Son"></param>
    public void m_LinkNode(VisualBehaviorTreeNode Father, VisualBehaviorTreeNode Son)
    {
        //判断是不是表现节点是否想当爹
        if (Father is BehaviorNode_VisualBehaviorTreeNode)
        {
            EditorUtility.DisplayDialog("操作错误！", "动作节点是最终子节点，可不能让它当爹呀！！", "(╯▽╰)");
            return;
        }
        //判断是否是根节点想当儿子
        if (Son is RootNode_VisualBehaviorTreeNode)
        {
            EditorUtility.DisplayDialog("操作错误！", "请使用\n“设置为起始节点”\n“连接到父节点”\n选项来设置节点为开始节点", "(╯▽╰)");
            return;
        }
        //自己连自己
        if (Father == Son)
        {
            EditorUtility.DisplayDialog("操作错误！", "自己不能连自己\n自己怎么能当儿子又当爹的呢？", "(╯▽╰)");
            return;
        }
        //如果儿子是终末节点，就清除儿子原来的父子关系
        if (Son is BehaviorNode_VisualBehaviorTreeNode)
            Son.m_DisConnectedFather();
        if (Father is RootNode_VisualBehaviorTreeNode)
        {
            RootNode_VisualBehaviorTreeNode Node = Father as RootNode_VisualBehaviorTreeNode;
            //断开原来的父关系
            if (Node.ChildID != -1)
            {
                (dic_Nodes[Node.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
            }
            Node.ChildID = Son.ID;
        }
        else if (Father is DecoratorNode_VisualBehaviorTreeNode)
        {
            DecoratorNode_VisualBehaviorTreeNode Node = Father as DecoratorNode_VisualBehaviorTreeNode;
            //断开原来的父关系
            if (Node.ChildID != -1)
            {
                (dic_Nodes[Node.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
            }
            Node.ChildID = Son.ID;
        }
        else if (Father is ControlNode_VisualBehaviorTreeNode)
        {
            ControlNode_VisualBehaviorTreeNode Node = Father as ControlNode_VisualBehaviorTreeNode;
            Son.m_DisConnectedFather();
            //有了不能重复添加
            if (Node.ChildsId.Contains(Son.ID))
            {
                return;
            }
            Node.ChildsId.Add(Son.ID);
        }

        //给孩子设置下爹滴呀
        Son.FatherID = Father.ID;
    }


    /// <summary>
    /// 开始创建行为树节点
    /// </summary>
    /// <param name="index"></param>
    public VisualBehaviorTreeNode CreateBehaviorNode(E_BehaviorType type, bool b_IsAddDic = true)
    {
        //设置坐标  目前自身坐标位置为 Event.current.mousePosition - Size.position - win.Pos_CurrentView
        Vector2 defaultNodePos = dropmenu.ShowPos - Size.position - Pos_CurrentView;
        VisualBehaviorTreeNode node = null;
        //根据节点类型创建不同
        switch (type)
        {
            case E_BehaviorType.RootNode:
                node = new RootNode_VisualBehaviorTreeNode(null, type);
                node.Name = "开始根节点";
                break;
            case E_BehaviorType.SelectTreeNode:
                node = new ControlNode_VisualBehaviorTreeNode(null, type);
                node.Name = "选择节点";
                break;

            case E_BehaviorType.SequeneTreeNode:
                node = new ControlNode_VisualBehaviorTreeNode(null, type);
                node.Name = "序列节点";
                break;

            case E_BehaviorType.ParallelTreeNode:
                node = new ControlNode_VisualBehaviorTreeNode(null, type);
                node.Name = "并行节点";
                break;

            case E_BehaviorType.ActionTreeNode:
                node = new BehaviorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "动作节点";
                break;

            case E_BehaviorType.ConditionNode:
                node = new BehaviorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "条件节点";
                break;

            case E_BehaviorType.ReverseDecoratorNode:
                node = new DecoratorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "反转节点";
                break;

            case E_BehaviorType.DelayDecoratorNode:
                node = new DecoratorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "延迟执行节点";
                break;

            case E_BehaviorType.RepeatDecoratorNode:
                node = new DecoratorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "重复执行节点";
                break;
        }
        node.dic_Nodes = dic_Nodes;
        node.Pos_Self = defaultNodePos;
        //Debug.Log($"鼠标:{ShowPos}  窗口大小位置:{win.Pos_CurrentView}");
        //添加数据
        if (b_IsAddDic)
            m_AddNode(node);
        return node;
    }
    /// <summary>
    /// 点击自动排序
    /// </summary>
    public void m_ClickAutoArrange()
    {
        Pos_CurrentView = Vector2.zero;
        dic_Arrange.Clear();
        foreach (var node in dic_Nodes.Values)
        {

            //从根节点开始排列
            if ((node as VisualBehaviorTreeNode).NodeType == E_BehaviorType.RootNode)
            {
                m_AutoLayer(node as VisualBehaviorTreeNode);
                m_AutoArrangeEveryLayer();
                return;
            }
        }

    }

    /// <summary>
    /// 排列用的上下间隔
    /// </summary>
    private Dictionary<int, List<VisualBehaviorTreeNode>> dic_Arrange = new Dictionary<int, List<VisualBehaviorTreeNode>>();
    /// <summary>
    /// 递归自动写入每一层节点的深度层级
    /// </summary>
    public void m_AutoLayer(VisualBehaviorTreeNode Node, int layer = 0)
    {
        //使用层级法
        //先判断该层有没有物体，没有先建立一层
        if (!dic_Arrange.ContainsKey(layer))
        {
            dic_Arrange.Add(layer, new List<VisualBehaviorTreeNode>());
        }
        switch (Node.NodeType)
        {
            case E_BehaviorType.RootNode:
                if ((Node as RootNode_VisualBehaviorTreeNode).ChildID == -1) break;
                dic_Arrange[layer].Add(Node);
                m_AutoLayer(dic_Nodes[(Node as RootNode_VisualBehaviorTreeNode).ChildID] as VisualBehaviorTreeNode, layer + 1);
                break;
            case E_BehaviorType.SelectTreeNode:
            case E_BehaviorType.SequeneTreeNode:
            case E_BehaviorType.ParallelTreeNode:
                dic_Arrange[layer].Add(Node);
                for (int i = 0; i < (Node as ControlNode_VisualBehaviorTreeNode).ChildsId.Count; i++)
                {
                    m_AutoLayer(dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]] as VisualBehaviorTreeNode, layer + 1);
                }
                break;
            case E_BehaviorType.ActionTreeNode:
            case E_BehaviorType.ConditionNode:
                dic_Arrange[layer].Add(Node);
                break;
            case E_BehaviorType.DelayDecoratorNode:
            case E_BehaviorType.ReverseDecoratorNode:
            case E_BehaviorType.RepeatDecoratorNode:
                dic_Arrange[layer].Add(Node);
                if ((Node as DecoratorNode_VisualBehaviorTreeNode).ChildID == -1) break;
                m_AutoLayer(dic_Nodes[(Node as DecoratorNode_VisualBehaviorTreeNode).ChildID] as VisualBehaviorTreeNode, layer + 1);
                break;
        }

    }
    /// <summary>
    /// 自动排序每一层
    /// </summary>
    public void m_AutoArrangeEveryLayer()
    {
        int maxLayer = 0;
        List<VisualBehaviorTreeNode> LayerNodes;
        //先找到最大深度
        foreach (var item in dic_Arrange.Keys)
        {
            if (maxLayer <= item) maxLayer = item;
        }
        //设置根节点位置
        RootNode_VisualBehaviorTreeNode.instance.Pos_Self = Vector2.right * Size.width / 4 + Vector2.up * Size.size.y / 16;
        for (int i = 1; i <= maxLayer; i++)
        {
            LayerNodes = dic_Arrange[i];
            float width = (LayerNodes.Count - 1) * 150;
            for (int j = 0; j < LayerNodes.Count; j++)
            {
                LayerNodes[j].Pos_Self = dic_Nodes[LayerNodes[j].FatherID].Pos_Self +
                    Vector2.up * 200 +
                    Vector2.right * 150F * j +
                    -width / 2 * Vector2.right
                    ;
            }
        }

    }

}
//arrangeHeightDistance = Vector2.up * Node.Size.y * 1.5f;
////通过帮孩子排列办法，递归
//switch (Node.NodeType)
//{
//    //根节点直接设置起点
//    case E_BehaviorType.RootNode:
//        //排列孩子
//        Node.Pos_Self = Vector2.right * Size.width / 4 + Vector2.up * Size.size.y / 16;

//        if ((Node as RootNode_VisualBehaviorTreeNode).ChildID == -1) return;
//        //根节点的崽子坐标是根节点的下面+10
//        dic_Nodes[(Node as RootNode_VisualBehaviorTreeNode).ChildID].Pos_Self =
//            Node.Pos_Self + arrangeHeightDistance;
//        m_AutoLayer(dic_Nodes[(Node as RootNode_VisualBehaviorTreeNode).ChildID] as VisualBehaviorTreeNode);
//        break;
//    case E_BehaviorType.SelectTreeNode:
//    case E_BehaviorType.SequeneTreeNode:
//    case E_BehaviorType.ParallelTreeNode:
//        if ((Node as ControlNode_VisualBehaviorTreeNode).ChildsId.Count == 0) return;
//        int count = (Node as ControlNode_VisualBehaviorTreeNode).ChildsId.Count;
//        int childSonCount = getChild_ChildCount(Node as ControlNode_VisualBehaviorTreeNode);
//        float TotalWidth = 0;
//        for (int i = 0; i < count; i++)
//        {
//            if (dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]] is ControlNode_VisualBehaviorTreeNode)
//                for (int j = 0; j < (dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]] as ControlNode_VisualBehaviorTreeNode).ChildsId.Count; j++)
//                {
//                    TotalWidth += dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]].Size.x;
//                }
//            //控制节点崽子坐标是父坐标下面+10，然后根据数量偏移Size的宽度乘1.2
//            if (childSonCount <= count)
//                dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]].Pos_Self =
//                    Node.Pos_Self + arrangeHeightDistance * Vector2.up + //向下偏移
//                    Vector2.right * arrangeWidthtDistance * 0.65F * (count / 2 - i)//间隔
//                    - Vector2.right * offset * arrangeWidthtDistance;//偶数偏移

//            else
//                dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]].Pos_Self =
//                    Node.Pos_Self + arrangeHeightDistance +  //向下偏移
//                    Vector2.right * arrangeWidthtDistance * (1.0f * childSonCount / count) * (count / 2 - i)  //间隔
//                    - Vector2.right * offset * arrangeWidthtDistance;  //偶数偏移

//            m_AutoLayer(dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]] as VisualBehaviorTreeNode);
//        }
//        break;
//    case E_BehaviorType.ActionTreeNode:
//    case E_BehaviorType.ConditionNode:
//        //我们表示没有孩子
//        return;
//    case E_BehaviorType.DelayDecoratorNode:
//    case E_BehaviorType.ReverseDecoratorNode:
//    case E_BehaviorType.RepeatDecoratorNode:

//        int ChildID = (Node as DecoratorNode_VisualBehaviorTreeNode).ChildID;
//        if (ChildID == -1) return;
//        dic_Nodes[ChildID].Pos_Self = Node.Pos_Self + arrangeHeightDistance;
//        m_AutoLayer(dic_Nodes[ChildID] as VisualBehaviorTreeNode);
//        break;
//}