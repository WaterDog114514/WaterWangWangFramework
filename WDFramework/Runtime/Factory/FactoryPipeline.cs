using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ˮ�߱ر��������Ǿ����������ɲ�Ʒ
/// </summary>
public interface IFactoryPipeline
{
    IFactoryProduct CreateNewProduct(int ProductIndex);
}

/// <summary>
/// ������ˮ�߻���
/// </summary>
/// <typeparam name="TConfigType"></typeparam>
public abstract class FactoryPipeline<TConfigType>: IFactoryPipeline where TConfigType : ExcelConfiguration
{
    public FactoryPipeline()
    {
        InitializePipeline();
    }
    /// <summary>
    /// ��ʼ����ˮ��
    /// </summary>
    public abstract void InitializePipeline();
    protected Dictionary<int,TConfigType> container;
    /// <summary>
    /// ��������
    /// </summary>
    protected List<IPipelineStep> ProductSteps =
       new List<IPipelineStep>();

    public void AddStep(IPipelineStep step)
    {
        if (ProductSteps.Contains(step)) Debug.LogWarning("��⵽�ظ�����ˮ�߲���");
        ProductSteps.Add(step);
    }
    /// <summary>
    /// �������ȼ�������,PriorityԽ��Խ��ǰ
    /// </summary>
    public void SortStep()
    {
        ProductSteps.Sort((a, b) => b.Priority.CompareTo(a.Priority));
    }
    /// <summary>
    /// ������ʼ����Ʒ��Ҳ���Ǵ���һ����տ��ĵ�Ĥ����������ˮ�߽��мӹ����ܵõ���Ʒ
    /// </summary>
    public abstract IFactoryProduct InitializeProduct();
    /// <summary>
    /// ͨ����ˮ������һ���µĲ�Ʒ
    /// </summary>
    /// <returns></returns>
    public IFactoryProduct CreateNewProduct(int ProductIndex)
    {
        var configuration = container[ProductIndex];
        var newProduct = InitializeProduct();
        //��ÿ�����迪ʼ�ӹ�����
        foreach (var step in ProductSteps)
        {
            step.Execute(newProduct,configuration);
        }
        return newProduct;
    }

}