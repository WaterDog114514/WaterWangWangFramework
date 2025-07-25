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
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Protobuff 转为Josn 字符串，并进行打印
/// </summary>
public class ProtoBuffConvert 
{
    /// <summary>
    /// Protobuff 转为Josn 字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="proto"></param>
    public static void ToJson<T>(T proto)
    {
        //Debuger.Log(JsonConvert.SerializeObject(proto));
    }
}
