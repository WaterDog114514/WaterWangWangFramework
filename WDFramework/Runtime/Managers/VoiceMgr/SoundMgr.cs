using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 单个音效 用来管理单个音效的播放，销毁
/// </summary>
public class Sound
{
    /// <summary>
    /// 是否在使用中 主要是实现音效复用逻辑
    /// </summary>
    public bool IsUsing;
    public AudioSource audio;
    /// <summary>
    /// 销毁音效，将其放入对象池子
    /// </summary>
    public void Destory()
    {
        audio.Stop();
        IsUsing = false;
        audio.clip = null;
      //  PoolManager.Instance.DestroyObj(audio.gameObject);
        SoundMgr.Instance.UnUsedSounds.Enqueue(this);
    }
    public void Stop()
    {
        audio?.Stop();
    }
    public void Pause()
    {
        audio?.Pause();
    }
    public void Play()
    {
        IsUsing = true;
        audio?.Play();
    }
}

/// <summary>
/// 音效管理器
/// </summary>
public class SoundMgr : Singleton<SoundMgr>
{
    //统一管理全部音效
    private List<Sound> soundList = new List<Sound>();
    //管理没有在使用的音效，没有使用的音效方方便拿出来直接用
    public Queue<Sound> UnUsedSounds = new Queue<Sound>();
    //全局音效音量大小
    private float soundValue = 0.1f;
    //音效是否在播放
    private bool soundIsPlay = true;
    private void Update()
    {
        if (!soundIsPlay)
            return;
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效在AB包中的名字</param>
    /// <param name="isLoop">循环播放吗</param>
    /// <param name="Is3D">是不是3D音效</param>
    /// <param name="isSync"></param>
    /// <param name="callBack"></param>
    public Sound PlaySound(string name, bool isLoop = false, bool Is3D = false, bool isSync = false, UnityAction<AudioSource> callBack = null)
    {

        Sound sound = null;
        //先看看有没有未使用待命音效，有了就直接用，没有新建一个
        if (UnUsedSounds.Count > 0)
            sound = UnUsedSounds.Dequeue();
        else
        {
            sound = new Sound();
            //添加到统一管理列表中
            soundList.Add(sound);
        }
        //加载音效资源 进行播放
      // ABLoaderMgr.Instance.LoadResAsync<AudioClip>("voice", name, (clip) =>
      // {
      //     //从缓存池中取出音效对象得到对应组件
      //     AudioSource source = PoolManager.Instance.GetObj("Sound/soundObj").GetComponent<AudioSource>();
      //     //基本设置
      //     sound.audio = source;
      //     source.clip = clip;
      //     source.loop = isLoop;
      //     //音量设置
      //     source.volume = soundValue;
      //     //2d 3d设置
      //     if (!Is3D)
      //     {
      //         source.spatialBlend = 0;
      //     }
      //     else
      //     {
      //         source.spatialBlend = 1;
      //         source.maxDistance = 2;
      //     }
      //     sound.Play();
      //     callBack?.Invoke(source);
      // }, isSync);
        return sound;
    }
    /// <summary>
    /// 改变全局音效大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundValue(float v)
    {
        //音量大小不变，就应该返回避免重复遍历浪费性能
        if (v == soundValue) return;
        soundValue = v;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].audio.volume = v;
        }
    }

    /// <summary>
    /// 继续播放或者暂停所有音效
    /// </summary>
    /// <param name="isPlay">是否是继续播放 true为播放 false为暂停</param>
    public void PlayOrPauseSound(bool isPlay)
    {
        if (isPlay)
        {
            soundIsPlay = true;
            for (int i = 0; i < soundList.Count; i++)
                soundList[i].Play();
        }
        else
        {
            soundIsPlay = false;
            for (int i = 0; i < soundList.Count; i++)
                soundList[i].Pause();
        }
    }
    //清空所有音效
    public void ClearSound()
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].Stop();
            soundList[i].audio.clip = null;
            soundList[i].Destory();

        }
        UnUsedSounds.Clear();
    }
}
