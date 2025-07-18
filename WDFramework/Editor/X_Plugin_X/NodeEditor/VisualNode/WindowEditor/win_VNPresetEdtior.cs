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
    //自定义显示节点预设Inspector设置面板，以便可调节节点预设
    public class win_VNPresetEdtior : BaseWindow<winDraw_VNPresetEdtior, winData_VNPresetEdtior>, IPresetWindowEditor
    {
        //初始化自定义必备
        [InitializeOnLoadMethod]
        private static void IntiEditor()
        {
            string iconPath = EditorPathHelper.GetRelativeAssetPath(Path.Combine(EditorPathHelper.EditorAssetPath, "Texture/VNPresetIcon.png"));
            EditorWindowPresetHelper.RegisterPresetInProject<win_VNPresetEdtior>("vnpreset", iconPath);

        }
        //创建
        [MenuItem("水汪汪编辑器/节点编辑器/创建节点预设")]
        [MenuItem("Assets/Create/节点编辑器/节点预设", priority = 0)]
        public static void CreateCustomFile()
        {
            EditorWindowPresetHelper.CreatePresetFile<VisualNode>("vnpreset", "未命名节点预设");
        }
        public override void OnOpenWindow()
        {
            //如果编译时重载，那就重新加载呗
            if (data.visualBaseNode == null && data.FilePath != null)
                LoadData(data.FilePath);
            //重设置一波删除方法
        }
        private void OnDisable()
        {
            SaveData();
        }
        /// <summary>
        /// 创建节点方法
        /// </summary>
        public void DrawCreateNewPar()
        {
            GenericMenu menu = new GenericMenu();
            //先获取所能创建的类型
            List<Type> CreateTypes = ReflectionHelper.GetSubclasses(typeof(VNParameter));
            foreach (var type in CreateTypes)
            {
                //先创建一个实例
                VNParameter instance = Activator.CreateInstance(type) as VNParameter;
                // 使用反射获取DropmenuShowName属性值
                PropertyInfo dropmenuShowNameProp = type.GetProperty("DropmenuShowName", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                //得到菜单名
                string dropmenuShowNameValue = dropmenuShowNameProp.GetValue(instance) as string;
                //添加菜单名
                menu.AddItem(new GUIContent(dropmenuShowNameValue), false, () =>
                {
                    instance.Name = "未命名的参数";
                    data.visualBaseNode.parameters.Add(instance);
                    //保存时验证一波看看合法吗
                    ValidateAndRenameParameters();
                    //设置一波删除方法
                    ResetDeleteAllParameter();

                });

            }
            menu.ShowAsContext();
        }
        public void LoadData(string path)
        {
            ////如果已经有数据了，就不必重复加载
            // if (data.visualBaseNode != null && !string.IsNullOrEmpty(data.FilePath)) return;
            data.FilePath = path;
            if (!string.IsNullOrEmpty(data.FilePath))
                data.visualBaseNode = BinaryManager.Load<VisualNode>(path);
            //加载成功后设置删除方法
            ResetDeleteAllParameter();
        }
        //保存数据
        public void SaveData()
        {
            if (data.visualBaseNode == null)
            {
                return;
            }
            //保存时验证一波看看合法吗
            ValidateAndRenameParameters();
            BinaryManager.SaveToPath(data.visualBaseNode, data.FilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 验证所有参数命名是否合法，包括命名
        /// </summary>
        public void ValidateAndRenameParameters()
        {
            HashSet<string> names = new HashSet<string>();
            int unnamedCount = 0;

            for (int i = 0; i < data.visualBaseNode.parameters.Count; i++)
            {
                var parameter = data.visualBaseNode.parameters[i];
                var name = parameter.Name;

                // 检查是否为空或null
                if (string.IsNullOrEmpty(name))
                {
                    name = "未命名的参数" + (++unnamedCount);
                    parameter.Name = name;
                }

                // 检查是否重复
                while (names.Contains(name))
                {
                    name = "未命名的参数" + (++unnamedCount);
                    parameter.Name = name;
                }

                names.Add(name);
            }
        }

        //重设置所有参数的删除方法
        public void ResetDeleteAllParameter()
        {
            //防止报空
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


