using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace WDEditor
{

    public class win_GameProjectSetting : BaseWindow<winDraw_GameProjectSetting,
        winData_GameProjectSetting>
    {

        public override void OnOpenWindow()
        {
            //加载配置文件
            data.settingData = SystemSettingLoader.Instance.LoadData<GameProjectSettingData>();
            //加载失败，则重新创建一个
            if (data.settingData == null)
            {
                Debug.Log("已经重新创建项目Setting配置");
                data.settingData = new GameProjectSettingData();
            }

        }
        [MenuItem("水汪汪框架/小框架主设定")]
        protected static void OpenWindow()
        {
            EditorWindow.GetWindow<win_GameProjectSetting>();
        }
        //退出时候自动保存
        public override void OnCloseWindow()
        {
            SaveData();
        }
        public void SaveData()
        {
            SystemSettingLoader.Instance.SaveData(data.settingData);
            AssetDatabase.Refresh();
        }
    }


}
