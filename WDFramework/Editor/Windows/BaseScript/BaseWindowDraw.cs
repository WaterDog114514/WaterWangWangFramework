
using System.IO;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    //窗口绘制类接口
    public abstract class BaseWindowDraw<Data> : IWinDraw where Data : BaseWindowData
    {
        public BaseWindowDraw(EditorWindow window, Data data)
        {
            this.window = window;
            this.data = data;
            //初始化标题
            IntiTitle();
            //初始化Windraw
            OnCreated();
        }
        //要绘制的数据
        protected Data data;
        protected EditorWindow window;
        //窗口宽高和位置信息
        protected Rect WindowRect => window.position;
        //初始化标题
        private void IntiTitle()
        {
            //防止未命名报空
            string title = string.IsNullOrEmpty(data.Title)? "没有命名的编辑器" : data.Title;
            window.titleContent = new GUIContent(title);
            //初始化大小
            window.position = new Rect(window.position.position,data.currentWindowSize.vector2);
        }
        //绘制背景图
        private void DrawBG()
        {
            //获取当前窗口宽高再绘制
            data.currentWindowSize.vector2 = new Vector2(WindowRect.width, WindowRect.height);
            //GUI.DrawTexture(new Rect(Vector2.zero, data.currentWindowSize.vector2),data.BackgroundTexture.texture, ScaleMode.StretchToFill);
        }
        /// <summary>
        /// 绘制控件
        /// </summary>
        public abstract void Draw();
        /// <summary>
        /// 绘制窗口总绘制逻辑
        /// </summary>
        public void DrawWindows()
        {
             DrawBG();
            Draw();
        }
        /// <summary>
        /// 子类重写，初始化创建的时候调用
        /// </summary>
        public abstract void OnCreated();
    }
}
