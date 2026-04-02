using ColorRush.Chaser;
using ColorRush.Color;
using ColorRush.Data;
using ColorRush.Difficulty;
using ColorRush.PowerUps;
using ColorRush.Runner;
using ColorRush.Scoring;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColorRush.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private RunnerController runnerController;
        [SerializeField] private SwipeInputController swipeInputController;
        [SerializeField] private ColorStateController colorStateController;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private ChaserController chaserController;
        [SerializeField] private PowerUpController powerUpController;
        [SerializeField] private DifficultyDirector difficultyDirector;

        [Header("Run Rules")]
        [SerializeField] private int maxStrikes = 3;

        public GameState CurrentState { get; private set; } = GameState.Boot;
        public int CurrentStrikes { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            GameSignals.OnGateResolved += HandleGateResolved;
        }

        private void OnDisable()
        {
            GameSignals.OnGateResolved -= HandleGateResolved;
        }

        private void Start()
        {
            SetState(GameState.MainMenu);
        }

        public void StartRun()
        {
            CurrentStrikes = 0;
            SetState(GameState.Playing);

            difficultyDirector.ResetRun();
            scoreManager.ResetRun();
            chaserController.ResetRun();
            powerUpController.ResetRun();
            colorStateController.ResetRun();
            runnerController.ResetRun();
            swipeInputController.EnableInput(true);

            GameSignals.OnStrikeChanged?.Invoke(CurrentStrikes);
            GameSignals.OnRunStarted?.Invoke();
        }

        public void PauseRun()
        {
            if (CurrentState != GameState.Playing) return;
            Time.timeScale = 0f;
            SetState(GameState.Paused);
        }

        public void ResumeRun()
        {
            if (CurrentState != GameState.Paused) return;
            Time.timeScale = 1f;
            SetState(GameState.Playing);
        }

        public void RetryRun()
        {
            Time.timeScale = 1f;
            StartRun();
        }

        public void ReturnToMenu()
        {
            Time.timeScale = 1f;
            SetState(GameState.MainMenu);
        }

        public void ReloadScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void HandleGateResolved(bool success)
        {
            if (!IsGameplayActive()) return;

            if (success)
            {
                chaserController.OnPerfectGate(scoreManager.CurrentStreak);
                return;
            }

            if (powerUpController.TryConsumeMistakeProtection())
            {
                return;
            }

            CurrentStrikes++;
            GameSignals.OnStrikeChanged?.Invoke(CurrentStrikes);
            scoreManager.OnMistake();
            runnerController.OnMistake(CurrentStrikes);
            chaserController.OnMistake(CurrentStrikes);

            if (CurrentStrikes >= maxStrikes)
            {
                EndRun();
            }
        }

        public void EndRun()
        {
            if (CurrentState == GameState.GameOver) return;

            swipeInputController.EnableInput(false);
            SetState(GameState.GameOver);
            GameSignals.OnRunEnded?.Invoke();
        }

        public bool IsGameplayActive() => CurrentState == GameState.Playing;

        private void SetState(GameState state)
        {
            CurrentState = state;
            GameSignals.OnGameStateChanged?.Invoke(state);
        }
    }
}
