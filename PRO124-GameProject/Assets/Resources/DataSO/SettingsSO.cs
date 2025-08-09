using UnityEngine;

[CreateAssetMenu(fileName = "SettingsSO", menuName = "Game/SettingsSO")]
public class SettingsSO : ScriptableObject
{
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    public bool musicMuted = false;
    public bool sfxMuted = false;

    private const string SAVE_KEY = "GameSettings";

    // Lưu ra PlayerPrefs (JSON)
    public void Save()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    // Load từ PlayerPrefs
    public void Load()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
