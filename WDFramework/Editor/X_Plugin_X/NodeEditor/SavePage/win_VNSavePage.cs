using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WDEditor
{
    //�Զ�����ʾ�ڵ�Ԥ��Inspector������壬�Ա�ɵ��ڽڵ�Ԥ��
    public class win_VNSavePage : BaseWindow<winDraw_VNSavePage, winData_VNSavePage>, IPresetWindowEditor
    {
        //��ʼ���Զ���ر�
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
                Debug.LogError("����ʧ�ܣ����л�Ϊnull");
        }

        public void OpenEditor()
        {
            var window = EditorWindow.GetWindow<win_NodeEditor>();
            window.OpenEditorFromSavePage(data.SavePage);
            //�رձ�����
            this.Close();
        }

        public void SaveData()
        {
        }
        //��ʼ��



    }
}


