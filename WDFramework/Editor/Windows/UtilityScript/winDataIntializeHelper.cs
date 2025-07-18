using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace WDEditor
{
    /// <summary>
    /// 编辑器配置文件初始化助手
    /// </summary>
    public class winDataIntializeHelper : Singleton<winDataIntializeHelper>
    {
        private bool IsChecked = false;
        private const string winDataCountKey = "WindataCount";
        //所有的win类型-对应其存储的路径
        private Dictionary<Type, string> dic_WinRecords = new Dictionary<Type, string>();
        //   public void 

        /// <summary>
        /// 检测所有文件  如果发生变化，则返回true
        /// </summary>
        public bool StartCheckChange()
        {

            //第一次检验
            if (IsChecked)
            {
                return false;
            }
            else IsChecked = true;
            //获取到所有窗口子类型
            var winTypes = ReflectionHelper.GetSubclassesOfGenericType(typeof(BaseWindow<,>));
            //初始化字典

            foreach (var type in winTypes)
            {
                string path = Path.Combine(EditorPathHelper.EditorWinDataPath, type.Name + ".windata");
                dic_WinRecords.Add(type, path);
            }
            //检测是否第一次或者有迭代改动
            //若发生改动则进行重新初始化
            if (CheckChange())
            {
                //弹窗警告
                EditorUtility.DisplayDialog("温馨提示", "检测到需要初始化，请稍等后再开启窗口", "好吧");

                //首次设置Key
                EditorPrefs.SetInt(winDataCountKey, dic_WinRecords.Count);
                //创建窗口的数据
                CreateWinDataAsset();
                return true;
            }
            return false;
        }
        private bool CheckChange()
        {
            if (!EditorPrefs.HasKey(winDataCountKey)) return true;
            //只要有一个数据缺失，就需要重检创建
            foreach (var winType in dic_WinRecords.Keys)
            {
                if (!File.Exists(dic_WinRecords[winType]))
                {
                    return true;
                }
            }
            return false;
        }
        //创建窗口数据，保存到本地
        private void CreateWinDataAsset()
        {
            foreach (var winType in dic_WinRecords.Keys)
            {
                //防止重复创建然后覆盖原有配置
                if (!File.Exists(dic_WinRecords[winType]))
                {
                    //获取它的WinData类型
                    Type WinDataType = ReflectionHelper.GetGenericArgumentType(winType.BaseType, 1);
                    var newData = Activator.CreateInstance(WinDataType);
                    //初始化其文件数据
                    (newData as BaseWindowData).IntiFirst();
                    //去绝对变相对路径
                    var SavePath = dic_WinRecords[winType];
                        BinaryManager.SaveToPath(newData, SavePath);
                    //创建文件
                    Debug.Log(($"成功创建{0}文件在{1}", WinDataType.Name, SavePath));
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


    }
}


