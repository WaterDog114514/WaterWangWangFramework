//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//namespace WDFramework
//{
//    //要改方向：
//    //2.写一个窗口编辑器，能够自动的识别所有Unity的Prefab，并生成一个Excel，默认设置
//    //专门负责预制体的加载工作
//    public class PrefabLoader : Singleton<PrefabLoader>
//    {
//        /// <summary>
//        /// 预制体存储（加载路径-存储信息）
//        /// </summary>
//        public Dictionary<string, GameObject> dic_Prefabs = new Dictionary<string, GameObject>();
//        public enum E_LoadPattren
//        {
//            AB,
//            Res
//        }
//        /// <summary>
//        /// 通过加载路径得到预制体，如果已经加载了，就直接返回
//        /// </summary>
//        /// <param name="path"></param>
//        /// <param name="loadPattren"></param>
//        /// <returns></returns>
//        public GameObject GetPrefab(string path, E_LoadPattren loadPattren = E_LoadPattren.Res)
//        {
//            //有了就返回
//            if (dic_Prefabs.ContainsKey(path)) return dic_Prefabs[path];
//            return LoadPrefab(path, loadPattren);
//        }
//        //私底下偷偷的加载预制体
//        private GameObject LoadPrefab(string path, E_LoadPattren loadPattren = E_LoadPattren.Res)
//        {
//            //没有加载预制体，那就先加载
//            GameObject prefab = null;
//            //直接使用同步加载，因为一定是已经进行预加载了的，Res会存储到字典里，不进行说明做不到位需要加载
//            switch (loadPattren)
//            {
//                case E_LoadPattren.AB:
//                    prefab = ResLoader.Instance.LoadAB_Sync<GameObject>(E_ABPackName.prefab, path);
//                    break;
//                case E_LoadPattren.Res:
//                    prefab = ResLoader.Instance.LoadRes_Sync<GameObject>(path);
//                    break;

//            }
//            if (prefab == null)
//            {
//                Debug.Log("加载预制体失败，找不到此预制体");
//            }
//            //添加记录后返回
//            dic_Prefabs.Add(path, prefab);
//            return prefab;
//        }
//    }
//}
