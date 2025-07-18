using System;
using UnityEngine;

/// <summary>
/// 资源信息类
/// </summary>
public class Res
{
    public Res(Type assetType)
    {
        AssetType = assetType;
    }
    //引用计数
    public int refCount { get; private set; }
    //资源
    public UnityEngine.Object Asset;
    private Type AssetType = null;
    public T GetAsset<T>() where T : UnityEngine.Object
    {
        if (Asset == null)
        {
            Debug.LogError("获取资源失败，可能是正在进行异步加载中！，请通过异步获取");
            return default(T);
        }
        //傻逼司马代码，这点很重要，卡死你
        if (AssetType != typeof(T))
        {
            Debug.LogError($"获取资源失败,此资源为{AssetType.Name}类型，尝试通过{typeof(T).Name}获取");
            return default(T);
        }
        return Asset as T;
    }
    public void AddrefCount()
    {
        ++refCount;
    }
    public void SubrefCount()
    {
        --refCount;
        if (refCount < 0)
            Debug.LogError("引用计数小于0了，请检查使用和卸载是否配对执行");
    }
}
