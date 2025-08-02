using DesignPatterns.Singleton;
using UnityEngine;
using System.Collections.Generic;

public enum AudioType
{
    BACKGROUND_MUSIC,
    SOUND_EFFECT
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource soundEffectSource;

    [Header("Audio Clips")]
    public List<AudioClip> soundEffects;
    public AudioClip backgroundMusic;

    private bool isMusicMuted;
    private bool isSfxMuted;

    private const string MUSIC_KEY = "MUSIC_MUTED";
    private const string SFX_KEY = "SFX_MUTED";

    protected override void Awake()
    {
        base.Awake();
        LoadAudioSettings();
        PlayBackgroundMusic();
    }

    private void LoadAudioSettings()
    {
        isMusicMuted = PlayerPrefs.GetInt(MUSIC_KEY, 0) == 1;
        isSfxMuted = PlayerPrefs.GetInt(SFX_KEY, 0) == 1;

        if (backgroundMusicSource != null)
            backgroundMusicSource.mute = isMusicMuted;

        if (soundEffectSource != null)
            soundEffectSource.mute = isSfxMuted;
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicSource == null || backgroundMusic == null || isMusicMuted) return;

        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlaySoundEffect(string clipName)
    {
        if (isSfxMuted || soundEffectSource == null) return;

        AudioClip clip = soundEffects.Find(s => s.name == clipName);
        if (clip != null)
        {
            soundEffectSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound effect '{clipName}' not found!");
        }
    }

    public void SetMute(AudioType type, bool isMuted)
    {
        switch (type)
        {
            case AudioType.BACKGROUND_MUSIC:
                isMusicMuted = isMuted;
                if (backgroundMusicSource != null)
                {
                    backgroundMusicSource.mute = isMuted;
                    if (isMuted) backgroundMusicSource.Stop();
                    else PlayBackgroundMusic();
                }
                PlayerPrefs.SetInt(MUSIC_KEY, isMuted ? 1 : 0);
                break;

            case AudioType.SOUND_EFFECT:
                isSfxMuted = isMuted;
                if (soundEffectSource != null)
                    soundEffectSource.mute = isMuted;

                PlayerPrefs.SetInt(SFX_KEY, isMuted ? 1 : 0);
                break;
        }
    }

    public bool GetMute(AudioType type)
    {
        return type switch
        {
            AudioType.BACKGROUND_MUSIC => isMusicMuted,
            AudioType.SOUND_EFFECT => isSfxMuted,
            _ => false
        };
    }
}
