using System;
using UnityEditor;
using UnityEngine;


namespace WDEditor
{
    public class win_JsonInitializedHelper : BaseWindow<winDraw_JsonInitializedHelper, winData_JsonInitializedHelper>
    {

        [MenuItem("水汪汪框架/Json生成模板")]
        protected static void OpenWindow()
        {
            GetWindow<win_JsonInitializedHelper>();
        }
        public void GenerateJsonModuel(string ClassName, global::JsonType jsonType)
        {
            //得到类
            var ClassType = ReflectionHelper.FindTypeInAssemblies(ClassName);
            if (ClassType == null)
            {
                throw new Exception($"不存在该{ClassType}类型的json");
            }
            //实例化该Type
            var JsonObj = Activator.CreateInstance(ClassType);
            //搞下上次路径
            if (string.IsNullOrEmpty(data.LastSaveDirectionPath))
                data.LastSaveDirectionPath = Application.dataPath;
            //得到路径
            string path = EditorUtility.SaveFilePanel("保存配置", data.LastSaveDirectionPath, null, "json");
            if (string.IsNullOrEmpty(path)) return;
            //保存上次路径
            int lastIndex = path.LastIndexOf("/");
            data.LastSaveDirectionPath = path.Substring(0, lastIndex-1);
            JsonManager.Instance.SaveDataToPath(JsonObj, path, jsonType);
        }

    }
}