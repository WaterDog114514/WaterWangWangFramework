using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��ʱ�������� ��Ҫ���ڿ�����ֹͣ�����õȵȲ����������ʱ��
/// </summary>
public class TimerManager : Singleton<TimerManager>
{
  
    //��ʱ����
    private Dictionary<int, TimerObj> dic_TimerObj = new Dictionary<int, TimerObj>();
    //���������еļ�ʱ��
    private Dictionary<int, Coroutine> dic_TimerCoroutine = new Dictionary<int, Coroutine>();
    public float UpdateIntervalTime;
    public TimerObj StartNewTimer(TimerObj.TimerType type, float totaltime, UnityAction Callback = null)
    {
        //��ȡһ��
        TimerObj timerObj =  new TimerObj(); //ObjectManager.Instance.getDataObjFromPool<TimerObj>();
        timerObj.IntiTimer(type, totaltime, Callback);
        //�����ֵ�
        dic_TimerObj.Add(timerObj.ID, timerObj);
        StartTimer(timerObj.ID);
        return timerObj;
    }

    //��ʼ��������
    public void InstantiateModule()
    {
        UpdateIntervalTime = 0.1f;
    }
    //�Ƴ���ʱ����������뻺���
    public void DestroyTimer(TimerObj obj)
    {
        StopTimer(obj.ID);

        //ObjectManager.Instance.DestroyObj(obj);
        dic_TimerObj.Remove(obj.ID);
    }

    //������ͣ��ǰ��ʱ����ʱ�����߼�
    public void StartOrPauseAllTimer(bool IsStart)
    {
        if (IsStart)
            foreach (var id in dic_TimerObj.Keys)
                StopTimer(id);
        else
            foreach (var id in dic_TimerObj.Keys)
                StartTimer(id);

    }
    //���м�ʱ�������߼�

    public void StartTimer(int id)
    {
        //���������⣬��Ҫ����dic���
        if (!dic_TimerCoroutine.ContainsKey(id))
            dic_TimerCoroutine.Add(id, UpdateSystem.Instance.StartCoroutine(dic_TimerObj[id].UpdateTime()));
        else
        {
            Debug.LogWarning($"��ʱ��(id:{id})�Ѿ����ڼ�ʱ�������ظ�������ʱ����");
        }

    }
    public void StopTimer(int id)
    {
        if (!dic_TimerCoroutine.ContainsKey(id))
        {
            Debug.LogWarning($"��ʱ��(id:{id})�����ڻ���ֹͣ�������ظ�ֹͣ��ʱ����");
            return;
        }
        UpdateSystem.Instance.StopCoroutine(dic_TimerCoroutine[id]);
        dic_TimerCoroutine.Remove(id);
    }

}

