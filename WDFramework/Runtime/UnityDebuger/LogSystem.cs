/*------------------------------------------------------------------
*
* Title: ��ҵ����־ϵͳ 
*
* Description: ֧�ֱ����ļ�д�롢�Զ�����ɫ��־��FPSʵʱ��ʾ���ֻ���־����ʱ�鿴����־��������޳���ProtoBuffתJson
* 
* Author: ��Ѷ���� ����xy
*
* Date: 2023.8.13
*
* Modify: 
-------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSystem : MonoBehaviour
{
    void Awake()
    {

#if OPEN_LOG
        //Debuger.InitLog(new LogConfig
        //{
        //    openLog = true,
        //    openTime = true,
        //    showThreadID = true,
        //    showColorName = true,
        //    logSave = true,
        //    showFPS = true,
        //});
        //Debuger.Log("Log");
        //Debuger.LogWarning("LogWarning");
        Debuger.LogError("LogError");
        Debuger.ColorLog(LogColor.Red, "ColorLog");
        Debuger.LogGreen("LogGreen");
        Debuger.LogYellow("LogYellow");
#else
     Debug.unityLogger.logEnabled = false;
#endif
    }


}
