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
        //绘制标题输入框
        private void DrawEditorInputField()
        {
            GUILayout.Label("编辑器名：", TextStyleHelper.Custom(18, Color.magenta));
            preset.EditorName = EditorGUILayout.TextField(preset.EditorName);

        }
        private Vector2 scrollPosition;
   
        private void DrawVNPresetList()
        {
            EditorGUILayout.BeginVertical();
            // 绘制上方白色分界线
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            //滑动条设置为3/5的区域
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(window.position.height * 3 / 5));
            // 循环绘制每个层级
            for (int i = 0; i < preset.dropmenuLayers.Count; i++)
            {
                var layer = preset.dropmenuLayers[i];
                // 单个层级的绘制
                layer.Draw(window.position);
                // 单个层级的处理拖拽
                layer.HandleDragAndDrop();
            }
            EditorGUILayout.EndScrollView();
            // 绘制下方白色分界线
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            EditorGUILayout.EndVertical();
            // 结束滚动视图
            // 继续绘制 "添加新层级" 按钮，放在滚动区域外
            if (GUILayout.Button("添加新层级")) AddNewDropmenuLayer();
        }
        public override void OnCreated()
        {
        }
        //绘制创建按钮 保存按钮
        public void DrawButton()
        {
            if (GUILayout.Button(new GUIContent($"刷新列表 同步所有节点预设")))
            {
                window.RefreshNodeLayer();
            }
            if (GUILayout.Button(new GUIContent($"打开" + preset.EditorName + "编辑器")))
            {
                win_NodeEditor.OpenEditorFromPreset(preset);
            }
            if (GUILayout.Button("保存预设"))
            {
                window.SaveData();
            }
        }
        //添加新菜单
        public void AddNewDropmenuLayer()
        {
            var newLayer = new VNDropmenuLayer()
            {
                LayerName = "未命名的层级"
            };
            //添加移除方法
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
