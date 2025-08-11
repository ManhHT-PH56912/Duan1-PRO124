using UnityEngine;
using UnityEngine.UI;

public class SettingAudioUI : MonoBehaviour
{
    private AudioManager audioManager;
    private FirebaseManager firebaseManager;

    [Header("UI Controls")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        firebaseManager = FirebaseManager.Instance;

        RefreshUI();

        if (firebaseManager != null)
            firebaseManager.OnPlayerDataUpdated += RefreshUI;

        volumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
    }

    private void OnDestroy()
    {
        if (firebaseManager != null)
            firebaseManager.OnPlayerDataUpdated -= RefreshUI;
    }

    private void RefreshUI()
    {
        var data = firebaseManager?.playerData?.settings;
        if (data == null) return;

        volumeSlider.SetValueWithoutNotify(data.musicVolume);
        sfxVolumeSlider.SetValueWithoutNotify(data.sfxVolume);
        musicToggle.SetIsOnWithoutNotify(data.isMusicOn);
        sfxToggle.SetIsOnWithoutNotify(data.isSfxOn);

        audioManager?.ApplyAudioSettings();
    }

    private void OnMusicVolumeChanged(float volume)
    {
        bool isOn = musicToggle.isOn;
        audioManager?.SetVolume(AudioType.BACKGROUND_MUSIC, volume, isOn);
        SaveToFirebase();
    }

    private void OnSfxVolumeChanged(float volume)
    {
        bool isOn = sfxToggle.isOn;
        audioManager?.SetVolume(AudioType.SOUND_EFFECT, volume, isOn);
        SaveToFirebase();
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        float volume = volumeSlider.value;
        audioManager?.SetVolume(AudioType.BACKGROUND_MUSIC, volume, isOn);
        SaveToFirebase();
    }

    private void OnSfxToggleChanged(bool isOn)
    {
        float volume = sfxVolumeSlider.value;
        audioManager?.SetVolume(AudioType.SOUND_EFFECT, volume, isOn);
        SaveToFirebase();
    }

    private void SaveToFirebase()
    {
        firebaseManager?.WriteData();
    }
}
