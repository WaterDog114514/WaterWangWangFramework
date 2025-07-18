using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    [Serializable]
    public class winData_NodeEditor : BaseWindowData
    {
        //�ϴμ��صĽڵ�Ԥ������������μ��ضԱ��Ƿ���ͬһԤ��
        //������������ʱ��������������
        /// <summary>
        /// ��ʱ���� ��ǰ�Ѽ��صĽڵ�
        /// </summary>
        [NonSerialized]
        public Dictionary<int, VisualNode> dic_Nodes = new Dictionary<int, VisualNode>();
        //�������λ�úʹ�С
        public SerializableRect LeftPanelRect;
        //�Ҳ�����λ�úʹ�С
        public SerializableRect RightPanelRect;
        //���ͼ �ڵ���ͼ�Ĵ�С
        [NonSerialized]
        public SerializableRect NodeViewRect;
        /// <summary>
        /// �ӿڹ�����λ��
        /// </summary>
        public SerializableVector2 ViewportPosition;
        /// <summary>
        /// ����ϵ��
        /// </summary>
        public float ScaleValue = 1F;
        /// <summary>
        /// ��ǰ�ӿ�����
        /// </summary>
        public SerializableVector2 Pos_CurrentView;
        /// <summary>
        /// ����������ʹ�С
        /// </summary>
        public SerializableRect Size;
        /// <summary>
        /// �༭����Ԥ��
        /// </summary>
        [NonSerialized]
        public VNEditorPreset editorPreset;
        [NonSerialized]
        public UnityEngine.Object LastOpenPage;
        /// <summary>
        /// �ϴδ洢�ڵ��λ��
        /// </summary>
        public string LastSavePagePath;
        public SerializableEditorTexture2D LeftPanelBG;
        public SerializableEditorTexture2D RightPanelBG;
        /// <summary>
        /// ���б༭��Ԥ�裬���������±༭���� ��ʽΪԤ��·�� Ԥ����
        /// </summary>
        [SerializeField]
        public Dictionary<string, string> ExistEditorPreset = new Dictionary<string, string>();

        public override string Title{
            get
            {
                if (editorPreset != null)
                {
                    return editorPreset.EditorName;
                }
                else
                {
                    return "δ�򿪵ı༭��";
                }
            }
        }

        public override void IntiLoad()
        {
            NodeViewRect.rect = new Rect(0, 0, 2000, 2000);
            ViewportPosition.vector2 = new Vector2(0, 0);
        }
        public override void IntiFirstCreate()
        {
            LeftPanelBG = new SerializableEditorTexture2D();
            RightPanelBG = new SerializableEditorTexture2D();
     
            //���ýڵ�༭����ʼ��ͼƬ
            string LeftBGPath = EditorPathHelper.GetRelativeAssetPath(Path.Combine(EditorPathHelper.EditorTexturePath, "NodeEditorLeftBG.png"));
            string RightBGPath = EditorPathHelper.GetRelativeAssetPath(Path.Combine(EditorPathHelper.EditorTexturePath, "NodeEditorRightBG.png"));
            LeftPanelBG.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(LeftBGPath);
            RightPanelBG.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(RightBGPath);
        }


    }
}