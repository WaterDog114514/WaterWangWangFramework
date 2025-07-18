using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using Newtonsoft.Json.Linq;
/// <summary>
/// 修改spine文件 版本
/// </summary>
public class SpineImporter : AssetPostprocessor
{
    //任何资源（包括文件夹）导入都会被调用的方法
    void OnPreprocessAsset()
    {
        try
        {
            if (!this.assetPath.EndsWith(".json"))
                return;
            // 读取导入的文件内容
            string msg = File.ReadAllText(this.assetPath, Encoding.UTF8);
            JObject jo = JObject.Parse(msg);
            //先判断是否是 spine 文件
            //依托与 检查是否包含"skeleton"和"spine"关键词
            if (jo["skeleton"] == null || jo["skeleton"]["spine"] == null)
            {
                //如果没有就跳出
                return;
            }

            string item = jo["skeleton"]["spine"].ToString();

            if (!string.IsNullOrEmpty(item) && item.ToString() != "3.8")
            {
                jo["skeleton"]["spine"] = "3.8";//修改版本为3.8版本
                File.WriteAllText(this.assetPath, jo.ToString());
                AssetDatabase.Refresh();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SpineImportSetting 异常 {e.Message}");
        }
    }
}

