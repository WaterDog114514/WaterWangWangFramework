using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace WDEditor
{
    public class win_NodeEditor : BaseWindow<winDraw_NodeEditor, winData_NodeEditor>
    {
        //编辑器预设
        public VNEditorPreset Preset => data.editorPreset;
        //下拉菜单
        public VNEditorDropmenu Dropmenu;
        //检测类
        public VNEditorCheck Checker;
        //序列化类
        // public VNEditorSerializer Serializer;
        //节点操作逻辑类
        public VNEditorOperator Operator;

        private const string EditorPresetSavePath = "Assets/Editor/NodeEditorPreset";

        [MenuItem("水汪汪编辑器/节点编辑器/打开")]
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
        //加载编辑器预设 最重要的一环，加载了才能加载其他辅助类
        public void LoadEditorPreset(VNEditorPreset preset)
        {
            data.editorPreset = preset;
            Checker = new VNEditorCheck(this);
            Dropmenu = new VNEditorDropmenu(this, preset);
            Operator = new VNEditorOperator(this);

        }
        //加载已有的节点编辑页面
        public void LoadSavePage(VNSavePage page)
        {
            //传null，一般为初始化或者创建新页用
            if (page == null)
            {
                data.LastOpenPage = null;
                //清空所有节点
                data.dic_Nodes = new Dictionary<int, VisualNode>();
                //重置视图
                ResetView();
                return;
            }
            //所有节点进行初始化，加载他们预设
            data.LastOpenPage = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(page.OpenFilePath);
            data.dic_Nodes = page.dic_Nodes;
            foreach (var node in data.dic_Nodes.Values)
            {
                node.IntiDrawHelper();
            }
            //加载视口信息
            data.ViewportPosition.vector2 = page.ViewportPosition.vector2;
            data.ScaleValue = page.ScaleValue;
        }

        /// <summary>
        /// 从编辑器预设窗口打开编辑器
        /// </summary>
        public static void OpenEditorFromPreset(VNEditorPreset preset)
        {
            win_NodeEditor window = GetWindow<win_NodeEditor>();
            window.LoadEditorPreset(preset);
        }
        public void OpenEditorFromSavePage(VNSavePage page)
        {
            //先加载预设
            LoadEditorPreset(page.editorPreset);
            //记录为上次打开page
            LoadSavePage(page);

        }
        //从打开文件提示面板中打开一个编辑器页
        public void OpenEditorFromOpenFilePanel()
        {
            string path = EditorUtility.OpenFilePanel("打开一个已有编辑页面", "Assets", "vnsavepage");
            if (path == null) return;
            //先加载预设
            VNSavePage savePage = BinaryManager.Load<VNSavePage>(path);
            LoadEditorPreset(savePage.editorPreset);
            //再加载所有节点信息
            LoadSavePage(savePage);
        }
        //通过选择一个编辑器然后创建新页面
        public void OpenEditorFromCreateNewPage(string editorName)
        {
            string editorPath = data.ExistEditorPreset[editorName];
            VNEditorPreset preset = BinaryManager.Load<VNEditorPreset>(editorPath);
            //加载编辑器预设
            LoadEditorPreset(preset);
            //重置页面
            LoadSavePage(null);
        }
        //重新定位所有已经创建的编辑器预设
        public void LocateAllEditorPreset()
        {
            string path = EditorPathHelper.GetPathOrCreateDirectory(EditorPresetSavePath, false);
            // 查找符合条件的文件路径，包括子目录
            string[] files = Directory.GetFiles(path, $"*.vneditorpreset", SearchOption.AllDirectories);
            //先提前清空
            data.ExistEditorPreset.Clear();
            //待解决——未命名节点编辑器就会报错哦！！！！！！
            //待解决——未命名节点编辑器就会报错哦！！！！！！
            //待解决——未命名节点编辑器就会报错哦！！！！！！
            //待解决——未命名节点编辑器就会报错哦！！！！！！
            foreach (string filepath in files)
            {
                //先加载预设
                VNEditorPreset preset = BinaryManager.Load<VNEditorPreset>(filepath);
                //防止重复名字编辑器操作
                while (data.ExistEditorPreset.ContainsKey(preset.EditorName))
                {
                    preset.EditorName += 1;
                }
                //添加
                data.ExistEditorPreset.Add(preset.EditorName, filepath);
            }
        }
        //保存一页，记录所有编辑器信息，方便下一次编辑
        public void SaveEditorPage(bool isSaveAs)
        {
            var SavePage = new VNSavePage()
            {
                //保存所有的data信息
                dic_Nodes = data.dic_Nodes,
                editorPreset = data.editorPreset,
                ScaleValue = data.ScaleValue,
            };
            //保存视口的位置信息
            SavePage.ViewportPosition.vector2 = data.ViewportPosition.vector2;

            //是另存为或上次地址不是null
            if (!string.IsNullOrEmpty(data.LastSavePagePath) && isSaveAs)
            {
                BinaryManager.SaveToPath(SavePage, data.LastSavePagePath);
            }
            //新创建一个
            else
            {
                string path = EditorUtility.SaveFilePanel("保存编辑页面", "Assets", "未命名的节点页", "vnsavepage");
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
        //重置视口信息
        public void ResetView()
        {
            data.ScaleValue = 1f;
            data.ViewportPosition.vector2 = data.NodeViewRect.rect.size / 2 - data.RightPanelRect.rect.size;
        }
    }
}