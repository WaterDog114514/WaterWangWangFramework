using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDEMO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        void PrintMessage(string message, int count)
        {
            Debug.Log($"{message} - Count: {count}");
        }

        Command<string, int> command = new Command<string, int>(PrintMessage, "Hello, Command Pattern with Two Parameters!", 5);
        command.Execute(); // Êä³ö: Hello, Command Pattern with Two Parameters! - Count: 5









        UIManager.Instance.PreLoadUIPanel("ui_prefab","DEMOPANEL","MAINPANEL", "LAJIPANEL");
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
