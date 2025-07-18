using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    //�ڶ����֣�ר�Ż������
    public partial class winDraw_NodeEditor : BaseWindowDraw<winData_NodeEditor>
    {
        private VNEditorCheck Checker => window.Checker;
        //�������ƶ�λ��
        private DrawIndexHelper LeftPanelDrawHelper = new DrawIndexHelper();
        //����������
        public void DrawLeftPanel()
        {
            //���»����߼�����
            LeftPanelDrawHelper.Update(LeftPanelRect);
            //���Ʊ���ͼ
            GUI.DrawTexture(LeftPanelRect, data.LeftPanelBG.texture);
            //���Ʊ߽���
            Handles.DrawSolidRectangleWithOutline(LeftPanelRect, Color.blue, Color.cyan);
            //���Ʊ���ͼ
            GUI.DrawTexture(LeftPanelRect, data.LeftPanelBG.texture);
            //����Ƿ�����˱༭��Ԥ��
            if (data.editorPreset == null)
            {
                DrawLeftPanel_NoPreset();
                return;
            }
            DrawLeftPanel_HavePreset();
        }
        private string[] EditorOptions = new string[0];
        private int EditorOptionsIndex = 0;
        //��û�м��ر༭��Ԥ��ʱ�򣬻����������߼�
        private void DrawLeftPanel_NoPreset()
        {
            //����һ���༭ҳ��
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "ѡ��һ���༭����������ҳ�棺");
            LeftPanelDrawHelper.BeginHorizontalLayout(2, 1.5F);
            EditorOptionsIndex = EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(), EditorOptionsIndex, EditorOptions);
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "ˢ���б�"))
                window.LocateAllEditorPreset();
            LeftPanelDrawHelper.EndHorizontalLayout();
            //��ť����������ҳ��
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "������ҳ��"))
                window.OpenEditorFromCreateNewPage(EditorOptions[EditorOptionsIndex]);
            //��ť�������ƴ򿪱༭ҳ��
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "��һ�����б༭ҳ��"))
                window.OpenEditorFromOpenFilePanel(); ;
        }
        //�����Ѿ�����Ԥ��ʱ��������
        private void DrawLeftPanel_HavePreset()
        {
            //���Ʊ༭��Ԥ����ʾ
            LeftPanelDrawHelper.BeginHorizontalLayout(2, 1.5F);
            GUITextScaler.DrawScaledLabel(LeftPanelDrawHelper.GetNextSingleRect(), "��ǰ�༭����");
            GUI.enabled = false;
            EditorGUI.TextField(LeftPanelDrawHelper.GetNextSingleRect(), data.editorPreset.EditorName);
            GUI.enabled = true;

            LeftPanelDrawHelper.EndHorizontalLayout();
            //��ť����������ͼ���� ������ͼ�߼�
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "������ͼ"))
                window.ResetView();

            //��ť�������ƽڵ㱣��
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "����༭ҳ��"))
                window.SaveEditorPage(false);
            //���
            LeftPanelDrawHelper.GetNextSingleRect(0.5F);
            //��ť�������Ϊ
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "���Ϊ�༭ҳ��"))
                window.SaveEditorPage(true);
            //���
            LeftPanelDrawHelper.GetNextSingleRect(0.5F);
            //���л���ť�߼�
            if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "���л�ΪRuntime����"))
                window.SerializeRuntimeData();

            //���ƽڵ���Ϣ
            DrawNodeInfo();

        }

        //��ǰ��ʱ��ѡ��ڵ�
        private VisualNode TempSelectedNode;
        //���ӽڵ㣺
        private List<VisualNode> TempEnterNodes = new List<VisualNode>();
        private List<VisualNode> TempExitNodes = new List<VisualNode>();
        private string[] PortPosOption;
        //������ڵ���Ϣ����
        public void DrawNodeInfo()
        {
            //�������ѡ����ĳ�ڵ㣬����ֱ����ҳ��༭
            if (TempSelectedNode != window.Checker.selectedNode)
            {
                //ˢ��ѡ�нڵ��б�
                if (window.Checker.selectedNode != null)
                {
                    //������ʱ���Ƶ�������Ϣ
                    TempEnterNodes = window.Checker.selectedNode.EnterNodes;
                    TempExitNodes = window.Checker.selectedNode.ExitNodes;
                }
            }
            TempSelectedNode = window.Checker.selectedNode;

            //δѡ��ʱ�򲻻����
            if (TempSelectedNode == null) return;
            //�ȵõ��ڵ�
            TempSelectedNode = window.Checker.selectedNode;
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "��ǰѡ��ڵ㣺" + TempSelectedNode.NodeName);
            //��������
            TempSelectedNode.drawInfo.Position = EditorGUI.Vector2Field(LeftPanelDrawHelper.GetNextSingleRect(1.5F), "���꣺", TempSelectedNode.drawInfo.Position);
             //���
            LeftPanelDrawHelper.GetNextSingleRect(1.25F);

            LeftPanelDrawHelper.BeginHorizontalLayout(2,2);
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "����λ�����ã�");
            //��������
            TempSelectedNode.drawInfo.EnterPortPos = 
                (VNDrawInfo.E_PortPos)EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect()  , (int)TempSelectedNode.drawInfo.EnterPortPos, PortPosOption);
            LeftPanelDrawHelper.EndHorizontalLayout();

            //��������
            LeftPanelDrawHelper.BeginHorizontalLayout(2,2);
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "����λ�����ã�");
            TempSelectedNode.drawInfo.ExitPortPos =
                (VNDrawInfo.E_PortPos)EditorGUI.Popup(LeftPanelDrawHelper.GetNextSingleRect(),  (int)TempSelectedNode.drawInfo.ExitPortPos, PortPosOption);
            LeftPanelDrawHelper.EndHorizontalLayout();

            //���
            LeftPanelDrawHelper.GetNextSingleRect(1.55F);
            //���ƽڵ�������Ϣ
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "�������ӣ�");
            //���ƽ��ڵ����нڵ�
            for (int i = 0; i < TempEnterNodes.Count; i++)
            {
                //�õ������ڵ�
                var node = TempEnterNodes[i];
                LeftPanelDrawHelper.BeginHorizontalLayout(2, 1.25f);
                //���Ʊ�ǩ
                GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"{node.NodeName}(id:{node.ID})");
                if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "�Ͽ�"))
                {
                    window.Operator.UnlinkNode(TempSelectedNode, TempEnterNodes[i]);
                }
                LeftPanelDrawHelper.EndHorizontalLayout();
            }
            //���Ƴ��ڵ���Ϣ
            GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), "�������ӣ�");
            //���ƽ��ڵ����нڵ�
            for (int i = 0; i < TempExitNodes.Count; i++)
            {
                //�õ������ڵ�
                var node = TempExitNodes[i];
                LeftPanelDrawHelper.BeginHorizontalLayout(2, 1.25f);
                //���Ʊ�ǩ
                GUI.Label(LeftPanelDrawHelper.GetNextSingleRect(), $"{node.NodeName}(id:{node.ID})");
                if (GUI.Button(LeftPanelDrawHelper.GetNextSingleRect(), "�Ͽ�"))
                {
                    window.Operator.UnlinkNode(TempSelectedNode, TempExitNodes[i]);
                }
                LeftPanelDrawHelper.EndHorizontalLayout();
            }
        }
    }


}

