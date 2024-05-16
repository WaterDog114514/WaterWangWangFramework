using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
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
           ResLoader.Instance.LoadAB_Async<GameObject>("gunprefab", "smg1", (G) => { Instantiate(G); });
           ResLoader.Instance.LoadAB_Async<GameObject>("gunprefab", "assault2", (G) => { Instantiate(G); });
           ResLoader.Instance.LoadAB_Async<GameObject>("gunprefab", "sniper2", (G) => { Instantiate(G); });
           ResLoader.Instance.LoadAB_Async<GameObject>("gunprefab", "shotgun1", (G) => { Instantiate(G); });
           ResLoader.Instance.LoadAB_Async<GameObject>("gunprefab", "pistol1", (G) => { Instantiate(G); });

        }

        if (GUILayout.Button("后续加载"))
        {
           ResLoader.Instance.LoadAB_Async<GameObject>("prpr", "OO1", (G) => { Instantiate(G); });
           ResLoader.Instance.LoadAB_Async<GameObject>("prpr", "OO2", (G) => { Instantiate(G); });
        }
        if (GUILayout.Button("jianyan"))
        {
          ResLoader.Instance.Demo();
        }

    }
    // Update is called once per frame
    void Update()
    {
    }

}
