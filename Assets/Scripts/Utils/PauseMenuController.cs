using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuController : PersistentSingleton<PauseMenuController>
{

    protected override void Awake()
    {
        base.Awake();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        OnGameStateChanged(GameStateManager.Instance.CurrentGameState);
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ResumeGame()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        gameObject.SetActive(newGameState == GameState.Paused);
    }

    
}
