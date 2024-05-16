using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DEMORead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
  

        ResLoader.Instance.CreatePreloadTaskFromExcel<MonsterPZContainer>("ArtPath");
        ResLoader.Instance.StartPreload();
        // ResLoader.Instance.CreatePreloadFromExcel<TestInfoContainer>("resPath");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
