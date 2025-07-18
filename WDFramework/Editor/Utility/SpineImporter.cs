using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using Newtonsoft.Json.Linq;
/// <summary>
/// �޸�spine�ļ� �汾
/// </summary>
public class SpineImporter : AssetPostprocessor
{
    //�κ���Դ�������ļ��У����붼�ᱻ���õķ���
    void OnPreprocessAsset()
    {
        try
        {
            if (!this.assetPath.EndsWith(".json"))
                return;
            // ��ȡ������ļ�����
            string msg = File.ReadAllText(this.assetPath, Encoding.UTF8);
            JObject jo = JObject.Parse(msg);
            //���ж��Ƿ��� spine �ļ�
            //������ ����Ƿ����"skeleton"��"spine"�ؼ���
            if (jo["skeleton"] == null || jo["skeleton"]["spine"] == null)
            {
                //���û�о�����
                return;
            }

            string item = jo["skeleton"]["spine"].ToString();

            if (!string.IsNullOrEmpty(item) && item.ToString() != "3.8")
            {
                jo["skeleton"]["spine"] = "3.8";//�޸İ汾Ϊ3.8�汾
                File.WriteAllText(this.assetPath, jo.ToString());
                AssetDatabase.Refresh();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SpineImportSetting �쳣 {e.Message}");
        }
    }
}

