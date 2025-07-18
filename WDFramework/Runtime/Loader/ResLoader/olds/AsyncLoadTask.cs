using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// �첽��������ֻ���첽���ز���Ҫͨ��������м���
/// </summary>
public class AsyncLoadTask
{
    public float LoadProcess;
    public Res ResInfo;
    protected bool IsStartedLoad = false;
    /// <summary>
    /// �ж�ĳ��Դ�����Ƿ����
    /// </summary>
    public bool isFinish => LoadProcess == 1 ? true : false;
    /// <summary>
    /// ֱ�ӵõ�ĳ���سɹ������Դ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetAsset<T>() where T : UnityEngine.Object => ResInfo.GetAsset<T>();

    /// <summary>
    /// ָ����У�����ʱ�����ǰ�洢ָ�������Ϻ�����
    /// </summary>
    private CommandQueue commandQueue = new CommandQueue();
    public void AddCallbackCommand(UnityAction callback)
    {
        AddCallbackCommand(new Command(callback));
    }
    public void AddCallbackCommand(ICommand command)
    {
        if (commandQueue == null)
        {
            Debug.LogError("����ɼ������񣬲�������ӻص�������");
            return;
        }
        commandQueue.AddCommand(command);
    }

    /// <summary>
    /// �����ص����Ľ��ȣ���Դ��ֵ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resInfo"></param>
    public void FinishTask(Res resInfo)
    {
        LoadProcess = 1;
        ResInfo = resInfo;
        //�������
        commandQueue.ExecuteCommands();
        commandQueue = null;
    }

    private Func<Coroutine> LoadOperation;
    /// <summary>
    /// ��ʼ��Э�̼��ز������������������ͨ��ί�п���ʲôʱ��ʼִ������
    /// </summary>
    /// <param name="operation"></param>
    public void IntiLoadOperation(IEnumerator operation)
    {
        LoadOperation += () =>
        {
            return UpdateSystem.Instance.StartCoroutine(operation);
        };
    }
    public Coroutine StartAsyncLoad()
    {
        if (LoadOperation == null)
        {
            Debug.LogError("������ʧ�ܣ�������Ҫ���ص���Դ");
            return null;
        }
        return LoadOperation?.Invoke();
    }
}