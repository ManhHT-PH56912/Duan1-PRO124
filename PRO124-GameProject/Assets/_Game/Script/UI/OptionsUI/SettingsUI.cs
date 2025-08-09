using UnityEngine;
using UnityEngine.UI;

public class SettingAudioUI : MonoBehaviour
{
    private AudioManager audioManager;

    [Header("UI Controls")]
    [SerializeField] private Slider volumeSlider;       // Âm lượng nhạc nền
    [SerializeField] private Slider sfxVolumeSlider;    // Âm lượng hiệu ứng
    [SerializeField] private Toggle musicToggle;        // Bật/tắt nhạc nền
    [SerializeField] private Toggle sfxToggle;          // Bật/tắt hiệu ứng

    private void Start()
    {
        if (audioManager == null)
        {
            audioManager = FindAnyObjectByType<AudioManager>();
        }

        // Set initial UI values
        volumeSlider.value = audioManager.backgroundMusicSource.volume;
        sfxVolumeSlider.value = audioManager.soundEffectSource.volume;

        musicToggle.isOn = !audioManager.GetMute(AudioType.BACKGROUND_MUSIC);
        sfxToggle.isOn = !audioManager.GetMute(AudioType.SOUND_EFFECT);

        // Add listeners
        volumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        if (audioManager != null && audioManager.backgroundMusicSource != null)
        {
            audioManager.backgroundMusicSource.volume = volume;
        }
    }

    private void OnSfxVolumeChanged(float volume)
    {
        if (audioManager != null && audioManager.soundEffectSource != null)
        {
            audioManager.soundEffectSource.volume = volume;
        }
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        if (audioManager != null)
        {
            audioManager.SetMute(AudioType.BACKGROUND_MUSIC, !isOn);
        }
    }

    private void OnSfxToggleChanged(bool isOn)
    {
        if (audioManager != null)
        {
            audioManager.SetMute(AudioType.SOUND_EFFECT, !isOn);
        }
    }
}
