using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : PersistentSingleton<PauseController>
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;

            if (currentGameState == GameState.InCutscene)
                return;

            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused
                : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);
        }
    }
}
