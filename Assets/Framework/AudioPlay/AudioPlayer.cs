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

    public void Play(AudioClip audio, bool loop = false, float volum = 1f)
    {
        _audioSource.clip = audio;
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    private void Pause()
    {
        _audioSource.Pause();
    }

    private void UnPause()
    {
        _audioSource.UnPause();
    }

    private void Stop()
    {
        _audioSource.Stop();
    }
}
