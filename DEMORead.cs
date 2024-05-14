using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMORead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResLoader.Instance.CreatePreloadFromExcel<MonsterPZContainer>("ArtPath");
        ResLoader.Instance.CreatePreloadFromExcel<TestInfoContainer>("resPath");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
