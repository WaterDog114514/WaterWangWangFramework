
/// <summary>
/// ������ˮ�߲���ӿ�
/// </summary>
public interface IPipelineStep
{
    /// <summary>
    /// ִ��˳��
    /// </summary>
    int Priority { get; }
    //ִ����ˮ�ߣ���Ҫ��������������������ܵ���
    void Execute(IFactoryProduct product,ExcelConfiguration configuration);
}
