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
        //上次加载的节点预设名，用于这次加载对比是否是同一预设
        //↓————临时变量↓————
        /// <summary>
        /// 临时变量 当前已加载的节点
        /// </summary>
        [NonSerialized]
        public Dictionary<int, VisualNode> dic_Nodes = new Dictionary<int, VisualNode>();
        //左侧面板的位置和大小
        public SerializableRect LeftPanelRect;
        //右侧面板的位置和大小
        public SerializableRect RightPanelRect;
        //大局图 节点视图的大小
        [NonSerialized]
        public SerializableRect NodeViewRect;
        /// <summary>
        /// 视口滚动条位置
        /// </summary>
        public SerializableVector2 ViewportPosition;
        /// <summary>
        /// 缩放系数
        /// </summary>
        public float ScaleValue = 1F;
        /// <summary>
        /// 当前视口坐标
        /// </summary>
        public SerializableVector2 Pos_CurrentView;
        /// <summary>
        /// 真正的坐标和大小
        /// </summary>
        public SerializableRect Size;
        /// <summary>
        /// 编辑器的预设
        /// </summary>
        [NonSerialized]
        public VNEditorPreset editorPreset;
        [NonSerialized]
        public UnityEngine.Object LastOpenPage;
        /// <summary>
        /// 上次存储节点的位置
        /// </summary>
        public string LastSavePagePath;
        public SerializableEditorTexture2D LeftPanelBG;
        public SerializableEditorTexture2D RightPanelBG;
        /// <summary>
        /// 已有编辑器预设，用来创建新编辑器用 形式为预设路径 预设名
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
                    return "未打开的编辑器";
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
     
            //设置节点编辑器初始化图片
            string LeftBGPath = EditorPathHelper.GetRelativeAssetPath(Path.Combine(EditorPathHelper.EditorTexturePath, "NodeEditorLeftBG.png"));
            string RightBGPath = EditorPathHelper.GetRelativeAssetPath(Path.Combine(EditorPathHelper.EditorTexturePath, "NodeEditorRightBG.png"));
            LeftPanelBG.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(LeftBGPath);
            RightPanelBG.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(RightBGPath);
        }


    }
}