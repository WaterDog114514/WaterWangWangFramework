using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;


public enum E_LoadModel
{
    Async,
    Sync
}
namespace WDFramework
{
    /// <summary>
    /// ������Դģʽ����ͨ���첽����ͬ�����м���
    /// </summary>

    internal class main_ABLoader
    {
        private GameProjectSettingData settingData;
        public Dictionary<string, AssetBundle> dic_Bundle = new Dictionary<string, AssetBundle>();
        /// <summary>
        /// ������������ ab ����ȡ
        /// </summary>
        private string MainName => settingData.abLoadSetting.ABMainName;
        /// <summary>
        /// �Ѽ��ص�������
        /// </summary>
        private AssetBundle mainAB = null;
        /// <summary>
        /// ����������ȡ�����ļ�
        /// </summary>
        private AssetBundleManifest manifest = null;
        /// <summary>
        /// AB��Դ���洢Ŀ¼������ϷĿ¼�ĵ�ַ��ͨ����������ý�������
        /// </summary>
        private string ABPath => settingData.abLoadSetting.ABRuntimeLoadPath;
        public main_ABLoader()
        {
            settingData = SystemSettingLoader.Instance.LoadData<GameProjectSettingData>();
            LoadMainBundle();
        }
        //����������ֻ�õ���һ�μ���
        private void LoadMainBundle()
        {
            if (mainAB == null)
            {
                mainAB = AssetBundle.LoadFromFile(Path.Combine(ABPath, MainName));
                manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
        }
        /// <summary>
        /// ͬ������������
        /// </summary>
        /// <param name="abName"></param>
        private void LoadDependenciesAsset_Sync(string abName)
        {
            if (mainAB == null) LoadMainBundle();
            string[] dependencies = manifest.GetAllDependencies(abName);
            //�������е���������
            foreach (string dependencyName in dependencies)
            {
                //���ع�����������ˣ�����������һ��
                if (dic_Bundle.ContainsKey(dependencyName)) continue;
                //ͬ������������
                AssetBundle ab = AssetBundle.LoadFromFile(Path.Combine(ABPath, dependencyName));
                dic_Bundle.Add(dependencyName, ab);
            }

        }

        /// <summary>
        /// �첽����������
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private IEnumerator LoadDependenciesAsset_Async(string abName)
        {
            if (mainAB == null) LoadMainBundle();
            string[] dependencies = manifest.GetAllDependencies(abName);

            //�������е���������
            foreach (string dependencyName in dependencies)
            {
                //���ع�����������ˣ�����������һ��
                if (dic_Bundle.ContainsKey(dependencyName))
                {
                    //ͬʱ����ʱ�򣬷�ֹ��Null
                    while (dic_Bundle[dependencyName] == null)
                        yield return null;

                    continue;
                }
                //һ��ʼ�첽���� �ͼ�¼ �����ʱ�ļ�¼�е�ֵ ��null ��֤�����ab�����ڱ��첽����
                dic_Bundle.Add(dependencyName, null);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Path.Combine(ABPath, dependencyName));
                yield return req;
                //�첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
                dic_Bundle[dependencyName] = req.assetBundle;

            }
        }

        //����ͬ������ ���һ
        public T ReallyLoadSync<T>(string abName, string resName) where T : UnityEngine.Object
        {
            //����������
            LoadDependenciesAsset_Sync(abName);
            //�ж��Ƿ������AB��
            AssetBundle ab = null;
            if (!dic_Bundle.ContainsKey(abName))
            {
                //����AB��
                ab = AssetBundle.LoadFromFile(Path.Combine(ABPath, abName));
                dic_Bundle.Add(abName, ab);
            }

            else
            {
                ab = dic_Bundle[abName];
            }
            //���ذ�����Դ
            Res resInfo = new Res(typeof(T));
            string[] aa = ab.GetAllAssetNames();
            //��������߼�
            T res = ab.LoadAsset<T>(resName);
            resInfo.Asset = res;
            //���������
            return res;
        }
        /// <summary>
        /// �������첽���� ����һ
        /// </summary>
        /// <typeparam name="T">���غõ���Դ</typeparam>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="callback">���ػص�</param>
        /// <returns></returns>
        public IEnumerator ReallyLoadAsync<T>(string abName, string resName, AsyncLoadTask task) where T : UnityEngine.Object
        {
            Res resInfo = null;
            //�첽���أ��ȵ��첽������������Ϻ��ټ���
            yield return UpdateSystem.Instance.StartCoroutine(LoadDependenciesAsset_Async(abName));

            //��һ�μ���ĳAB����
            if (!dic_Bundle.ContainsKey(abName))
            {
                //��һ�ν����첽���أ��ȸ�dic�ֿռ�
                dic_Bundle.Add(abName, null);
                //����AB���е���Դ�߼�
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Path.Combine(ABPath, abName));
                yield return req;
                //�첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
                dic_Bundle[abName] = req.assetBundle;
            }
            //ͬʱ����ʱ�򣬷�ֹ��Null
            while (dic_Bundle[abName] == null)
                yield return null;
            //ɵ���������������
            AssetBundleRequest abq = dic_Bundle[abName].LoadAssetAsync<T>(resName);
            resInfo = new Res(typeof(T));
            //���ؽ��ȸ����߼�
            while (!abq.isDone)
            {
                task.LoadProcess = abq.progress;
                yield return null;
            }

