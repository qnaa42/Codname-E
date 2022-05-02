using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    [System.Serializable]
    public class BaseGameManager : MonoBehaviour
    {
        //Game State placeholders
        public Game.State currentGameState;
        public Game.State targetGameState;
        public Game.State lastGameState;

        public bool paused;

        //private bool Paused;


        /// <summary>
        /// Seting target state, if target state is different than current => update the state
        /// </summary>
        /// <param name="aState">Target State</param>
        public void SetTargetState(Game.State aState)
        {
            targetGameState = aState;

            if (targetGameState != currentGameState)
                UpdateTargetState();
        }

        public Game.State GetCurrentState()
        {
            return currentGameState;
        }

        //[Header("GameEvenets")]

        public UnityEvent OnLoading;
        public UnityEvent OnLoaded;

        public UnityEvent OnGameStarting;
        public UnityEvent OnGameStarted;

        public UnityEvent OnLevelStarting;
        public UnityEvent OnLevelStarted;

        public UnityEvent OnGamePlaying;

        public UnityEvent OnLevelEnding;
        public UnityEvent OnlevelEnded;
        public UnityEvent OnGameEnded;

        public UnityEvent OnGamePausing;
        public UnityEvent OnGameUnPaused;

        public UnityEvent OnRestarting;

        public virtual void Loading() { OnLoading.Invoke(); }
        public virtual void Loaded() { OnLoaded.Invoke(); }

        public virtual void GameStarting() { OnGameStarting.Invoke(); }
        public virtual void GameStarted() { OnGameStarted.Invoke(); }


        public virtual void GamePlaying() { OnGamePlaying.Invoke(); }

        public virtual void GameEnded() 
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }

        public virtual void GamePausing() { OnGamePausing.Invoke(); }
        public virtual void GameUnPausing() { OnGameUnPaused.Invoke(); }

        public virtual void Restarting() 
        {
            var thisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(thisScene.name); 
            OnRestarting.Invoke(); 
        }


        public virtual void UpdateTargetState()
        {
            if (targetGameState == currentGameState)
                return; 
            switch (targetGameState)
            {
                case Game.State.idle:
                    break;

                case Game.State.loading:
                    break;

                case Game.State.loaded:
                    break;

                case Game.State.gameStarting:
                    break;

                case Game.State.gameStarted:
                    break;

                case Game.State.gamePlaying:
                    break;


                case Game.State.gameEnded:
                    break;

                case Game.State.gamePausing:
                    break;

                case Game.State.gameUnPausing:
                    break;

                case Game.State.restarting:
                    break;
            }
            currentGameState = targetGameState;
        }

        public virtual void UpdateCurrentState()
        {
            switch (currentGameState)
            {
                case Game.State.idle:
                    break;

                case Game.State.loading:
                    break;

                case Game.State.loaded:
                    break;

                case Game.State.gameStarting:
                    break;

                case Game.State.gameStarted:
                    break;


                case Game.State.gamePlaying:
                    break;


                case Game.State.gameEnded:
                    break;

                case Game.State.gamePausing:
                    
                    break;

                case Game.State.gameUnPausing:

                    break;

                case Game.State.restarting:

                    break;
            }
        }

        public virtual void GamePaused()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            OnGamePausing.Invoke();
            Paused = true;
        }

        public virtual void GameUnPaused()
        {
            OnGameUnPaused.Invoke();
            Paused = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public bool Paused
        {
            get
            {
                return paused;
            }
            set
            {
                paused = value;

                if (paused)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }
}
