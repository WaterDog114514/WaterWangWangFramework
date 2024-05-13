using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMODMEO : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
 

    }
    private void OnGUI()
    {
        if (GUILayout.Button("加载"))
        {

            AsyncLoadTask asyncLoadTask = ResLoader.Instance.LoadRes_Async<GameObject>("Prefab/smg1", (G) => { Instantiate(G);});
            asyncLoadTask.StartAsyncLoad();
            GameObject gg = ResLoader.Instance.LoadRes_Sync<GameObject>("Prefab/smg1");
            Instantiate(gg);

        }
        if (GUILayout.Button("后续加载"))
        {
            GameObject ins = ResLoader.Instance.LoadAB_Sync<GameObject>("gunprefab", "smg1");
            GameObject ins2 = ResLoader.Instance.LoadAB_Sync<GameObject>("gunprefab", "pistol1");
            Instantiate(ins);
            Instantiate(ins2);

            // task2 = ResLoader.Instance.LoadAB_Sync<GameObject>("gunprefab", "smg1");

            //     ResLoader.Instance.LoadABAllRes_Async<GameObject>("gunprefab", (qq) =>
            //     {
            //         foreach (var item in qq)
            //         {
            //             Instantiate(item);
            //         }
            //     });
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
