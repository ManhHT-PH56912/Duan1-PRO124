using DesignPatterns.Singleton;
using UnityEngine;

public enum GameState
{
    None = 0,
    Start = 1,
    Playing = 2,
    Pause = 3,
    GameOver = 4,
}

public class GameManager : Singleton<GameManager>
{
    public static GameState gameState;
    private bool isPaused = false;

    protected override void Awake()
    {
        base.Awake();
        if (gameState == GameState.None)
        {
            gameState = GameState.Start;
        }
    }

    private void Start()
    {
        if (gameState == GameState.Start)
        {
            Debug.Log("Game started.");
            LoadingSceneManager.Instance.LoadScene(SceneIndexs.AUTH);
        }
    }

    public bool IsPaused => isPaused;

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        gameState = isPaused ? GameState.Pause : GameState.Playing;
        Debug.Log($"Game {(isPaused ? "paused" : "resumed")}.");
    }
}
