using Codice.Client.Common.FsNodeReaders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    public class winDraw_VNEditorPresetEditor : BaseWindowDraw<winData_VNEditorPresetEditor>
    {
        private new win_VNEditorPresetEditor window => (base.window as win_VNEditorPresetEditor);
        private VNEditorPreset preset => window.EditorPreset;
        public winDraw_VNEditorPresetEditor(EditorWindow window, winData_VNEditorPresetEditor data) : base(window, data)
        {
        }
        public override void Draw()
        {
            DrawEditorInputField();
            DrawVNPresetList();
            DrawButton();
        }
        //���Ʊ��������
        private void DrawEditorInputField()
        {
            GUILayout.Label("�༭������", TextStyleHelper.Custom(18, Color.magenta));
            preset.EditorName = EditorGUILayout.TextField(preset.EditorName);

        }
        private Vector2 scrollPosition;
   
        private void DrawVNPresetList()
        {
            EditorGUILayout.BeginVertical();
            // �����Ϸ���ɫ�ֽ���
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            //����������Ϊ3/5������
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(window.position.height * 3 / 5));
            // ѭ������ÿ���㼶
            for (int i = 0; i < preset.dropmenuLayers.Count; i++)
            {
                var layer = preset.dropmenuLayers[i];
                // �����㼶�Ļ���
                layer.Draw(window.position);
                // �����㼶�Ĵ�����ק
                layer.HandleDragAndDrop();
            }
            EditorGUILayout.EndScrollView();
            // �����·���ɫ�ֽ���
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            EditorGUILayout.EndVertical();
            // ����������ͼ
            // �������� "����²㼶" ��ť�����ڹ���������
            if (GUILayout.Button("����²㼶")) AddNewDropmenuLayer();
        }
        public override void OnCreated()
        {
        }
        //���ƴ�����ť ���水ť
        public void DrawButton()
        {
            if (GUILayout.Button(new GUIContent($"ˢ���б� ͬ�����нڵ�Ԥ��")))
            {
                window.RefreshNodeLayer();
            }
            if (GUILayout.Button(new GUIContent($"��" + preset.EditorName + "�༭��")))
            {
                win_NodeEditor.OpenEditorFromPreset(preset);
            }
            if (GUILayout.Button("����Ԥ��"))
            {
                window.SaveData();
            }
        }
        //����²˵�
        public void AddNewDropmenuLayer()
        {
            var newLayer = new VNDropmenuLayer()
            {
                LayerName = "δ�����Ĳ㼶"
            };
            //����Ƴ�����
            newLayer.DeleteLayer = () =>
            {
                RemoveDropmenuLayer(newLayer);
            };
            preset.dropmenuLayers.Add(newLayer);
        }
        public void RemoveDropmenuLayer(VNDropmenuLayer layer)
        {
            preset.dropmenuLayers.Remove(layer);
        }
    }
}
