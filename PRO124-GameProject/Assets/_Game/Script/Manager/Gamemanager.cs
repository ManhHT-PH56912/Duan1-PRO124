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
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        if (gameState == GameState.None)
        {
            gameState = GameState.Start;
        }
    }

    [System.Obsolete]
    private void Start()
    {
        if (gameState == GameState.Start)
        {
            Debug.Log("Game started.");
            LoadingSceneManager.Instance.LoadScene(SceneIndexs.AUTH);
        }
    }
}
