//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
//namespace WDFramework
//{



//    /// <summary>
//    /// ��������UI���Ĺ�����
//    /// ע�⣺���Ԥ������Ҫ���������һ�£���������
//    /// </summary>
//    public class UIManager : Singleton<UIManager>
//    {

//        //ui�����ؼ�
//        private Camera uiCamera;
//        private Canvas uiCanvas;
//        private EventSystem uiEventSystem;

//        //�㼶������
//        private Transform bottomLayer;
//        private Transform middleLayer;
//        private Transform topLayer;
//        private Transform systemLayer;

//        /// <summary>
//        /// ���ڴ洢���е�������
//        /// </summary>
//        private Dictionary<string, UIBasePanel> panelDic = new Dictionary<string, UIBasePanel>();
//        private GameProjectSettingData SettingData;
//        public UIManager()
//        {
//            IntiManager();
//        }

//        /// <summary>
//        /// ��ʼ����������ʵ����������UI�ؼ�
//        /// </summary>
//        public void IntiManager()
//        {

//        }
//        /// <summary>
//        /// Ԥ�������
//        /// </summary>
//        /// <param name="abname"></param>
//        /// <param name="panels"></param>
//        public void PreLoadUIPanel(string abname, params string[] panelNames)
//        {
//            string[] paths = new string[panelNames.Length];
//            for (int i = 0; i < panelNames.Length; i++)
//            {
//                if (!panelDic.ContainsKey(panelNames[i]))
//                    panelDic.Add(panelNames[i], null);
//                paths[i] = abname + "/" + panelNames[i];

//            }
//            ResLoader.Instance.CreatePreloadTaskFromPaths(paths, (Panels) =>
//            {
//                for (int i = 0; i < Panels.Length; i++)
//                {
//                    GameObject panelObj = Object.Instantiate(Panels[i].Asset as GameObject);
//                    UIBasePanel panelInfo = (panelObj).GetComponent<UIBasePanel>();
//                    panelDic[panelInfo.GetType().Name] = panelInfo;
//                    panelObj.transform.SetParent(middleLayer, false);
//                    panelInfo.HideMe();
//                }
//            });

//        }

//        /// <summary>
//        /// ��ȡ��Ӧ�㼶�ĸ�����
//        /// </summary>
//        /// <param name="layer">�㼶ö��ֵ</param>
//        /// <returns></returns>
//        public Transform GetLayerFather(E_UILayer layer)
//        {
//            switch (layer)
//            {
//                case E_UILayer.Bottom:
//                    return bottomLayer;
//                case E_UILayer.Middle:
//                    return middleLayer;
//                case E_UILayer.Top:
//                    return topLayer;
//                case E_UILayer.System:
//                    return systemLayer;
//                default:
//                    return null;
//            }
//        }

//        /// <summary>
//        /// ��ʾ���
//        /// </summary>
//        /// <param name="layer">�����ʾ�Ĳ㼶</param>
//        public void ShowPanel<T>(E_UILayer layer = E_UILayer.Middle) where T : UIBasePanel
//        {
//            //ͨ���������ȡ��� Ԥ������������������һ�� 
//            T panelInfo = GetPanel<T>();
//            //�����Ԥ���崴������Ӧ�������� ���ұ���ԭ�������Ŵ�С
//            panelInfo.gameObject.transform.SetParent(GetLayerFather(layer), false);
//            //����һ�о��� ���Ҳ���ˣ���ֱ�Ӳ�����
//            //���Ҫ��ʾ��� ��ִ��һ������Ĭ����ʾ�߼�
//            panelInfo.ShowMe();
//        }

       
        

        

//        /// <summary>
//        /// ��ȡ���
//        /// </summary>
//        /// <typeparam name="T">��������</typeparam>




//    }

//}