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
        /// 资源信息类
        /// </summary>
        public class ABPackLoader : BaseResLoadModuel
        {
            public ABPackLoader(Dictionary<string, Res> dic_LoadedRes) : base(dic_LoadedRes)
            {
            }
            private GameProjectSettingData settingData;
            /// <summary>
            /// 已加载到的主包
            /// </summary>
            private AssetBundle mainAB = null;
            /// <summary>
            /// 主包依赖获取配置文件
            /// </summary>
            private AssetBundleManifest manifest = null;

            private bool IsFirstLoadABPack = true;
            /// AB资源包存储目录，在游戏目录的地址，
            /// 通过框架总设置进行设置
            /// </summary>
            private string ABPath => settingData.abLoadSetting.ABRuntimeLoadPath;
            /// <summary>
            /// 加载中的AB包的回调
            /// </summary>
            private Dictionary<string, UnityAction> dic_ABPackCallback;
            /// <summary>
            /// 已经加载的AB包
            /// </summary>
            public Dictionary<string, AssetBundle> dic_Bundle = new Dictionary<string, AssetBundle>();
            private TaskProcess currentLoadingProcess = null;
            protected override void initializedLoader()
            {
                IsFirstLoadABPack = true;
                //初始化容器
                dic_ABPackCallback = new Dictionary<string, UnityAction>();
                dic_Bundle = new Dictionary<string, AssetBundle>();
                //加载配置文件
                settingData = SystemSettingLoader.Instance.LoadData<GameProjectSettingData>();
            }
            /// <summary>
            /// 第一次加载AB包
            /// </summary>
            private void FirstLoadABPack()
            {
                //加载主包
                LoadMainBundle();
            }
            //加载主包，只用调用一次即可
            private void LoadMainBundle()
            {
                //得到主包名
                string MainABPackName = settingData.abLoadSetting.ABMainName;
                var loadPath = Path.Combine(ABPath, MainABPackName);
                //路径错误
                if (!File.Exists(loadPath))
                {
                    throw new Exception("无法加载AB包。因为不存在AB包：主包，路径：" + loadPath);
                }
                //加载主包
                mainAB = AssetBundle.LoadFromFile(loadPath);
                //加载依赖包配置
                manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //加载单个AB包
            private IEnumerator LoadSingleABPack(string ABPackName)
            {

                //得到加载请求
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Path.Combine(ABPath, ABPackName));
                float lastLoadingProcess = 0f;
                //没有完成之前需要频繁更新信息
                while (!req.isDone)
                {
                    yield return null;
                    //更新加载进度
                    if (currentLoadingProcess != null)
                    {
                        currentLoadingProcess.UpdateProcess(req.progress - lastLoadingProcess);
                        lastLoadingProcess = req.progress;
                    }
                }
                //防止加载进度卡99.9%
                currentLoadingProcess?.UpdateProcess(1F);
                //加载完成，添加资源
                dic_Bundle[ABPackName] = req.assetBundle;
            }
            /// <summary>
            /// 卸载AB包
            /// </summary>
            /// <param name="abName"></param>
            /// <returns></returns>
            public IEnumerator UnLoadAssetBundle(string abName)
            {
                if (dic_Bundle.ContainsKey(abName))
                {
                    //有资源加载就等到加载结束
                    while (dic_ABPackCallback.ContainsKey(abName))
                    {
                        yield return null;
                    }
                    //再等一帧再卸载
                    yield return null;
                    dic_Bundle[abName].Unload(false);
                    dic_Bundle.Remove(abName);
                    yield break;
                }
                Debug.LogError("资源卸载失败，不存在此AB包");
            }
            //加载AB包
            public IEnumerator LoadABPack(E_ABPackName abPackName, TaskProcess processInfo, UnityAction Callback = null)
            {
                if (IsFirstLoadABPack)
                {
                    FirstLoadABPack();
                }
                var ABPackName = abPackName.ToString();
                //1.已经加载完毕
                if (dic_Bundle.ContainsKey(ABPackName))
                {
                    Callback?.Invoke();
                    yield break;
                }

                //2.已经加载过，但还在加载中
                if (dic_ABPackCallback.ContainsKey(ABPackName))
                {
                    //还在加载，但是还没加载好
                    Debug.Log("正在进行加载" + ABPackName);
                    if (Callback != null)
                        dic_ABPackCallback[ABPackName] += Callback;

                    yield break;
                }
                //3.从来没有加载过
                //重设加载信息
                currentLoadingProcess = processInfo;
                //占位加载
                dic_ABPackCallback[ABPackName] = Callback;
                //获取所有依赖包名
                string[] dependenciesList = manifest.GetAllDependencies(ABPackName.ToString());
                //设置总任务数量：目标AB包1个+依赖包总数
                processInfo.SetTask(dependenciesList.Length + 1);
                // 先加载依赖包 
                foreach (string dependencyName in dependenciesList)
                {
                    if (!dic_Bundle.ContainsKey(dependencyName))
                    {
                        //不包含需要进行加载依赖包
                        yield return UpdateSystem.Instance.StartCoroutine(LoadSingleABPack(dependencyName));
                    }
                    // 加载过此AB包，直接跳过
                    //更新加载进度
                    processInfo.UpdateTaskCount(1);
                }
                //加载自己
                yield return UpdateSystem.Instance.StartCoroutine(LoadSingleABPack(ABPackName.ToString()));
                //完成回调
                if (dic_ABPackCallback.ContainsKey(ABPackName))
                {
                    dic_ABPackCallback[ABPackName]?.Invoke();
                    dic_ABPackCallback.Remove(ABPackName);
                }
                processInfo.UpdateTaskCount(1);
            }
            //从已加载的ab包中得到包中资源
            public Res GetABPackRes<T>(E_ABPackName ABPackName, string resName) where T : UnityEngine.Object
            {
                //得到AB包唯一Key
                string abkey = $"AB_{ABPackName}_{resName}";
                //判断是否已加载，有了直接拿咯
                if (dic_LoadedRes.ContainsKey(abkey))
                {
                    return dic_LoadedRes[abkey];
                }
                //得到包名
                var abName = ABPackName.ToString();
                if (!dic_Bundle.ContainsKey(abName))
                {
                    //加载AB包
                    throw new Exception($"没有完成{ABPackName}的加载，无法获取");
                }
                //得到包中资源
                //采用同步加载，因为是非常快的
                var res = dic_Bundle[abName].LoadAsset<T>(resName);
                if (res == null)
                {
                    throw new Exception($"从{abName}中加载资源{resName}报空，检查资源名和类型是否正确");
                }
                //资源信息
                Res resInfo = new Res(typeof(T));
                resInfo.Asset = res;
                //注册进入字典
                dic_LoadedRes[abkey] = resInfo;
                //完成任务啦
                return resInfo;
            }
            //实践出真知之费时比
            public IEnumerator GetABPackResAsync<T>(E_ABPackName ABPackName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
            {

                //得到AB包唯一Key
                string abkey = $"AB_{ABPackName}_{resName}";
                //判断是否已加载，有了直接拿咯
                if (dic_LoadedRes.ContainsKey(abkey))
                {
                    callback?.Invoke(dic_LoadedRes[abkey].GetAsset<T>());
                }
                //得到包名
                var abName = ABPackName.ToString();
                if (!dic_Bundle.ContainsKey(abName))
                {
                    //加载AB包
                    throw new Exception($"没有完成{ABPackName}的加载，无法获取");
                }
                //得到包中资源
                //采用同步加载，因为是非常快的
                var requset = dic_Bundle[abName].LoadAssetAsync<T>(resName);
                while (!requset.isDone)
                {
                    Debug.Log($"加载包中资源{resName}，进度{requset.progress}");
                    yield return null;
                }
                var res = requset.asset;
                if (res == null)
                {
                    throw new Exception($"从{abName}中加载资源{resName}报空，检查资源名和类型是否正确");
                }
                //资源信息
                Res resInfo = new Res(typeof(T));
                resInfo.Asset = res;
                //注册进入字典
                dic_LoadedRes[abkey] = resInfo;
                callback?.Invoke(dic_LoadedRes[abkey].GetAsset<T>());
            }
        }
    }
}