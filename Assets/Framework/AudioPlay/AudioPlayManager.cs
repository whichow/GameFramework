using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayManager : MonoSingleton<AudioPlayManager>
{
    private List<AudioPlayer> _cachedPlayers;
    private AudioPlayer _bgPlayer;

    void Awake()
    {
        _Init();
        _InitAudioPlayers();
    }

    private void _Init()
    {
        _cachedPlayers = new List<AudioPlayer>();
    }

    private void _InitAudioPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            _CreateAudioPlayer();
        }
        _CreateBgPlayer();
    }

    /// <summary>
    /// 设置全局音量大小
    /// </summary>
    /// <param name="volum">音量</param>
    public void SetVolum(float volum)
    {
        AudioListener.volume = volum;
    }

    /// <summary>
    /// 全局音频开关
    /// </summary>
    /// <param name="pause">启用音频播放</param>
    public void SetAudioPlay(bool enable)
    {
        AudioListener.pause = !enable;
    }

    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="clip">音频片段</param>
    /// <param name="loop">是否循环播放</param>
    /// <param name="volum">音量大小</param>
    /// <returns>音频播放器</returns>
    public AudioPlayer PlayAudio(AudioClip clip, bool loop = false, float volum = 1f)
    {
        var player = _GetUnusedPlayer();
        player.Play(clip, loop, volum);
        return player;
    }

    /// <summary>
    /// 播放背景音乐，会替换当前正在播放的背景音乐
    /// </summary>
    /// <param name="clip">音频片段</param>
    /// <param name="loop">是否循环播放</param>
    /// <param name="volum">音量大小</param>
    /// <returns>音频播放器</returns>
    public AudioPlayer PlayBgAudio(AudioClip clip, bool loop = true, float volum = 1f)
    {
        _bgPlayer.Play(clip, loop, volum);
        return _bgPlayer;
    }

    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    public void StopBgAudio()
    {
        _bgPlayer.Stop();
    }

    /// <summary>
    /// 暂停背景音乐播放
    /// </summary>
    public void PauseBgAudio()
    {
        _bgPlayer.Pause();
    }

    /// <summary>
    /// 恢复背景音乐播放
    /// </summary>
    public void UnPauseBgAudio()
    {
        _bgPlayer.UnPause();
    }

    /// <summary>
    /// 停止播放所有音频（不包括背景音乐）
    /// </summary>
    public void StopAllAudio()
    {
        for (int i = 0; i < _cachedPlayers.Count; i++)
        {
            _cachedPlayers[i].Stop();
        }
    }

    /// <summary>
    /// 暂停所有音频播放（不包括背景音乐）
    /// </summary>
    public void PauseAllAudio()
    {
        for (int i = 0; i < _cachedPlayers.Count; i++)
        {
            _cachedPlayers[i].Pause();
        }
    }

    /// <summary>
    /// 恢复所有音频播放（不包括背景音乐）
    /// </summary>
    public void UnPauseAllAudio()
    {
        for (int i = 0; i < _cachedPlayers.Count; i++)
        {
            _cachedPlayers[i].UnPause();
        }
    }

    private AudioPlayer _GetUnusedPlayer()
    {
        for (int i = 0; i < _cachedPlayers.Count; i++)
        {
            var player = _cachedPlayers[i];
            if (!player.IsPlaying)
            {
                return player;
            }
        }
        return _CreateAudioPlayer();
    }

    private AudioPlayer _CreateAudioPlayer()
    {
        var go = new GameObject("AudioPlayer");
        var player = go.AddComponent<AudioPlayer>();
        _cachedPlayers.Add(player);
        return player;
    }

    private AudioPlayer _CreateBgPlayer()
    {
        var go = new GameObject("BgPlayer");
        var player = go.AddComponent<AudioPlayer>();
        return player;
    }
}
