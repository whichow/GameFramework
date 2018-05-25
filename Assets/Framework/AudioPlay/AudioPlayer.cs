using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;

    public bool IsPlaying
    {
        get
        {
            return _audioSource.isPlaying;
        }
    }

    public bool Loop
    {
        get
        {
            return _audioSource.loop;
        }
        set
        {
            _audioSource.loop = value;
        }
    }

    private AudioClip Clip
    {
        get
        {
            return _audioSource.clip;
        }
        set
        {
            _audioSource.clip = value;
        }
    }

    private float Time
    {
        get
        {
            return _audioSource.time;
        }
        set
        {
            _audioSource.time = value;
        }
    }

    private float Volum
    {
        get
        {
            return _audioSource.volume;
        }
        set
        {
            _audioSource.volume = value;
        }
    }

    void Awake()
    {
        _Init();
    }

    private void _Init()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="audio">音频片段</param>
    /// <param name="loop">是否循环</param>
    /// <param name="volum">声音大小</param>
    public void Play(AudioClip audio, bool loop = false, float volum = 1f)
    {
        _audioSource.clip = audio;
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    /// <summary>
    /// 暂停播放
    /// </summary>
    public void Pause()
    {
        _audioSource.Pause();
    }

    /// <summary>
    /// 恢复播放
    /// </summary>
    public void UnPause()
    {
        _audioSource.UnPause();
    }
    
    /// <summary>
    /// 停止播放
    /// </summary>
    public void Stop()
    {
        _audioSource.Stop();
    }
}
