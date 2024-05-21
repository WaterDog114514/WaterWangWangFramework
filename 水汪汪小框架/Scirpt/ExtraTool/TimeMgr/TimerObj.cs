using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 计时器对象 里面存储了计时器的相关数据
/// </summary>
public class TimerObj : DataObj
{
    public TimerType timerType;
    public enum TimerType
    {
        ReallyTime, ScaleTime
    }
    /// <summary>
    /// 总需要记的时间
    /// </summary>
    public float TotalTime;
    /// <summary>
    /// 总计时器已走时间
    /// </summary>
    private float CurrentTotalTime;
    /// <summary>
    /// 获取剩余时间
    /// </summary>
    public float GetSurplusTime => this.TotalTime - this.CurrentTotalTime;
    /// <summary>
    /// 间隔周期调用总需时间
    /// </summary>
    private float TotalInterCallbackTime;
    /// <summary>
    /// 间隔调用已走时间
    /// </summary>
    private float CurrentIntervalCallbackTime;
    private event UnityAction Event_IntervalCallback;
    private event UnityAction Event_FinishCallback;
    private WaitForSeconds waitSeconds;
    private WaitForSecondsRealtime waitRealtimeSeconds;

    /// <summary>
    /// 初始化计时器
    /// </summary>
    public void IntiTimer(TimerType timerType, float TotalTime, UnityAction callback)
    {
        this.timerType = timerType;
        this.TotalTime = TotalTime;
        this.Event_FinishCallback = callback;
        switch (timerType)
        {
            case TimerType.ReallyTime:
                waitRealtimeSeconds = new WaitForSecondsRealtime(TimerMgr.Instance.UpdateIntervalTime);
                break;
            case TimerType.ScaleTime:
                waitSeconds = new WaitForSeconds(TimerMgr.Instance.UpdateIntervalTime);
                break;
        }
    }



    /// <summary>
    /// 重启计时器，会刷新所有记录
    /// </summary>
    /// <returns></returns>
    public void ReStart(float TotalTime)
    {
        //有待完成！！！
        TimerMgr.Instance.StartTimer(this.ID);
    }
    public void Stop()
    {
       TimerMgr.Instance.StopTimer(this.ID);
    }
    /// <summary>
    /// 销毁计时器方法
    /// </summary>
    public void Destroy()
    {
        ClearTimer();
        TimerMgr.Instance.DestroyTimer(this);
    }
    

    public TimerObj SetIntervalCallback(float totalIntervalTime, params UnityAction[] action)
    {
        this.TotalInterCallbackTime = totalIntervalTime;
        AddIntervalCallback(action);
        return this;
    }
    public TimerObj AddIntervalCallback(params UnityAction[] action)
    {
        for (int i = 0; i < action.Length; i++)
            Event_IntervalCallback += action[i];
        return this;
    }
    public TimerObj AddFinishCallback(params UnityAction[] action)
    {
        for (int i = 0; i < action.Length; i++)
            Event_FinishCallback += action[i];
        return this;
    }
    //清除计时器操作
    public void ClearTimer()
    {
        TotalTime = -1;
        CurrentTotalTime = 0;
        CurrentIntervalCallbackTime = 0;
        Event_FinishCallback = null;
        Event_IntervalCallback = null;
    }
    /// <summary>
    /// 更新自己时间
    /// </summary>
    public IEnumerator UpdateTime()
    {
        while (true)
        {
    
            switch (timerType)
            {
                case TimerType.ReallyTime:
                    yield return waitRealtimeSeconds;
                    break;
                case TimerType.ScaleTime:
                    yield return waitSeconds;
                    break;
            }
            CurrentTotalTime += TimerMgr.Instance.UpdateIntervalTime;
            CurrentIntervalCallbackTime += TimerMgr.Instance.UpdateIntervalTime;
            //到触发间隔了
            if (CurrentIntervalCallbackTime >= TotalInterCallbackTime)
            {

                CurrentIntervalCallbackTime = 0;
                Event_IntervalCallback?.Invoke();
            }
            //到总时间了 停止计时了
            if (CurrentTotalTime >= TotalTime)
            {
                Event_FinishCallback?.Invoke();
                Destroy();
            }

        }
    }

}
