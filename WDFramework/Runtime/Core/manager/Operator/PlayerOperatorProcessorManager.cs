using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

/// <summary>
/// 玩家操作层
/// </summary>
public class PlayerOperatorProcessorManager : Singleton<PlayerOperatorProcessorManager>, IKernelSystem
{
    /// <summary>
    /// 正在管控的操作层
    /// </summary>
    /// Key:游戏系统的类型
    public List<BaseOperatorProcessor> activeProcessor { get; private set; }
    public void InitializedKernelSystem()
    {  //初始化
        activeProcessor = new List<BaseOperatorProcessor>();
         //得到所有周期Type，先添加一波先
         var types = ReflectionHelper.GetSubclassesOfGenericType(typeof(PlayerOperatorProcessor<>));
        foreach (var itemType in types)
        {
            //创建操纵者
            var instance = Activator.CreateInstance(itemType) as BaseOperatorProcessor;
            activeProcessor.Add(instance);
        }
    }
}

