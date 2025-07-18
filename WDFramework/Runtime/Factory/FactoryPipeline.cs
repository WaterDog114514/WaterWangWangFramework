using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 工厂流水线必备工作，那就是运作生成产品
/// </summary>
public interface IFactoryPipeline
{
    IFactoryProduct CreateNewProduct(int ProductIndex);
}

/// <summary>
/// 工厂流水线基类
/// </summary>
/// <typeparam name="TConfigType"></typeparam>
public abstract class FactoryPipeline<TConfigType>: IFactoryPipeline where TConfigType : ExcelConfiguration
{
    public FactoryPipeline()
    {
        InitializePipeline();
    }
    /// <summary>
    /// 初始化流水线
    /// </summary>
    public abstract void InitializePipeline();
    protected Dictionary<int,TConfigType> container;
    /// <summary>
    /// 生产步骤
    /// </summary>
    protected List<IPipelineStep> ProductSteps =
       new List<IPipelineStep>();

    public void AddStep(IPipelineStep step)
    {
        if (ProductSteps.Contains(step)) Debug.LogWarning("检测到重复的流水线步骤");
        ProductSteps.Add(step);
    }
    /// <summary>
    /// 根据优先级排序步骤,Priority越大，越靠前
    /// </summary>
    public void SortStep()
    {
        ProductSteps.Sort((a, b) => b.Priority.CompareTo(a.Priority));
    }
    /// <summary>
    /// 创建初始化产品，也就是创建一个最空旷的底膜，给后面流水线进行加工才能得到产品
    /// </summary>
    public abstract IFactoryProduct InitializeProduct();
    /// <summary>
    /// 通过流水线生产一个新的产品
    /// </summary>
    /// <returns></returns>
    public IFactoryProduct CreateNewProduct(int ProductIndex)
    {
        var configuration = container[ProductIndex];
        var newProduct = InitializeProduct();
        //让每个步骤开始加工生产
        foreach (var step in ProductSteps)
        {
            step.Execute(newProduct,configuration);
        }
        return newProduct;
    }

}