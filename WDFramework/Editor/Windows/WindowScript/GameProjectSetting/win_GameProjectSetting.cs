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
            //���������ļ�
            data.settingData = SystemSettingLoader.Instance.LoadData<GameProjectSettingData>();
            //����ʧ�ܣ������´���һ��
            if (data.settingData == null)
            {
                Debug.Log("�Ѿ����´�����ĿSetting����");
                data.settingData = new GameProjectSettingData();
            }

        }
        [MenuItem("ˮ�������/С������趨")]
        protected static void OpenWindow()
        {
            EditorWindow.GetWindow<win_GameProjectSetting>();
        }
        //�˳�ʱ���Զ�����
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
