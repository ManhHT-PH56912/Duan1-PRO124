using DesignPatterns.Singleton;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    public float musicVolume = 1f;
    public float sfxVolume = 0.5f;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}
