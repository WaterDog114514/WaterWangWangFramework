//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//namespace WDFramework
//{
//    //Ҫ�ķ���
//    //2.дһ�����ڱ༭�����ܹ��Զ���ʶ������Unity��Prefab��������һ��Excel��Ĭ������
//    //ר�Ÿ���Ԥ����ļ��ع���
//    public class PrefabLoader : Singleton<PrefabLoader>
//    {
//        /// <summary>
//        /// Ԥ����洢������·��-�洢��Ϣ��
//        /// </summary>
//        public Dictionary<string, GameObject> dic_Prefabs = new Dictionary<string, GameObject>();
//        public enum E_LoadPattren
//        {
//            AB,
//            Res
//        }
//        /// <summary>
//        /// ͨ������·���õ�Ԥ���壬����Ѿ������ˣ���ֱ�ӷ���
//        /// </summary>
//        /// <param name="path"></param>
//        /// <param name="loadPattren"></param>
//        /// <returns></returns>
//        public GameObject GetPrefab(string path, E_LoadPattren loadPattren = E_LoadPattren.Res)
//        {
//            //���˾ͷ���
//            if (dic_Prefabs.ContainsKey(path)) return dic_Prefabs[path];
//            return LoadPrefab(path, loadPattren);
//        }
//        //˽����͵͵�ļ���Ԥ����
//        private GameObject LoadPrefab(string path, E_LoadPattren loadPattren = E_LoadPattren.Res)
//        {
//            //û�м���Ԥ���壬�Ǿ��ȼ���
//            GameObject prefab = null;
//            //ֱ��ʹ��ͬ�����أ���Ϊһ�����Ѿ�����Ԥ�����˵ģ�Res��洢���ֵ��������˵��������λ��Ҫ����
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
//                Debug.Log("����Ԥ����ʧ�ܣ��Ҳ�����Ԥ����");
//            }
//            //��Ӽ�¼�󷵻�
//            dic_Prefabs.Add(path, prefab);
//            return prefab;
//        }
//    }
//}
