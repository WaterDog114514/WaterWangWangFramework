using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDEMO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.PreLoadUIPanel("ui_prefab","DEMOPANEL");
        ResLoader.Instance.StartPreload();
    }
    private void OnGUI()
    {
        if (GUILayout.Button("okoko")) { 
         UIManager.Instance.ShowPanel<DEMOPANEL>();
          //  UIManager.Instance.LoadPanel<DEMOPANEL>();
        }
        if (GUILayout.Button("NONONO"))
        {
            UIManager.Instance.HidePanel<DEMOPANEL>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
