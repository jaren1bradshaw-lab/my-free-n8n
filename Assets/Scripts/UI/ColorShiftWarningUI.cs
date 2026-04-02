using ColorRush.Core;
using ColorRush.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorRush.UI
{
    public class ColorShiftWarningUI : MonoBehaviour
    {
        [SerializeField] private Image currentColorImage;
        [SerializeField] private Image nextColorImage;
        [SerializeField] private Image countdownRing;
        [SerializeField] private TMP_Text warningText;
        [SerializeField] private CanvasGroup warningGroup;
        [SerializeField] private Data.ColorConfigSO colorConfig;

        private void OnEnable()
        {
            GameSignals.OnColorShiftWarningStarted += HandleWarningStart;
            GameSignals.OnColorShiftWarningTick += HandleWarningTick;
            GameSignals.OnColorShiftApplied += HandleShiftApplied;
        }

        private void OnDisable()
        {
            GameSignals.OnColorShiftWarningStarted -= HandleWarningStart;
            GameSignals.OnColorShiftWarningTick -= HandleWarningTick;
            GameSignals.OnColorShiftApplied -= HandleShiftApplied;
        }

        private void HandleWarningStart(ColorId current, ColorId next, float duration)
        {
            currentColorImage.color = colorConfig.Get(current).color;
            nextColorImage.color = colorConfig.Get(next).color;
            warningText.text = "SWITCH INCOMING";
            warningGroup.alpha = 1f;
            countdownRing.fillAmount = 1f;
        }

        private void HandleWarningTick(float normalizedRemaining)
        {
            countdownRing.fillAmount = normalizedRemaining;
            float pulse = 0.9f + Mathf.PingPong(Time.time * 4f, 0.1f);
            warningGroup.transform.localScale = Vector3.one * pulse;
        }

        private void HandleShiftApplied(ColorId newColor)
        {
            currentColorImage.color = colorConfig.Get(newColor).color;
            warningGroup.alpha = 0f;
        }
    }
}
