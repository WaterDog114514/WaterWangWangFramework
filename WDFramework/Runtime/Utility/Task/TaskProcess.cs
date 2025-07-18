using System;
using UnityEngine;

/// <summary>
/// 任务进度信息
/// </summary>
public class TaskProcess
{
    public bool isFinishTask;
    /// <summary>
    /// 正在进行中任务进度
    /// </summary>
    public float OngoingProcess;
    /// <summary>
    /// 总任务进度 =当前任务小百分比 + 已完成任务百分比
    /// </summary>
    public float TotalTaskProcess => (OngoingProcess / TotalTaskCount) + (FinishedTaskCount * 1.0F / TotalTaskCount);
    /// <summary>
    /// 已完成的任务数量
    /// </summary>
    public int FinishedTaskCount;
    /// <summary>
    /// 总任务数量
    /// </summary>
    public int TotalTaskCount;
    public TaskProcess()
    {

    }
    public TaskProcess(int totalTaskCount)
    {
        SetTask(totalTaskCount);
    }
    public void SetTask(int totalTaskCount)
    {
        isFinishTask = false;
        TotalTaskCount = totalTaskCount;
    }
    /// <summary>
    /// 更新进度信息 精确到小数点进度
    /// </summary>
    public void UpdateProcess(float addtional)
    {
        OngoingProcess += addtional;
        if (OngoingProcess >= 1f)
        {
            UpdateTaskCount(1);
        }
    }
    public void UpdateTaskCount(int finishCount)
    {
        //清空单个进行中任务进度
        OngoingProcess = 0;
        //任务加载数量计算
        if (FinishedTaskCount < TotalTaskCount)
            FinishedTaskCount += finishCount;

        //任务加载完毕
        if (FinishedTaskCount >= TotalTaskCount)
        {
            isFinishTask = true;
        }
    }
    public static TaskProcess CreateTaskProcess(int totalTaskCount)
    {
        return new TaskProcess(totalTaskCount);
    }

    public void DemoTest()
    {
        Debug.Log($"总任务进度：{TotalTaskProcess} 已完成任务：{FinishedTaskCount} 总任务数：{TotalTaskCount}");
    }
}
