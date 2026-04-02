using ColorRush.Core;
using ColorRush.Scoring;
using TMPro;
using UnityEngine;

namespace ColorRush.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text bestScoreText;
        [SerializeField] private TMP_Text longestStreakText;
        [SerializeField] private ScoreManager scoreManager;

        private const string BestScoreKey = "best_score";

        private void OnEnable()
        {
            GameSignals.OnGameStateChanged += OnGameStateChanged;
            GameSignals.OnRunEnded += Populate;
        }

        private void OnDisable()
        {
            GameSignals.OnGameStateChanged -= OnGameStateChanged;
            GameSignals.OnRunEnded -= Populate;
        }

        private void OnGameStateChanged(Data.GameState state)
        {
            root.SetActive(state == Data.GameState.GameOver);
        }

        private void Populate()
        {
            int final = scoreManager.CurrentScore;
            int best = Mathf.Max(final, PlayerPrefs.GetInt(BestScoreKey, 0));
            PlayerPrefs.SetInt(BestScoreKey, best);

            finalScoreText.text = final.ToString("N0");
            bestScoreText.text = best.ToString("N0");
            longestStreakText.text = scoreManager.LongestStreak.ToString();
        }

        public void OnRetryPressed() => GameManager.Instance.RetryRun();
        public void OnMenuPressed() => GameManager.Instance.ReturnToMenu();
        public void OnRevivePressed() { /* rewarded-ad hook placeholder */ }
    }
}
