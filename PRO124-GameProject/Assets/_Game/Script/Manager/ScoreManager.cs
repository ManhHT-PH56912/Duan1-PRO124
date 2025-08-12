using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private FirebaseManager firebase;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        firebase = FirebaseManager.Instance;

        // Đăng ký lắng nghe khi data thay đổi từ Firebase
        firebase.OnPlayerDataUpdated += OnPlayerDataUpdated;
    }

    private void OnDestroy()
    {
        if (firebase != null)
            firebase.OnPlayerDataUpdated -= OnPlayerDataUpdated;
    }

    private void OnPlayerDataUpdated()
    {
        Debug.Log("ScoreManager: Data updated from Firebase.");
        // Nếu muốn update UI, gọi UIManager ở đây
        UIManager.Instance?.OnPlayerDataChanged();
    }

    // ====== HÀM QUẢN LÝ SCORE ======

    public int GetCoins() => firebase.playerData.stats.coins;
    public int GetExp() => firebase.playerData.stats.exp;
    public int GetLevel() => firebase.playerData.stats.level;

    public void AddCoins(int amount)
    {
        firebase.playerData.stats.coins += amount;
        Save();
    }

    public void AddExp(int amount)
    {
        firebase.playerData.stats.exp += amount;

        // Kiểm tra lên cấp
        if (firebase.playerData.stats.exp >= ExpToNextLevel(firebase.playerData.stats.level))
        {
            firebase.playerData.stats.level++;
            firebase.playerData.stats.exp = 0;
            Debug.Log($"Leveled up to {firebase.playerData.stats.level}!");
        }

        Save();
    }

    public void SetCheckpoint(int checkpointId)
    {
        firebase.playerData.stats.checkpoint = checkpointId;
        Save();
    }

    // ====== HÀM HỖ TRỢ ======
    private int ExpToNextLevel(int currentLevel)
    {
        // Công thức tính exp cần để lên level mới
        return 100 + (currentLevel * 50);
    }

    private void Save()
    {
        firebase.WriteData();
    }
}
