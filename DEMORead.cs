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
            PoolManager.Instance.DEMOTEST();
        }
        if (GUILayout.Button("Å£±Æ"))
        {
            PoolManager.Instance.GetGameObj("OO1").SetPosition(Vector3.one * 3);
            PoolManager.Instance.GetGameObj("smg1").SetPosition(Vector3.one * -4);
            PoolManager.Instance.GetGameObj("sniper2").SetPosition(Vector3.one * 13);
            PoolManager.Instance.GetGameObj("OO2").SetPosition(Vector3.one * 5);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
