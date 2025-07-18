using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFactory<T> : Singleton<T> where T : class, new()
{
    public BaseFactory()
    {
        InitializeFactory();
    }

    /// <summary>
    /// 初始化工厂，让子类必须完善添加流水线的工作
    /// </summary>
    public abstract void InitializeFactory();

    /// <summary>
    /// 以生产好的产品模板，到时候直接拿来复制即可，不用再进行流水线了。
    /// </summary>
    protected Dictionary<string, IFactoryProduct> productTempPlate;
    /// <summary>
    /// 流水线，专门是生成工作流
    /// </summary>
    public IFactoryPipeline pipeline { get;protected set;}

    //都留给子类去实现把
    public void Inti_LoadDataInAndroid()
    {
        throw new System.NotImplementedException();
    }
    public void Inti_LoadDataInWindows()
    {
        throw new System.NotImplementedException();
    }
}
