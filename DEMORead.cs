using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DEMORead : MonoBehaviour
{
    public Text text;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        string[] text = TextTool.SplitString("ab£»µÐ·¨£»À­Â¶¶÷;¿ÕÉ¶µÄ£»kdsa;koaf", ';');
        string ces = TextTool.GetDecimalStr(321.1145141919810f, 6);
        //  PrefabLoaderManager.Instance.PreLoadPrefabFrmoExcel<MonsterPZContainer>();
        //   ResLoader.Instance.StartPreload();
        //   ResLoader.Instance.CreatePreloadTaskFromExcel<MonsterPZContainer>("ArtPath");
        // ResLoader.Instance.CreatePreloadFromExcel<TestInfoContainer>("resPath");
    }


    private void OnGUI()
    {
        if (GUILayout.Button("²âÊÔ´°¿Ú"))
        {
            //PrefabLoaderManager.Instance.DemoTEst();
        }
        if (GUILayout.Button("Å£±Æ"))
        {
            ObjectManager.Instance.GetGameObjFromPool("OO1").SetPosition(Vector3.one * 3);
            ObjectManager.Instance.GetGameObjFromPool("smg1").SetPosition(Vector3.one * -4);
            ObjectManager.Instance.GetGameObjFromPool("sniper2").SetPosition(Vector3.one * 13);
            ObjectManager.Instance.GetGameObjFromPool("OO2").SetPosition(Vector3.one * 5);
        }
    }
    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        int total = (int)time;
        text.text =TextTool.SecondToHMS_Semicolon(total);
    }
}
