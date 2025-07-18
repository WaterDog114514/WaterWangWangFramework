using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.NVIDIA;
using UnityEngine.Rendering.VirtualTexturing;
namespace WDEditor
{
    /// <summary>
    /// 水汪汪编辑器窗口基类 ，用来规范，避免直接继承EditorWindow
    /// </summary>
    public abstract class WDWindow : EditorWindow { }
    /// <summary>
    /// 水汪汪编辑窗口基类  
    /// 绘制者和数据的载体
    /// </summary>
    public abstract class BaseWindow<WinDraw, Data> : WDWindow
        where Data : BaseWindowData
        where WinDraw : BaseWindowDraw<Data>
    {
        //窗口绘制者
        public WinDraw winDraw;
        //窗口数据类
        public Data data;
        //暂时存储的data地址，为了的是Save用
        private string DataPath;
        public bool isEditorListeningUpdate = true;
        //每次绘制的基本逻辑
        private void OnGUI()
        {

            //绘制窗口
            winDraw.DrawWindows();
            //释放热狗，避免控制老子,老生常谈错误
            if (Event.current.type == EventType.KeyDown || Event.current.type == EventType.MouseDown)
            {
                GUIUtility.hotControl = 0;
            }

        }
        //加载数据文件，初次加载不存在的话，则创建
        public void CreateWinDraw()
        {
            winDraw = (WinDraw)Activator.CreateInstance(typeof(WinDraw), this, data);
        }
        /// <summary>
        /// 退出时候会自动保存
        /// </summary>
        private void SaveData()
        {
            BinaryManager.SaveToPath(data, DataPath);
            AssetDatabase.Refresh();
        }
        private void LoadData()
        {

            //获取加载路径
            DataPath = Path.Combine(EditorPathHelper.EditorWinDataPath, GetType().Name + ".windata");
            //存在 
            if (File.Exists(DataPath))
            {
                data = BinaryManager.Load<Data>(DataPath);
                //初始化窗口数据
                data.IntiLoad();
            }
            else
            {
                Debug.LogError($"加载失败，不存在{GetType().Name}的窗口数据");
            }
        }
        //  启动窗口执行方法
        private void OnEnable()
        {
            //先检测 并初始化数据文件 如果返回true，表明需要初始化，停止所有操作
            if (winDataIntializeHelper.Instance.StartCheckChange())
            {
                Debug.Log("正在初始化，需要重启窗口");
                // 延迟关闭窗口，确保窗口完全加载后再执行
                EditorApplication.delayCall += () => Close();
                //重新编译代码
                return;
            }

            //加载数据
            LoadData();
            //测试data
            //创建绘制工具
            CreateWinDraw();
            //调用子类重写
            OnOpenWindow();

        }
        //关闭窗口执行方法
        private void OnDestroy()
        {
            OnCloseWindow();
            //自动保存
            SaveData();  
        }
        /// <summary>
        /// 打开窗口时候触发 子类重写！
        /// </summary>
        public virtual void OnOpenWindow()
        {

        }
        /// <summary>
        /// 关闭窗口时候触发 子类重写！
        /// </summary>
        public virtual void OnCloseWindow()
        {
            isEditorListeningUpdate = false;
        }
        /// <summary>
        /// 给window添加update监听，成功加入到EditorAppliaction
        /// </summary>
        /// <param name="action"></param>
        public void AddUpdateListener(UnityAction action)
        {
            EditorApplication.update += () =>
            {
                if (isEditorListeningUpdate) action?.Invoke();
            };
        }

    }


}