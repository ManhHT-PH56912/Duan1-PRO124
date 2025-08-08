using UnityEngine;

public class PlayButton : BaseButtom
{
    private LoadingSceneManager loadingSceneManager;
    void Start()
    {
        loadingSceneManager = FindFirstObjectByType<LoadingSceneManager>();
    }

    protected override void OnButtonClick()
    {
        Debug.Log("Play Button Clicked!");
        loadingSceneManager.LoadScene(SceneIndexs.MAP1);
    }
}