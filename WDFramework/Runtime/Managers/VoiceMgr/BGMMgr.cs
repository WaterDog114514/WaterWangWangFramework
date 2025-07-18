using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �������ֹ�����
/// </summary>
public class BGMMgr : Singleton<BGMMgr>
{
    /// <summary>
    /// �������ֲ������
    /// </summary>
    private AudioSource BGM_Player = null;

    /// <summary>
    ///  �������ִ�С
    /// </summary>
    public float BGM_Value
    {
        get => bgm_Value;
        set
        {
            //��ֵ�͸�������С
            if (bgm_Value == value) return;
            bgm_Value = value;
            ChangeVoice_BGMValue(value);
        }
    }
    private float bgm_Value=0.2F;
    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="name"></param>
    public void PlayBGM(string name)
    {
        //��̬�������ű������ֵ���� ���� ����������Ƴ� 
        //��֤���������ڹ�����ʱҲ�ܲ���
        if (BGM_Player == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BGM_Player";
            Object.DontDestroyOnLoad(obj);
            BGM_Player = obj.AddComponent<AudioSource>();
        }

        //���ݴ���ı����������� �����ű�������
     // ABLoaderMgr.Instance.LoadResAsync<AudioClip>("Voice", name, (clip) =>
     // {
     //     BGM_Player.clip = clip;
     //     BGM_Player.loop = true;
     //     BGM_Player.volume = BGM_Value;
     //     BGM_Player.Play();
     // });
    }
    //ֹͣ��������
    public void StopVoice_BGM()
    {
        if (BGM_Player == null)
            return;
        BGM_Player.Stop();
    }
    //��ͣ��������
    public void PauseVoice_BGM()
    {
        if (BGM_Player == null)
            return;
        BGM_Player.Pause();
    }
    //���ñ������ִ�С
    public void ChangeVoice_BGMValue(float v)
    {
        BGM_Value = v;
        if (BGM_Player == null)
            return;
        BGM_Player.volume = BGM_Value;
    }

}
