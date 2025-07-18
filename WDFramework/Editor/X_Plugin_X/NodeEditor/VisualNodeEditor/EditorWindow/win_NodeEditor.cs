using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace WDEditor
{
    public class win_NodeEditor : BaseWindow<winDraw_NodeEditor, winData_NodeEditor>
    {
        //�༭��Ԥ��
        public VNEditorPreset Preset => data.editorPreset;
        //�����˵�
        public VNEditorDropmenu Dropmenu;
        //�����
        public VNEditorCheck Checker;
        //���л���
        // public VNEditorSerializer Serializer;
        //�ڵ�����߼���
        public VNEditorOperator Operator;

        private const string EditorPresetSavePath = "Assets/Editor/NodeEditorPreset";

        [MenuItem("ˮ�����༭��/�ڵ�༭��/��")]
        public static void OpenWindow()
        {
            EditorWindow.GetWindow<win_NodeEditor>();
        }
        public override void OnOpenWindow()
        {
            LoadEditorPreset(null);
            LoadSavePage(null);
        }
        public void IntiCreate()
        {

        }
        //���ر༭��Ԥ�� ����Ҫ��һ���������˲��ܼ�������������
        public void LoadEditorPreset(VNEditorPreset preset)
        {
            data.editorPreset = preset;
            Checker = new VNEditorCheck(this);
            Dropmenu = new VNEditorDropmenu(this, preset);
            Operator = new VNEditorOperator(this);

        }
        //�������еĽڵ�༭ҳ��
        public void LoadSavePage(VNSavePage page)
        {
            //��null��һ��Ϊ��ʼ�����ߴ�����ҳ��
            if (page == null)
            {
                data.LastOpenPage = null;
                //������нڵ�
                data.dic_Nodes = new Dictionary<int, VisualNode>();
                //������ͼ
                ResetView();
                return;
            }
            //���нڵ���г�ʼ������������Ԥ��
            data.LastOpenPage = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(page.OpenFilePath);
            data.dic_Nodes = page.dic_Nodes;
            foreach (var node in data.dic_Nodes.Values)
            {
                node.IntiDrawHelper();
            }
            //�����ӿ���Ϣ
            data.ViewportPosition.vector2 = page.ViewportPosition.vector2;
            data.ScaleValue = page.ScaleValue;
        }

        /// <summary>
        /// �ӱ༭��Ԥ�贰�ڴ򿪱༭��
        /// </summary>
        public static void OpenEditorFromPreset(VNEditorPreset preset)
        {
            win_NodeEditor window = GetWindow<win_NodeEditor>();
            window.LoadEditorPreset(preset);
        }
        public void OpenEditorFromSavePage(VNSavePage page)
        {
            //�ȼ���Ԥ��
            LoadEditorPreset(page.editorPreset);
            //��¼Ϊ�ϴδ�page
            LoadSavePage(page);

        }
        //�Ӵ��ļ���ʾ����д�һ���༭��ҳ
        public void OpenEditorFromOpenFilePanel()
        {
            string path = EditorUtility.OpenFilePanel("��һ�����б༭ҳ��", "Assets", "vnsavepage");
            if (path == null) return;
            //�ȼ���Ԥ��
            VNSavePage savePage = BinaryManager.Load<VNSavePage>(path);
            LoadEditorPreset(savePage.editorPreset);
            //�ټ������нڵ���Ϣ
            LoadSavePage(savePage);
        }
        //ͨ��ѡ��һ���༭��Ȼ�󴴽���ҳ��
        public void OpenEditorFromCreateNewPage(string editorName)
        {
            string editorPath = data.ExistEditorPreset[editorName];
            VNEditorPreset preset = BinaryManager.Load<VNEditorPreset>(editorPath);
            //���ر༭��Ԥ��
            LoadEditorPreset(preset);
            //����ҳ��
            LoadSavePage(null);
        }
        //���¶�λ�����Ѿ������ı༭��Ԥ��
        public void LocateAllEditorPreset()
        {
            string path = EditorPathHelper.GetPathOrCreateDirectory(EditorPresetSavePath, false);
            // ���ҷ����������ļ�·����������Ŀ¼
            string[] files = Directory.GetFiles(path, $"*.vneditorpreset", SearchOption.AllDirectories);
            //����ǰ���
            data.ExistEditorPreset.Clear();
            //���������δ�����ڵ�༭���ͻᱨ��Ŷ������������
            //���������δ�����ڵ�༭���ͻᱨ��Ŷ������������
            //���������δ�����ڵ�༭���ͻᱨ��Ŷ������������
            //���������δ�����ڵ�༭���ͻᱨ��Ŷ������������
            foreach (string filepath in files)
            {
                //�ȼ���Ԥ��
                VNEditorPreset preset = BinaryManager.Load<VNEditorPreset>(filepath);
                //��ֹ�ظ����ֱ༭������
                while (data.ExistEditorPreset.ContainsKey(preset.EditorName))
                {
                    preset.EditorName += 1;
                }
                //���
                data.ExistEditorPreset.Add(preset.EditorName, filepath);
            }
        }
        //����һҳ����¼���б༭����Ϣ��������һ�α༭
        public void SaveEditorPage(bool isSaveAs)
        {
            var SavePage = new VNSavePage()
            {
                //�������е�data��Ϣ
                dic_Nodes = data.dic_Nodes,
                editorPreset = data.editorPreset,
                ScaleValue = data.ScaleValue,
            };
            //�����ӿڵ�λ����Ϣ
            SavePage.ViewportPosition.vector2 = data.ViewportPosition.vector2;

            //�����Ϊ���ϴε�ַ����null
            if (!string.IsNullOrEmpty(data.LastSavePagePath) && isSaveAs)
            {
                BinaryManager.SaveToPath(SavePage, data.LastSavePagePath);
            }
            //�´���һ��
            else
            {
                string path = EditorUtility.SaveFilePanel("����༭ҳ��", "Assets", "δ�����Ľڵ�ҳ", "vnsavepage");
                data.LastSavePagePath = path;
                BinaryManager.SaveToPath(SavePage, path);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public void SerializeRuntimeData()
        {
            VNEditorSerializer.SerializeNodes(data.dic_Nodes);
        }
        //�����ӿ���Ϣ
        public void ResetView()
        {
            data.ScaleValue = 1f;
            data.ViewportPosition.vector2 = data.NodeViewRect.rect.size / 2 - data.RightPanelRect.rect.size;
        }
    }
}