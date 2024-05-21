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
    public float GetSurplusTime => TotalTime - CurrentTotalTime;

    private bool isNeedStop = false;

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
    /// <summary>
    /// 初始化计时器
    /// </summary>
    public void IntiTimer(TimerType timerType, float TotalTime, UnityAction callback)
    {
        this.timerType = timerType;
        this.TotalTime = TotalTime;
        this.Event_FinishCallback = callback;
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
        isNeedStop = true;
    }
    public void Destroy()
    {
        ClearTimer();
        TimerMgr.Instance.DestroyTimer(this);
        isNeedStop = true;
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
            //判断要停止吗
            if (isNeedStop)
            {
                isNeedStop = false;
                yield break;
            }

            switch (timerType)
            {
                case TimerType.ReallyTime:
                    yield return TimerMgr.Instance.waitSecondsRealtime;
                    break;
                case TimerType.ScaleTime:
                    yield return TimerMgr.Instance.waitSecondsRealtime;
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