            //��ɼ���
            yield return abq;
            resInfo.Asset = abq.asset as T;
            task.FinishTask(resInfo);
            //��������͵���

        }

        /// <summary>
        /// ж��ָ��AB��
        /// </summary>
        public void UnLoadAssetBundle(string abName)
        {
            UpdateSystem.Instance.StartCoroutine(ReallyUnLoadAssetBundle(abName));
        }
        /// <param name="abName"></param>
        private IEnumerator ReallyUnLoadAssetBundle(string abName)
        {
            if (dic_Bundle.ContainsKey(abName))
            {
                //����Դ���ؾ͵ȵ����ؽ���
                while (dic_Bundle[abName] == null)
                {
                    yield return null;
                }
                dic_Bundle[abName].Unload(false);
                dic_Bundle.Remove(abName);
                yield break;
            }
            Debug.LogError("��Դж��ʧ�ܣ������ڴ�AB��");
        }

        /// <summary>
        /// Ԥ����ĳ��Դ�������е���Դ��
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public IEnumerator getABAllResName(string abName, UnityAction<string[]> callback)
        {
            if (!dic_Bundle.ContainsKey(abName))
            {
                //��һ�ν����첽���أ��ȸ�dic�ֿռ�
                dic_Bundle.Add(abName, null);
                //����AB���е���Դ�߼�
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Path.Combine(ABPath, abName));
                yield return req;
                //�첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
                dic_Bundle[abName] = req.assetBundle;
            }
            callback(dic_Bundle[abName].GetAllAssetNames());

        }



    }
}


/// <summary>
/// Ԥ���ض��� ����һ��AB��������Դ
/// </summary>
/// <typeparam name = "T" ></ typeparam >
/// < param name="abName"></param>
/// <returns></returns>
//public IEnumerator ReallyLoadAllAssetAsync<T>(string abName, AsyncLoadTask task, UnityAction<AsyncLoadTask[]> callback) where T : UnityEngine.Object
//{
//    �첽���أ��ȵ��첽������������Ϻ��ټ���
//    yield return UpdateSystem.Instance.StartCoroutine(LoadDependenciesAsset_Async(abName));

//    if (!dic_Bundle.ContainsKey(abName))
//    {
//        ��һ�ν����첽���أ��ȸ�dic�ֿռ�
//        dic_Bundle.Add(abName, null);
//        ����AB���е���Դ�߼�
//        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(ABPath + abName);
//        yield return req;
//        �첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
//        dic_Bundle[abName] = req.assetBundle;
//    }
//    ���ذ���������Դ


//    AssetBundleRequest abq = dic_Bundle[abName].LoadAllAssetsAsync<T>();
//    ���ؽ��ȸ����߼�
//    while (!abq.isDone)
//    {
//        task.LoadProcess = abq.progress;
//        Debug.Log(abq.progress);
//        yield return null;
//    }
//    yield return abq;

//    T[] loadRes = new T[abq.allAssets.Length];
//    �Ȱ���Դװ��ȥ��
//    for (int i = 0; i < loadRes.Length; i++)
//    {
//        loadRes[i] = abq.allAssets[i] as T;
//    }

//    �����м��غõ���Դ������������洢���ⲿ
//    AsyncLoadTask[] tasks = new AsyncLoadTask[loadRes.Length];


//    for (int i = 0; i < tasks.Length; i++)
//    {
//        Res<T> resInfo = new Res<T>();
//        resInfo.asset = loadRes[i];
//        tasks[i] = new AsyncLoadTask();
//        ������ִ�����
//        tasks[i].FinishTask(resInfo);
//    }

//    ��������ɲ���
//    Res<T[]> TotalInfo = new Res<T[]>();
//    TotalInfo.asset = loadRes;
//    task.FinishTask(TotalInfo);
//    ��Ҫ������
//    ��Ҫ��д������
//     ResInfo�Խӣ���ȡ���ñ���ز���
//    ��Ҫ������
//    ��Ҫ������

//    ����ص����������װ��dic����
//    callback(tasks);
//}