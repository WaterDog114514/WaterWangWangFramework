using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WDFramework;


//为了给框架更加方便的调用，开发一套关联Object的伪Obj来使用
/// <summary>
/// 所有物体基类
/// </summary>
public abstract class Obj
{
    /// <summary>
    /// 所属对象池标识，数据对象通过Type区分，游戏对象通过名字区分
    /// </summary>
    //使用此标记来在Excel中详细设计分组，对象池约束和类型等等
    //不设置通常使用默认的对象池设置：为扩容池
    public string PoolIdentity;
    //进出对象池回调
    public UnityAction EnterPoolCallback;
    public UnityAction QuitPoolCallback;
    // 销毁时候回调
    public UnityAction DeepDestroyCallback;
    /// <summary>
    /// 浅销毁，把他放进对象池里
    /// </summary>
    public void DestroyToPool()
    {
        ObjectManager.Instance.DestroyObj(this);
    }
    /// <summary>
    /// 深度销毁对象，完全从内存中移除
    /// </summary>
    public void DeepDestroy()
    {

        ObjectManager.Instance.DeepDestroyObj(this);

    }

}
