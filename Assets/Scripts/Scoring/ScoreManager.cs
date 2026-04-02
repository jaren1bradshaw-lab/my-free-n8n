using ColorRush.Core;
using UnityEngine;

namespace ColorRush.Scoring
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int gateBaseScore = 100;
        [SerializeField] private float distanceScorePerSecond = 12f;

        public int CurrentScore { get; private set; }
        public int CurrentStreak { get; private set; }
        public int LongestStreak { get; private set; }
        public float CurrentMultiplier { get; private set; } = 1f;

        private void OnEnable()
        {
            GameSignals.OnGateResolved += HandleGateResolved;
        }

        private void OnDisable()
        {
            GameSignals.OnGateResolved -= HandleGateResolved;
        }

        public void ResetRun()
        {
            CurrentScore = 0;
            CurrentStreak = 0;
            LongestStreak = 0;
            CurrentMultiplier = 1f;
            Broadcast();
        }

        private void Update()
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsGameplayActive()) return;
            int add = Mathf.RoundToInt(distanceScorePerSecond * Time.deltaTime * CurrentMultiplier);
            if (add <= 0) return;
            CurrentScore += add;
            Broadcast();
        }

        private void HandleGateResolved(bool success)
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsGameplayActive()) return;

            if (!success) return;

            CurrentStreak++;
            LongestStreak = Mathf.Max(LongestStreak, CurrentStreak);
            CurrentMultiplier = 1f + Mathf.Floor(CurrentStreak / 5f) * 0.25f;
            int gain = Mathf.RoundToInt(gateBaseScore * CurrentMultiplier);
            CurrentScore += gain;
            Broadcast();
        }

        public void OnMistake()
        {
            CurrentStreak = 0;
            CurrentMultiplier = Mathf.Max(1f, CurrentMultiplier - 0.5f);
            Broadcast();
        }

        private void Broadcast()
        {
            GameSignals.OnScoreChanged?.Invoke(CurrentScore, CurrentMultiplier);
            GameSignals.OnStreakChanged?.Invoke(CurrentStreak, CurrentMultiplier);
        }
    }
}
