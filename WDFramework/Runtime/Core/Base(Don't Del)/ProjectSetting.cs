using System;
using UnityEngine;

/// <summary>
/// ��Ŀ�趨��ʼ����
/// ������Ŀ���趨�ĳ�ʼ��
/// </summary>
public abstract class InitializedProjectSetting
{
    protected GameStageManager GSManager => GameStageManager.Instance;
    /// <summary>
    /// ��ʼ����Ŀ���趨
    /// </summary>
   public abstract void RegisterProjectSetting();
}