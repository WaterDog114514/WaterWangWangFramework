using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 计时器管理器 主要用于开启、停止、重置等等操作来管理计时器
/// </summary>
public class TimerManager : Singleton<TimerManager>
{
  
    //计时容器
    private Dictionary<int, TimerObj> dic_TimerObj = new Dictionary<int, TimerObj>();
    //正在启用中的计时器
    private Dictionary<int, Coroutine> dic_TimerCoroutine = new Dictionary<int, Coroutine>();
    public float UpdateIntervalTime;
    public TimerObj StartNewTimer(TimerObj.TimerType type, float totaltime, UnityAction Callback = null)
    {
        //创取一波
        TimerObj timerObj =  new TimerObj(); //ObjectManager.Instance.getDataObjFromPool<TimerObj>();
        timerObj.IntiTimer(type, totaltime, Callback);
        //加入字典
        dic_TimerObj.Add(timerObj.ID, timerObj);
        StartTimer(timerObj.ID);
        return timerObj;
    }

    //初始化管理器
    public void InstantiateModule()
    {
        UpdateIntervalTime = 0.1f;
    }
    //移除计时器，让其进入缓存池
    public void DestroyTimer(TimerObj obj)
    {
        StopTimer(obj.ID);

        //ObjectManager.Instance.DestroyObj(obj);
        dic_TimerObj.Remove(obj.ID);
    }

    //重启暂停当前计时器计时更新逻辑
    public void StartOrPauseAllTimer(bool IsStart)
    {
        if (IsStart)
            foreach (var id in dic_TimerObj.Keys)
                StopTimer(id);
        else
            foreach (var id in dic_TimerObj.Keys)
                StartTimer(id);

    }
    //所有计时器更新逻辑

    public void StartTimer(int id)
    {
        //开两次问题，需要声明dic求解
        if (!dic_TimerCoroutine.ContainsKey(id))
            dic_TimerCoroutine.Add(id, UpdateSystem.Instance.StartCoroutine(dic_TimerObj[id].UpdateTime()));
        else
        {
            Debug.LogWarning($"计时器(id:{id})已经正在计时，请勿重复开启计时！！");
        }

    }
    public void StopTimer(int id)
    {
        if (!dic_TimerCoroutine.ContainsKey(id))
        {
            Debug.LogWarning($"计时器(id:{id})不存在或已停止，请勿重复停止计时！！");
            return;
        }
        UpdateSystem.Instance.StopCoroutine(dic_TimerCoroutine[id]);
        dic_TimerCoroutine.Remove(id);
    }

}

