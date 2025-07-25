/*------------------------------------------------------------------
*
* Title: 毕业级日志系统 
*
* Description: 支持本地文件写入、自定义颜色日志、FPS实时显示、手机日志运行时查看、日志代码编译剔除、ProtoBuff转Json
* 
* Author: 腾讯课堂 铸梦xy
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
