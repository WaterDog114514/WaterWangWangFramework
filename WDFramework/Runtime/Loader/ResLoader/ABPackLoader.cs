using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace WDFramework
{
    public partial class ResLoader : Singleton<ResLoader>, IFrameworkSystem
    {
        /// <summary>
        /// ��Դ��Ϣ��
        /// </summary>
        public class ABPackLoader : BaseResLoadModuel
        {
            public ABPackLoader(Dictionary<string, Res> dic_LoadedRes) : base(dic_LoadedRes)
            {
            }
            private GameProjectSettingData settingData;
            /// <summary>
            /// �Ѽ��ص�������
            /// </summary>
            private AssetBundle mainAB = null;
            /// <summary>
            /// ����������ȡ�����ļ�
            /// </summary>
            private AssetBundleManifest manifest = null;

            private bool IsFirstLoadABPack = true;
            /// AB��Դ���洢Ŀ¼������ϷĿ¼�ĵ�ַ��
            /// ͨ����������ý�������
            /// </summary>
            private string ABPath => settingData.abLoadSetting.ABRuntimeLoadPath;
            /// <summary>
            /// �����е�AB���Ļص�
            /// </summary>
            private Dictionary<string, UnityAction> dic_ABPackCallback;
            /// <summary>
            /// �Ѿ����ص�AB��
            /// </summary>
            public Dictionary<string, AssetBundle> dic_Bundle = new Dictionary<string, AssetBundle>();
            private TaskProcess currentLoadingProcess = null;
            protected override void initializedLoader()
            {
                IsFirstLoadABPack = true;
                //��ʼ������
                dic_ABPackCallback = new Dictionary<string, UnityAction>();
                dic_Bundle = new Dictionary<string, AssetBundle>();
                //���������ļ�
                settingData = SystemSettingLoader.Instance.LoadData<GameProjectSettingData>();
            }
            /// <summary>
            /// ��һ�μ���AB��
            /// </summary>
            private void FirstLoadABPack()
            {
                //��������
                LoadMainBundle();
            }
            //����������ֻ�õ���һ�μ���
            private void LoadMainBundle()
            {
                //�õ�������
                string MainABPackName = settingData.abLoadSetting.ABMainName;
                var loadPath = Path.Combine(ABPath, MainABPackName);
                //·������
                if (!File.Exists(loadPath))
                {
                    throw new Exception("�޷�����AB������Ϊ������AB����������·����" + loadPath);
                }
                //��������
                mainAB = AssetBundle.LoadFromFile(loadPath);
                //��������������
                manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //���ص���AB��
            private IEnumerator LoadSingleABPack(string ABPackName)
            {

                //�õ���������
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Path.Combine(ABPath, ABPackName));
                float lastLoadingProcess = 0f;
                //û�����֮ǰ��ҪƵ��������Ϣ
                while (!req.isDone)
                {
                    yield return null;
                    //���¼��ؽ���
                    if (currentLoadingProcess != null)
                    {
                        currentLoadingProcess.UpdateProcess(req.progress - lastLoadingProcess);
                        lastLoadingProcess = req.progress;
                    }
                }
                //��ֹ���ؽ��ȿ�99.9%
                currentLoadingProcess?.UpdateProcess(1F);
                //������ɣ������Դ
                dic_Bundle[ABPackName] = req.assetBundle;
            }
            /// <summary>
            /// ж��AB��
            /// </summary>
            /// <param name="abName"></param>
            /// <returns></returns>
            public IEnumerator UnLoadAssetBundle(string abName)
            {
                if (dic_Bundle.ContainsKey(abName))
                {
                    //����Դ���ؾ͵ȵ����ؽ���
                    while (dic_ABPackCallback.ContainsKey(abName))
                    {
                        yield return null;
                    }
                    //�ٵ�һ֡��ж��
                    yield return null;
                    dic_Bundle[abName].Unload(false);
                    dic_Bundle.Remove(abName);
                    yield break;
                }
                Debug.LogError("��Դж��ʧ�ܣ������ڴ�AB��");
            }
            //����AB��
            public IEnumerator LoadABPack(E_ABPackName abPackName, TaskProcess processInfo, UnityAction Callback = null)
            {
                if (IsFirstLoadABPack)
                {
                    FirstLoadABPack();
                }
                var ABPackName = abPackName.ToString();
                //1.�Ѿ��������
                if (dic_Bundle.ContainsKey(ABPackName))
                {
                    Callback?.Invoke();
                    yield break;
                }

                //2.�Ѿ����ع��������ڼ�����
                if (dic_ABPackCallback.ContainsKey(ABPackName))
                {
                    //���ڼ��أ����ǻ�û���غ�
                    Debug.Log("���ڽ��м���" + ABPackName);
                    if (Callback != null)
                        dic_ABPackCallback[ABPackName] += Callback;

                    yield break;
                }
                //3.����û�м��ع�
                //���������Ϣ
                currentLoadingProcess = processInfo;
                //ռλ����
                dic_ABPackCallback[ABPackName] = Callback;
                //��ȡ������������
                string[] dependenciesList = manifest.GetAllDependencies(ABPackName.ToString());
                //����������������Ŀ��AB��1��+����������
                processInfo.SetTask(dependenciesList.Length + 1);
                // �ȼ��������� 
                foreach (string dependencyName in dependenciesList)
                {
                    if (!dic_Bundle.ContainsKey(dependencyName))
                    {
                        //��������Ҫ���м���������
                        yield return UpdateSystem.Instance.StartCoroutine(LoadSingleABPack(dependencyName));
                    }
                    // ���ع���AB����ֱ������
                    //���¼��ؽ���
                    processInfo.UpdateTaskCount(1);
                }
                //�����Լ�
                yield return UpdateSystem.Instance.StartCoroutine(LoadSingleABPack(ABPackName.ToString()));
                //��ɻص�
                if (dic_ABPackCallback.ContainsKey(ABPackName))
                {
                    dic_ABPackCallback[ABPackName]?.Invoke();
                    dic_ABPackCallback.Remove(ABPackName);
                }
                processInfo.UpdateTaskCount(1);
            }
            //���Ѽ��ص�ab���еõ�������Դ
            public Res GetABPackRes<T>(E_ABPackName ABPackName, string resName) where T : UnityEngine.Object
            {
                //�õ�AB��ΨһKey
                string abkey = $"AB_{ABPackName}_{resName}";
                //�ж��Ƿ��Ѽ��أ�����ֱ���ÿ�
                if (dic_LoadedRes.ContainsKey(abkey))
                {
                    return dic_LoadedRes[abkey];
                }
                //�õ�����
                var abName = ABPackName.ToString();
                if (!dic_Bundle.ContainsKey(abName))
                {
                    //����AB��
                    throw new Exception($"û�����{ABPackName}�ļ��أ��޷���ȡ");
                }
                //�õ�������Դ
                //����ͬ�����أ���Ϊ�Ƿǳ����
                var res = dic_Bundle[abName].LoadAsset<T>(resName);
                if (res == null)
                {
                    throw new Exception($"��{abName}�м�����Դ{resName}���գ������Դ���������Ƿ���ȷ");
                }
                //��Դ��Ϣ
                Res resInfo = new Res(typeof(T));
                resInfo.Asset = res;
                //ע������ֵ�
                dic_LoadedRes[abkey] = resInfo;
                //���������
                return resInfo;
            }
            //ʵ������֪֮��ʱ��
            public IEnumerator GetABPackResAsync<T>(E_ABPackName ABPackName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
            {

                //�õ�AB��ΨһKey
                string abkey = $"AB_{ABPackName}_{resName}";
                //�ж��Ƿ��Ѽ��أ�����ֱ���ÿ�
                if (dic_LoadedRes.ContainsKey(abkey))
                {
                    callback?.Invoke(dic_LoadedRes[abkey].GetAsset<T>());
                }
                //�õ�����
                var abName = ABPackName.ToString();
                if (!dic_Bundle.ContainsKey(abName))
                {
                    //����AB��
                    throw new Exception($"û�����{ABPackName}�ļ��أ��޷���ȡ");
                }
                //�õ�������Դ
                //����ͬ�����أ���Ϊ�Ƿǳ����
                var requset = dic_Bundle[abName].LoadAssetAsync<T>(resName);
                while (!requset.isDone)
                {
                    Debug.Log($"���ذ�����Դ{resName}������{requset.progress}");
                    yield return null;
                }
                var res = requset.asset;
                if (res == null)
                {
                    throw new Exception($"��{abName}�м�����Դ{resName}���գ������Դ���������Ƿ���ȷ");
                }
                //��Դ��Ϣ
                Res resInfo = new Res(typeof(T));
                resInfo.Asset = res;
                //ע������ֵ�
                dic_LoadedRes[abkey] = resInfo;
                callback?.Invoke(dic_LoadedRes[abkey].GetAsset<T>());
            }
        }
    }
}