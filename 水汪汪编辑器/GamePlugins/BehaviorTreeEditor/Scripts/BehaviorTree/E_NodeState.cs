using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 行为树的执行结果状态美剧
/// </summary>
public enum E_NodeState
{
    /// <summary>
    /// 最近一次成功 先记录状态
    /// </summary>
    LastSucceed,
    /// <summary>
    /// 成功
    /// </summary>
    Succeed,
    /// <summary>
    /// 失败
    /// </summary>
    Faild,
    /// <summary>
    /// 正在执行 下次还会继续进入这个逻辑里
    /// </summary>
    Running
}
