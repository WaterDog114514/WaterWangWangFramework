using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

/// <summary>
/// ��Ҳ�����
/// </summary>
public class PlayerOperatorProcessorManager : Singleton<PlayerOperatorProcessorManager>, IKernelSystem
{
    /// <summary>
    /// ���ڹܿصĲ�����
    /// </summary>
    /// Key:��Ϸϵͳ������
    public List<BaseOperatorProcessor> activeProcessor { get; private set; }
    public void InitializedKernelSystem()
    {  //��ʼ��
        activeProcessor = new List<BaseOperatorProcessor>();
         //�õ���������Type�������һ����
         var types = ReflectionHelper.GetSubclassesOfGenericType(typeof(PlayerOperatorProcessor<>));
        foreach (var itemType in types)
        {
            //����������
            var instance = Activator.CreateInstance(itemType) as BaseOperatorProcessor;
            activeProcessor.Add(instance);
        }
    }
}

