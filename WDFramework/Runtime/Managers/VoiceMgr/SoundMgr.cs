using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ������Ч ������������Ч�Ĳ��ţ�����
/// </summary>
public class Sound
{
    /// <summary>
    /// �Ƿ���ʹ���� ��Ҫ��ʵ����Ч�����߼�
    /// </summary>
    public bool IsUsing;
    public AudioSource audio;
    /// <summary>
    /// ������Ч���������������
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
/// ��Ч������
/// </summary>
public class SoundMgr : Singleton<SoundMgr>
{
    //ͳһ����ȫ����Ч
    private List<Sound> soundList = new List<Sound>();
    //����û����ʹ�õ���Ч��û��ʹ�õ���Ч�������ó���ֱ����
    public Queue<Sound> UnUsedSounds = new Queue<Sound>();
    //ȫ����Ч������С
    private float soundValue = 0.1f;
    //��Ч�Ƿ��ڲ���
    private bool soundIsPlay = true;
    private void Update()
    {
        if (!soundIsPlay)
            return;
    }
    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="name">��Ч��AB���е�����</param>
    /// <param name="isLoop">ѭ��������</param>
    /// <param name="Is3D">�ǲ���3D��Ч</param>
    /// <param name="isSync"></param>
    /// <param name="callBack"></param>
    public Sound PlaySound(string name, bool isLoop = false, bool Is3D = false, bool isSync = false, UnityAction<AudioSource> callBack = null)
    {

        Sound sound = null;
        //�ȿ�����û��δʹ�ô�����Ч�����˾�ֱ���ã�û���½�һ��
        if (UnUsedSounds.Count > 0)
            sound = UnUsedSounds.Dequeue();
        else
        {
            sound = new Sound();
            //��ӵ�ͳһ�����б���
            soundList.Add(sound);
        }
        //������Ч��Դ ���в���
      // ABLoaderMgr.Instance.LoadResAsync<AudioClip>("voice", name, (clip) =>
      // {
      //     //�ӻ������ȡ����Ч����õ���Ӧ���
      //     AudioSource source = PoolManager.Instance.GetObj("Sound/soundObj").GetComponent<AudioSource>();
      //     //��������
      //     sound.audio = source;
      //     source.clip = clip;
      //     source.loop = isLoop;
      //     //��������
      //     source.volume = soundValue;
      //     //2d 3d����
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
    /// �ı�ȫ����Ч��С
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundValue(float v)
    {
        //������С���䣬��Ӧ�÷��ر����ظ������˷�����
        if (v == soundValue) return;
        soundValue = v;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].audio.volume = v;
        }
    }

    /// <summary>
    /// �������Ż�����ͣ������Ч
    /// </summary>
    /// <param name="isPlay">�Ƿ��Ǽ������� trueΪ���� falseΪ��ͣ</param>
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
    //���������Ч
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
