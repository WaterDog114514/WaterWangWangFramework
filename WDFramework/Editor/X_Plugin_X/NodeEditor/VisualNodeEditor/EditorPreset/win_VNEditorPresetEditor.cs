using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Presets;

namespace WDEditor
{
    //�ڵ�༭��Ԥ�� �༭��
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
                        //ͨ��GUID����
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


        //��ʼ���Զ���ر�
        [InitializeOnLoadMethod]
        private static void IntiEditor()
        {
            string iconPath = EditorPathHelper.GetRelativeAssetPath(Path.Combine(EditorPathHelper.EditorAssetPath, "Texture/VNEditorPresetIcon.png"));
            EditorWindowPresetHelper.RegisterPresetInProject<win_VNEditorPresetEditor>("vneditorpreset", iconPath);

        }

        //����
        [MenuItem("ˮ�����༭��/�ڵ�༭��/�����༭��Ԥ��")]
        [MenuItem("Assets/Create/�ڵ�༭��/�༭��Ԥ��", priority = 1)]
        public static void CreateCustomFile()
        {
            EditorWindowPresetHelper.CreatePresetFile<VNEditorPreset>("vneditorpreset", "δ�����ڵ�Ԥ��");
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
            //ȡ�����·��
            var loadpath  = EditorPathHelper.GetRelativeAssetPath(path);
            EditorPreset = BinaryManager.Load<VNEditorPreset>(loadpath);
            data.GUID = AssetDatabase.AssetPathToGUID(loadpath);

            //����ʱ�����ˢ���б�
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