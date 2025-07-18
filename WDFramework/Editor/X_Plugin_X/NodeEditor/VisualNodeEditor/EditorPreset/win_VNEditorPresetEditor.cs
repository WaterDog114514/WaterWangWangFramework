using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Presets;

namespace WDEditor
{
    //节点编辑器预设 编辑器
    public class win_VNEditorPresetEditor : BaseWindow<winDraw_VNEditorPresetEditor, winData_VNEditorPresetEditor>, IPresetWindowEditor
    {
        private bool isAutoLoaded = true;
        public VNEditorPreset EditorPreset
        {
            get
            {
                if (isAutoLoaded && data._preset== null)
                {
                    isAutoLoaded = false;
                    if (!string.IsNullOrEmpty(data.GUID))
                    {
                        //通过GUID加载
                        string path = AssetDatabase.GUIDToAssetPath(data.GUID);
                        LoadData(EditorPathHelper.GetAbsolutePath(path));
                    }
                }
                return data._preset;
            }
            set
            {
                data._preset = value;
            }
        }


        //初始化自定义必备
        [InitializeOnLoadMethod]
        private static void IntiEditor()
        {
            string iconPath = EditorPathHelper.GetRelativeAssetPath(Path.Combine(EditorPathHelper.EditorAssetPath, "Texture/VNEditorPresetIcon.png"));
            EditorWindowPresetHelper.RegisterPresetInProject<win_VNEditorPresetEditor>("vneditorpreset", iconPath);

        }

        //创建
        [MenuItem("水汪汪编辑器/节点编辑器/创建编辑器预设")]
        [MenuItem("Assets/Create/节点编辑器/编辑器预设", priority = 1)]
        public static void CreateCustomFile()
        {
            EditorWindowPresetHelper.CreatePresetFile<VNEditorPreset>("vneditorpreset", "未命名节点预设");
        }
        public override void OnOpenWindow()
        {
        }
        public override void OnCloseWindow()
        {
            SaveData();
        }
        public void LoadData(string path)
        {
            //取得相对路径
            var loadpath  = EditorPathHelper.GetRelativeAssetPath(path);
            EditorPreset = BinaryManager.Load<VNEditorPreset>(loadpath);
            data.GUID = AssetDatabase.AssetPathToGUID(loadpath);

            //加载时候进行刷新列表
            RefreshNodeLayer();
        }
        public void SaveData()
        {
            
            if (EditorPreset == null) return;
             string savePath = EditorPathHelper.GetAbsolutePath(AssetDatabase.GUIDToAssetPath(data.GUID));
            BinaryManager.SaveToPath(EditorPreset, savePath);
        }
        public void RefreshNodeLayer()
        {
            foreach (var layer in EditorPreset.dropmenuLayers)
            {
                layer.DeleteLayer=()=> { winDraw.RemoveDropmenuLayer(layer); };
                layer.RefreshVisualNode();
            }
        }
    }
}