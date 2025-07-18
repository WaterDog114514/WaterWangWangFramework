using System;
using System.IO;
using UnityEngine;
/// <summary>
/// 游戏内核，万物的起源
/// 有且只能有一个
/// </summary>
public class GameKernelMono : MonoBehaviour
{
    private bool willKilled = false;
    public static GameKernelMono Instance { private set; get; }
    void Awake()
    {
        if(Instance== null)
            Instance = this;
        else
        {
            Debuger.LogCyan("场景上有多个GameKernel，请只保持一个！！");
            willKilled = true;
            Destroy(this);
            return;
        }
        GameKernelCore.StartGame();

    }

}