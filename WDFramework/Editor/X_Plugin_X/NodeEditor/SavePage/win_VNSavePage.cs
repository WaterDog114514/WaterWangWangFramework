using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    //自定义显示节点预设Inspector设置面板，以便可调节节点预设
    public class win_VNSavePage : BaseWindow<winDraw_VNSavePage, winData_VNSavePage>, IPresetWindowEditor
    {
        //初始化自定义必备
        [InitializeOnLoadMethod]
        private static void IntiEditor()
        {
            string iconPath = EditorPathHelper.GetRelativeAssetPath(Path.Combine(EditorPathHelper.EditorAssetPath, "Texture/VNSavepage.png"));
            EditorWindowPresetHelper.RegisterPresetInProject<win_VNSavePage>("vnsavepage", iconPath);
        }


        public void LoadData(string path)
        {
            var page = BinaryManager.Load<VNSavePage>(path);
            if (page != null)
            {
                data.SavePage = page;
                data.SavePage.OpenFilePath = path;
            }
            else
                Debug.LogError("加载失败，序列化为null");
        }

        public void OpenEditor()
        {
            var window = EditorWindow.GetWindow<win_NodeEditor>();
            window.OpenEditorFromSavePage(data.SavePage);
            //关闭本窗口
            this.Close();
        }

        public void SaveData()
        {
        }
        //初始化



    }
}


