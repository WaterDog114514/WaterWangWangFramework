using System;
using UnityEngine;

/// <summary>
/// ���������Ϣ
/// </summary>
public class TaskProcess
{
    public bool isFinishTask;
    /// <summary>
    /// ���ڽ������������
    /// </summary>
    public float OngoingProcess;
    /// <summary>
    /// ��������� =��ǰ����С�ٷֱ� + ���������ٷֱ�
    /// </summary>
    public float TotalTaskProcess => (OngoingProcess / TotalTaskCount) + (FinishedTaskCount * 1.0F / TotalTaskCount);
    /// <summary>
    /// ����ɵ���������
    /// </summary>
    public int FinishedTaskCount;
    /// <summary>
    /// ����������
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
    /// ���½�����Ϣ ��ȷ��С�������
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
        //��յ����������������
        OngoingProcess = 0;
        //���������������
        if (FinishedTaskCount < TotalTaskCount)
            FinishedTaskCount += finishCount;

        //����������
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
        Debug.Log($"��������ȣ�{TotalTaskProcess} ���������{FinishedTaskCount} ����������{TotalTaskCount}");
    }
}
