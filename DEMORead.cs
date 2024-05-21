using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DEMORead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PrefabLoaderManager.Instance.PreLoadPrefabFrmoExcel<MonsterPZContainer>();
        ResLoader.Instance.StartPreload();
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

    }
}
