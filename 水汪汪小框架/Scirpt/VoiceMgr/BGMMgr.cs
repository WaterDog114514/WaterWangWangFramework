using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 背景音乐管理器
/// </summary>
public class BGMMgr : Singleton_UnMono<BGMMgr>
{
    /// <summary>
    /// 背景音乐播放组件
    /// </summary>
    private AudioSource BGM_Player = null;

    /// <summary>
    ///  背景音乐大小
    /// </summary>
    public float BGM_Value
    {
        get => bgm_Value;
        set
        {
            //改值就改音量大小
            if (bgm_Value == value) return;
            bgm_Value = value;
            ChangeVoice_BGMValue(value);
        }
    }
    private float bgm_Value=0.2F;
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBGM(string name)
    {
        //动态创建播放背景音乐的组件 并且 不会过场景移除 
        //保证背景音乐在过场景时也能播放
        if (BGM_Player == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BGM_Player";
            Object.DontDestroyOnLoad(obj);
            BGM_Player = obj.AddComponent<AudioSource>();
        }

        //根据传入的背景音乐名字 来播放背景音乐
     // ABLoaderMgr.Instance.LoadResAsync<AudioClip>("Voice", name, (clip) =>
     // {
     //     BGM_Player.clip = clip;
     //     BGM_Player.loop = true;
     //     BGM_Player.volume = BGM_Value;
     //     BGM_Player.Play();
     // });
    }
    //停止背景音乐
    public void StopVoice_BGM()
    {
        if (BGM_Player == null)
            return;
        BGM_Player.Stop();
    }
    //暂停背景音乐
    public void PauseVoice_BGM()
    {
        if (BGM_Player == null)
            return;
        BGM_Player.Pause();
    }
    //设置背景音乐大小
    public void ChangeVoice_BGMValue(float v)
    {
        BGM_Value = v;
        if (BGM_Player == null)
            return;
        BGM_Player.volume = BGM_Value;
    }

}
