using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DesignPatterns.Singleton;

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
    public AudioClip soundEffects;
    public List<AudioClip> backgroundMusic;

    public PlayerDataModel playerData;

    private Coroutine musicTransitionCoroutine;

    private void Start()
    {
        if (playerData == null && FirebaseManager.Instance != null)
            playerData = FirebaseManager.Instance.playerData;

        ApplyAudioSettings();
        PlayBackgroundMusic();
    }

    public void ApplyAudioSettings()
    {
        if (playerData == null) return;

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = playerData.settings.musicVolume;
            backgroundMusicSource.mute = !playerData.settings.isMusicOn;
        }
        if (soundEffectSource != null)
        {
            soundEffectSource.volume = playerData.settings.sfxVolume;
            soundEffectSource.mute = !playerData.settings.isSfxOn;
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicSource != null && backgroundMusic.Count > 0)
            PlayBackgroundMusicWithTransition(backgroundMusic[0]);
    }

    public void PlayBackgroundMusicWithTransition(AudioClip newClip)
    {
        if (musicTransitionCoroutine != null)
            StopCoroutine(musicTransitionCoroutine);
        musicTransitionCoroutine = StartCoroutine(FadeAndSwitchMusic(newClip));
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null || soundEffectSource == null) return;

        soundEffectSource.PlayOneShot(clip);
    }

    private IEnumerator FadeAndSwitchMusic(AudioClip newClip)
    {
        float startVolume = backgroundMusicSource.volume;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            backgroundMusicSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        backgroundMusicSource.clip = newClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();

        t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            backgroundMusicSource.volume = Mathf.Lerp(0f, playerData.settings.musicVolume, t);
            yield return null;
        }
    }

    public void SetVolume(AudioType type, float volume, bool isOn)
    {
        if (playerData == null) return;

        switch (type)
        {
            case AudioType.BACKGROUND_MUSIC:
                playerData.settings.musicVolume = volume;
                playerData.settings.isMusicOn = isOn;
                backgroundMusicSource.volume = volume;
                backgroundMusicSource.mute = !isOn;
                break;

            case AudioType.SOUND_EFFECT:
                playerData.settings.sfxVolume = volume;
                playerData.settings.isSfxOn = isOn;
                soundEffectSource.volume = volume;
                soundEffectSource.mute = !isOn;
                break;
        }
    }
}
