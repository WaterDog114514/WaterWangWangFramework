using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    //��һ���֣��������á��Լ�����������߼�
    public partial class winDraw_NodeEditor : BaseWindowDraw<winData_NodeEditor>
    {
        public new win_NodeEditor window => base.window as win_NodeEditor;
        //�����˵�
        public VNEditorDropmenu Dropmenu => window.Dropmenu;
        //�����
        public VNEditorCheck Check => window.Checker;
        // �ڵ���
        public Dictionary<int, VisualNode> dic_Nodes => window.data.dic_Nodes;
        public winDraw_NodeEditor(EditorWindow window, winData_NodeEditor data) : base(window, data)
        {

        }
        //�������λ�úʹ�С
        private Rect LeftPanelRect
        {
            set => data.LeftPanelRect.rect = value;
            get => data.LeftPanelRect.rect;
        }
        //�Ҳ�����λ�úʹ�С
        private Rect RightPanelRect
        {
            set => data.RightPanelRect.rect = value;
            get => data.RightPanelRect.rect;
        }
        /// <summary>
        /// ���ƽڵ�༭������
        /// </summary>
        public override void Draw()
        {
            DrawRightPanel();
            DrawLeftPanel();
            //�ػ�ˢ��
            window.Repaint();
        }
        /// <summary>
        /// ����̫��ӷ�ף�дһ�������
        /// </summary>
        public int CurrentIndex = 0;
        /// <summary>
        /// �Ѿ�ѡ�еĽڵ�
        /// </summary>
        public VisualNode SelectedNode;
        public override void OnCreated()
        {
            IntiPanelSize();
            IntiData();
        }
        private void IntiData()
        {
            if (data.ExistEditorPreset.Count == 0)
            {
                window.LocateAllEditorPreset();
            }
            //��ѡ��༭����option��ֵ
            if (data.ExistEditorPreset.Count > 0)
                EditorOptions = data.ExistEditorPreset.Keys.ToArray();
            //�ڵ��λ��ö��ѡ���ʼ��
            PortPosOption = Enum.GetNames(typeof(VNDrawInfo.E_PortPos));
        }
        //��ʼ������
        //��ʼ������С
        private void IntiPanelSize()
        {
            //ÿ�ζ�Ҫ����һ�¾��ο��
            window.AddUpdateListener(() =>
            {
                //������������Ϊ1/4 λ�þ���(0,0)
                LeftPanelRect = new Rect(0, 0, WindowRect.width / 4, WindowRect.height);
                //�Ҳ����������Ϊ3/4 λ�þ���������֮��
                RightPanelRect = new Rect(LeftPanelRect.width, 0, WindowRect.width * 3 / 4, WindowRect.height);
            });

        }
    }
}