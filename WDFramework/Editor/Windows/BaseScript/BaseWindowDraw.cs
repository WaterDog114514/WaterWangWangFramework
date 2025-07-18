
using System.IO;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    //���ڻ�����ӿ�
    public abstract class BaseWindowDraw<Data> : IWinDraw where Data : BaseWindowData
    {
        public BaseWindowDraw(EditorWindow window, Data data)
        {
            this.window = window;
            this.data = data;
            //��ʼ������
            IntiTitle();
            //��ʼ��Windraw
            OnCreated();
        }
        //Ҫ���Ƶ�����
        protected Data data;
        protected EditorWindow window;
        //���ڿ�ߺ�λ����Ϣ
        protected Rect WindowRect => window.position;
        //��ʼ������
        private void IntiTitle()
        {
            //��ֹδ��������
            string title = string.IsNullOrEmpty(data.Title)? "û�������ı༭��" : data.Title;
            window.titleContent = new GUIContent(title);
            //��ʼ����С
            window.position = new Rect(window.position.position,data.currentWindowSize.vector2);
        }
        //���Ʊ���ͼ
        private void DrawBG()
        {
            //��ȡ��ǰ���ڿ���ٻ���
            data.currentWindowSize.vector2 = new Vector2(WindowRect.width, WindowRect.height);
            //GUI.DrawTexture(new Rect(Vector2.zero, data.currentWindowSize.vector2),data.BackgroundTexture.texture, ScaleMode.StretchToFill);
        }
        /// <summary>
        /// ���ƿؼ�
        /// </summary>
        public abstract void Draw();
        /// <summary>
        /// ���ƴ����ܻ����߼�
        /// </summary>
        public void DrawWindows()
        {
             DrawBG();
            Draw();
        }
        /// <summary>
        /// ������д����ʼ��������ʱ�����
        /// </summary>
        public abstract void OnCreated();
    }
}
