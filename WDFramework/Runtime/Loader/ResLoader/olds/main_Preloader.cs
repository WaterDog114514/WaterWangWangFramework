//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;


///// <summary>
///// Ԥ���غ���
///// </summary>
//namespace WDFramework
//{
//    /// <summary>
//    /// AB��Ԥ����ģ��
//    /// </summary>
//   internal class main_Preloader
//    {
//        /// <summary>
//        /// ����Ҫ������Դ������
//        /// </summary>
//        public int TotalResNum;
//        /// <summary>
//        /// �Ѽ�����Դ����
//        /// </summary>
//        public int LoadedResNum;
//        /// <summary>
//        /// ���ڼ��ص�������
//        /// </summary>
//        public string CurrentTaskName;
//        public List<PreLoadTask> preloadResTasks = new List<PreLoadTask>();
//        /// <summary>
//        /// ��ʼ����Ԥ����
//        /// </summary>
//        public void StartLoad()
//        {

//            if (preloadResTasks.Count == 0) Debug.LogError("Ԥ��������Ϊ0�������Ԥ�����������ִ��");
//            UpdateSystem.Instance.StartCoroutine(ReallyPreLoadRes());
//        }
//        /// <summary>
//        /// ������м�������
//        /// </summary>
//        private void ClearAllTasks()
//        {
//            //�ͷ����м��ؼ�¼����Ϣ
//            preloadResTasks.Clear();
//            TotalResNum = 0;
//            LoadedResNum = 0;
//            CurrentTaskName = null;
//            TempPath.Clear();
//        }

//        /// <summary>
//        /// Ԥ������Դ һ���Ǽ��س���ʱ����� ֻ��Ԥ������ϲ��ܼ����³���
//        /// </summary>
//        public IEnumerator ReallyPreLoadRes()
//        {
//            //��ͳ������Ҫ������Դ������
//            TotalResNum += preloadResTasks.Count;
//            Coroutine currentCoroutine = null;
//            //�ȸ�����Ԥ��������ֳ�С��֧һ��ͬ��ļ�������
//            for (int j = 0; j < preloadResTasks.Count; j++)
//            {
//                //���ȸ���֮����

//                PreLoadTask preLoadTask = preloadResTasks[j];
//                //�ص���Դ
//                Res[] LoadedRes = new Res[preLoadTask.TaskList.Count];
//                //�ٸ����������е��첽�������
//                for (int i = 0; i < preLoadTask.TaskList.Count; i++)
//                {

//                    AsyncLoadTask task = preLoadTask.TaskList[i];
//                    //������ɲ��ؼ���
//                    if (task.isFinish) continue;
//                    currentCoroutine = task.StartAsyncLoad();
//                    yield return currentCoroutine;
//                    //��¼���غ�Ļص���Դ
//                    LoadedRes[i] = task.ResInfo;
//                    //����Դ�������
//                    LoadedResNum++;
//                    //�����������߼�С����ȣ�ʹ���¼�����
//                    Debug.Log($"���ؽ���{LoadedResNum}/{TotalResNum}");
//                }
//                //������һ��Ԥ���ش�����ͻ����һ��
//                preLoadTask.callback?.Invoke(LoadedRes);
//            }



//            //������ϣ������������
//            ClearAllTasks();
//            //�ص�һ��Ԥ���غõ���ԴŶ
//        }
//        /// <summary>
//        /// ����Ԥ��������
//        /// </summary>
//        public void CreatePreLoadTask(PreLoadTask task)
//        {
//            preloadResTasks.Add(task);
//        }
//        //��ʱ�Ե�·���洢����ֹ�ظ�����
//        private List<string> TempPath = new List<string>();
//        /// <summary>
//        /// ����AB����Ԥ��������
//        /// </summary>
//        /// <param name="paths">AB��������Դ��</param>
//        /// <param name="callback"></param>
//        public void CreatePreloadTaskFromPaths((E_ABPackName, string)[] paths, UnityAction<Res[]> callback = null)
//        {
//            PreLoadTask preLoadTask = new PreLoadTask();
//            foreach (var path in paths)
//            {
//                // ���������ʹ��nameFieldValue
//                    string abName = path.Item1.ToString();
//                    string resName = path.Item2;
//                    AsyncLoadTask task = ResLoader.Instance.CreateAB_Async<UnityEngine.Object>((E_ABPackName)Enum.Parse(typeof(E_ABPackName), abName), resName, null);
//                    preLoadTask.TaskList.Add(task);

//            }
//            //�ص�
//            preLoadTask.callback = callback;
//            CreatePreLoadTask(preLoadTask);

//        }
//    }
//   internal class PreLoadTask
//    {
//        public UnityAction<Res[]> callback;
//        public List<AsyncLoadTask> TaskList = new List<AsyncLoadTask>();
//    }
//}


