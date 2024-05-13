using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行为树AI物体驱动器
/// </summary>
public class BTNodeObjcetDriver : MonoBehaviour
{

    /// <summary>
    /// 是否开启动态监测行为树
    /// </summary>
    public bool b_DynamicCheck ;
    [HideInInspector]
    /// <summary>
    /// 动态检测监测
    /// </summary>
    public Dictionary<string, BTNodeInfo> dynamicDic;
    /// <summary>
    /// 根节点
    /// </summary>
    public RootTreeNode RootNode;
    public BTNodeData data;
    public void Start()
    {
        IntiNode();
    }
    void Update()
    {
        if (RootNode != null)
        {
            RootNode.Execute();
        }

        //在动态监测情况下 每次执行完一次就清空状态 执行成功0.45F后清除
        if (b_DynamicCheck)
            if (RootNode.ChildState == E_NodeState.Succeed)
            {
                if (Time.time >= NextClearTime)
                {
                    RootNode.ResetShowState();
                    NextClearTime = Time.time + 0.25f;
                }
            }

    }

    float NextClearTime;
    /// <summary>
    /// 初始化节点，加载节点数据
    /// </summary>
    public void IntiNode()
    {
            RootNode = BTNodeLoader.Instance.Load(this);
        //Debug.Log(dynamicDic);
    }
}
