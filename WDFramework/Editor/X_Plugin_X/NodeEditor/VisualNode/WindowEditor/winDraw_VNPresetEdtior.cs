using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    /// <summary>
    /// �������֣������������룬�������window��������
    /// </summary>
    public class winDraw_VNPresetEdtior : BaseWindowDraw<winData_VNPresetEdtior>
    {
        //inspetor����
        private new win_VNPresetEdtior window => (base.window as win_VNPresetEdtior);
        //�ڵ����ݱ���
        private VisualNode Node => data.visualBaseNode;

        public winDraw_VNPresetEdtior(EditorWindow window, winData_VNPresetEdtior data) : base(window, data)
        {
        }
        public override void OnCreated()
        {
        }
        public override void Draw()
        {
            if (data.visualBaseNode == null)
            {
                GUILayout.Label("��ǰδѡ���κνڵ�");
                return;
            }
            //���ƽڵ��������
            DrawTitleSetting();
            //���ƽڵ���ʽ����
            DrawStyleSetting();
            DrawParameter();
            DrawButton();
        }
        //���Ʊ�������
        private void DrawTitleSetting()
        {
            //����
            GUILayout.Label("�ڵ����ƣ�", TextStyleHelper.Custom(18, Color.magenta));
            Node.NodeName = EditorGUILayout.TextField(Node.NodeName);

        }
        //���ƽڵ���ʽ����
        public void DrawStyleSetting()
        {

            //����������ɫ
            Node.drawInfo.TitleColor.color = EditorGUILayout.ColorField("������ɫ��", Node.drawInfo.TitleColor.color);
            //��������ɫ
            Node.drawInfo.TitleAreaColor.color = EditorGUILayout.ColorField("�������ɫ��", Node.drawInfo.TitleAreaColor.color);
            //����ɫ
            Node.drawInfo.BackgroundColor.color = EditorGUILayout.ColorField("������ɫ��", Node.drawInfo.BackgroundColor.color);
        }


        public void DrawParameter()
        {
            //��ȡ���Ʋ���ʱ��ĳ�ʼλ��
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect DrawRect = new Rect(lastRect.position + Vector2.up * lastRect.height, new Vector2(lastRect.width, 0));
            float totalHeight = 0;
            for (int i = 0; i < Node.parameters.Count; i++)
            {
                // ��ȡ��ǰ����
                var parameter = Node.parameters[i];
                //  ���Ʋ�������
                parameter.Draw_Inspctor(DrawRect);
                //����֮ǰ�Ļ��Ƹ߶�
                DrawRect.position += Vector2.up * parameter.CurrentInspectorHeight;
                totalHeight += parameter.CurrentInspectorHeight;
            }
            //������Layout�ν�
            EditorGUILayout.GetControlRect(false, totalHeight);
            //�ճ�����λ��
            GUILayout.Space(10);
        }

        //���ƴ�����ť ���水ť
        public void DrawButton()
        {
            if (GUILayout.Button("����µĲ���"))
            {
                window.DrawCreateNewPar();
            }
            if (GUILayout.Button("���浱ǰ�ڵ�Ԥ��"))
            {
                window.SaveData();

            }
        }
    }
}