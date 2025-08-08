using DesignPatterns.Singleton;
using UnityEngine;
using System.Collections.Generic;

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

    protected override void Awake()
    {
        base.Awake();
        LoadAudioSettings();
        PlayBackgroundMusic();
    }


    private void LoadAudioSettings()
    {
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
                break;

            case AudioType.SOUND_EFFECT:
                isSfxMuted = isMuted;
                if (soundEffectSource != null)
                    soundEffectSource.mute = isMuted;

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
