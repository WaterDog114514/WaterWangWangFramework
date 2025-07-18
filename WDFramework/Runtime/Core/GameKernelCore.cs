using UnityEngine;
using System;
/// <summary>
/// ��Ϸ�ںˣ��������Դ
/// һ��������ʼҲ������������һ�еĺ���
/// </summary>
public static class GameKernelCore
{

    // ��Ϸ������ڣ�ÿ��������Ϸ������
    public static void StartGame()
    {
        Debug.Log("��ʽ������Ϸ");
        //������Ϸ��������
        // 1. ��ʼ���ں�ϵͳ
        RegisterKernelSystems();
        //2.��ʼ�����п�ܲ�ϵͳ
        InitializedAllFrameworkSystem();
        //3.ע������趨����ʼ�������趨
        InitializedCoreSetting();
        //4. ��ʽ���������������
        // �����һ���׶Σ���������Ŀ��ʵ��һ���̳�
        EnterInitializationState();
    }
    private static void EnterInitializationState()
    {
        Debug.Log("�ںˣ�����ɽ����ʼ���׶�");
        GameStageManager.Instance.SwitchToFirstStage();
    }
    /// <summary>
    /// ��ʼ��4���ں�ϵͳ
    /// </summary>
    private static void RegisterKernelSystems()
    {
        //��ʼ�����ϵͳ������
        FrameworkSystemManager.Instance.InitializedKernelSystem();
        //��ʼ����Ϸϵͳ������
        GameSystemManager.Instance.InitializedKernelSystem();
        //��ʼ����������ϵͳ������
        GameStageManager.Instance.InitializedKernelSystem();
        //��ʼ������ϵͳ
        UpdateSystem.Instance.InitializedKernelSystem();
        //��ʼ��������
        PlayerOperatorProcessorManager.Instance.InitializedKernelSystem();
    }
    /// <summary>
    /// ��ʼ�������趨
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
    /// ��ʼ�����п�ܲ�ϵͳ
    /// </summary>
    private static void InitializedAllFrameworkSystem()
    {
        FrameworkSystemManager.Instance.InitializedAllFrameworkSystem();
    }
    //��Ϸ�������رս��̵���
    public static void ExitGame()
    {

    }


}