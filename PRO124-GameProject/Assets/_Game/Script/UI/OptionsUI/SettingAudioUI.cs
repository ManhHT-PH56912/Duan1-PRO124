using DesignPatterns.Singleton;
using UnityEngine;
using UnityEngine.UI;

public class SettingAudioUI : Singleton<SettingAudioUI>
{
    private AudioManager audioManager;
    [Header("UI Controls")]
    [SerializeField] private Slider volumeSlider;       // Âm lượng nhạc nền
    [SerializeField] private Slider sfxVolumeSlider;    // Âm lượng hiệu ứng
    [SerializeField] private Toggle musicToggle;        // Bật/tắt nhạc nền
    [SerializeField] private Toggle sfxToggle;          // Bật/tắt hiệu ứng

    protected override void Awake()
    {
        base.Awake();
    }
}
