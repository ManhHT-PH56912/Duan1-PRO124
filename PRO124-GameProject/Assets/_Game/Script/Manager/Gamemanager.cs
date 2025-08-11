using UnityEngine;
using DesignPatterns.Singleton;
using Assets._Game.Script.Helper;

public enum GameState
{
    None,
    Start,
    Playing,
    Pause,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    public static GameState gameState = GameState.None;

    private bool isPaused;

    public bool IsPaused => isPaused;

    protected override void Awake()
    {
        base.Awake();

        // Khởi tạo trạng thái mặc định
        if (gameState == GameState.None)
            gameState = GameState.Start;
    }

    private void Start()
    {
        if (gameState == GameState.Start)
        {
            LoadingSceneManager.Instance.LoadScene(SceneIndexs.AUTH);
            Debug.Log("Game started.");
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        gameState = isPaused ? GameState.Pause : GameState.Playing;

        Debug.Log(isPaused ? "Game paused." : "Game resumed.");
    }
}
