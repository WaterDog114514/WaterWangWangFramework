//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
//namespace WDFramework
//{



//    /// <summary>
//    /// 管理所有UI面板的管理器
//    /// 注意：面板预设体名要和面板类名一致！！！！！
//    /// </summary>
//    public class UIManager : Singleton<UIManager>
//    {

//        //ui基本控件
//        private Camera uiCamera;
//        private Canvas uiCanvas;
//        private EventSystem uiEventSystem;

//        //层级父对象
//        private Transform bottomLayer;
//        private Transform middleLayer;
//        private Transform topLayer;
//        private Transform systemLayer;

//        /// <summary>
//        /// 用于存储所有的面板对象
//        /// </summary>
//        private Dictionary<string, UIBasePanel> panelDic = new Dictionary<string, UIBasePanel>();
//        private GameProjectSettingData SettingData;
//        public UIManager()
//        {
//            IntiManager();
//        }

//        /// <summary>
//        /// 初始化管理器，实例化出基本UI控件
//        /// </summary>
//        public void IntiManager()
//        {

//        }
//        /// <summary>
//        /// 预加载面板
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
//        /// 获取对应层级的父对象
//        /// </summary>
//        /// <param name="layer">层级枚举值</param>
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
//        /// 显示面板
//        /// </summary>
//        /// <param name="layer">面板显示的层级</param>
//        public void ShowPanel<T>(E_UILayer layer = E_UILayer.Middle) where T : UIBasePanel
//        {
//            //通过面板名获取面板 预设体名必须和面板类名一致 
//            T panelInfo = GetPanel<T>();
//            //将面板预设体创建到对应父对象下 并且保持原本的缩放大小
//            panelInfo.gameObject.transform.SetParent(GetLayerFather(layer), false);
//            //所有一切就绪 面板也有了，就直接操作了
//            //如果要显示面板 会执行一次面板的默认显示逻辑
//            panelInfo.ShowMe();
//        }

       
        

        

//        /// <summary>
//        /// 获取面板
//        /// </summary>
//        /// <typeparam name="T">面板的类型</typeparam>




//    }

//}