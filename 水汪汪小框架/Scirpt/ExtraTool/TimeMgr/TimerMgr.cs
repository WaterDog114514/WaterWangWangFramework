using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 计时器管理器 主要用于开启、停止、重置等等操作来管理计时器
/// </summary>
public class TimerMgr : Singleton_UnMono<TimerMgr>
{
    public TimerMgr()
    {
        IntiMgr();
    }
    //计时容器
    private Dictionary<int, TimerObj> dic_TimerObj = new Dictionary<int, TimerObj>();
    public float UpdateIntervalTime;
    public TimerObj StartNewTimer(TimerObj.TimerType type, float totaltime, UnityAction Callback = null)
    {
        //创取一波
        TimerObj timerObj = ObjectManager.Instance.getDataObjFromPool<TimerObj>();
        timerObj.IntiTimer(type, totaltime, Callback);
        //加入字典
        dic_TimerObj.Add(timerObj.ID, timerObj);
        StartTimer(timerObj.ID);
        return timerObj;
    }

    //初始化管理器
    public void IntiMgr()
    {
        UpdateIntervalTime = 0.1f;
        waitSeconds = new WaitForSeconds(UpdateIntervalTime);
        waitSecondsRealtime = new WaitForSecondsRealtime(UpdateIntervalTime);
    }

    public WaitForSeconds waitSeconds;
    public WaitForSecondsRealtime waitSecondsRealtime;
    //移除计时器
    public void DestroyTimer(TimerObj obj)
    {
        StopTimer(obj.ID);
        ObjectManager.Instance.DestroyObj(obj);
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
        MonoManager.Instance.StartCoroutine(dic_TimerObj[id].UpdateTime());
    }
    public void StopTimer(int id)
    {
        dic_TimerObj[id].Stop();
    }
}

