using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace WDFramework
{
    /// <summary>
    /// �༭���м���AB��
    /// </summary>
    internal class main_EditorABLoader
    {
        private GameProjectSettingData settingData;



        //���ڷ�����Ҫ�����AB���е���Դ·�� 
        private string rootPath => settingData.abLoadSetting.ABEditorLoadPath;

        public main_EditorABLoader()
        {
            settingData = SystemSettingLoader.Instance.LoadData<GameProjectSettingData>();
        }

        //1.���ص�����Դ��
        public T LoadEditorRes<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            //��׺�������Լ����ƣ���Ϊ����·�� ��Ҫ��д��׺��һ��
            string suffixName = getSuffixName<T>();
            T res = AssetDatabase.LoadAssetAtPath<T>(rootPath + path + suffixName);
            return res;
#else
        return null;
#endif
        }
        //����� ��ȡĳ·��������Դ
        public Object[] LoadEditorAllRes(string path)
        {
#if UNITY_EDITOR
            //��׺�������Լ����ƣ���Ϊ����·�� ��Ҫ��д��׺��һ��
            //  string suffixName = getSuffixName<T>();
            // T res = AssetDatabase.LoadAllAssetsAtPath<T>(rootPath + path + suffixName);
            return null;
#else
        return null;
#endif
        }
        private string getSuffixName<T>()
        {
            //Ԥ���塢����ͼƬ������������Ч�ȵ�
            if (typeof(T) == typeof(GameObject))
                return ".prefab";
            else if (typeof(T) == typeof(Material))
                return ".mat";
            else if (typeof(T) == typeof(Texture))
                return ".png";
            else if (typeof(T) == typeof(AudioClip))
                return ".mp3";
            return null;
        }
        //2.����ͼ�������Դ��
        public Sprite LoadSprite(string path, string spriteName)
        {
#if UNITY_EDITOR
            //����ͼ���е���������Դ 
            Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
            //������������Դ �õ�ͬ��ͼƬ����
            foreach (var item in sprites)
            {
                if (spriteName == item.name)
                    return item as Sprite;
            }
            return null;
#else
        return null;
#endif
        }

        //����ͼ���ļ��е�������ͼƬ�����ظ��ⲿ
        public Dictionary<string, Sprite> LoadSprites(string path)
        {
#if UNITY_EDITOR
            Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
            Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
            foreach (var item in sprites)
            {
                spriteDic.Add(item.name, item as Sprite);
            }
            return spriteDic;
#else
        return null;
#endif
        }


    }
}