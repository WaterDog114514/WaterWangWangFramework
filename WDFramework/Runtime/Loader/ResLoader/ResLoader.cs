using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace WDFramework
{
    /// <summary>
    /// ��Դ���صĺ����࣬��װ�˸�����ģ��ļ���
    /// </summary>
    public partial class ResLoader : Singleton<ResLoader>, IFrameworkSystem
    {
        //������ģ��
        private ResourcesLoader resourcesLoader;
        private ABPackLoader abLoader;
        //�������������ļ�
        /// <summary>
        /// �Ѿ����ع�����Դ
        /// </summary>
        private Dictionary<string, Res> dic_LoadedRes = new Dictionary<string, Res>();
        public void InitializedSystem()
        {
            //��ʼ�����бر�������ģ��
            resourcesLoader = new ResourcesLoader(dic_LoadedRes);
            abLoader = new ABPackLoader(dic_LoadedRes);
        }

        /// <summary>
        /// ����AB��
        /// </summary>
        /// <returns>������ȣ�ʵʱ����</returns>
        public TaskProcess LoadABPack(E_ABPackName ABPackName, UnityAction callback = null)
        {
            //�������ؽ���
            var taskProcess = TaskProcess.CreateTaskProcess(-1);
            UpdateSystem.Instance.StartCoroutine(abLoader.LoadABPack(ABPackName, taskProcess, callback));
            return taskProcess;
        }
        //���Ѽ��ص�ab���еõ�������Դ
        public T GetABPackRes<T>(E_ABPackName ABPackName, string resName) where T : UnityEngine.Object
        {
            var res = abLoader.GetABPackRes<T>(ABPackName, resName);
            return res.GetAsset<T>();
        }
        //���Ѽ��ص�ab���еõ�������Դ-�첽
        public void GetABPackResAsync<T>(E_ABPackName ABPackName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            UpdateSystem.Instance.StartCoroutine(abLoader.GetABPackResAsync<T>(ABPackName, resName, callback));
        }
        /// <summary>
        /// ж��AB��
        /// </summary>
        public void DeleteABPack(E_ABPackName ABPackName)
        {
            UpdateSystem.Instance.StartCoroutine(abLoader.UnLoadAssetBundle(ABPackName.ToString()));
        }
        public void LoadResourcesAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
            resourcesLoader.LoadResourcesAsync<T>(path, callback);

        }
        /// <summary>
        /// ͬ�����ټ��������ļ��ȣ�ֱ�����
        /// </summary>
        public T LoadResourcesSync<T>(string path) where T : UnityEngine.Object
        {
            return resourcesLoader.LoadResourcesSync<T>(path);
        }
    }
}

