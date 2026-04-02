using ColorRush.Core;
using ColorRush.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorRush.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text streakText;
        [SerializeField] private TMP_Text multiplierText;
        [SerializeField] private Image activeColorImage;
        [SerializeField] private Image nextColorImage;
        [SerializeField] private Data.ColorConfigSO colorConfig;

        private void OnEnable()
        {
            GameSignals.OnScoreChanged += OnScoreChanged;
            GameSignals.OnStreakChanged += OnStreakChanged;
            GameSignals.OnColorShiftApplied += OnColorApplied;
            GameSignals.OnColorShiftWarningStarted += OnWarningStarted;
        }

        private void OnDisable()
        {
            GameSignals.OnScoreChanged -= OnScoreChanged;
            GameSignals.OnStreakChanged -= OnStreakChanged;
            GameSignals.OnColorShiftApplied -= OnColorApplied;
            GameSignals.OnColorShiftWarningStarted -= OnWarningStarted;
        }

        private void OnScoreChanged(int score, float multiplier)
        {
            scoreText.text = score.ToString("N0");
            multiplierText.text = $"x{multiplier:0.00}";
        }

        private void OnStreakChanged(int streak, float multiplier)
        {
            streakText.text = $"Streak {streak}";
        }

        private void OnColorApplied(ColorId color)
        {
            activeColorImage.color = colorConfig.Get(color).color;
        }

        private void OnWarningStarted(ColorId current, ColorId next, float duration)
        {
            nextColorImage.color = colorConfig.Get(next).color;
        }
    }
}
