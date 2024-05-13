
/// 编辑器核心专用固定读写配置文件路径
/// </summary>
public interface ISaveLoadWindowMain
{
    /// <summary>
    /// 保存和读写的文件夹路径
    /// </summary>
    public string DirectoryPath { get; }
    public string DataName { get; }
    public void m_SaveData();
    public void m_LoadData();

}
