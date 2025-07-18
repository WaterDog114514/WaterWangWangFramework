using Excel;
using System.Data;
using System.IO;

/// <summary>
/// Excel �ļ���ȡʵ����
/// ���ܣ�ͨ����ʱ�ļ�����ԭ�ļ���ռ������
/// </summary>
public class ExcelReader 
{
    public DataTableCollection ReadExcel(string path)
    {
        // �ļ����ڼ��
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Excel�ļ�������: {path}");
        }

        // ������ʱ�ļ�·�������Excel�ļ���ռ�����⣩
        string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(path));
        File.Copy(path, tempPath, true);

        try
        {
            // ʹ��ExcelDataReader��ȡ�ļ�
            using (var fs = new FileStream(tempPath, FileMode.Open, FileAccess.Read))
            {
                var reader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                return reader.AsDataSet().Tables;
            }
        }
        finally
        {
            // ȷ��ɾ����ʱ�ļ�
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }
}