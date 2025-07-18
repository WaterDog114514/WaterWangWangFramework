using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace WDEditor
{
    //�Զ�����ʾ�ڵ�Ԥ��Inspector������壬�Ա�ɵ��ڽڵ�Ԥ��
    public class win_VNPresetEdtior : BaseWindow<winDraw_VNPresetEdtior, winData_VNPresetEdtior>, IPresetWindowEditor
    {
        //��ʼ���Զ���ر�
        [InitializeOnLoadMethod]
        private static void IntiEditor()
        {
            string iconPath = EditorPathHelper.GetRelativeAssetPath(Path.Combine(EditorPathHelper.EditorAssetPath, "Texture/VNPresetIcon.png"));
            EditorWindowPresetHelper.RegisterPresetInProject<win_VNPresetEdtior>("vnpreset", iconPath);

        }
        //����
        [MenuItem("ˮ�����༭��/�ڵ�༭��/�����ڵ�Ԥ��")]
        [MenuItem("Assets/Create/�ڵ�༭��/�ڵ�Ԥ��", priority = 0)]
        public static void CreateCustomFile()
        {
            EditorWindowPresetHelper.CreatePresetFile<VisualNode>("vnpreset", "δ�����ڵ�Ԥ��");
        }
        public override void OnOpenWindow()
        {
            //�������ʱ���أ��Ǿ����¼�����
            if (data.visualBaseNode == null && data.FilePath != null)
                LoadData(data.FilePath);
            //������һ��ɾ������
        }
        private void OnDisable()
        {
            SaveData();
        }
        /// <summary>
        /// �����ڵ㷽��
        /// </summary>
        public void DrawCreateNewPar()
        {
            GenericMenu menu = new GenericMenu();
            //�Ȼ�ȡ���ܴ���������
            List<Type> CreateTypes = ReflectionHelper.GetSubclasses(typeof(VNParameter));
            foreach (var type in CreateTypes)
            {
                //�ȴ���һ��ʵ��
                VNParameter instance = Activator.CreateInstance(type) as VNParameter;
                // ʹ�÷����ȡDropmenuShowName����ֵ
                PropertyInfo dropmenuShowNameProp = type.GetProperty("DropmenuShowName", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                //�õ��˵���
                string dropmenuShowNameValue = dropmenuShowNameProp.GetValue(instance) as string;
                //��Ӳ˵���
                menu.AddItem(new GUIContent(dropmenuShowNameValue), false, () =>
                {
                    instance.Name = "δ�����Ĳ���";
                    data.visualBaseNode.parameters.Add(instance);
                    //����ʱ��֤һ�������Ϸ���
                    ValidateAndRenameParameters();
                    //����һ��ɾ������
                    ResetDeleteAllParameter();

                });

            }
            menu.ShowAsContext();
        }
        public void LoadData(string path)
        {
            ////����Ѿ��������ˣ��Ͳ����ظ�����
            // if (data.visualBaseNode != null && !string.IsNullOrEmpty(data.FilePath)) return;
            data.FilePath = path;
            if (!string.IsNullOrEmpty(data.FilePath))
                data.visualBaseNode = BinaryManager.Load<VisualNode>(path);
            //���سɹ�������ɾ������
            ResetDeleteAllParameter();
        }
        //��������
        public void SaveData()
        {
            if (data.visualBaseNode == null)
            {
                return;
            }
            //����ʱ��֤һ�������Ϸ���
            ValidateAndRenameParameters();
            BinaryManager.SaveToPath(data.visualBaseNode, data.FilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// ��֤���в��������Ƿ�Ϸ�����������
        /// </summary>
        public void ValidateAndRenameParameters()
        {
            HashSet<string> names = new HashSet<string>();
            int unnamedCount = 0;

            for (int i = 0; i < data.visualBaseNode.parameters.Count; i++)
            {
                var parameter = data.visualBaseNode.parameters[i];
                var name = parameter.Name;

                // ����Ƿ�Ϊ�ջ�null
                if (string.IsNullOrEmpty(name))
                {
                    name = "δ�����Ĳ���" + (++unnamedCount);
                    parameter.Name = name;
                }

                // ����Ƿ��ظ�
                while (names.Contains(name))
                {
                    name = "δ�����Ĳ���" + (++unnamedCount);
                    parameter.Name = name;
                }

                names.Add(name);
            }
        }

        //���������в�����ɾ������
        public void ResetDeleteAllParameter()
        {
            //��ֹ����
            if (data == null || data.visualBaseNode == null) return;
            for (int i = 0; i < data.visualBaseNode.parameters.Count; i++)
            {
                var par = data.visualBaseNode.parameters[i];
                par.deleteParameter = () =>
                {
                    data.visualBaseNode.parameters.Remove(par);

                };
            }
        }
    }
}


