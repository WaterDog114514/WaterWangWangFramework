using Excel;
using System.Data;
using System.IO;

/// <summary>
/// Excel 文件读取实现类
/// 功能：通过临时文件避免原文件被占用问题
/// </summary>
public class ExcelReader 
{
    public DataTableCollection ReadExcel(string path)
    {
        // 文件存在检查
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Excel文件不存在: {path}");
        }

        // 创建临时文件路径（解决Excel文件被占用问题）
        string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(path));
        File.Copy(path, tempPath, true);

        try
        {
            // 使用ExcelDataReader读取文件
            using (var fs = new FileStream(tempPath, FileMode.Open, FileAccess.Read))
            {
                var reader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                return reader.AsDataSet().Tables;
            }
        }
        finally
        {
            // 确保删除临时文件
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }
}