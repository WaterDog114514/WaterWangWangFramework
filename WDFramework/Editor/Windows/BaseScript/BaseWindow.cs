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
    /// ˮ�����༭�����ڻ��� �������淶������ֱ�Ӽ̳�EditorWindow
    /// </summary>
    public abstract class WDWindow : EditorWindow { }
    /// <summary>
    /// ˮ�����༭���ڻ���  
    /// �����ߺ����ݵ�����
    /// </summary>
    public abstract class BaseWindow<WinDraw, Data> : WDWindow
        where Data : BaseWindowData
        where WinDraw : BaseWindowDraw<Data>
    {
        //���ڻ�����
        public WinDraw winDraw;
        //����������
        public Data data;
        //��ʱ�洢��data��ַ��Ϊ�˵���Save��
        private string DataPath;
        public bool isEditorListeningUpdate = true;
        //ÿ�λ��ƵĻ����߼�
        private void OnGUI()
        {

            //���ƴ���
            winDraw.DrawWindows();
            //�ͷ��ȹ��������������,������̸����
            if (Event.current.type == EventType.KeyDown || Event.current.type == EventType.MouseDown)
            {
                GUIUtility.hotControl = 0;
            }

        }
        //���������ļ������μ��ز����ڵĻ����򴴽�
        public void CreateWinDraw()
        {
            winDraw = (WinDraw)Activator.CreateInstance(typeof(WinDraw), this, data);
        }
        /// <summary>
        /// �˳�ʱ����Զ�����
        /// </summary>
        private void SaveData()
        {
            BinaryManager.SaveToPath(data, DataPath);
            AssetDatabase.Refresh();
        }
        private void LoadData()
        {

            //��ȡ����·��
            DataPath = Path.Combine(EditorPathHelper.EditorWinDataPath, GetType().Name + ".windata");
            //���� 
            if (File.Exists(DataPath))
            {
                data = BinaryManager.Load<Data>(DataPath);
                //��ʼ����������
                data.IntiLoad();
            }
            else
            {
                Debug.LogError($"����ʧ�ܣ�������{GetType().Name}�Ĵ�������");
            }
        }
        //  ��������ִ�з���
        private void OnEnable()
        {
            //�ȼ�� ����ʼ�������ļ� �������true��������Ҫ��ʼ����ֹͣ���в���
            if (winDataIntializeHelper.Instance.StartCheckChange())
            {
                Debug.Log("���ڳ�ʼ������Ҫ��������");
                // �ӳٹرմ��ڣ�ȷ��������ȫ���غ���ִ��
                EditorApplication.delayCall += () => Close();
                //���±������
                return;
            }

            //��������
            LoadData();
            //����data
            //�������ƹ���
            CreateWinDraw();
            //����������д
            OnOpenWindow();

        }
        //�رմ���ִ�з���
        private void OnDestroy()
        {
            OnCloseWindow();
            //�Զ�����
            SaveData();  
        }
        /// <summary>
        /// �򿪴���ʱ�򴥷� ������д��
        /// </summary>
        public virtual void OnOpenWindow()
        {

        }
        /// <summary>
        /// �رմ���ʱ�򴥷� ������д��
        /// </summary>
        public virtual void OnCloseWindow()
        {
            isEditorListeningUpdate = false;
        }
        /// <summary>
        /// ��window���update�������ɹ����뵽EditorAppliaction
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