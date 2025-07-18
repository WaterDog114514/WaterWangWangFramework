using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 节点数据们的数据，也是序列化的文件，一页的数据
/// </summary>
[Serializable]
public class RuntimeNodeContainer
{
    //对应  节点id——节点本身
    public Dictionary<int, RuntimeNode> Nodes = new Dictionary<int, RuntimeNode>();
}
