
/// <summary>
/// 定义流水线步骤接口
/// </summary>
public interface IPipelineStep
{
    /// <summary>
    /// 执行顺序
    /// </summary>
    int Priority { get; }
    //执行流水线，需要传入正在生产的物体才能调用
    void Execute(IFactoryProduct product,ExcelConfiguration configuration);
}
