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
    /// �༭�������ļ���ʼ������
    /// </summary>
    public class winDataIntializeHelper : Singleton<winDataIntializeHelper>
    {
        private bool IsChecked = false;
        private const string winDataCountKey = "WindataCount";
        //���е�win����-��Ӧ��洢��·��
        private Dictionary<Type, string> dic_WinRecords = new Dictionary<Type, string>();
        //   public void 

        /// <summary>
        /// ��������ļ�  ��������仯���򷵻�true
        /// </summary>
        public bool StartCheckChange()
        {

            //��һ�μ���
            if (IsChecked)
            {
                return false;
            }
            else IsChecked = true;
            //��ȡ�����д���������
            var winTypes = ReflectionHelper.GetSubclassesOfGenericType(typeof(BaseWindow<,>));
            //��ʼ���ֵ�

            foreach (var type in winTypes)
            {
                string path = Path.Combine(EditorPathHelper.EditorWinDataPath, type.Name + ".windata");
                dic_WinRecords.Add(type, path);
            }
            //����Ƿ��һ�λ����е����Ķ�
            //�������Ķ���������³�ʼ��
            if (CheckChange())
            {
                //��������
                EditorUtility.DisplayDialog("��ܰ��ʾ", "��⵽��Ҫ��ʼ�������ԵȺ��ٿ�������", "�ð�");

                //�״�����Key
                EditorPrefs.SetInt(winDataCountKey, dic_WinRecords.Count);
                //�������ڵ�����
                CreateWinDataAsset();
                return true;
            }
            return false;
        }
        private bool CheckChange()
        {
            if (!EditorPrefs.HasKey(winDataCountKey)) return true;
            //ֻҪ��һ������ȱʧ������Ҫ�ؼ촴��
            foreach (var winType in dic_WinRecords.Keys)
            {
                if (!File.Exists(dic_WinRecords[winType]))
                {
                    return true;
                }
            }
            return false;
        }
        //�����������ݣ����浽����
        private void CreateWinDataAsset()
        {
            foreach (var winType in dic_WinRecords.Keys)
            {
                //��ֹ�ظ�����Ȼ�󸲸�ԭ������
                if (!File.Exists(dic_WinRecords[winType]))
                {
                    //��ȡ����WinData����
                    Type WinDataType = ReflectionHelper.GetGenericArgumentType(winType.BaseType, 1);
                    var newData = Activator.CreateInstance(WinDataType);
                    //��ʼ�����ļ�����
                    (newData as BaseWindowData).IntiFirst();
                    //ȥ���Ա����·��
                    var SavePath = dic_WinRecords[winType];
                        BinaryManager.SaveToPath(newData, SavePath);
                    //�����ļ�
                    Debug.Log(($"�ɹ�����{0}�ļ���{1}", WinDataType.Name, SavePath));
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


    }
}


