using System;
using System.IO;
using UnityEngine;
/// <summary>
/// ��Ϸ�ںˣ��������Դ
/// ����ֻ����һ��
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
            Debuger.LogCyan("�������ж��GameKernel����ֻ����һ������");
            willKilled = true;
            Destroy(this);
            return;
        }
        GameKernelCore.StartGame();

    }

}