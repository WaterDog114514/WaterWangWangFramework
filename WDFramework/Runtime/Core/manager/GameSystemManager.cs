using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class GameSystemManager : Singleton<GameSystemManager>, IKernelSystem
{

    /// <summary>
    /// �Ѿ������ϵͳ
    /// </summary>
    /// Key:��Ϸϵͳ������
    public Dictionary<Type, IGameSystem> activeSystems { get; private set; }

    /// <summary>
    /// ��ͬ���������Զ�����ϵͳ
    /// </summary>
    /// Key �������ڵ����� Value:������ϵͳ��List
    private Dictionary<Type, List<Type>> gameStageAutoLauncherSystems;

    public void InitializedKernelSystem()
    {  //��ʼ��
        activeSystems = new Dictionary<Type, IGameSystem>();
        gameStageAutoLauncherSystems = new Dictionary<Type, List<Type>>();
        //�õ���������Type�������һ����
        var types = ReflectionHelper.GetSubclasses(typeof(GameStage));
        foreach (var itemType in types)
        {
            gameStageAutoLauncherSystems.Add(itemType, new List<Type>());
        }
        //ע�������¼�
        EventCenterSystem.Instance.AddEventListener<E_FrameworkEvent, GameStage>(E_FrameworkEvent.OnExitGameStage, OnNewStageCleanup, 1);
        //ע���л��¼�
        EventCenterSystem.Instance.AddEventListener<E_FrameworkEvent, GameStage>(E_FrameworkEvent.OnEnterGameStageRegisterGameSystem, OnNewStageAutoLauncherSystem, 2);
    }
    /// <summary>
    /// ͨ�� Type ע��� Mono ϵͳ
    /// </summary>
    /// <param name="systemType">ϵͳ����</param>
    private void LauncherUnMonoSystem(Type systemType)
    {
        // ��������Ƿ�����Լ��
        if (systemType.IsClass && !systemType.IsAbstract && typeof(IGameSystem).IsAssignableFrom(systemType))
        {
            // ʹ�÷��䴴��ʵ��
            object systemInstance = Activator.CreateInstance(systemType);
            if (systemInstance is IGameSystem system)
            {
                activeSystems.Add(systemType, system);
                //Ϊ�¼���ϵͳע������¼�(����еĻ�)
                RegisterGameSystemUpdate(systemInstance as IGameSystem);
            }
        }
    }
    /// <summary>
    /// ͨ�� Type ע�� Mono ϵͳ
    /// </summary>
    /// <param name="systemType">ϵͳ����</param>
    private void LauncherMonoSystem(Type systemType)
    {
        // ��������Ƿ�����Լ��
        if (systemType.IsClass && !systemType.IsAbstract && typeof(MonoBehaviour).IsAssignableFrom(systemType) && typeof(IGameSystem).IsAssignableFrom(systemType))
        {
            // ���� GameObject ���������
            GameObject systemObject = new GameObject(systemType.Name);
            IGameSystem systemInstance = systemObject.AddComponent(systemType) as IGameSystem;
            if (systemInstance != null)
            {
                activeSystems.Add(systemType, systemInstance);
                //Ϊ�¼���ϵͳע������¼�(����еĻ�)
                RegisterGameSystemUpdate(systemInstance);
            }
        }
    }
    /// <summary>
    /// ע���Զ�������ϵͳ
    /// </summary>
    public void RegisterActiveSystem( Type System,params Type[] AutoStageList)
    {
        foreach (var gameStage in AutoStageList)
        {

            var systemsList = gameStageAutoLauncherSystems[gameStage];
            if (!typeof(IGameSystem).IsAssignableFrom(System))
            {
                Debug.Log("��ϵͳ������Ϸϵͳ���޷�ע��");
                return;
            }
            if (systemsList.Contains(System))
            {
                Debug.Log("��ϵͳ�Ѿ���ӹ��ˡ��޷��ظ����");
                return;
            }
            systemsList.Add(System);
        }
    }
    /// <summary>
    /// �׶��л��¼�����
    /// </summary>
    /// <param name="phase">Ŀ��׶�</param>
    private void OnNewStageAutoLauncherSystem(GameStage NewState)
    {
        //ͨ���½׶ε����ͣ��õ�Ҫ�л��׶ε�����ϵͳType��List
        List<Type> AutoLauncherSystems = gameStageAutoLauncherSystems[NewState.GetType()];
        //��ȡ�ж�
        if (AutoLauncherSystems == null)
        {
            throw new Exception("�޷��õ�State��Typeö��");
        }

        //��������������������Ϸϵͳ
        foreach (Type systemType in AutoLauncherSystems)
        {
            //�Ѿ���������ϵͳ�Ͳ�Ҫ�ظ�������
            if (activeSystems.ContainsKey(systemType))
            {
                Debug.Log("�Ѿ���������" + systemType.Name);  
                continue;
            }
            // �ж��Ƿ�Ϊ MonoBehaviour ϵͳ
            if (typeof(MonoBehaviour).IsAssignableFrom(systemType))
            {
                // ���� RegisterMonoSystem
                LauncherMonoSystem(systemType);
            }
            else
            {
                // ���� RegisterSystem
                LauncherUnMonoSystem(systemType);
            }
        }
    }
    /// <summary>
    /// ����ǰ�׶β���Ҫ��ϵͳ
    /// </summary>
    private void OnNewStageCleanup(GameStage newStage)
    {
        //��õ�ǰ�������ڵ�����
        Type CurrentStage = newStage.GetType();
        //�õ���ǰ��������Ӧ���е�List
        List<Type> AutoLauncherSystems = gameStageAutoLauncherSystems[CurrentStage];
        //Ҫ�����ϵͳ
        List<Type> deleteSystems = new List<Type>();
        // �������м����ϵͳ
        foreach (var systemType in activeSystems.Keys)
        {
            //  Ҫö�ٻ���������˼��
            // ���ϵͳ�������½׶Σ�������
            if (!AutoLauncherSystems.Contains(systemType))
            {
                //����ɾ���б�
                deleteSystems.Add(systemType);
            }
        }
        //��������ɾ������
        for (int i = deleteSystems.Count - 1; i >= 0; i--)
        {
            var systemType = deleteSystems[i];
            var system = activeSystems[systemType];
            //�ж��Ƿ���IUpdate��������Ҳ���Ƴ��˰�
            if (typeof(IUpdate).IsAssignableFrom(system.GetType()))
            {
                UpdateSystem.Instance.RemoveUpdateListener(E_UpdateLayer.GameSystem, (system as IUpdate));
            }
            //����ϵͳ��
            system.DestorySystem();
            //�Ӽ������Ƴ�
            activeSystems.Remove(systemType);
        }
        Debug.Log("������ϡ���������" + deleteSystems.Count);
    }
    /// <summary>
    /// �Զ�����/ע�����Ϸϵͳ���еĸ����߼�
    /// </summary>
    /// <param name="phase"></param>
    private void RegisterGameSystemUpdate(IGameSystem system)
    {
        //��������Ϸϵͳ�Ƿ�ӵ�и�����
        //���ݼ̳в�ͬ��mono���£���������ͬupdate
        if (typeof(IUpdate).IsAssignableFrom(system.GetType()))
        {
            UpdateSystem.Instance.AddUpdateListener(E_UpdateLayer.GameSystem, (system as IUpdate));
        }
    }
}

