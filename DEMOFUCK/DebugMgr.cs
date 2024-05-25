using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMgr : Singleton_AutoMono<DebugMgr>
{

    public Text rizi;
    public InputField rizi2;
    public InputField rizi3;
    private void Awake()
    {
        Application.logMessageReceived += HandleLog;
        rizi = GameObject.Find("R1").GetComponent<Text>();
        rizi2 = GameObject.Find("R2").GetComponent<InputField>();
        rizi3 = GameObject.Find("R3").GetComponent<InputField>();
    }
    void Update()
    {
        
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        rizi2.text+=$"{type.ToString()}:{logString}\n";
        rizi3.text+=$"{type.ToString()}:{stackTrace}\n";
    }
}
