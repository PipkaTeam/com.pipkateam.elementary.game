using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace Elementary.Game
{
    public enum GameState
    {
        Playing,
        Paused
    }

    public class Game : Singleton<Game>
    {
        private GameState currentGameState = GameState.Playing;

        public Action<GameState> OnGameStateChanged;

        public GameState GetGameState()
        {
            return currentGameState;
        }

        public void SetGameState(GameState newGameState)
        {
            if (currentGameState == newGameState)
            {
                Debug.LogWarning($"[Elementary Game][Game] Trying to set game state to {newGameState}, but it's already in that state.");
                return;
            }

            currentGameState = newGameState;
            Debug.Log($"[Elementary Game][Game] Game state set to: {currentGameState}");

            if (currentGameState == GameState.Paused)
            {
                Time.timeScale = 0f;
                Debug.Log("[Elementary Game][Game] Game paused.");
            }
            else
            {
                Time.timeScale = 1f;
                Debug.Log("[Elementary Game][Game] Game resumed.");
            }

            OnGameStateChanged?.Invoke(currentGameState);
        }

        public void OpenLevel(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
            Debug.Log($"[Elementary Game][Game] OpenLevel: Level {buildIndex} opened.");
        }

        public void RestartLevel()
        {
            Debug.Log("[Elementary Game][Game] RestartLevel: Restarting the current level.");

            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene == null)
            {
                Debug.LogError("[Elementary Game][Game] RestartLevel error: Could not retrieve the current scene.");
                return;
            }

            OpenLevel(currentScene.buildIndex);
            Debug.Log($"[Elementary Game][Game] RestartLevel: Level {currentScene.name} restarted.");
        }

        public void OpenNextLevel()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int nextSceneBuildIndex = currentScene.buildIndex + 1;

            if (nextSceneBuildIndex < SceneManager.sceneCountInBuildSettings)
            {
                OpenLevel(nextSceneBuildIndex);
            }
            else
            {
                Debug.LogWarning("[Elementary Game][Game] OpenNextLevel: No next level available. Current level is the last one.");
            }
        }

        public int GetLevelIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        public int GetNextLevelIndex()
        {
            return GetLevelIndex() + 1;
        }

        public bool ContainNextLevel()
        {
            int nextSceneIndex = GetNextLevelIndex();

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            SetGameState(GameState.Playing);
        }
    }
}