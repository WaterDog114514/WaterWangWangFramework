using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ڵ������ǵ����ݣ�Ҳ�����л����ļ���һҳ������
/// </summary>
[Serializable]
public class RuntimeNodeContainer
{
    //��Ӧ  �ڵ�id�����ڵ㱾��
    public Dictionary<int, RuntimeNode> Nodes = new Dictionary<int, RuntimeNode>();
}
