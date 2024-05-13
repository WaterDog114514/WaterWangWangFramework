using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 缓冲池对象基类
/// 所有缓存池的对象必须继承或挂载它 用于设置最大值和初始化
/// </summary>
public class PoolObject : MonoBehaviour
{
    /// <summary>
    /// 每次重新启用缓存池对象调用的方法，可用于刷新对象数据
    /// </summary>
    private UnityAction IntiEvent;
    public void AddIntiEvent(UnityAction action)
    {
        IntiEvent += action;
    }
    public void RemoveIntiEvent(UnityAction action)
    {
        IntiEvent -= action;
    }
 
    /// <summary>
    /// 初始化方法
    /// </summary>
    private void OnEnable()
    {
        IntiEvent?.Invoke();
    }
}
