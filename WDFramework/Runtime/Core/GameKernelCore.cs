using UnityEngine;
using System;
/// <summary>
/// 游戏内核，万物的起源
/// 一切由他开始也由它结束，是一切的核心
/// </summary>
public static class GameKernelCore
{

    // 游戏启动入口，每当启动游戏后会调用
    public static void StartGame()
    {
        Debug.Log("正式启动游戏");
        //整个游戏启动流程
        // 1. 初始化内核系统
        RegisterKernelSystems();
        //2.初始化所有框架层系统
        InitializedAllFrameworkSystem();
        //3.注入核心设定，初始化所有设定
        InitializedCoreSetting();
        //4. 正式开启框架生命周期
        // 进入第一个阶段，必须在项目层实现一个继承
        EnterInitializationState();
    }
    private static void EnterInitializationState()
    {
        Debug.Log("内核：已完成进入初始化阶段");
        GameStageManager.Instance.SwitchToFirstStage();
    }
    /// <summary>
    /// 初始化4大内核系统
    /// </summary>
    private static void RegisterKernelSystems()
    {
        //初始化框架系统管理器
        FrameworkSystemManager.Instance.InitializedKernelSystem();
        //初始化游戏系统管理器
        GameSystemManager.Instance.InitializedKernelSystem();
        //初始化生命周期系统管理器
        GameStageManager.Instance.InitializedKernelSystem();
        //初始化更新系统
        UpdateSystem.Instance.InitializedKernelSystem();
        //初始化操作层
        PlayerOperatorProcessorManager.Instance.InitializedKernelSystem();
    }
    /// <summary>
    /// 初始化核心设定
    /// </summary>
    private static void InitializedCoreSetting()
    {
        var list =   ReflectionHelper.GetSubclasses(typeof(InitializedRegister));
        foreach (var classType in list)
        {
            Activator.CreateInstance(classType);
        }
    }
    /// <summary>
    /// 初始化所有框架层系统
    /// </summary>
    private static void InitializedAllFrameworkSystem()
    {
        FrameworkSystemManager.Instance.InitializedAllFrameworkSystem();
    }
    //游戏结束，关闭进程调用
    public static void ExitGame()
    {

    }


}