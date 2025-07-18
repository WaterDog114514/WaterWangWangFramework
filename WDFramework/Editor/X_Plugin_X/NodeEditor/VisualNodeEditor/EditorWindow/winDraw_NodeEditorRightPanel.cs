using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WDEditor
{
    //ר�Ż����Ҳ����
    public partial class winDraw_NodeEditor : BaseWindowDraw<winData_NodeEditor>
    {
        //�������ƶ�λ��
        private DrawIndexHelper RightPanelDrawHelper = new DrawIndexHelper();
        //�����Ҳ�ڵ�༭���
        public void DrawRightPanel()
        {
            //��������
            RightPanelDrawHelper.Update(RightPanelRect);
            //���Ʊ���ͼ
            //  GUI.DrawTexture(RightPanelRect, data.RightPanelBG);
            if (data.editorPreset == null)
            {
                DrawRightPanel_NoPreset();
                return;
            }
            DrawRightPanel_HavePreset();
        }
        //��û��Ԥ��ʱ�滭�ұ�����߼�
        private void DrawRightPanel_NoPreset()
        {
            Rect center = RightPanelDrawHelper.GetCenterRect(window.position.width / 6, window.position.height / 7);
            GUI.Label(center, "ѡ��ڵ�༭������ܱ༭������", TextStyleHelper.Custom(10, Color.red));
        }

        //����Ԥ��ʱ�滭�ұ�����߼�
        private void DrawRightPanel_HavePreset()
        {
            //Ӧ�ô�������ϵ����߼�
            Checker.CheckUpdate_Window();
            //������ͼ���߼�
            DrawNodeView();
        }
        //������ͼ���� �������ع�����
        //�������нڵ�
        private void DrawNodeView()
        {

            Rect FixedRect = new Rect(Vector2.zero, data.NodeViewRect.rect.size * data.ScaleValue);
            //���ù�����
            GUIStyle horizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
            GUIStyle verticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
            // ����Ϊ�հ���ʽ���Ӷ�ʹ���������ɼ�
            horizontalScrollbar.fixedHeight = 0;
            verticalScrollbar.fixedWidth = 0;
            //���ƽڵ��ӿ��ӿ�
            data.ViewportPosition.vector2 = GUI.BeginScrollView(RightPanelRect, data.ViewportPosition.vector2, FixedRect, horizontalScrollbar, verticalScrollbar);
            //Ӧ���ӿ�����ϵ����߼�
            Checker.CheckUpdate_NodeView();
            GUI.DrawTexture(FixedRect, data.RightPanelBG.texture);
            //���ƽڵ�
            DrawNodes();
            //ȡ���ӿ�����Ӧ��
            GUI.EndScrollView();
        }
        private void DrawNodes()
        {
            foreach (var vnode in dic_Nodes.Values)
            {
                //��ʼ�滭
                vnode.vnDraw.Draw(data.ScaleValue);
            }
        }
    }
   
}
